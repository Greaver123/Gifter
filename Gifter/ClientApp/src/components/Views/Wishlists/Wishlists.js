import React, { Component } from 'react';
import classes from './Wishlists.module.css';
import CreateWishlist from './CreateWishlist/CreateWishlist';
import { Route, Switch } from 'react-router-dom';
import Wishlist from './Wishlist/Wishlist';
import EditWishlist from './EditWishlist/EditWishlist';
import Button from '../../UI/Button/Button';
import WishlistElement from './WishlistElement/WishListElement';
import Modal from '../../UI/Modal/Modal';
import { withAuth0 } from '@auth0/auth0-react';
import { axiosDevInstance } from '../../../axios/axios';

class Wishlists extends Component {
  state = {
    wishlists: [],
    showDeleteModal: false,
    title: '',
    wishlistIdDelete: null,
  };

  cancelCreateWishlist = () => {
    this.props.history.goBack();
  };

  createWishList = async () => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    axiosDevInstance
      .post(
        '/wishlist',
        { title: this.state.title },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
      .then((response) => {
        const wishlistsUpdate = [...this.state.wishlists];
        wishlistsUpdate.push(response.data);
        this.setState({ wishlists: wishlistsUpdate });
        this.props.history.push({
          pathname: `${this.props.match.url}/edit/${response.data.id}`,
        });
      })
      .catch((error) => {
        console.log('Could not add new wishlist', error);
      });
  };

  showDeleteModal = (id) => {
    this.setState({ showDeleteModal: true, wishlistIdDelete: id });
  };

  cancelDelete = () => {
    this.setState({ showDeleteModal: false, wishlistIdDelete: null });
  };

  deleteWishlist = async () => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    axiosDevInstance
      .delete(`/wishlist/${this.state.wishlistIdDelete}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((reponse) => {
        let wishlistsUpdated = [...this.state.wishlists];

        wishlistsUpdated = wishlistsUpdated.filter(
          (w) => w.id !== this.state.wishlistIdDelete
        );
        this.setState({
          wishlists: wishlistsUpdated,
          wishlistIdDelete: null,
          showDeleteModal: false,
        });
      })
      .catch((error) => {
        console.log('Could not delete wishlist', error);
      });
  };

  showEdit = (id) => {
    this.props.history.push({ pathname: `${this.props.match.url}/edit/${id}` });
  };

  showView = (id) => {
    this.props.history.push({ pathname: `${this.props.match.url}/view/${id}` });
  };

  titleChanged = (e) => {
    const title = e.target.value;

    this.setState({ title: title });
  };

  async componentDidMount() {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    axiosDevInstance
      .get(`/wishlist`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        this.setState({ wishlists: response.data });
      })
      .catch((error) => {
        console.log(error);
      });

    console.log('[Wishlist] Component did mount');
  }

  render() {
    let wishlistsView = null;

    let wishlists = [];
    if (
      this.props.location.pathname === `/wishlists` &&
      this.state.wishlists.length > 0
    ) {
      wishlists = this.state.wishlists.map((wishlist) => {
        return (
          <WishlistElement
            key={wishlist.id}
            id={wishlist.id}
            title={wishlist.title}
            assigned={wishlist.assigned}
            viewClicked={this.showView.bind(this, wishlist.id)}
            deleteClicked={this.showDeleteModal.bind(this, wishlist.id)}
            editClicked={this.showEdit.bind(this, wishlist.id)}
          />
        );
      });
    }

    let createWishlistButton = null;

    if (this.props.location.pathname === `/wishlists`) {
      createWishlistButton = (
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
    }

    wishlistsView = (
      <React.Fragment>
        <div>{wishlists}</div>
        {createWishlistButton}
      </React.Fragment>
    );

    return (
      <div className={classes.Wishlists}>
        <h1>WishList</h1>
        {wishlistsView}
        <Switch>
          <Route
            path={`${this.props.match.url}/view/:id`}
            component={Wishlist}
          />
          <Route path={`${this.props.match.url}/create`}>
            <CreateWishlist
              cancel={this.cancelCreateWishlist}
              ok={this.createWishList}
              titleChanged={this.titleChanged}
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
            yesClicked={this.deleteWishlist}
            noClicked={this.cancelDelete}
          >
            <p>Are you sure you want to delete wishlist? It can't be undone.</p>
          </Modal>
        }
      </div>
    );
  }
}

export default withAuth0(Wishlists);
