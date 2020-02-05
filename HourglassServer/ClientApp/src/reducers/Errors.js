export default function Errors(state = [], action) {
   switch (action.type) {
      case "EDUCATION_POINT_ERROR":
         return state.concat(action.details);
      default:
         return state;
   }
}
