import "bootstrap/dist/css/bootstrap.css";
import React from "react";
import ReactDOM from "react-dom";
import { ConnectedRouter } from "connected-react-router";
import configureStore, { history } from "./configureStore";
import App from "./App";
import registerServiceWorker from "./registerServiceWorker";
import urlToState from "./store/urlHandling";
import { Provider, ReactReduxContext } from "react-redux";

const initialState = urlToState(window.location.href);
const store = configureStore(initialState);

const rootElement = document.getElementById("root");

ReactDOM.render(
   <Provider store={store} context={ReactReduxContext}>
      <ConnectedRouter history={history} context={ReactReduxContext}>
         <App />
      </ConnectedRouter>
   </Provider>,
   rootElement
);

registerServiceWorker();
