import React, { Component } from 'react';
import classes from './ImageInput.module.css';
import image from '../../../../assets/images/imagePreview256px.png';
class ImageInput extends Component {
  imageInput = React.createRef();

  state = {
    selectedUrl: null,
  };

  selectImage = () => {
    if (this.imageInput.current.files.length) {
      const selectedImage = this.imageInput.current.files[0];

      const imgUrl = URL.createObjectURL(selectedImage);
      this.setState({ selectedUrl: imgUrl });

      const imageSelected = this.props?.imageSelected;

      if (imageSelected) {
        imageSelected(this.props.wishId, selectedImage);
      }
    }
  };

  showFileExplorer = () => {
    this.imageInput.current.click();
  };

  render() {
    let imageInput = (
      <input type="image" src={image} alt="image/photo of wish" disabled />
    );
    if (!this.props.displayOnly) {
      imageInput = (
        <React.Fragment>
          <input
            ref={this.imageInput}
            type="file"
            accept="image/png, image/jpg"
            onChange={this.selectImage}
          />
          <input
            type="image"
            src={
              this.state.selectedUrl
                ? this.state.selectedUrl
                : this.props.image
                ? this.props.image
                : image
            }
            alt="image/photo of wish"
            onClick={this.showFileExplorer}
          ></input>
        </React.Fragment>
      );
    }

    return <div className={classes.ImageInput}>{imageInput}</div>;
  }
}

export default ImageInput;
