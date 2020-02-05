import React from "react";
import { Route } from "react-router";
import Layout from "./components/Layout";
import Home from "./components/Home";
import InitiativeDetail from "./components/InitiativeDetail";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import * as actionCreators from "./actions/actionCreators";
import MapOverview from "./components/Map/MapOverview";

export function mapStateToProps(state) {
   return {
      Education: state.Education,
      Error: state.Error,
      search: state.router.location.search
   };
}

export function mapDispatchToProps(dispatch) {
   return bindActionCreators(actionCreators, dispatch);
}

const Main = props => (
   <Layout>
      <Route exact path="/" render={() => <Home {...props} />} />
      <Route path="/education" render={() => <InitiativeDetail {...props} />} />
      <Route path="/map" render={() => <MapOverview {...props} />} />
   </Layout>
);

const App = withRouter(connect(mapStateToProps, mapDispatchToProps)(Main));

export default App;
