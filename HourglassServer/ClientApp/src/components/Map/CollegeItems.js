import React from "react";
import ReactMapGL, { Marker, Popup } from "react-map-gl";

const CollegeItems = props => {
   const updateSetColleges = props.updateSetColleges;
   const selectedColleges = props.selectedColleges;
   const clearSet = props.clearSet;

   return (
      <React.Fragment>
         {props.Education.Universities.map(college => (
            <Marker
               key={college.properties.COLLEGE_ID}
               latitude={college.geometry.coordinates[1]}
               longitude={college.geometry.coordinates[0]}
            >
               <button
                  className="marker-btn"
                  onClick={e => {
                     e.preventDefault();
                     updateSetColleges([college]);
                  }}
               >
                  <img
                     src={require("../../images/college-icon.png")}
                     alt="College Icon"
                  />
               </button>
            </Marker>
         ))}

         {selectedColleges.length > 0 ? (
            <Popup
               latitude={selectedColleges[0].geometry.coordinates[1]}
               longitude={selectedColleges[0].geometry.coordinates[0]}
               offsetLeft={20}
               offsetTop={30}
               anchor="top-left"
               onClose={clearSet}
               className="popup"
            >
               <div>
                  <h4>{selectedColleges[0].properties.NAME}</h4>
                  <p>
                     {" "}
                     Number of Degrees:{" "}
                     {selectedColleges[0].properties.NUM_DEGREES}
                  </p>
                  <img src={selectedColleges[0].properties.IMAGE} />
               </div>
            </Popup>
         ) : null}
      </React.Fragment>
   );
};
export default CollegeItems;
