import React from 'react';
import classes from './Navigation.module.css';
import { NavLink } from 'react-router-dom';
import LoginButton from '../../../auth/LoginButton/LoginButton';
import LogoutButton from '../../../auth/LogoutButton/LogoutButton';
import { useAuth0 } from '@auth0/auth0-react';

const Navigation = () => {
  const { isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <h2 style={{ textAlign: 'center', margin: '20px' }}>Loading...</h2>;
  }

  return (
    <div className={classes.Navigation}>
      <div>
        <NavLink to="/" exact>
          Home
        </NavLink>
        <NavLink to="/fetch-data">Fetch Data</NavLink>
      </div>
      <div>{!isAuthenticated ? <LoginButton /> : <LogoutButton />}</div>
    </div>
  );
};

export default Navigation;
