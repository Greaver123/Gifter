import React, { Component } from 'react';
import classes from './EditWishlist.module.css';
import Wish from './Wish/Wish';

class EditWishlist extends Component {
  state = {
    title: 'My Wishist',
    wishes: [{ id: 1, name: 'Razer', link: 'www.razer.com', price: 299 }],
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
      this.state.wishes.length == 0
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
      <div className={classes.Wish}>
        <h3>{this.state.title}</h3>
        {wishes}
        <div className={classes.AddWishWrapper}>
          <button className={classes.AddWishButton} onClick={this.addWish}>
            Add
          </button>
        </div>
      </div>
    );
  }
}

export default EditWishlist;
