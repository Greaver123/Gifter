import React from 'react';
import classes from './LinkInput.module.css';
import { useState, useEffect, useRef } from 'react';

const LinkInput = (props) => {
  const [isVisibleEditWindow, setIsVisibleEditWindow] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const [isToolbarVisible, setIsToolbarVisible] = useState(false);
  const urlInput = useRef(null);

  // useEffect(() => {
  //   document.addEventListener('click', function (e) {
  //     if (!e.target.classList.contains(classes.EditWindow)) {
  //       console.log('event');
  //       setIsVisibleEditWindow(false);
  //     }
  //   });
  // });

  const showEditWindow = function () {
    setIsVisibleEditWindow(true);
  };

  const startEditMode = function () {
    console.log(urlInput);
    setIsEditMode(true);
    // urlInput.current.focus();
    console.log('Edit mode');
  };

  const saveLink = function () {
    console.log('Save link');
    setIsEditMode(false);
    setIsVisibleEditWindow(false);
  };

  const deleteLink = function (e) {
    console.log('Delete link');
    //props.deleteLink();
    props.onDeleteLink();
    // setIsVisibleEditWindow(false);
    setIsEditMode(true);
  };

  const showToolbar = function () {
    setIsToolbarVisible((prevState) => !prevState);
  };

  const placeholderText = 'Enter URL';

  const toolbar = (
    <div className={classes.Toolbar}>
      <React.Fragment>
        {isEditMode ? (
          <button
            className={`${classes.SaveLinkButton} ${classes.ToolbarBtn}`}
            onClick={saveLink}
          >
            S
          </button>
        ) : null}

        {!isEditMode ? (
          <button
            className={`${classes.EditLinkButton} ${classes.ToolbarBtn}`}
            onClick={startEditMode}
          >
            E
          </button>
        ) : null}

        <button
          className={`${classes.DeleteLinkButton} ${classes.ToolbarBtn}`}
          onClick={deleteLink}
        >
          X
        </button>
      </React.Fragment>
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
    <div className={classes.EditWindow} onTransitionEnd={showToolbar}>
      {isEditMode ? input : props.url === null ? input : editWindowLink}
      {props.url === null || props.url === '' ? null : toolbar}
    </div>
  );

  const link = (
    <div className={classes.InputLink}>
      {isEditMode || isVisibleEditWindow ? null : (
        <div className={classes.Link} onClick={showEditWindow}>
          {props.url}
        </div>
      )}
      {isVisibleEditWindow ? editWindow : null}
    </div>
  );

  return link;
};

export default LinkInput;
