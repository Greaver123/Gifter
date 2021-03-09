import React from 'react';
import classes from './TextInput.module.css';

const TextInput = (props) => {
  return <input {...props} type="text" className={classes.TextInput} />;
};

export default TextInput;
