import React from 'react';
// import classes from './CreateWishlist.module.css';

const CreateWishlist = (props) => {
  return (
    <div>
      <input type="text" placeholder="Enter Title" />
      <div>
        <button onClick={props.cancel}>Cancel</button>
        <button onClick={props.ok}>Ok</button>
      </div>
    </div>
  );
};

export default CreateWishlist;
