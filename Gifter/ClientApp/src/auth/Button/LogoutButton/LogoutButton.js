import React from 'react';
import classes from './LogoutButton.module.css';
import baseClasses from '../AuthButton.module.css';
import { useAuth0 } from '@auth0/auth0-react';

const LogoutButton = () => {
  const { logout } = useAuth0();

  return (
    <button
      className={[baseClasses.AuthButton, classes.LogoutButton].join(' ')}
      onClick={() => logout({ returnTo: window.location.origin })}
    >
      Logout
    </button>
  );
};

export default LogoutButton;
