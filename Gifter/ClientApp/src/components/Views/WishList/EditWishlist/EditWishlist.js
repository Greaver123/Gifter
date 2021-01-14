import React, { Component } from 'react';
import classes from './EditWishlist.module.css';
import Wish from './Wish/Wish';

class EditWishlist extends Component {
  state = {
    title: 'My Wishist',
    wishes: [{ name: 'Razer', link: 'www.razer.com', price: 299 }],
  };

  removeWish = () => {
    console.log('Remove wish');
  };

  addWish = () => {
    console.log('Add Wish');
    let updatedWishes = [...this.state.wishes];
    updatedWishes.push({ name: 'Test', link: 'Test', price: 299 });
    this.setState({ wishes: updatedWishes });
  };

  render() {
    let wishes = this.state.wishes.map((wish, index) => {
      return <Wish key={index} clicked={this.removeWish} />;
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
