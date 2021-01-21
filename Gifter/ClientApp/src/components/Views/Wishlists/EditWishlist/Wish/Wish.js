import React from 'react';
import ImageInput from '../../../../UI/Inputs/ImageInput/ImageInput';
import classes from './Wish.module.css';

const Wish = (props) => {
  return (
    <div className={classes.Wish} data-id={props.id}>
      <button className={classes.DeleteButton} onClick={props.clicked}>
        X
      </button>
      <div className={classes.WishInputs}>
        <input
          type="text"
          name="name"
          placeholder="Name"
          value={props.name}
          onChange={props.changed}
        />
        <input
          type="url"
          name="link"
          placeholder="Link"
          value={props.link}
          onChange={props.changed}
        />
        <input
          type="text"
          name="price"
          placeholder="Price"
          value={props.price}
          onChange={props.changed}
        />
      </div>
      <div className={classes.ImagePreview}>
        <ImageInput />
      </div>
    </div>
  );
};

export default Wish;
