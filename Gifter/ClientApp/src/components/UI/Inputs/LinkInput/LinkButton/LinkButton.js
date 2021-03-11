import React, { useState } from 'react';
import classes from './LinkButton.module.css';

const LinkButton = (props) => {
  const [clicked, setClicked] = useState(false);

  return (
    <div
      className={`${classes.LinkButton} ${clicked ? classes.Clicked : ''}`}
      onClick={() => setClicked((prevState) => !prevState)}
      onTransitionEnd={props.onClick}
    >
      {props.url?.replace(/^((http)|(https))(:\/\/)/, '')}
    </div>
  );
};

export default LinkButton;
