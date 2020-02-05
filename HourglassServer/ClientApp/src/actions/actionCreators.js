import * as api from "../api";
import { push } from "connected-react-router";

export function updateDegreesData(universities, cb) {
   return (dispatch, prevState) => {
      Promise.all(universities.map(api.getDegreesByYear))
         .then(points => dispatch({ type: "UPDATE_DEGREES_BY_YEAR", points }))
         .then(() => {
            if (cb) cb();
         })
         .catch(error =>
            dispatch({ type: "DEGREES_POINT_ERROR", details: error })
         );
   };
}

export function updateUniversitiesData(cb) {
   return (dispatch, prevState) => {
      api.getUniversities()
         .then(collegeData =>
            dispatch({ type: "UPDATE_UNIVERSITIES", collegeData })
         )
         .then(() => {
            if (cb) {
               cb();
            }
         })
         .catch(error =>
            dispatch({ type: "UNIVERSITIES_FETCH_ERROR", details: error })
         );
   };
}

export function updateActiveSchools(names, cb) {
   return (dispatch, prevState) => {
      Promise.all(names.map(api.getSchool))
         .then(schools => dispatch({ type: "UPDATE_ACTIVE_SCHOOLS", schools }))
         .then(() => {
            if (cb) cb();
         })
         .catch(error => dispatch({ type: "GET_SCHOOL_ERR", details: error }));
   };
}

export function clearActiveSchools(cb) {
   return (dispatch, prevState) => {
      return new Promise(() => dispatch({ type: "CLEAR_ACTIVE_SCHOOLS" })).then(
         () => {
            if (cb) cb();
         }
      );
   };
}

export function updateActiveYear(year, cb) {
   return (dispatch, prevState) => {
      return new Promise(() => dispatch({ type: "UPDATE_ACTIVE_YEAR", year }))
         .then(() => dispatch(push(`?year=${year}`)))
         .then(() => {
            if (cb) cb();
         });
   };
}

export function clearActiveYear(cb) {
   return (dispatch, prevState) => {
      return new Promise(() => dispatch({ type: "CLEAR_ACTIVE_YEAR" })).then(
         () => {
            if (cb) cb();
         }
      );
   };
}

export function addToSearchParams(key, value, search_string, cb) {
   return (dispatch, prevState) => {
      return new Promise(() =>
         dispatch(push(addToSearchString(key, value, search_string)))
      ).then(() => {
         if (cb) cb();
      });
   };
}

export function deleteFromSearchParams(key, value, search_string, cb) {
   return (dispatch, prevState) => {
      Promise.resolve()
         .then(() =>
            dispatch(push(removeFromSearchString(key, value, search_string)))
         )
         .then(() => {
            if (cb) cb();
         });
   };
}

export function updateSchoolType(school_type_data, cb) {
   return (dispatch, prevState) => {
      dispatch({ type: "UPDATE_SCHOOL_TYPE", school_type_data })
         .then(() => {
            if (cb) cb();
         })
         .catch(error => dispatch({ type: "GET_SCHOOL_ERR", details: error }));
   };
}

export function clearSchoolType(cb) {
   return (dispatch, prevState) => {
      return new Promise(() => dispatch({ type: "CLEAR_SCHOOL_TYPE" })).then(
         () => {
            if (cb) cb();
         }
      );
   };
}

// update search string with the given key and value
// search string of type ?x=y&w=z
// key and value are evaluated as strings
const addToSearchString = (key, value, search_string) => {
   let str = [];
   let obj = {};

   if (search_string) {
      search_string = search_string.slice(1);
      obj = JSON.parse(
         '{"' +
            decodeURI(search_string)
               .replace(/"/g, '\\"')
               .replace(/&/g, '","')
               .replace(/=/g, '":"') +
            '"}'
      );
   }

   obj[key] = value;
   for (var p in obj)
      if (obj.hasOwnProperty(p)) {
         str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
      }
   return "?" + str.join("&");
};

const removeFromSearchString = (key, search_string) => {
   let str = [];
   let obj = {};

   if (!search_string) return search_string;

   search_string = search_string.slice(1);
   obj = JSON.parse(
      '{"' +
         decodeURI(search_string)
            .replace(/"/g, '\\"')
            .replace(/&/g, '","')
            .replace(/=/g, '":"') +
         '"}'
   );

   delete obj[key];
   for (var p in obj)
      if (obj.hasOwnProperty(p)) {
         str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
      }
   return "?" + str.join("&");
};
