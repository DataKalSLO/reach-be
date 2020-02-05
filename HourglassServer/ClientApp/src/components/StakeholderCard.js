import React from "react";

import Card from "@material-ui/core/Card";
import CardContent from "@material-ui/core/CardContent";
import Typography from "@material-ui/core/Typography";
import Grid from "@material-ui/core/Grid";

function StakeholderCard(props) {
   return (
      <Grid item xs={3}>
         <Grid container justify="space-evenly" align="center">
            <Card>
               <CardContent>{props.name}</CardContent>
            </Card>
         </Grid>
      </Grid>
   );
}

export default StakeholderCard;
