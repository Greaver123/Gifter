import React from 'react';
import classes from './CreateWishlist.module.css';
import Button from '../../../UI/Button/Button';
const CreateWishlist = (props) => {
  return (
    <div className={classes.CreateWishlist}>
      <input type="text" placeholder="Enter Title" />
      <div>
        <Button type="Cancel" clicked={props.cancel}>
          Cancel
        </Button>
        <Button type="Add" clicked={props.ok}>
          Ok
        </Button>
      </div>
    </div>
  );
};

export default CreateWishlist;
