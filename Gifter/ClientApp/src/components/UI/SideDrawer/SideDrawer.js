import React from 'react';
import MenuBurger from './MenuBurger/MenuBurger';
import classes from './SideDrawer.module.css';
import { NavLink } from 'react-router-dom';

const SideDrawer = (props) => {
  let attachedClasses = [classes.Closed];
  if (props.opened) {
    attachedClasses = [classes.Open];
  }

  return (
    <div className={classes.SideDrawer}>
      <MenuBurger clicked={props.clicked} />
      <div className={attachedClasses.join(' ')} onClick={props.clicked}>
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
    </div>
  );
};

export default SideDrawer;
