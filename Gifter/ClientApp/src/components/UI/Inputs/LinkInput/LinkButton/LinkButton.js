import React, { useState } from 'react';
import classes from './LinkButton.module.css';

const LinkButton = (props) => {
  const [slideLeft, setSlideLeft] = useState(false);

  return (
    <div
      className={`${classes.LinkButton} ${
        !props.visible ? classes.Hidden : null
      }`}
      onClick={props.onClick}
    >
      {props.url?.replace(/^((http)|(https))(:\/\/)/, '')}
    </div>
  );
};

export default LinkButton;
