import React, { Component } from 'react';
import classes from './ImageInput.module.css';
import defaultImage from '../../../../assets/images/imagePreview256px.png';

class ImageInput extends Component {
  imageInput = React.createRef();

  state = {
    overlay: false,
    isUploading: false,
  };

  handleImageChange = async () => {
    this.setState({ isUploading: true });

    if (this.imageInput.current.files.length === 0) return;
    const selectedImage = this.imageInput.current.files[0];

    if (!this.props?.uploadImage) return;

    await this.props.uploadImage(this.props.wishId, selectedImage);

    this.setState({ isUploading: false });
  };

  deleteImage = () => {
    console.log(this.deleteImage);
    if (!this.state.isUploading) {
      this.props.deleteImage();
    }
  };

  selectImage = () => {
    this.imageInput.current.click();
  };

  toggleOverlay = () => {
    this.setState((prevState) => ({
      overlay: !prevState.overlay,
    }));
  };

  showOverlay = () => {
    this.setState({ showOverlay: true });
  };

  render() {
    let imageInput = (
      <input
        type="image"
        src={defaultImage}
        alt="image/photo of wish"
        disabled
      />
    );

    let overlay = (
      <div className={classes.Overlay}>
        <div className={classes.OverlayButtons}>
          <button className={classes.BtnUpload} onClick={this.selectImage}>
            Select
          </button>
          {this.props.image != null ? (
            <button className={classes.BtnDelete} onClick={this.deleteImage}>
              Delete
            </button>
          ) : null}
        </div>
      </div>
    );

    if (!this.props.displayOnly) {
      imageInput = (
        <div
          className={classes.ImageInput}
          onMouseEnter={this.toggleOverlay}
          onMouseLeave={this.toggleOverlay}
          onClick={this.showOverlay}
        >
          <input
            ref={this.imageInput}
            type="file"
            accept="image/png, image/jpeg, image/gif"
            onChange={this.handleImageChange}
          />
          <img
            type="image"
            src={this.props.image ? this.props.image : defaultImage}
            alt="image/photo of wish"
          ></img>
          {this.state.overlay ? overlay : null}
        </div>
      );
    }

    return imageInput;
  }
}

export default ImageInput;
