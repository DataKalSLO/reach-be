export default function ActiveSchools(state = [], action) {
   switch (action.type) {
      case "UPDATE_ACTIVE_SCHOOLS":
         return action.schools;
      case "CLEAR_ACTIVE_SCHOOLS":
         return [];
      default:
         return state;
   }
}
