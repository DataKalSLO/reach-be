import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import Card from "@material-ui/core/Card";
import CardActionArea from "@material-ui/core/CardActionArea";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import CardMedia from "@material-ui/core/CardMedia";
import Button from "@material-ui/core/Button";
import Typography from "@material-ui/core/Typography";
import education from "../images/education.jpeg";
import Grid from "@material-ui/core/Grid";
import PropTypes from "prop-types";

const useStyles = makeStyles({
   card: {
      maxWidth: 345,
      borderRadius: "25px"
   }
});

function InitiativeCard(props) {
   const classes = useStyles();
   const { name, image, url } = props;

   return (
      <Grid item xs={3}>
         <Grid container justify="space-evenly" align="center">
            <Card className={classes.card}>
               <CardActionArea>
                  <a href={url}>
                     <CardMedia component="img" height="250" image={image} />
                  </a>
               </CardActionArea>
            </Card>
         </Grid>
      </Grid>
   );
}

InitiativeCard.propTypes = {
   name: PropTypes.string.isRequired,
   image: PropTypes.string.isRequired
};

export default InitiativeCard;
