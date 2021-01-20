import React, { Component, useState } from 'react';
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
    }
  };

  showFileExplorer = () => {
    this.imageInput.current.click();
  };

  render() {
    return (
      <div className={classes.ImageInput}>
        <input
          ref={this.imageInput}
          type="file"
          accept="image/png, image/jpg"
          onChange={this.selectImage}
        />
        <input
          type="image"
          src={this.state.selectedUrl ? this.state.selectedUrl : image}
          onClick={this.showFileExplorer}
        ></input>
      </div>
    );
  }
}

export default ImageInput;
