import React from "react";
import { connect } from "react-redux";

import CentralCoastMap from "../images/CentralCoastMap.png";
import Diablo from "../images/Diablo.png";
import Education from "../images/Education.png";
import Housing from "../images/Housing.png";
import Infrastructure from "../images/Infrastructure.png";
import Space from "../images/Space.png";
import Vandenburg from "../images/Vandenburg.png";
import Aerospace from "../images/Aerospace.png";
import Energy from "../images/Energy.png";
import AgTech from "../images/AgTech.png";

import Grid from "@material-ui/core/Grid";
import Card from "./Card.js";

const Home = props => {
   return (
      <div>
         <h1>Overview</h1>

         <Grid container spacing={3}>
            <Card name="Central Coast Map" image={CentralCoastMap} url="/map" />
            <Card name="Infrastructure" image={Infrastructure} />
            <Card name="Housing" image={Housing} />
         </Grid>

         <h1 style={{ paddingTop: "40px" }}>Initiatives</h1>

         <Grid container spacing={3}>
            <Card name="Space Technology" image={Space} />
            <Card name="Education" image={Education} url="/Education" />
            <Card name="Diablo" image={Diablo} />
            <Card name="Vandenburg" image={Vandenburg} />
            <Card name="Renewable Energy" image={Energy} />
            <Card name="Aerospace" image={Aerospace} />
            <Card name="Agriculture Technology" image={AgTech} />
         </Grid>
      </div>
   );
};

export default connect()(Home);
