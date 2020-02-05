import React, { useEffect, Fragment } from "react";
import { useState } from "react";
import Button from "@material-ui/core/Button";
import Grid from "@material-ui/core/Grid";
import MetricCard from "./MetricCard";
import StakeholderCard from "./StakeholderCard";
import education from "../images/education_plain.png";
import map from "../images/slo_map.png";
import data from "../images/data.png";
import Card from "./Card.js";

import "./InitiativeDetail.css";

import {
   XYPlot,
   XAxis,
   YAxis,
   VerticalGridLines,
   HorizontalGridLines,
   LineSeries,
   MarkSeries,
   ChartLabel
} from "react-vis";

const universities = [
   "Allan Hancock College",
   "University of California-Santa Barbara",
   "Cuesta College",
   "Fielding Graduate University",
   "Santa Barbara City College",
   "Westmont College",
   "Central California School of Continuing Education",
   "Design's School of Cosmetology",
   "California Polytechnic State University-San Luis Obispo",
   "Laurus College"
];

function InitiativeDetail(props) {
   const [universityIndex, setIndex] = useState(0);

   useEffect(() => props.updateDegreesData(universities[universityIndex]), []);

   return (
      <Fragment>
         <h1>Education</h1>
         <Grid container justify="center" spacing={6}>
            <Card name="Central Coast Map" image={education} />
            <Card name="Infrastructure" image={data} />
            <Card name="Housing" image={map} />
         </Grid>

         <h2 className="titles">About</h2>
         {loremIpsum}
         <h2 className="titles">Stakeholders</h2>
         <Grid container justify="center" spacing={3}>
            {stakeholders.map(stakeholder => (
               <StakeholderCard name={stakeholder} />
            ))}
         </Grid>
         <h2 className="titles">Overview</h2>
         <Grid container justify="center" spacing={3}>
            <MetricCard value="65%" title="K-12 Graduation" />
            <MetricCard value="43%" title="College Graduation" />
         </Grid>
         <h2 className="titles">Data</h2>

         <Grid container justify="space-between">
            <Grid item xs={4}>
               <h4>Number of Bachelor Degrees by College</h4>
               {loremIpsum}
            </Grid>

            <Grid item xs={6}>
               <div className="centered-and-flexed">
                  <div className="centered-and-flexed-controls">
                     <div className="campus-title">
                        {" "}
                        {`Campus: ${universities[universityIndex]}`}{" "}
                     </div>
                     <div>
                        <Button
                           variant="contained"
                           onClick={() => {
                              if (universityIndex - 1 < 0) {
                                 setIndex(0);
                                 props.updateDegreesData([universities[0]]);
                              } else {
                                 setIndex(universityIndex - 1);
                                 props.updateDegreesData([
                                    universities[universityIndex - 1]
                                 ]);
                              }
                           }}
                           color="primary"
                        >
                           Prev
                        </Button>
                        &nbsp;
                        <Button
                           variant="contained"
                           onClick={() => {
                              if (
                                 universityIndex + 1 >
                                 universities.length - 1
                              ) {
                                 setIndex(universities.length - 1);
                                 props.updateDegreesData(
                                    universities[universities.length - 1]
                                 );
                              } else {
                                 setIndex(universityIndex + 1);
                                 props.updateDegreesData(
                                    universities[universityIndex + 1]
                                 );
                              }
                           }}
                           color="primary"
                        >
                           Next
                        </Button>
                     </div>
                  </div>
                  <XYPlot
                     width={300}
                     height={300}
                     yDomain={[
                        0,
                        Math.max(
                           ...props.Education.DegreesByYear.map(
                              point => point.y
                           )
                        )
                     ]}
                     margin={{ left: 48, right: 24 }}
                  >
                     <XAxis
                        title="Year"
                        tickValues={[2012, 2013, 2014, 2015, 2016]}
                        tickFormat={v => v.toString()}
                     />
                     <YAxis title="Number of Bachelors Degrees" />
                     <LineSeries
                        animation={"noWobble"}
                        data={props.Education.DegreesByYear}
                     />
                  </XYPlot>
               </div>
            </Grid>
         </Grid>
         <Button variant="contained" color="primary">
            Download Raw Data
         </Button>
      </Fragment>
   );
}

const stakeholders = [
   "Allan Hancock College",
   "Cuesta College",
   "Cal Poly San Luis Obispo",
   "UC Santa Barbara"
];

const loremIpsum =
   "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

export default InitiativeDetail;
