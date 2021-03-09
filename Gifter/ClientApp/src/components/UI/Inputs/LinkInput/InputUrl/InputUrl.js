import React from 'react';
import classes from './InputUrl.module.css';

const InputUrl = (props) => {
  return (
    <input
      className={`${classes.UrlInput}`}
      type="url"
      value={props.url ?? ''}
      placeholder="Enter URL"
      pattern="https://.*"
      name="link"
      onBlur={props.onFocusOut}
      onChange={props.onChange}
    />
  );
};

export default InputUrl;
