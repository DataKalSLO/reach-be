import React from "react";

import Card from "@material-ui/core/Card";
import CardContent from "@material-ui/core/CardContent";
import Typography from "@material-ui/core/Typography";
import Grid from "@material-ui/core/Grid";

function MetricCard(props) {
   return (
      <Grid item xs={3}>
         <Grid container justify="space-evenly" align="center">
            <Card>
               <CardContent>
                  <Typography gutterBottom variant="h3" component="h1">
                     {props.value}
                  </Typography>
                  <Typography gutterBottom variant="h5" component="h2">
                     {props.title}
                  </Typography>
               </CardContent>
            </Card>
         </Grid>
      </Grid>
   );
}

export default MetricCard;
