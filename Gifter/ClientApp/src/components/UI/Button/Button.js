import React from 'react';
import Spinner from '../Spinner/Spinner';
import classes from './Button.module.css';

const Button = (props) => {
  return (
    <button
      className={[classes.Button, classes[props.type]].join(' ')}
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
