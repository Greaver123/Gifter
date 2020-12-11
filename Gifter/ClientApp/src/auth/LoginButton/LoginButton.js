import React from 'react';
import classes from './LoginButton.module.css';

import { useAuth0 } from '@auth0/auth0-react';
const LoginButton = () => {
  const { loginWithRedirect } = useAuth0();

  return (
    <button className={classes.LoginButton} onClick={() => loginWithRedirect()}>
      Log in
    </button>
  );
};

export default LoginButton;
