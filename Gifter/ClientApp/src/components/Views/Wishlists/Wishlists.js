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
import LoadingIndicator from '../../UI/LoadingIndicator/LoadingIndicator';
import { cloneDeep } from 'lodash';

class Wishlists extends Component {
  state = {
    wishlists: [],
    showDeleteModal: false,
    title: '',
    wishlistIdDelete: null,
    loading: false,
  };

  cancelCreateWishlist = () => {
    this.props.history.goBack();
  };

  createWishList = async () => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    try {
      let response = await axiosDevInstance.post(
        '/wishlist',
        { title: this.state.title },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      const wishlistsUpdate = cloneDeep(this.state.wishlists);
      wishlistsUpdate.push(response.data.data);
      this.setState({ wishlists: wishlistsUpdate });
      this.props.history.push({
        pathname: `${this.props.match.url}/edit/${response.data.data.id}`,
      });
    } catch (err) {
      console.error(err);
      alert(
        'Something went wrong. Could not create wishlist. Please try again.'
      );
    }
  };

  deleteWishlist = async () => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    try {
      await axiosDevInstance.delete(
        `/wishlist/${this.state.wishlistIdDelete}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      let wishlistsUpdated = cloneDeep(this.state.wishlists);

      wishlistsUpdated = wishlistsUpdated.filter(
        (w) => w.id !== this.state.wishlistIdDelete
      );

      this.setState({
        wishlists: wishlistsUpdated,
        wishlistIdDelete: null,
        showDeleteModal: false,
      });
    } catch (err) {
      console.error(err);
      alert(
        'Something went wrong. Could not delete wishlist. Please try again.'
      );
    }
  };

  showDeleteModal = (id) => {
    this.setState({ showDeleteModal: true, wishlistIdDelete: id });
  };

  cancelDelete = () => {
    this.setState({ showDeleteModal: false, wishlistIdDelete: null });
  };

  showEdit = (id) => {
    this.props.history.push({ pathname: `${this.props.match.url}/edit/${id}` });
  };

  showView = (id) => {
    this.props.history.push({ pathname: `${this.props.match.url}/view/${id}` });
  };

  titleChanged = (e) => {
    this.setState({ title: e.target.value });
  };

  getWishlistsElements = () => {
    return this.state.wishlists.map((wishlist) => (
      <WishlistElement
        key={wishlist.id}
        id={wishlist.id}
        title={wishlist.title}
        assigned={wishlist.assigned}
        viewClicked={this.showView.bind(this, wishlist.id)}
        deleteClicked={this.showDeleteModal.bind(this, wishlist.id)}
        editClicked={this.showEdit.bind(this, wishlist.id)}
      />
    ));
  };

  async componentDidMount() {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();
    this.setState({ loading: true });

    try {
      let response = await axiosDevInstance.get(`/wishlist`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      this.setState({ wishlists: response.data.data });
    } catch (err) {
      console.error(err);
      alert(
        'Someting went wrong. Could not load wishlists. Please try again later.'
      );
    } finally {
      this.setState({ loading: false });
    }
  }

  render() {
    const wishlistsView = (
      <React.Fragment>
        {this.state.loading ? (
          <LoadingIndicator />
        ) : (
          <div>{this.getWishlistsElements()}</div>
        )}
        {
          <Button
            className={classes.CreateWishlistBtn}
            type="Add"
            clicked={() => {
              this.props.history.push({
                pathname: `${this.props.match.url}/create`,
              });
            }}
          >
            Create
          </Button>
        }
        <Modal
          show={this.state.showDeleteModal}
          yesClicked={this.deleteWishlist}
          noClicked={this.cancelDelete}
        >
          <p>Are you sure you want to delete wishlist? It can't be undone.</p>
        </Modal>
      </React.Fragment>
    );

    // console.log('[Wishlists] render');
    return (
      <div className={classes.Wishlists}>
        <h1>WishList</h1>
        <Switch>
          <Route
            exact
            path={`${this.props.match.path}`}
            render={() => wishlistsView}
          />
          <Route
            path={`${this.props.match.path}/view/:id`}
            component={Wishlist}
          />
          <Route path={`${this.props.match.path}/create`}>
            <CreateWishlist
              cancel={this.cancelCreateWishlist}
              ok={this.createWishList}
              titleChanged={this.titleChanged}
            />
          </Route>
          <Route
            path={`${this.props.match.path}/edit/:id`}
            component={EditWishlist}
          />
        </Switch>
      </div>
    );
  }
}

export default withAuth0(Wishlists);
