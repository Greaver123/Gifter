import React, { Component } from 'react';
import classes from './Wishlist.module.css';
import Button from '../../../UI/Button/Button';
import Wish from '../../../Views/Wishlists/Common/Wish/Wish';
import { withAuth0 } from '@auth0/auth0-react';
import { axiosDevInstance } from '../../../../axios/axios';

class Wishlist extends Component {
  state = {
    title: 'My Wishist',
    wishes: [],
    giftGroup: {
      name: '',
      date: '',
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

  async componentDidMount() {
    const { getAccessTokenSilently } = this.props.auth0;
    const token = await getAccessTokenSilently();

    axiosDevInstance
      .get(`/wishlist/${this.props.match.params.id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        this.setState({
          title: response.data.title,
          wishes: response.data.wishes,
          giftGroup: {
            name: response.data.giftGroupName ?? 'None',
            date: response.data.eventDate ?? 'None',
          },
        });
      })
      .catch((error) => {
        console.log('Could not fetch wishlist', error);
      });
  }

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
              <p>{this.state.giftGroup.name}</p>
            </div>
            <div className={classes.Date}>
              <p>Date</p>
              <p>{this.state.giftGroup.date}</p>
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

export default withAuth0(Wishlist);
