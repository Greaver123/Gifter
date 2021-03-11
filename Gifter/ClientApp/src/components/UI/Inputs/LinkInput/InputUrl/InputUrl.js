import React, { useEffect, useRef } from 'react';
import classes from './InputUrl.module.css';
import InputToobar from './InputToolbar/InputToolbar';
import Link from './Link/Link';

const InputUrl = (props) => {
  const inputRef = useRef(null);

  useEffect(() => {
    if (props.editMode) {
      console.log('EditMODE');
      inputRef.current.focus();
    }
  });

  const keyEventsHandle = function (e) {
    if (e.key === 'Enter') {
      props.onSaveClick();
    } else if (e.key === 'Escape') {
      props.onCancel();
    }
  };

  return (
    <div className={classes.InputUrl}>
      <React.Fragment>
        <input
          ref={inputRef}
          className={`${!props.editMode ? classes.Hidden : ''}`}
          type="url"
          value={props.url ?? ''}
          placeholder="Enter URL"
          pattern="https://.*"
          name="link"
          onFocus={(e) => {
            e.target.select();
          }}
          onBlur={props.onFocusOut}
          onChange={props.onChange}
          onKeyDown={keyEventsHandle}
        />
        {!props.editMode ? <Link url={props.url ?? 'Enter URL'} /> : null}
      </React.Fragment>
      <InputToobar
        editMode={props.editMode}
        onSaveClick={props.onSaveClick}
        onEditClick={() => {
          props.onEditClick();
          inputRef.current.focus();
        }}
        onDeleteClick={props.onDeleteClick}
      />
    </div>
  );
};

export default InputUrl;
