export default function SchoolType(state = { school_type: "college" }, action) {
   switch (action.type) {
      case "UPDATE_SCHOOL_TYPE":
         return action.school_type_data;
      case "CLEAR_SCHOOL_TYPE":
         return {};
      default:
         return state;
   }
}
