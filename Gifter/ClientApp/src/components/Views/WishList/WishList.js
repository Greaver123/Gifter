import React, { Component } from 'react';
import classes from './WishList.module.css';
import CreateWishlist from './CreateWishlist/CreateWishlist';
import { Route } from 'react-router-dom';
import EditWishlist from './EditWishlist/EditWishlist';
import Button from '../../UI/Button/Button';

class WishList extends Component {
  state = {
    wishlists: [],
  };

  //TODO Validate Inputs
  cancelCreateWishlist = () => {
    console.log('CANCEL CREATE WISHLIST');
    this.props.history.goBack();
  };

  createWishList = () => {
    //Send PostRequst
    //create enttry in db
    //get response with Id
    //redirect to edit window
    let id = 1;
    this.props.history.push({ pathname: `/wishlist/edit/${id}` });
  };

  componentDidMount() {
    //1. Check if there are any wishlist.
    //  If exist
    //    Fetch wishlists and show wishlists
    //  else
    //    Show only create wishlist button
    console.log('[Wishlist] Component did mount');
  }

  render() {
    let createWishlistButton =
      this.props.location.pathname !== `/wishlist` ? null : (
        <Button
          type="Add"
          clicked={() => {
            this.props.history.push({
              pathname: `/wishlist/create`,
            });
          }}
        >
          Create
        </Button>
      );

    return (
      <div className={classes.Wishlist}>
        <h1>WishList</h1>
        {createWishlistButton}
        <Route path="/wishlist/create">
          <CreateWishlist
            cancel={this.cancelCreateWishlist}
            ok={this.createWishList}
          />
        </Route>
        <Route path="/wishlist/edit" component={EditWishlist} />
      </div>
    );
  }
}

export default WishList;
