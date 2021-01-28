import React, { Component } from 'react';
import classes from './EditWishlist.module.css';
import Wish from '../Common/Wish/Wish';
import Button from '../../../UI/Button/Button';
import Dropdown from '../../../UI/Inputs/Dropdown/Dropdown';
import Modal from '../../../UI/Modal/Modal';
import { withAuth0 } from '@auth0/auth0-react';
import { axiosDevInstance } from '../../../../axios/axios';
import LoadingIndicator from '../../../UI/LoadingIndicator/LoadingIndicator';

class EditWishlist extends Component {
  state = {
    id: this.props.match.params.id,
    title: '',
    wishes: [],
    giftGroups: [
      { id: 1, value: 'Christmas 2022' },
      { id: 2, value: 'Birthday' },
      { id: 3, value: 'Other' },
    ],
    selectedGiftGroupId: -1,
    showDeleteModal: false,
    loading: false,
  };

  removeWish = (e) => {
    const wishId = Number(e.target.parentElement.attributes['data-id'].value);
    let updatedWishes = [...this.state.wishes].filter(
      (wish) => wish.id !== wishId
    );
    this.setState({ wishes: updatedWishes });
  };

  addWish = () => {
    let updatedWishes = [...this.state.wishes];
    let newIndex =
      this.state.wishes.length === 0
        ? 1
        : this.state.wishes[this.state.wishes.length - 1].id + 1;
    updatedWishes.push({
      id: newIndex,
      name: '',
      link: '',
      price: '',
      isNew: true,
    });
    this.setState({ wishes: updatedWishes });
  };

  onInputChange = (e) => {
    let value = e.target.value;
    const wishId = Number(
      e.target.parentElement.parentElement.attributes['data-id'].value
    );
    let updatedWishes = [...this.state.wishes];
    const found = updatedWishes.find((wish) => wish.id === wishId);
    found[e.target.name] = value;
    this.setState({ wishes: updatedWishes });
  };

  deleteWishlist = () => {
    this.setState({ showDeleteModal: true });
  };

  cancelDelete = () => {
    this.setState({ showDeleteModal: false });
  };

  approveDelete = () => {
    this.setState({ showDeleteModal: false });
    this.props.history.push({ pathname: `/wishlists` });
  };

  cancelWishlist = () => {
    this.props.history.push({ pathname: `/wishlists` });
  };

  saveWishlist = async () => {
    this.props.history.push({ pathname: `/wishlists` });
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    axiosDevInstance
      .put(
        `/wishlist/${this.state.id}`,
        {
          id: this.state.id,
          title: this.state.title,
          wishes: this.state.wishes,
          giftgroupid: this.state.selectedGiftGroupId,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
      .then((reponse) => {
        console.log(reponse);
      })
      .catch((error) => {
        console.log('Could not save wishlist.', error);
      });
  };

  async componentDidMount() {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    this.setState({ loading: true });

    axiosDevInstance
      .get(`/wishlist/${this.state.id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        this.setState({ loading: false });
        this.setState({
          title: response.data.title,
          wishes: response.data.wishes?.map((w) => {
            w.isNew = false;
            return w;
          }),
        });
      })
      .catch((error) => {
        this.setState({ loading: false });
        console.log('Could not load data', error);
      });
  }

  render() {
    let wishes = this.state.wishes.map((wish, index) => {
      return (
        <Wish
          key={wish.id}
          id={wish.id}
          clicked={this.removeWish}
          name={wish.name}
          link={wish.link}
          price={wish.price}
          changed={this.onInputChange}
        />
      );
    });

    let editWishlistView = this.state.loading ? (
      <LoadingIndicator />
    ) : (
      <React.Fragment>
        <h3>{this.state.title}</h3>
        {wishes}
        <div className={classes.AddWishWrapper}>
          <Button type="Add" clicked={this.addWish}>
            Add
          </Button>
        </div>
        <div className={classes.GiftGroup}>
          <Dropdown items={this.state.giftGroups} />
        </div>
        <div className={classes.Buttons}>
          <Button type="Delete" clicked={this.deleteWishlist}>
            Delete
          </Button>
          <div>
            <Button type="Cancel" clicked={this.cancelWishlist}>
              Cancel
            </Button>
            <Button type="Save" clicked={this.saveWishlist}>
              Save
            </Button>
          </div>
          <Modal
            show={this.state.showDeleteModal}
            yesClicked={this.approveDelete}
            noClicked={this.cancelDelete}
          >
            <p>
              Are you sure you want to delete current wishlist? It can't be
              undone.
            </p>
          </Modal>
        </div>
      </React.Fragment>
    );

    return editWishlistView;
  }
}

export default withAuth0(EditWishlist);
