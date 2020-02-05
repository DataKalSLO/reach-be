import { combineReducers } from "redux";

import ActiveSchools from "./ActiveSchools";
import ActiveYear from "./ActiveYear";
import DegreesByYear from "./DegreesByYear";
import SchoolType from "./SchoolType";
import Universities from "./Universities";

const Education = combineReducers({
   ActiveSchools,
   DegreesByYear,
   ActiveYear,
   SchoolType,
   Universities
});

export default Education;
