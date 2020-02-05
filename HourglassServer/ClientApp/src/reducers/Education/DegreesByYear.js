export default function DegreesByYear(state = [], action) {
   switch (action.type) {
      case "UPDATE_DEGREES_BY_YEAR":
         return action.points;
      default:
         return state;
   }
}
