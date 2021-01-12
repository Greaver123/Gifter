import React from 'react';
import classes from './MenuBurger.module.css';

const MenuBurger = (props) => {
  return (
    <div className={classes.MenuBurger} onClick={props.clicked}>
      <div></div>
      <div></div>
      <div></div>
    </div>
  );
};

export default MenuBurger;
