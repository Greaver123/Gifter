import React, { useEffect, useRef, useState } from 'react';
import classes from './InputUrl.module.css';
import InputToobar from './InputToolbar/InputToolbar';
import Link from './Link/Link';
import LoadingIndicator from '../../../LoadingIndicator/LoadingIndicator';

const InputUrl = (props) => {
  const [isWaitingForRespnse, setIsWaitingForResponse] = useState(false);
  const [isLinkVisible, setIsLinkVisible] = useState(false);
  const [isToolbarVisible, setIsToolbarVisible] = useState(false);
  const [isDisabled, setIsDisabled] = useState(true);
  const [isEnabled, setIsEnabled] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const inputRef = useRef(null);

  const saveClick = function () {
    inputRef.current.setSelectionRange(0, 0);
    setIsWaitingForResponse(true);
    setIsDisabled(true);

    const wait = (ms) => new Promise((resolve) => setTimeout(resolve, ms));
    wait(3000).then(() => {
      setIsEditMode(false);
      setIsWaitingForResponse(false);
      setIsToolbarVisible(false);
      setIsEnabled(false);
    });
  };

  const editClick = function () {
    stopPreviewMode();
    startEditMode();
  };

  const deleteClick = function (e) {
    setIsWaitingForResponse(true);
    const wait = (ms) => new Promise((resolve) => setTimeout(resolve, ms));
    wait(3000).then(() => {
      props.onDeleteLink();
      setIsWaitingForResponse(false);
      setIsLinkVisible(false);
      setIsToolbarVisible(false);
    });
  };

  const enableInput = function (e) {
    setIsEnabled(true);
    console.log('Wrapper click');
  };

  const startEditMode = function (e) {
    setIsToolbarVisible(true);
    setIsDisabled(false);
    setIsEditMode(true);
  };

  const stopEditMode = function (e) {
    setIsToolbarVisible(false);
    setIsDisabled(true);
    setIsEditMode(false);
  };

  const startPreviewMode = function (e) {
    setIsLinkVisible(true);
    setIsToolbarVisible(true);
  };

  const stopPreviewMode = function (e) {
    setIsLinkVisible(false);
    setIsToolbarVisible(false);
  };

  useEffect(() => {
    console.log('[InputUrl UseEffect]');
    if (isEditMode) {
      console.log('[InputUrl UseEffect - EditMode - Focus]');

      inputRef.current.focus();
      inputRef.current.select();
    }
  }, [isEditMode]);

  const keyEventsHandle = function (e) {
    if (e.key === 'Enter') {
      saveClick();
    } else if (e.key === 'Escape') {
      deleteClick();
    }
  };

  return (
    <div className={classes.InputUrl} onClick={enableInput}>
      <React.Fragment>
        <input
          ref={inputRef}
          className={` ${classes.Input} ${
            isEnabled ? classes.SlideLeft : null
          } ${isLinkVisible ? classes.Hidden : null}`}
          type="url"
          value={props.url ?? ''}
          placeholder="Enter URL"
          pattern="https://.*"
          name="link"
          onFocus={(e) => {
            // if (props.editMode) {
            //   e.target.select();
            //   console.log('On focus');
            // }
          }}
          onBlur={(e) => {
            // console.log('blur');
          }}
          onChange={props.onChange}
          onKeyDown={keyEventsHandle}
          onTransitionEnd={(e) => {
            if (
              (props.url === null || props.url === '') &&
              !isEditMode &&
              isEnabled
            ) {
              // start edit mode
              console.log('[TransitionEnd] StartEditMode()');

              startEditMode(e);
            } else if (
              (props.url !== null || props.url !== '') &&
              !isEditMode &&
              isEnabled
            ) {
              //If there were some input loaded
              console.log('[TransitionEnd] StartPreviewMode()');

              startPreviewMode();
            } else if (!isEnabled) {
              console.log('[TransitionEnd] StopEditMode()');

              stopEditMode();
            }
            // if (props.isActiveWindow) {
            //   if (props.editMode) {
            //     console.log('FOCUS');
            //     // e.target.focus();
            //     // e.target.select();
            //   } else {
            //     // props.showLink();
            //     // props.hidePlaceholder();
            //   }
            //   // props.showToolbar();
            // } else {
            //   // console.log('BLUR');
            //   e.target.blur();
            //   // props.showPlaceholder();
            // }
            console.log('TRANSITION END');
          }}
          disabled={isDisabled}
        />
        {isLinkVisible ? (
          <Link url={props.url ?? props.placeholderText} />
        ) : null}
      </React.Fragment>

      {isToolbarVisible ? (
        isWaitingForRespnse ? (
          <LoadingIndicator style={{ margin: 'auto', fontSize: '3px' }} />
        ) : (
          <InputToobar
            editMode={isEditMode}
            onSaveClick={() => {
              console.log('Save');
              // inputRef.current.selectionStart = 0;
              // inputRef.current.selectionEnd = 0;
              // inputRef.current.blur();
              // props.toggleDisabled();
              // console.dir(inputRef.current);
              // console.log(inputRef.current.selectionStart);
              // console.log(inputRef.current.selectionEnd);
              saveClick();
            }}
            onEditClick={() => {
              // props.toggleDisabled();
              // inputRef.current.focus();
              editClick();
            }}
            onDeleteClick={deleteClick}
          />
        )
      ) : null}
    </div>
  );
};

export default InputUrl;
