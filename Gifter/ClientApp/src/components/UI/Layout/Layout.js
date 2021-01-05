import React, { Component } from 'react';
import { Container } from 'reactstrap';
import Navigation from '../Navigation/Navigation';

export class Layout extends Component {
  render() {
    return (
      <div>
        <Navigation />
        <Container>{this.props.children}</Container>
      </div>
    );
  }
}
