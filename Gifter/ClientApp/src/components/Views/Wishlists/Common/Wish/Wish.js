import React from 'react';
import ImageInput from '../../../../UI/Inputs/ImageInput/ImageInput';
import LinkInput from '../../../../UI/Inputs/LinkInput/LinkInput';
import TextInput from '../../../../UI/Inputs/TextInput/TextInput';
import classes from './Wish.module.css';
import { CloseOutline } from 'react-ionicons';

const Wish = (props) => {
  return (
    <div className={classes.Wish} data-id={props.id}>
      {props.displayOnly ? null : (
        <CloseOutline
          onClick={props.deleteWish}
          cssClasses={classes.WishDeleteBtn}
        />
      )}
      <div className={classes.WishInputs}>
        <TextInput
          name="name"
          placeholder="Name"
          value={props.name}
          onChange={props.changed}
          disabled={props.displayOnly}
        />
        <TextInput
          name="price"
          placeholder="Price"
          value={props.price}
          onChange={props.changed}
          disabled={props.displayOnly}
        />
        <LinkInput
          url={props.link}
          onChange={props.changed}
          onDeleteLink={props.onDeleteLink}
        />
      </div>
      <div className={classes.ImagePreview}>
        <ImageInput
          displayOnly={props.displayOnly}
          fetchImage={props.fetchImage}
          uploadImage={props.uploadImage}
          deleteImage={props.deleteImage}
          imageId={props.imageId}
          wishId={props.id}
        />
      </div>
    </div>
  );
};

export default Wish;
