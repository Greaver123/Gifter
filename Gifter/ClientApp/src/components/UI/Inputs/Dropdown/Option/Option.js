import React from 'react';
import classes from './Option.module.css';

const Option = (props) => {
  return <option value={props.value}>{props.children}</option>;
};

export default Option;
