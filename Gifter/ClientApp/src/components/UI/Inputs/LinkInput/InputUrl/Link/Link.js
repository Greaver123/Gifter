import React from 'react';
import classes from './Link.module.css';

const Link = (props) => {
  return (
    <a className={classes.Link} href={props.url} target="_blank">
      {props.url}
    </a>
  );
};

export default Link;
