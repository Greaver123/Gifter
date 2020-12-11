import React, { Component } from 'react';
import LoginButton from '../auth/LoginButton/LoginButton';
import LogoutButton from '../auth/LogoutButton/LogoutButton';

export class Home extends Component {
  render() {
    return (
      <div>
        <LoginButton />
        <LogoutButton />
      </div>
    );
  }
}
