import React, { useState, Fragment, useEffect } from "react";
import ReactMapGL, { Marker, Popup } from "react-map-gl";
import Grid from "@material-ui/core/Grid";
import "./Map.css";
import MAP_STYLE from "./MapStyle";
import BarChart from "../BarChart";
import LineChart from "../LineChart";
import CollegeItems from "./CollegeItems.js";
import HighSchoolItems from "./HighSchoolItems.js";

const Map = props => {
   const [viewport, setViewport] = useState({
      width: 1200,
      height: 570,
      latitude: 35.12027728426617,
      longitude: -120.59208534929661,
      zoom: 8.336394076562108
   });

   useEffect(() => {
      props.updateUniversitiesData();
   }, []);

   const [selectedSchools, setSelectedSchools] = useState(
      props.Education.ActiveSchools.map(name =>
         props.Education.Universities.find(obj => obj.properties.NAME === name)
      )
   );

   const updateSet = selectedSchools => {
      const selectedNames = selectedSchools.map(
         school => school.properties.NAME
      );
      props.updateActiveSchools(selectedNames);
      props.updateDegreesData(selectedNames);
      props.addToSearchParams("schools", selectedNames, props.search);
      setSelectedSchools(selectedSchools);
   };

   const clearSet = () => {
      props.clearActiveSchools();
      setSelectedSchools([]);
   };

   return (
      <Fragment>
         <ReactMapGL
            ref={this._map}
            mapStyle={MAP_STYLE}
            {...viewport}
            onViewportChange={setViewport}
            mapboxApiAccessToken={process.env.REACT_APP_MAPBOX_TOKEN}
         >
            {props.Education.SchoolType.school_type === "college" ? (
               <CollegeItems
                  selectedColleges={selectedSchools}
                  updateSetColleges={updateSet}
                  clearSet={clearSet}
                  {...props}
               />
            ) : null}
            {props.Education.SchoolType.school_type === "high_school" ? (
               <HighSchoolItems
                  selectedHighSchools={selectedSchools}
                  updateSetHighSchools={updateSet}
                  clearSet={clearSet}
                  {...props}
               />
            ) : null}
         </ReactMapGL>
         <Grid container direction="row" justify="center" alignItems="center">
            <BarChart {...props} />
            {props.Education.ActiveSchools.length > 0 ? (
               <LineChart {...props} />
            ) : null}
         </Grid>
      </Fragment>
   );
};

export default Map;
