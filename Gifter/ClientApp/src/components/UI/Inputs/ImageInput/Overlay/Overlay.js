import React from 'react';
import classes from './Overlay.module.css';
import { CloudUploadOutline, CloseOutline } from 'react-ionicons';
const Overlay = (props) => {
  return (
    <div className={classes.Overlay}>
      <CloudUploadOutline
        onClick={props.onSelectClick}
        cssClasses={[classes.Icon, classes.UploadIcon].join(' ')}
        title="Upload image"
      />
      {props.image != null ? (
        <CloseOutline
          onClick={props.onDeleteClick}
          cssClasses={[classes.Icon, classes.DeleteIcon].join(' ')}
          title="Delete image"
        />
      ) : null}
    </div>
  );
};

export default Overlay;
