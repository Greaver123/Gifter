import React from 'react';
import classes from './LoginButton.module.css';
import baseClasses from '../AuthButton.module.css';
import { useAuth0 } from '@auth0/auth0-react';

const LoginButton = () => {
  const { loginWithRedirect } = useAuth0();

  return (
    <button
      className={[baseClasses.AuthButton, classes.LoginButton].join(' ')}
      onClick={() => loginWithRedirect()}
    >
      Log in
    </button>
  );
};

export default LoginButton;
