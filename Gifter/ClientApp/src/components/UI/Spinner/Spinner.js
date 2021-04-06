import React from 'react';
import classes from './Spinner.module.css';

const Spinner = (props) => {
  const classesArray = [classes.Spinner];
  if (props.size === 'small') classesArray.push(classes.Small);
  if (props.size === 'medium') classesArray.push(classes.Medium);
  return <div className={classesArray.join(' ')}></div>;
};

export default Spinner;
