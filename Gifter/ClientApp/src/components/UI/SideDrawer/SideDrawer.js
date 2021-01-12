import React from 'react';
import MenuBurger from './MenuBurger/MenuBurger';
import Navigation from '../Navigation/Navigation';
import LoginButton from '../../../auth/LoginButton/LoginButton';
import LogoutButton from '../../../auth/LogoutButton/LogoutButton';
import classes from './SideDrawer.module.css';
import { useAuth0 } from '@auth0/auth0-react';

const SideDrawer = (props) => {
  const { isAuthenticated } = useAuth0();

  let attachedClasses = [classes.SideDrawer, classes.Closed];
  if (props.opened) {
    attachedClasses = [classes.SideDrawer, classes.Open];
  }

  return (
    <div className={classes.Toolbar}>
      <MenuBurger clicked={props.clicked} />
      <div>{!isAuthenticated ? <LoginButton /> : <LogoutButton />}</div>
      <div className={attachedClasses.join(' ')} onClick={props.clicked}>
        <Navigation />
      </div>
    </div>
  );
};

export default SideDrawer;
