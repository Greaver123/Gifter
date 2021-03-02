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
        // let newIndex = updatedWishes.length
        //   ? Math.max(...updatedWishes.map((w) => w.id)) + 1
        //   : 1;
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

  uploadImage = async (wishId, image) => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    let formData = new FormData();
    formData.append('ImageFile', image);
    formData.append('wishId', wishId);

    axiosDevInstance
      .post('/image/upload', formData, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        switch (response.data.status) {
          case apiStatusCodes.SUCCESS:
            break;
          case apiStatusCodes.FAIL:
          case apiStatusCodes.ERROR:
            console.log(response.data.data.message);
           break;
        }
      })
      .catch((error) => {
        console.log('Could not upload image', error);
      });
  };

  getImages = async () => {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    this.state.wishes.forEach((wish) => {
      if (wish.imageId !== null) {
        axiosDevInstance
          .get(`/image/${wish.imageId}`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          })
          .then((response) => {
            switch (response.data.status) {
              case apiStatusCodes.SUCCESS:
                const updatedWishes = [...this.state.wishes];
                const wishes = updatedWishes.map((w) => {
                  if (w.id == wish.id) {
                    w.image = response.data.data.image;
                  }
                  return w;
                });
                this.setState({ wishes: wishes });
                break;
              case apiStatusCodes.FAIL:
              case apiStatusCodes.ERROR:
                console.log(response.data.message);
                break;
            }
          })
          .catch((error) => {
            //TODO
            console.log(error);
          });
      }
    });
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

    console.log('[EditWishList] deleteImage()');
    axiosDevInstance
      .delete(`/image/${imageId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        switch (response.data.status) {
          case apiStatusCodes.SUCCESS:
            const updatedWishes = [...this.state.wishes];
            const imageToDeleteId = updatedWishes.findIndex(
              (w) => w.imageId == imageId
            );

            updatedWishes[imageToDeleteId].image = null;
            this.setState({ wishes: updatedWishes });
            break;
          case apiStatusCodes.FAIL:
          case apiStatusCodes.ERROR:
            console.log(response.data.data.message);
            break;
        }
      })
      .catch((error) => {
        console.log('Could not upload image', error);
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
          title: response.data.data.title,
          wishes: response.data.data.wishes?.map((w) => {
            w.name = w.name ?? '';
            w.link = w.link ?? '';
            w.price = w.price ?? '';
            w.isNew = false;
            return w;
          }),
        });

        this.getImages();
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
          image={wish.image}
          changed={this.onInputChange}
          uploadImage={this.uploadImage}
          deleteImage={this.deleteImage.bind(this, wish.imageId)}
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
