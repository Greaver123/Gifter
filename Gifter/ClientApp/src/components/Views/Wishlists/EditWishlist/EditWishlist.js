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

  removeWish = async (e) => {
    const wishId = Number(e.target.parentElement.attributes['data-id'].value);
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    axiosDevInstance
      .delete(`wish/${wishId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        if (response.status === 200) {
          let updatedWishes = [...this.state.wishes].filter(
            (wish) => wish.id !== wishId
          );
          this.setState({ wishes: updatedWishes });
        } else if (response.status === 404) {
          //Show message
        }
      })
      .catch((error) => console.error(error));
  };

  getLastIndex = (wishes) => {
    if (wishes === null || wishes.length == 0) return 1;

    let lastIndex = 0;
    for (const wish of wishes) {
      if (wish.id > lastIndex) {
        lastIndex = wish.id;
      }
    }
    return lastIndex;
  };

  addWish = async () => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    axiosDevInstance
      .post(
        `wish`,
        {
          wishlistId: this.state.id,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
      .then((response) => {
        let updatedWishes = [...this.state.wishes];
        updatedWishes.push({
          id: response.data.data.id,
          name: '',
          link: '',
          price: '',
          isNew: true,
        });
        this.setState({ wishes: updatedWishes });
      })
      .catch((error) => {
        console.error(error);
        //TODO Show message
      });
  };

  onInputChange = (e) => {
    console.log(e.target.value);
    console.log(e.target.name);
    let value = e.target.value;
    const wishId = Number(e.target.closest('div[data-id]').dataset.id);
    let updatedWishes = [...this.state.wishes];
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

    let result = false;

    try {
      const response = await axiosDevInstance.post('/image/upload', formData, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.data.status !== apiStatusCodes.SUCCESS) {
        alert(response.data.data.message);
      }

      const wishes = [...this.state.wishes];
      const wishToUpdateId = wishes.findIndex((w) => w.id === wishId);

      wishes[wishToUpdateId].imageId = response.data.data.id;
      wishes[wishToUpdateId].image = URL.createObjectURL(image);
      this.setState({
        wishes: wishes,
      });

      result = true;
    } catch (error) {
      alert(error);
    } finally {
      return result;
    }
  };

  fetchImage = async (token, imageId) => {
    try {
      let response = await axiosDevInstance.get(`/image/${imageId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.data.status !== apiStatusCodes.SUCCESS) {
        alert('Something went wrong retriving images');
        return;
      }

      return response.data.data.image;
    } catch (error) {
      alert(error);
    }
  };

  fetchImages = async () => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    const wishesUpdate = [...this.state.wishes];

    await Promise.all(
      wishesUpdate.map(async (wish) => {
        if (wish.imageId !== null)
          wish.image = await this.fetchImage(token, wish.imageId);
      })
    );

    this.setState({ wishes: wishesUpdate });
  };

  deleteLink = async (wishId) => {
    console.log('DELETE LINK', wishId);
    const updatedWishes = [...this.state.wishes];
    let index = updatedWishes.findIndex((w) => w.id === wishId);
    updatedWishes[index].link = '';
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

      if (!apiStatusCodes.SUCCESS) alert('Something went wrong');

      wishlist = response.data.data;
    } catch (error) {
      console.log(error);
    } finally {
      return wishlist;
    }
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

  deleteImage = async (imageId) => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    let isSuccess = false;

    try {
      let response = await axiosDevInstance.delete(`/image/${imageId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.data.status === apiStatusCodes.SUCCESS) {
        const updatedWishes = [...this.state.wishes];
        const imageToDeleteId = updatedWishes.findIndex(
          (w) => w.imageId == imageId
        );

        updatedWishes[imageToDeleteId].image = null;
        this.setState({ wishes: updatedWishes });
        isSuccess = true;
      }
    } catch (error) {
      alert('Something went wrong.');
    } finally {
      return isSuccess;
    }
  };

  async componentDidMount() {
    console.log('[EditWishlist] COMPONENT_DID_MOUNT');
    this.setState({ loading: true });

    let wishlist = await this.fetchWishlist(this.state.id);

    if (!wishlist) alert('Could not fetch Wishlist');

    this.setState({
      loading: false,
      title: wishlist.title,
      wishes: wishlist.wishes?.map((w) => {
        w.name = w.name ?? '';
        w.link = w.link ?? null;
        w.price = w.price ?? '';
        w.isNew = false;
        return w;
      }),
    });

    await this.fetchImages();
  }

  render() {
    console.log('[EditWishlist] RENDER');
    let wishes = this.state.wishes.map((wish, index) => {
      return (
        <Wish
          key={wish.id}
          id={wish.id}
          clicked={this.removeWish}
          name={wish.name}
          link={wish.link}
          price={wish.price}
          image={wish.image}
          changed={this.onInputChange}
          uploadImage={this.uploadImage}
          deleteImage={this.deleteImage.bind(this, wish.imageId)}
          onDeleteLink={this.deleteLink.bind(this, wish.id)}
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
          <Button type="Delete" clicked={this.showDeleteWishlistModal}>
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
