import React from 'react';
import classes from './WishlistElement.module.css';
import Button from '../../../UI/Button/Button';

const WishlistElement = (props) => {
  let buttons = !props.assigned ? (
    <React.Fragment>
      <Button type="Edit" clicked={props.editClicked}>
        Edit
      </Button>
      <Button type="Delete" clicked={props.deleteClicked}>
        Delete
      </Button>
    </React.Fragment>
  ) : null;
  return (
    <div className={classes.WishlistElement}>
      <Button type="View" clicked={props.viewClicked}>
        {props.title}
      </Button>
      <div className={classes.ButtonsWrapper}>{buttons}</div>
    </div>
  );
};

export default WishlistElement;
