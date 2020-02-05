const baseURL = "http://localhost:5000/api/";
const headers = new Headers();
var cookie;

headers.set("Content-Type", "application/JSON");

const reqConf = {
   headers: headers,
   credentials: "include"
};

function constructGetParameters(params) {
   return Object.keys(params).reduce((accumulator, current, idx) => {
      return (
         accumulator +
         (idx ? "&" : "?") +
         (params[current] ? `${current}=${params[current]}` : "")
      );
   }, "");
}

/**
 * @param {string} errTag
 * @param {string} lang
 */
export function errorTranslate(errTag, lang = "en") {
   return errMap[lang][errTag] || "Unknown Error!";
}

var tryFetch = function(url, request) {
   return fetch(url, request)
      .catch(err => Promise.reject(["Server connection failed"]))
      .then(response => {
         if (!response.ok) {
            return response.json().then(errs => {
               throw errs.map(err => errorTranslate(err.tag));
            });
         }
         return Promise.resolve(response);
      });
};

export function post(endpoint, body) {
   return tryFetch(baseURL + endpoint, {
      method: "POST",
      body: JSON.stringify(body),
      ...reqConf
   });
}

export function put(endpoint, body) {
   return tryFetch(baseURL + endpoint, {
      method: "PUT",
      body: JSON.stringify(body),
      ...reqConf
   });
}

export function get(endpoint) {
   return tryFetch(baseURL + endpoint, {
      method: "GET",
      ...reqConf
   });
}

export function del(endpoint) {
   return tryFetch(baseURL + endpoint, {
      method: "DELETE",
      ...reqConf
   });
}

export function getDegreesByYear(university) {
   return get(
      "Education/degrees" + constructGetParameters({ university })
   ).then(rsp => rsp.json());
}

export function getSchool(name) {
   return Promise.resolve({ name });
}

export function getUniversities() {
   return get("Education/universities")
      .then(rsp => rsp.json())
      .then(rsp => rsp.features);
}

export function getDummyCode() {
   return get("api/Dummy").then(response => response.json());
}

const errMap = {
   en: {
      missingField: "Field missing from request: ",
      badValue: "Field has bad value: ",
      notFound: "Entity not present in DB",
      badLogin: "Email/password combination invalid",
      dupEmail: "Email duplicates an existing email",
      noTerms: "Acceptance of terms is required",
      forbiddenRole: "Role specified is not permitted.",
      noOldPwd: "Change of password requires an old password",
      oldPwdMismatch: "Old password that was provided is incorrect.",
      dupTitle: "Conversation title duplicates an existing one",
      dupEnrollment: "Duplicate enrollment",
      forbiddenField: "Field in body not allowed.",
      queryFailed: "Query failed (server problem)."
   }
};
