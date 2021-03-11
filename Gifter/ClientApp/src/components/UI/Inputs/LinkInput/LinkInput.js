import React from 'react';
import classes from './LinkInput.module.css';
import { useState, useEffect, useRef } from 'react';
import InputUrl from './InputUrl/InputUrl';
import LinkButton from './LinkButton/LinkButton';

const LinkInput = (props) => {
  const [isVisibleEditWindow, setIsVisibleEditWindow] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const inputUrlRef = useRef(null);
  const linkInputWrapper = useRef(null);

  useEffect(() => {
    console.log('[ComponentDidMount] LinkInput');
    console.log(inputUrlRef);
  }, [inputUrlRef]);

  const showEditWindow = function () {
    setIsVisibleEditWindow(true);
    if (props.url === '' || props.url === null) {
      setIsEditMode(true);
    }
  };

  const startEditMode = function () {
    setIsEditMode(true);
  };

  const saveLink = function () {
    //TODO POOST
    setIsEditMode(false);
    setIsVisibleEditWindow(false);
  };

  const deleteLink = function (e) {
    props.onDeleteLink();
    setIsEditMode(false);
    setIsVisibleEditWindow(false);
  };

  const cancelEdit = function (e) {
    setIsEditMode(false);
    setIsVisibleEditWindow(false);
  };

  return (
    <div ref={linkInputWrapper} className={classes.LinkInputWrapper}>
      {isVisibleEditWindow ? (
        <InputUrl
          url={props.url}
          onFocusOut={cancelEdit}
          onChange={(e) => {
            props.onChange(e);
          }}
          onSaveClick={saveLink}
          onEditClick={startEditMode}
          onDeleteClick={deleteLink}
          onCancel={cancelEdit}
          editMode={isEditMode}
        />
      ) : (
        <LinkButton
          url={props.url == null || props.url == '' ? 'Enter URL' : props.url}
          onClick={showEditWindow}
        />
      )}
    </div>
  );
};

export default LinkInput;
