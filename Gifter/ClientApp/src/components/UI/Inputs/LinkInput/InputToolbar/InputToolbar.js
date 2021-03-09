import React from 'react';
import classes from './InputToolbar.module.css';

const InputToolbar = (props) => {
  return (
    <div className={classes.Toolbar}>
      <React.Fragment>
        {props.editMode ? (
          <button
            className={`${classes.SaveLinkButton} ${classes.ToolbarBtn}`}
            onClick={props.onSaveClick}
          >
            S
          </button>
        ) : null}

        {!props.editMode ? (
          <button
            className={`${classes.EditLinkButton} ${classes.ToolbarBtn}`}
            onClick={props.onEditClick}
          >
            E
          </button>
        ) : null}

        <button
          className={`${classes.DeleteLinkButton} ${classes.ToolbarBtn}`}
          onClick={props.onDeleteClick}
        >
          X
        </button>
      </React.Fragment>
    </div>
  );
};

export default InputToolbar;
