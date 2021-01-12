import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { withAuth0 } from '@auth0/auth0-react';
import SideDrawer from '../SideDrawer/SideDrawer';
import Toolbar from '../Toolbar/Toolbar';

class Layout extends Component {
  render() {
    const { isLoading } = this.props.auth0;

    if (isLoading) {
      return (
        <h2 style={{ textAlign: 'center', margin: '20px' }}>Loading...</h2>
      );
    }

    return (
      <div>
        <Toolbar />
        <Container>{this.props.children}</Container>
      </div>
    );
  }
}

export default withAuth0(Layout);
