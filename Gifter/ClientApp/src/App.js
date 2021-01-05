import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/UI/Layout/Layout';
import { Home } from './components/Views/Home/Home';
import FetchData from './components/FetchData';
import GiftGroups from './components/Views/GiftGroups/GiftGroups';
import WishList from './components/Views/WishList/WishList';
import MyGifts from './components/Views/MyGifts/MyGifts';
import MyIdeas from './components/Views/MyIdeas/MyIdeas';
import EventCalendar from './components/Views/EventCalendar/EventCalendar';
class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Switch>
          <Route path="/eventcalendar" component={EventCalendar} />
          <Route path="/giftgroups" component={GiftGroups} />
          <Route path="/wishlist" component={WishList} />
          <Route path="/mygifts" component={MyGifts} />
          <Route path="/myideas" component={MyIdeas} />
          <Route path="/fetch-data" component={FetchData} />
          <Route path="/" component={Home} />
        </Switch>
      </Layout>
    );
  }
}

export default App;
