import { combineReducers } from "redux";
import { connectRouter } from "connected-react-router";

import Education from "./Education/Education";
import Errors from "./Errors";

const createRootReducer = history =>
   combineReducers({
      router: connectRouter(history),
      Education,
      Errors
   });

export default createRootReducer;
