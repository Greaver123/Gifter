import React, { Component } from 'react';
import { Container } from 'reactstrap';
import SideDrawer from '../SideDrawer/SideDrawer';
import { withAuth0 } from '@auth0/auth0-react';

class Layout extends Component {
  state = {
    sideDrawerOpened: false,
  };

  SideDrawerClicked = () => {
    this.setState((prevState) => {
      return { sideDrawerOpened: !prevState.sideDrawerOpened };
    });
  };

  render() {
    const { isAuthenticated, isLoading } = this.props.auth0;

    if (isLoading) {
      return (
        <h2 style={{ textAlign: 'center', margin: '20px' }}>Loading...</h2>
      );
    }

    return (
      <div>
        <SideDrawer
          opened={this.state.sideDrawerOpened}
          clicked={this.SideDrawerClicked}
        ></SideDrawer>
        <Container>{this.props.children}</Container>
      </div>
    );
  }
}

export default withAuth0(Layout);
