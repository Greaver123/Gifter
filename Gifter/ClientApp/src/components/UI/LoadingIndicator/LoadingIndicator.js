import React from 'react';
import classes from './LoadingIndicator.module.css';

const LoadingIndicator = (props) => {
  return (
    <div className={classes.Loader} style={props.style}>
      {props.children}
    </div>
  );
};

export default LoadingIndicator;
