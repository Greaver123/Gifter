import React from 'react';
import classes from './LinkInput.module.css';
import { useState, useEffect, useRef } from 'react';
import InputToobar from './InputToolbar/InputToolbar';
import InputUrl from './InputUrl/InputUrl';
import Link from './Link/Link';
import LinkButton from './LinkButton/LinkButton';

const LinkInput = (props) => {
  const [isVisibleEditWindow, setIsVisibleEditWindow] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const urlInput = useRef(null);
  const linkInputWrapper = useRef(null);

  // useEffect(() => {
  //   const closeEditWindow = function (e) {
  //     console.log('close');
  //     setIsEditMode(false);
  //     setIsVisibleEditWindow(false);
  //   };

  //   const closeEditWindowOnClick = function (e) {
  //     console.log(isVisibleEditWindow);
  //     console.dir(e.target);

  //     // const target = e.target.contains(urlInput.current);
  //     // console.log(target);

  //     console.log(e.target.closest(`.${classes.InputLink}`));
  //     if (
  //       isVisibleEditWindow &&
  //       e.target.closest(`.${classes.InputLink}`) === null
  //     ) {
  //       closeEditWindow();
  //       console.log('close');
  //     }
  //   };

  //   console.log('Use Effect');

  //   document.addEventListener('keydown', closeEditWindow);
  //   document.addEventListener('click', closeEditWindowOnClick);

  //   return () => {
  //     document.removeEventListener('keydown', escape);
  //     document.removeEventListener('click', closeEditWindowOnClick);
  //   };
  // }, [linkInput, isVisibleEditWindow]);

  const showEditWindow = function () {
    setIsVisibleEditWindow(true);
  };

  const startEditMode = function () {
    console.log(urlInput);
    setIsEditMode(true);
  };

  const saveLink = function () {
    //TODO POOST
    setIsEditMode(false);
    setIsVisibleEditWindow(false);
  };

  const deleteLink = function (e) {
    props.onDeleteLink();
    setIsEditMode(true);
  };

  const inputUrl = (
    <InputUrl
      url={props.url}
      onFocusOut={() => {
        console.log('focus out');
        setIsEditMode(false);
        setIsVisibleEditWindow(false);
      }}
      onChange={(e) => {
        props.onChange(e);
      }}
    />
  );

  const linkInput = (
    <div className={classes.LinkInput}>
      {isEditMode ? (
        inputUrl
      ) : props.url === null ? (
        inputUrl
      ) : (
        <Link url={props.url} />
      )}
      {isVisibleEditWindow ? (
        <InputToobar
          editMode={isEditMode}
          onDeleteClick={deleteLink}
          onEditClick={startEditMode}
          onSaveClick={saveLink}
        />
      ) : null}
    </div>
  );

  return (
    <div ref={linkInputWrapper} className={classes.LinkInputWrapper}>
      {isVisibleEditWindow ? (
        linkInput
      ) : (
        <LinkButton url={props.url} onClick={showEditWindow} />
      )}
    </div>
  );
};

export default LinkInput;
