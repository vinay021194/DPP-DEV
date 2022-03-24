import "primereact/resources/themes/saga-blue/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import "primeflex/primeflex.css";

import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
//import { Provider } from "react-redux";
import { BrowserRouter } from "react-router-dom";
// import store from "./redux/store";

ReactDOM.render(
  <BrowserRouter basename={"/dpp_demo"}>
    {/* <Provider store={store}> */}
    <App />
    {/* </Provider> */}
  </BrowserRouter>,
  document.getElementById("root")
);

reportWebVitals();
