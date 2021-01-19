import React, { Component } from 'react';
import classes from './EditWishlist.module.css';
import Wish from './Wish/Wish';
import Button from '../../../UI/Button/Button';
import Dropdown from '../../../UI/Dropdown/Dropdown';
import Modal from '../../../UI/Modal/Modal';

class EditWishlist extends Component {
  state = {
    title: 'My Wishist',
    wishes: [{ id: 1, name: 'Razer', link: 'www.razer.com', price: 299 }],
    giftGroups: [
      { id: 1, value: 'Christmas 2022' },
      { id: 2, value: 'Birthday' },
      { id: 3, value: 'Other' },
    ],
    showDeleteModal: false,
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
    this.props.history.push({ pathname: `/wishlist` });
  };

  cancelWishlist = () => {
    console.log('CANCEL WISHLIST');
  };

  saveWishlist = () => {
    console.log('SAVE WISHLIST');
  };

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

    return (
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
  }
}

export default EditWishlist;