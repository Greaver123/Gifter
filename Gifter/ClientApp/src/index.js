import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { Auth0Provider } from '@auth0/auth0-react';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <Auth0Provider
      domain="giftter-dev.eu.auth0.com"
      clientId="aOyDaNo4il105Kub3qqUys8bGyvydAQQ"
      audience="https://giftter-dev.com"
      redirectUri={window.location.origin}
      //scope
    >
      <App />
    </Auth0Provider>
  </BrowserRouter>,
  rootElement
);

registerServiceWorker();
