import React, { Component } from 'react';
import Backdrop from './Backdrop/Backdrop';
import classes from './Modal.module.css';
import Button from '../Button/Button';

class Modal extends Component {
  render() {
    return this.props.show ? (
      <React.Fragment>
        <div className={classes.Modal}>
          <div className={classes.Toolbar}></div>
          <div className={classes.ModalContent}>{this.props.children}</div>
          <div className={classes.ButtonsWrapper}>
            <Button type="No" clicked={this.props.noClicked}>
              No
            </Button>
            <Button type="Yes" clicked={this.props.yesClicked}>
              Yes
            </Button>
          </div>
        </div>
        <Backdrop />
      </React.Fragment>
    ) : null;
  }
}

export default Modal;
