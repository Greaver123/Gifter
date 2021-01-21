import React, { Component } from 'react';
import classes from './Wishlist.module.css';
import Button from '../../../UI/Button/Button';
import Wish from '../../../Views/Wishlists/Common/Wish/Wish';
class Wishlist extends Component {
  state = {
    title: 'My Wishist',
    wishes: [
      { id: 1, name: 'Mouse', link: 'www.razer.com', price: 299 },
      { id: 2, name: 'Keyboard', link: 'www.razer.com', price: 200 },
    ],
    event: {
      id: 1,
      date: '07.01.21',
      giftgroup: {
        id: 1,
        name: 'My Birthday',
      },
    },
  };
  //TODO fetch id
  backToWishlists = () => {
    this.props.history.goBack();
  };

  showEditWishlist = () => {
    this.props.history.push({
      pathname: `/wishlists/edit/${this.props.match.params.id}`,
    });
  };

  render() {
    let wishes = this.state.wishes.map((wish) => {
      return (
        <Wish
          key={wish.id}
          id={wish.id}
          name={wish.name}
          link={wish.link}
          price={wish.price}
          displayOnly
        />
      );
    });

    return (
      <React.Fragment>
        <h3>{this.state.title}</h3>
        {wishes}
        <div className={classes.GiftGroup}>
          <div>Assigned to</div>
          <div className={classes.EventWrapper}>
            <div className={classes.Event}>
              <p>Event</p>
              <p>{this.state.event.giftgroup.name}</p>
            </div>
            <div className={classes.Date}>
              <p>Date</p>
              <p>{this.state.event.date}</p>
            </div>
          </div>
        </div>
        <div className={classes.Buttons}>
          <Button type="Edit" clicked={this.showEditWishlist}>
            Edit
          </Button>
          <Button type="Cancel" clicked={this.backToWishlists}>
            Back
          </Button>
        </div>
      </React.Fragment>
    );
  }
}

export default Wishlist;
