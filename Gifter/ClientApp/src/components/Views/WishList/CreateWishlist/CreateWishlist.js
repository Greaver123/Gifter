import React from 'react';
import classes from './CreateWishlist.module.css';

const CreateWishlist = (props) => {
  return (
    <div className={classes.CreateWishlist}>
      <input type="text" placeholder="Enter Title" />
      <div>
        <button className={classes.CancelButton} onClick={props.cancel}>
          Cancel
        </button>
        <button className={classes.OkButton} onClick={props.ok}>
          Ok
        </button>
      </div>
    </div>
  );
};

export default CreateWishlist;
