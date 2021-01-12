import React, { Component } from 'react';
import SideDrawer from '../SideDrawer/SideDrawer';
import classes from './Toolbar.module.css';
import { withAuth0 } from '@auth0/auth0-react';
import LoginButton from '../../../auth/LoginButton/LoginButton';
import LogoutButton from '../../../auth/LogoutButton/LogoutButton';
import Navigation from '../Navigation/Navigation';
class Toolbar extends Component {
  state = {
    sideDrawerOpened: false,
  };

  SideDrawerClicked = () => {
    this.setState((prevState) => {
      return { sideDrawerOpened: !prevState.sideDrawerOpened };
    });
  };

  render() {
    return (
      <div className={classes.Toolbar}>
        <SideDrawer
          opened={this.state.sideDrawerOpened}
          clicked={this.SideDrawerClicked}
        />
        <Navigation />
        {!this.props.auth0.isAuthenticated ? <LoginButton /> : <LogoutButton />}
      </div>
    );
  }
}

export default withAuth0(Toolbar);
