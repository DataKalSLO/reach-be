// Parse url string to initialize redux store
const urlToState = url_string => {
   const state = {};
   const url = new URL(url_string);
   for (const param in param_to_store) {
      let param_val = url.searchParams.get(param);
      if (param_val) {
         const store_data = param_to_store[param];
         switch (store_data.type) {
            case "int":
               param_val = parseInt(param_val, 10);
               break;
            default:
               break;
         }
         addToState(
            state,
            store_data.base_reducer,
            store_data.reducer,
            store_data.attribute,
            param_val
         );
      }
   }
   return state;
};

// mapping of url params
const param_to_store = {
   year: {
      type: "int",
      base_reducer: "Education",
      reducer: "ActiveYear",
      attribute: "year"
   },
   school: {
      type: "string",
      base_reducer: "Education",
      reducer: "ActiveSchool",
      attribute: "name"
   }
};

const addToState = (state, base_reducer, reducer, attribute, value) => {
   if (!state.hasOwnProperty(base_reducer)) state[base_reducer] = {};
   if (!state[base_reducer].hasOwnProperty(reducer))
      state[base_reducer][reducer] = {};
   state[base_reducer][reducer][attribute] = value;
   return state;
};

export default urlToState;
