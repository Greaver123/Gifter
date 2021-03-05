import React from 'react';
import classes from './LinkInput.module.css';
import { useState, useEffect, useRef } from 'react';

const LinkInput = (props) => {
  const [isVisibleEditWindow, setIsVisibleEditWindow] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const urlInput = useRef(null);

  // useEffect(() => {
  //   document.addEventListener('click', function (e) {
  //     if (!e.target.classList.contains(classes.EditWindow)) {
  //       console.log('event');
  //       // setIsVisibleEditWindow(false);
  //     }
  //   });
  // });

  const showEditWindow = function () {
    setIsVisibleEditWindow(true);
  };

  const startEditMode = function () {
    setIsEditMode(true);
    // urlInput.current.focus();
    console.log('Edit mode');
  };

  const saveLink = function () {
    console.log('Save link');
    setIsEditMode(false);
    setIsVisibleEditWindow(false);
  };

  const deleteLink = function () {
    console.log('Delete link');
    //props.deleteLink();
    props.onDeleteLink();
    setIsVisibleEditWindow(false);
  };

  const addLink = function () {
    setIsVisibleEditWindow(true);
    setIsEditMode(true);
    console.log(urlInput.current);
    // urlInput.current.focus();
  };

  const placeholderText = 'Enter URL';

  const toolbar = (
    <div className={classes.Toolbar}>
      {isEditMode ? (
        <React.Fragment>
          {props.url !== '' && props.url !== null ? (
            <button
              className={`${classes.SaveLinkButton} ${classes.ToolbarBtn}`}
              onClick={saveLink}
            >
              S
            </button>
          ) : null}

          <button
            className={`${classes.DeleteLinkButton} ${classes.ToolbarBtn}`}
            onClick={deleteLink}
          >
            D
          </button>
        </React.Fragment>
      ) : (
        <button
          className={`${classes.EditLinkButton} ${classes.ToolbarBtn}`}
          onClick={startEditMode}
        >
          E
        </button>
      )}
    </div>
  );

  const input = (
    <input
      ref={urlInput}
      className={`${classes.UrlInput}`}
      type="url"
      value={props.url ?? ''}
      placeholder={placeholderText}
      pattern="https://.*"
      name="link"
      onBlur={() => {
        console.log('focus out');
      }}
      onChange={(e) => {
        props.onChange(e);
      }}
    />
  );

  const editWindowLink = (
    <a className={classes.Url} href={props.url} target="_blank">
      {props.url}
    </a>
  );

  const linkPlaceholder = (
    <span className={classes.Placeholder}>{placeholderText}</span>
  );

  const editWindow = (
    <div className={classes.EditWindow}>
      {!isEditMode
        ? props.url === null || props.url === ''
          ? linkPlaceholder
          : editWindowLink
        : input}
      {toolbar}
    </div>
  );

  const link = (
    <div className={classes.InputLink}>
      {isEditMode ? null : (
        <div className={classes.Link} onClick={showEditWindow}>
          {props.url}
        </div>
      )}
      {isVisibleEditWindow ? editWindow : null}
    </div>
  );

  const addLinkButton = <button onClick={addLink}>Add Link</button>; // Move Up to Wish or EditWishlist ?

  const linkInput =
    (props.url === null && !isVisibleEditWindow) ||
    (props.url === '' && !isVisibleEditWindow)
      ? addLinkButton
      : link;

  return linkInput;
};

export default LinkInput;
