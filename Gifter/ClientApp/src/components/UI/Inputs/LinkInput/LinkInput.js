import React from 'react';
import classes from './LinkInput.module.css';
import { useState, useEffect, useRef } from 'react';
import InputUrl from './InputUrl/InputUrl';
import LinkButton from './LinkButton/LinkButton';

const LinkInput = (props) => {
  const [isVisibleEditWindow, setIsVisibleEditWindow] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const linkInputWrapper = useRef(null);

  const showEditWindow = function () {
    setIsVisibleEditWindow(true);
    if (props.url === '' || props.url === null) {
      setIsEditMode(true);
    }
  };

  const hideEditWindow = function (e) {
    setIsVisibleEditWindow((prev) => !prev);
  };

  const startEditMode = function () {
    setIsEditMode(true);
  };

  return (
    <div ref={linkInputWrapper} className={classes.LinkInputWrapper}>
      {/* <LinkButton
        url={props.url !== null ? props.url : 'Placeholder'}
        onClick={(e) => {
          showEditWindow();
        }}
        visible={!isVisibleEditWindow}
      /> */}

      <InputUrl
        // visible={isVisibleEditWindow}
        toggle={hideEditWindow}
        url={props.url}
        onChange={(e) => {
          props.onChange(e);
        }}
        onSaveClick={() => {
          console.log('[LinkInput] HideEditWindow');
        }}
        onEditClick={() => {}}
        onDeleteClick={() => {}}
        onCancel={() => {}}
        editMode={isEditMode}
        placeholderText={'Placeholder'}
      />
    </div>
  );
};

export default LinkInput;
