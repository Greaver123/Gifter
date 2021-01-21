import React, { Component } from 'react';
import classes from './Wishlists.module.css';
import CreateWishlist from './CreateWishlist/CreateWishlist';
import { Route, Switch } from 'react-router-dom';
import EditWishlist from './EditWishlist/EditWishlist';
import Button from '../../UI/Button/Button';
import WishlistElement from './WishlistElement/WishListElement';
import Modal from '../../UI/Modal/Modal';

class Wishlist extends Component {
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
    console.log(this.props);
    this.props.history.push({ pathname: `${this.props.match.url}/edit/${id}` });
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

  showEdit = (id) => {
    this.props.history.push({ pathname: `${this.props.match.url}/edit/${id}` });
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
    let wishlistsView = null;

    if (this.props.location.pathname === `/wishlists`) {
      let wishlists = this.state.wishlists.map((wishlist) => {
        return (
          <WishlistElement
            key={wishlist.id}
            id={wishlist.id}
            title={wishlist.title}
            assigned={wishlist.assigned}
            deleteClicked={this.showDeleteModal}
            editClicked={this.showEdit.bind(this, wishlist.id)}
          />
        );
      });

      const createWishlistButton = (
        <Button
          type="Add"
          clicked={() => {
            this.props.history.push({
              pathname: `${this.props.match.url}/create`,
            });
          }}
        >
          Create
        </Button>
      );

      wishlistsView = (
        <React.Fragment>
          <div>{wishlists}</div>
          {createWishlistButton}
        </React.Fragment>
      );
    }

    return (
      <div className={classes.Wishlist}>
        <h1>WishList</h1>
        {wishlistsView}
        <Switch>
          <Route path={`${this.props.match.url}/create`}>
            <CreateWishlist
              cancel={this.cancelCreateWishlist}
              ok={this.createWishList}
            />
          </Route>
          <Route
            path={`${this.props.match.url}/edit/:id`}
            component={EditWishlist}
          />
        </Switch>

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

export default Wishlist;
