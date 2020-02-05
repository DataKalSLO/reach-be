import React from "react";
import * as highSchoolData from "../../data/high_school_data.json";
import ReactMapGL, { Marker, Popup } from "react-map-gl";

const HighSchoolItems = props => {
   const updateSetHighSchools = props.updateSetHighSchools;
   const selectedHighSchools = props.selectedHighSchools;
   const clearSet = props.clearSet;

   return (
      <React.Fragment>
         {highSchoolData.features.map(high_school => (
            <Marker
               key={high_school.properties.HIGH_SCHOOL_ID}
               latitude={high_school.geometry.coordinates[1]}
               longitude={high_school.geometry.coordinates[0]}
            >
               <button
                  className="marker-btn"
                  onClick={e => {
                     e.preventDefault();
                     updateSetHighSchools([high_school]);
                  }}
               >
                  <img
                     src={require("../../images/college-icon-2.png")}
                     alt="High School Icon"
                  />
               </button>
            </Marker>
         ))}

         {selectedHighSchools.length > 0 ? (
            <Popup
               latitude={selectedHighSchools[0].geometry.coordinates[1]}
               longitude={selectedHighSchools[0].geometry.coordinates[0]}
               offsetLeft={20}
               offsetTop={30}
               anchor="top-left"
               onClose={clearSet}
               className="popup"
            >
               <div>
                  <h4>{selectedHighSchools[0].properties.NAME}</h4>
                  <p>
                     {" "}
                     Number of Degrees:{" "}
                     {selectedHighSchools[0].properties.NUM_DEGREES}
                  </p>
                  <img src={selectedHighSchools[0].properties.IMAGE} />
               </div>
            </Popup>
         ) : null}
      </React.Fragment>
   );
};
export default HighSchoolItems;
