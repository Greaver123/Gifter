import React from 'react';
import classes from './Navigation.module.css';
import { NavLink } from 'react-router-dom';
// import { useAuth0 } from '@auth0/auth0-react';

const Navigation = () => {
  return (
    <div className={classes.Navigation}>
      <NavLink to="/" exact>
        Home
      </NavLink>
      <NavLink to="/eventcalendar">Event Calendar</NavLink>
      <NavLink to="/giftgroups">Gift Groups</NavLink>
      <NavLink to="/wishlist">WishList</NavLink>
      <NavLink to="/mygifts">My gifts</NavLink>
      <NavLink to="/myideas">My Ideas</NavLink>
      <NavLink to="/profile">Profile</NavLink>
    </div>
  );
};

export default Navigation;
