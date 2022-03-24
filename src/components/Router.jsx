import React from "react";
import { Route, Switch } from "react-router-dom";
import { AppTopbar } from "./AppTopbar";
const Routes = (props) => {
  return (
    <>
      <Switch />
      <Route exact path="/" render={(props) => <AppTopbar {...props} />} />
      <Switch />
    </>
  );
};

export default Routes;
