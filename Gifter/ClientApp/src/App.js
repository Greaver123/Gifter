import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import Layout from './components/UI/Layout/Layout';
import Home from './components/Views/Home/Home';
import GiftGroups from './components/Views/GiftGroups/GiftGroups';
import Wishlists from './components/Views/Wishlists/Wishlists';
import MyGifts from './components/Views/MyGifts/MyGifts';
import MyIdeas from './components/Views/MyIdeas/MyIdeas';
import EventCalendar from './components/Views/EventCalendar/EventCalendar';
import ProtectedRoute from './auth/ProtectedRoute/ProtectedRoute';
class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Switch>
          <ProtectedRoute path="/eventcalendar" component={EventCalendar} />
          <ProtectedRoute path="/giftgroups" component={GiftGroups} />
          <ProtectedRoute path="/wishlists" component={Wishlists} />
          <ProtectedRoute path="/mygifts" component={MyGifts} />
          <ProtectedRoute path="/myideas" component={MyIdeas} />
          <Route path="/" component={Home} />
        </Switch>
      </Layout>
    );
  }
}

export default App;
