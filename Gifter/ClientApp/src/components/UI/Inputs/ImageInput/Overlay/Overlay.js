import React from 'react';
import classes from './Overlay.module.css';
import { CloudUploadOutline, CloseOutline } from 'react-ionicons';
import { SyncOutline } from 'react-ionicons';

const Overlay = (props) => {
  let buttons = !props.fetchError ? (
    <React.Fragment>
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
    </React.Fragment>
  ) : (
    <SyncOutline
      cssClasses={[classes.Icon].join(' ')}
      onClick={props.onRefreshClick}
      title="Refresh"
    />
  );

  return (
    <div
      className={[
        classes.Overlay,
        props.isVisible || props.fetchError ? classes.Visible : '',
      ].join(' ')}
    >
      {buttons}
    </div>
  );
};

export default Overlay;
