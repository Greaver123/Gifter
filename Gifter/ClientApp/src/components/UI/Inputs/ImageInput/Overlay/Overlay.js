import React from 'react';
import classes from './Overlay.module.css';

const Overlay = (props) => {
  return (
    <div className={classes.Overlay}>
      <div className={classes.OverlayButtons}>
        <button className={classes.BtnUpload} onClick={props.onSelectClick}>
          Select
        </button>
        {props.image != null ? (
          <button className={classes.BtnDelete} onClick={props.onDeleteClick}>
            Delete
          </button>
        ) : null}
      </div>
    </div>
  );
};

export default Overlay;
