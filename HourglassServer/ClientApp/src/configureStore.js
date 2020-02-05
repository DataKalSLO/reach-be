import { createBrowserHistory } from "history";
import { applyMiddleware, compose, createStore } from "redux";
import thunk from "redux-thunk";
import { routerMiddleware } from "connected-react-router";
import createRootReducer from "./reducers/index";

export const history = createBrowserHistory();

export default function configureStore(initialState) {
   const middleware = [thunk, routerMiddleware(history)];

   // In development, use the browser's Redux dev tools extension if installed
   const enhancers = [];
   const isDevelopment = process.env.NODE_ENV === "development";
   if (
      isDevelopment &&
      typeof window !== "undefined" &&
      window.devToolsExtension
   ) {
      enhancers.push(window.devToolsExtension());
   }

   return createStore(
      createRootReducer(history),
      initialState,
      compose(applyMiddleware(...middleware), ...enhancers)
   );
}
