import React from "react";
import Map from "./Map";
import Grid from "@material-ui/core/Grid";

const MapOverview = props => {
   return (
      <Grid container justify="center" spacing={6}>
         <Grid item>
            <h3>Interactive Map</h3>
            <p>Click on any pins on the map to explore education data.</p>
            <Map {...props} />
         </Grid>
      </Grid>
   );
};

export default MapOverview;
