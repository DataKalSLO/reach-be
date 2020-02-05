export default function Universities(state = [], action) {
   switch (action.type) {
      case "UPDATE_UNIVERSITIES":
         return action.collegeData;
      default:
         return state;
   }
}
