import React from 'react';
import classes from './Wish.module.css';

const Wish = (props) => {
  return (
    <div className={classes.Wish}>
      <button className={classes.DeleteButton} onClick={props.clicked}>
        X
      </button>
      <div className={classes.WishInputs}>
        <input type="text" placeholder="Name" />
        <input type="url" placeholder="Link" />
        <input type="text" placeholder="Price" />
      </div>
      <div className={classes.ImagePreview}>
        <div>
          <input type="file" />
        </div>
      </div>
    </div>
  );
};

export default Wish;
