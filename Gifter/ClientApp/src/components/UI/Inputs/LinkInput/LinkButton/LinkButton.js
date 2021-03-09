import React from 'react';
import classes from './LinkButton.module.css';

const LinkButton = (props) => {
  return (
    <div className={classes.LinkButton} onClick={props.onClick}>
      {props.url}
    </div>
  );
};

export default LinkButton;
