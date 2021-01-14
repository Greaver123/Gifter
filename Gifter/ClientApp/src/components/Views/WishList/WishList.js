import React, { Component } from 'react';
import classes from './WishList.module.css';
import CreateWishlist from './CreateWishlist/CreateWishlist';
import { Route } from 'react-router-dom';
import EditWishlist from './EditWishlist/EditWishlist';
class WishList extends Component {
  state = {
    wishlists: [],
    createWishlist: false,
    editWishlist: false,
  };

  //TODO Validate Inputs

  showCreateWishlist = () => {
    console.log('SHOW CREATE WISHLIST');
    this.setState({ createWishlist: true });
  };
  cancelCreateWishlist = () => {
    console.log('CANCEL CREATE WISHLIST');
    this.setState({ createWishlist: false });
  };

  showEditWishlist = () => {
    console.log('SHOW EDIT WISHLIST');
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
    let createWishlist = null;

    if (this.state.createWishlist) {
      createWishlist = (
        <CreateWishlist
          cancel={this.cancelCreateWishlist}
          ok={this.showEditWishlist}
        />
      );
    }
    return (
      <div>
        <h1>WishList</h1>
        {createWishlist}
        <button onClick={this.showCreateWishlist}>Create WishList</button>
        <Route path="/wishlist/edit" component={EditWishlist} />
      </div>
    );
  }
}

export default WishList;
