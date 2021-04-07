import React from 'react';
import Spinner from '../Spinner/Spinner';
import classes from './Button.module.css';

const Button = (props) => {
  let cssClasses = [classes.Button, classes[props.type]];
  if (props.className) cssClasses.push(props.className);

  return (
    <button
      className={cssClasses.join(' ')}
      onClick={() => {
        if (props.showSpinner) return;
        props.clicked();
      }}
    >
      {props.showSpinner ? <Spinner size="medium" /> : props.children}
    </button>
  );
};

export default Button;
