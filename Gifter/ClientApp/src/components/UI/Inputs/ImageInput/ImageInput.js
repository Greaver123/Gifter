import React, { Component } from 'react';
import classes from './ImageInput.module.css';
import defaultImage from '../../../../assets/images/imagePreview256px.png';
import LoadingIndicator from '../../LoadingIndicator/LoadingIndicator';
import Overlay from './Overlay/Overlay';
class ImageInput extends Component {
  imageInput = React.createRef();

  state = {
    overlay: false,
    isHandlingRequest: false,
  };

  handleImageChange = async () => {
    this.setState({ isUploading: true });

    if (this.imageInput.current.files.length === 0) return;
    const selectedImage = this.imageInput.current.files[0];

    if (!this.props?.uploadImage) return;

    await this.props.uploadImage(this.props.wishId, selectedImage);

    this.setState({ isUploading: false });
  };

  deleteImage = async () => {
    if (this.state.isHandlingRequest) return;
    this.setState({ isHandlingRequest: true });
    try {
      await this.props.deleteImage();
    } finally {
      this.setState({ isHandlingRequest: false });
    }
  };

  selectImage = () => {
    this.imageInput.current.click();
  };

  hideOverlay = () => {
    this.setState({ overlay: false });
  };

  showOverlay = () => {
    this.setState({ overlay: true });
  };

  render() {
    if (this.state.isHandlingRequest || this.props.isLoadingImage)
      return <LoadingIndicator />;

    let imageInput = (
      <input
        type="image"
        src={defaultImage}
        alt="image/photo of wish"
        disabled
      />
    );

    if (!this.props.displayOnly) {
      imageInput = (
        <div
          className={classes.ImageInput}
          onMouseEnter={this.showOverlay}
          onMouseLeave={this.hideOverlay}
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
          {this.state.overlay ? (
            <Overlay
              image={this.props.image}
              onSelectClick={this.selectImage}
              onDeleteClick={this.deleteImage}
            />
          ) : null}
        </div>
      );
    }

    return imageInput;
  }
}

export default ImageInput;
