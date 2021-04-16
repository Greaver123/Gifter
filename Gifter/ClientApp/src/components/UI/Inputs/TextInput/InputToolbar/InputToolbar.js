import React from 'react';
import classes from './InputToolbar.module.css';
import { CheckmarkOutline, CloseOutline } from 'react-ionicons';
import Spinner from '../../../Spinner/Spinner';

const InputToolbar = (props) => {
  return (
    <div
      className={[classes.Toolbar, !props.visible ? classes.Hidden : ''].join(
        ' '
      )}
    >
      {!props.spinner ? (
        <React.Fragment>
          <CheckmarkOutline
            cssClasses={[classes.Btn, classes.Ok].join(' ')}
            onClick={props.onSaveClick}
          />
          <CloseOutline
            cssClasses={[classes.Btn, classes.Delete].join(' ')}
            onClick={props.onDeleteClick}
          />
        </React.Fragment>
      ) : (
        <Spinner size="small" />
      )}
    </div>
  );
};

export default InputToolbar;
