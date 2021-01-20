import React, { Component } from 'react';
import classes from './WishList.module.css';
import CreateWishlist from './CreateWishlist/CreateWishlist';
import { Route } from 'react-router-dom';
import EditWishlist from './EditWishlist/EditWishlist';
import Button from '../../UI/Button/Button';
import WishlistElement from './WishlistElement/WishListElement';
import Modal from '../../UI/Modal/Modal';

class WishList extends Component {
  state = {
    wishlists: [
      { id: 1, title: 'My Wishlist 1', assigned: false },
      { id: 2, title: 'My Wishlist 2', assigned: true },
      { id: 3, title: 'My Wishlist 3', assigned: false },
    ],
    showDeleteModal: false,
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

  showDeleteModal = () => {
    this.setState({ showDeleteModal: true });
  };

  cancelDelete = () => {
    this.setState({ showDeleteModal: false });
  };

  approveDelete = () => {
    this.setState({ showDeleteModal: false });
    // this.props.history.push({ pathname: `/wishlist` });
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

    let wishlists = this.state.wishlists.map((wishlist) => {
      return (
        <WishlistElement
          key={wishlist.id}
          id={wishlist.id}
          title={wishlist.title}
          assigned={wishlist.assigned}
          deleteClicked={this.showDeleteModal}
        />
      );
    });

    return (
      <div className={classes.Wishlist}>
        <h1>WishList</h1>
        <div>{wishlists}</div>
        {createWishlistButton}
        <Route path="/wishlist/create">
          <CreateWishlist
            cancel={this.cancelCreateWishlist}
            ok={this.createWishList}
          />
        </Route>
        <Route path="/wishlist/edit" component={EditWishlist} />
        {
          <Modal
            show={this.state.showDeleteModal}
            yesClicked={this.approveDelete}
            noClicked={this.cancelDelete}
          >
            <p>Are you sure you want to delete wishlist? It can't be undone.</p>
          </Modal>
        }
      </div>
    );
  }
}

export default WishList;
