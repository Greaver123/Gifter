import React, { Component } from 'react';
import classes from './ImageInput.module.css';
import defaultImage from '../../../../assets/images/imagePreview256px.png';
import Overlay from './Overlay/Overlay';
import Spinner from '../../Spinner/Spinner';

class ImageInput extends Component {
  imageInput = React.createRef();

  state = {
    overlay: false,
    isHandlingRequest: false,
    fileMaxSize: this.props.fileMaxSize ?? 2000000, //2MB
    image: null,
    fetchError: false,
    // validate: {
    //   isValid: true,
    //   fileMaxSize: 20000000,
    //   acceptedFormat: ['Gif', 'jpg', 'jpeg', 'png'],
    // },
  };

  handleImageChange = async () => {
    this.setState({ isHandlingRequest: true });
    if (this.imageInput.current.files.length === 0) return;
    const selectedImage = this.imageInput.current.files[0];
    if (this.imageInput.current.files[0].size > this.state.fileMaxSize) {
      alert(
        `File too big: ${(
          this.imageInput.current.files[0].size / 1000000
        ).toFixed(2)} MB`
      );
      this.setState({ isHandlingRequest: false });
      this.imageInput.current.value = null;
      return;
    }

    try {
      await this.props.uploadImage(this.props.wishId, selectedImage);
      this.setState({
        isHandlingRequest: false,
        image: URL.createObjectURL(selectedImage),
      });
    } catch (err) {
    } finally {
      this.setState({
        isHandlingRequest: false,
        overlay: false,
      });
    }
  };

  deleteImage = async () => {
    if (this.state.isHandlingRequest) return;
    this.setState({ isHandlingRequest: true });
    try {
      await this.props.deleteImage();
      this.setState({ image: null });
    } catch (err) {
    } finally {
      this.setState({ isHandlingRequest: false, overlay: false });
    }
  };

  selectImage = () => {
    this.imageInput.current.click();
  };

  fetchImage = async () => {
    try {
      if (!this.props.imageId) return;
      this.setState({ isHandlingRequest: true });
      let image = await this.props.fetchImage();
      this.setState({ image: image, fetchError: false, overlay: false });
    } catch (err) {
      this.setState({ fetchError: true });
    } finally {
      this.setState({ isHandlingRequest: false });
    }
  };

  hideOverlay = () => {
    this.setState({ overlay: false });
  };

  showOverlay = () => {
    this.setState({ overlay: true });
  };

  async componentDidMount() {
    this.fetchImage();
  }

  render() {
    if (this.state.isHandlingRequest) return <Spinner />;

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
            src={this.state.image ? this.state.image : defaultImage}
            alt="image/photo of wish"
          ></img>
          <Overlay
            isVisible={this.state.overlay}
            fetchError={this.state.fetchError}
            image={this.state.image}
            onSelectClick={this.selectImage}
            onDeleteClick={this.deleteImage}
            onRefreshClick={this.fetchImage}
          />
        </div>
      );
    }

    return imageInput;
  }
}

export default ImageInput;
