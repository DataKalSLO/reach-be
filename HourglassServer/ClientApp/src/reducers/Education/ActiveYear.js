export default function ActiveYear(state = {}, action) {
   switch (action.type) {
      case "UPDATE_ACTIVE_YEAR":
         return action.year;
      case "CLEAR_ACTIVE_YEAR":
         return {};
      default:
         return state;
   }
}
