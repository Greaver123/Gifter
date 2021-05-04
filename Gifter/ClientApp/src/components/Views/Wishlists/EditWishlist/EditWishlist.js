import React, { Component } from 'react';
import classes from './EditWishlist.module.css';
import Wish from '../Common/Wish/Wish';
import Button from '../../../UI/Button/Button';
import Dropdown from '../../../UI/Inputs/Dropdown/Dropdown';
import Modal from '../../../UI/Modal/Modal';
import { withAuth0 } from '@auth0/auth0-react';
import { axiosDevInstance } from '../../../../axios/axios';
import LoadingIndicator from '../../../UI/LoadingIndicator/LoadingIndicator';
import { apiStatusCodes } from '../../../../api/constants';
import { cloneDeep } from 'lodash';

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
    isFetchingWishlist: false,
    isAddingWish: false,
  };

  deleteWish = async (wishId) => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    try {
      const response = await axiosDevInstance.delete(`wish/${wishId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      let updatedWishes = cloneDeep(this.state.wishes).filter(
        (wish) => wish.id !== wishId
      );

      this.setState({ wishes: updatedWishes });
    } catch (error) {
      alert('Could not remove Wish. Try again.');
    }
  };

  addWish = async () => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();
    this.setState({ isAddingWish: true });
    try {
      const response = await axiosDevInstance.post(
        `wish`,
        {
          wishlistId: this.state.id,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      let updatedWishes = cloneDeep(this.state.wishes);
      updatedWishes.push({
        id: response.data.data.id,
        name: '',
        link: null,
        price: '',
        isNew: true,
      });

      this.setState({ wishes: updatedWishes });
    } catch (error) {
      alert('Could not add Wish. Please try again.');
    } finally {
      this.setState({ isAddingWish: false });
    }
  };

  onInputChange = (e) => {
    let value = e.target.value;
    const wishId = Number(e.target.closest('div[data-id]').dataset.id);
    let updatedWishes = cloneDeep(this.state.wishes);
    const found = updatedWishes.find((wish) => wish.id === wishId);
    found[e.target.name] = value;
    this.setState({ wishes: updatedWishes });
  };

  showDeleteWishlistModal = () => {
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

  uploadImage = async (wishId, image) => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    let formData = new FormData();
    formData.append('ImageFile', image);
    formData.append('wishId', wishId);

    try {
      const response = await axiosDevInstance.post('/image/upload', formData, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.data.status !== apiStatusCodes.SUCCESS) {
        alert(response.data.data.message);
      }

      const wishes = cloneDeep(this.state.wishes);
      const wishToUpdateId = wishes.findIndex((w) => w.id === wishId);

      wishes[wishToUpdateId].imageId = response.data.data.id;
      wishes[wishToUpdateId].image = URL.createObjectURL(image);
      this.setState({
        wishes: wishes,
      });
      return image;
    } catch (error) {
      console.error(error.message);
      alert(error);
      throw error;
    }
  };

  fetchImage = async (imageId) => {
    try {
      const { getAccessTokenSilently } = this.props.auth0;
      const token = await getAccessTokenSilently();

      let response = await axiosDevInstance.get(`/image/${imageId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      // if (random(1, 2) == 1) throw new Error('Error fetching image');
      return response.data.data.image;
    } catch (error) {
      console.error(error.message);
      alert('Could not fetch image.');
      throw error;
    }
  };

  fetchImages = async () => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();
    const wishesUpdate = cloneDeep(this.state.wishes);

    wishesUpdate.map(async (wish) => {
      if (wish.imageId !== null) {
        let wishesUpdateLoading = cloneDeep(this.state.wishes);
        const wishIndex = wishesUpdateLoading.findIndex(
          (w) => w.id === wish.id
        );

        wishesUpdateLoading[wishIndex].isLoadingImage = true;
        this.setState({ wishes: wishesUpdateLoading });
        const image = await this.fetchImage(token, wish.imageId);

        wishesUpdateLoading = cloneDeep(this.state.wishes);
        wishesUpdateLoading[wishIndex].isLoadingImage = false;
        wishesUpdateLoading[wishIndex].image = image;
        this.setState({ wishes: wishesUpdateLoading });
      }
    });
  };

  deleteImage = async (imageId) => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    try {
      let response = await axiosDevInstance.delete(`/image/${imageId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      const updatedWishes = cloneDeep(this.state.wishes);
      const imageToDeleteId = updatedWishes.findIndex(
        (w) => w.imageId === imageId
      );

      updatedWishes[imageToDeleteId].image = null;
      this.setState({ wishes: updatedWishes });
    } catch (error) {
      console.error(error.message);
      alert('Could not delete Image. Please try again.');
      throw error;
    }
  };

  deleteLink = async (wishId) => {
    const updatedWishes = cloneDeep(this.state.wishes);
    let index = updatedWishes.findIndex((w) => w.id === wishId);
    updatedWishes[index].link = null;
    this.setState(updatedWishes);
  };

  fetchWishlist = async (id) => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    let wishlist = null;

    try {
      const response = await axiosDevInstance.get(`/wishlist/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status != 200) alert(response.data.data.message);

      wishlist = response.data.data;
    } catch (error) {
      console.error(error);
      alert('Could not fetch wishlist. Please try again.');
    } finally {
      return wishlist;
    }
  };

  patchWish = async (id, property, patchOperation) => {
    try {
      const { getAccessTokenSilently } = this.props.auth0;
      const token = await getAccessTokenSilently();

      let wish = this.state.wishes.find((w) => w.id === id);
      if (!wish) return;

      let response = await axiosDevInstance.patch(
        `wish/${id}`,
        [patchOperation],
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (patchOperation.op == 'remove') {
        // clean state from removed value
        const updatedWishes = cloneDeep(this.state.wishes);
        updatedWishes.find((w) => w.id == id)[property] = '';
        this.setState({ wishes: updatedWishes });
      }

      return response;
    } catch (err) {
      throw err;
    }
  };

  saveWish = async (id) => {
    console.log('SAVE WISH');
    try {
      const { getAccessTokenSilently } = this.props.auth0;
      const token = await getAccessTokenSilently();

      let wish = this.state.wishes.find((w) => w.id === id);
      if (!wish) return;
      console.log(wish);
      let response = await axiosDevInstance.put(
        'wish/',
        {
          id: id,
          name: wish.name,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      return response;
    } catch (err) {
      throw err;
    }
  };

  getWishes = () => {
    return this.state.wishes.map((wish) => {
      return (
        <Wish
          key={wish.id}
          {...wish}
          saveWish={this.saveWish.bind(this, wish.id)}
          patchWish={this.patchWish.bind(this)}
          deleteWish={this.deleteWish.bind(this, wish.id)}
          changed={this.onInputChange}
          fetchImage={this.fetchImage.bind(this, wish.imageId)}
          uploadImage={this.uploadImage}
          deleteImage={this.deleteImage.bind(this, wish.imageId)}
          onDeleteLink={this.deleteLink.bind(this, wish.id)}
        />
      );
    });
  };

  async componentDidMount() {
    this.setState({ isFetchingWishlist: true });

    let wishlist = await this.fetchWishlist(this.state.id);
    if (!wishlist) return;

    this.setState({
      isFetchingWishlist: false,
      title: wishlist.title,
      wishes: wishlist.wishes?.map((w) => {
        w.name = w.name ?? '';
        w.link = w.link;
        w.price = w.price ?? '';
        w.isNew = false;
        return w;
      }),
    });

    // await this.fetchImages();
  }

  render() {
    let wishes = this.getWishes();
    let editWishlistView = this.state.isFetchingWishlist ? (
      ''
    ) : (
      <React.Fragment>
        <h3>{this.state.title}</h3>
        {wishes}
        <div className={classes.AddWishWrapper}>
          <Button
            type="Add"
            clicked={this.addWish}
            showSpinner={this.state.isAddingWish}
          >
            Add
          </Button>
        </div>
        <div className={classes.GiftGroup}>
          {/* <Dropdown items={this.state.giftGroups} /> */}
        </div>
        <div className={classes.Buttons}>
          <Button type="Delete" clicked={this.showDeleteWishlistModal}>
            Delete
          </Button>
          <div>
            <Button type="Cancel" clicked={this.cancelWishlist}>
              Back
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
