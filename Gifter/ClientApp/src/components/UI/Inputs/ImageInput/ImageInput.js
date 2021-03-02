import React, { Component } from 'react';
import classes from './ImageInput.module.css';
import defaultImage from '../../../../assets/images/imagePreview256px.png';
import LoadingIndicator from '../../LoadingIndicator/LoadingIndicator';

class ImageInput extends Component {
  imageInput = React.createRef();

  state = {
    newImageUrl: null,
    overlay: false,
    isLoading: false,
    isUploading: false,
  };

  uploadImage = () => {
    if (this.imageInput.current.files.length) {
      const selectedImage = this.imageInput.current.files[0];
      const imgUrl = URL.createObjectURL(selectedImage);
      this.setState({ newImageUrl: imgUrl });

      const uploadImg = this.props?.uploadImage;

      if (!uploadImg) return;
      uploadImg(this.props.wishId, selectedImage);
    }
  };

  deleteImage = () => {
    this.props.deleteImage();
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
          {this.state.newImageUrl != null || this.props.image != null ? (
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
            onChange={this.uploadImage}
          />
          <img
            type="image"
            src={
              this.state.newImageUrl
                ? this.state.newImageUrl
                : this.props.image
                ? this.props.image
                : defaultImage
            }
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
