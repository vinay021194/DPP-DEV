import React, { useState, useRef } from "react";
import classNames from "classnames";
import { Route, Router, useHistory } from "react-router-dom";
import { CSSTransition } from "react-transition-group";
import { AppTopbar } from "./components/AppTopbar";
import { AppMenu } from "./components/AppMenu";
// import { AppProfile } from "./components/AppProfile";
// import { Table } from "./components/Table";
// import { CustomTable } from "./components/CustomTable";
// import { BarCharts } from "./components/BarCharts";
// import { LineChart } from "./components/LineChart";
// import { ScatterChart } from "./components/ScatterChart";
//import { Charts } from "./components/Charts";
import { MaterialForm } from "./components/MaterialForm";

import "primereact/resources/themes/saga-blue/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import "primeflex/primeflex.css";
import "prismjs/themes/prism-coy.css";
// import "@fullcalendar/core/main.css";
// import "@fullcalendar/daygrid/main.css";
// import "@fullcalendar/timegrid/main.css";
import "./layout/flags/flags.css";
import "./layout/layout.scss";
import "./App.scss";
import { DemandAndInventoryAnalysis } from "./components/DemandAndInventoryAnalysis";
import { CategoryView } from "./components/Categoryview";

import { CostAndIndexPriceAnalysis } from "./components/CostAndIndexPriceAnalysis";
// import { Optimization } from "./components/Optimization";
import { Forcast } from "./components/Forcast";
import { EditOptimize } from "./components/EditOptimize";
import { FinalResult } from "./components/FinalResult";
import { Demo } from "./components/Demo";

const App = () => {
  const [layoutMode, setLayoutMode] = useState("static");
  const [layoutColorMode, setLayoutColorMode] = useState("dark");
  const [staticMenuInactive, setStaticMenuInactive] = useState(false);
  const [overlayMenuActive, setOverlayMenuActive] = useState(false);
  const [mobileMenuActive, setMobileMenuActive] = useState(false);
  // const [inputStyle, setInputStyle] = useState("outlined");
  // const [ripple, setRipple] = useState(false);
  const sidebar = useRef();
  const history = useHistory();
  let menuClick = false;

  const menu = [
    {
      label: "Home",
      icon: "pi pi-fw pi-home",
      to: "/",
    },
    {
      label: "Category View",
      icon: "pi pi-fw pi-user",
      to: "/CategoryView",
    },
    {
      label: "Demand And Inventory Analysis",
      icon: "pi pi-fw pi-clone",
      to: "/DemandAndInventoryAnalysis",
    },

    {
      label: "Cost And Index Price Analysis",
      icon: "pi pi-fw pi-chart-line",
      to: "/CostAndIndexPriceAnalysis",
    },
    {
      label: "Optimization",
      icon: "pi pi-fw pi-chart-bar",
      to: "/Optimization",
    },
    
  ];

  const wrapperClass = classNames("layout-wrapper", {
    "layout-overlay": layoutMode === "overlay",
    "layout-static": layoutMode === "static",
    "layout-static-sidebar-inactive":
      staticMenuInactive && layoutMode === "static",
    "layout-overlay-sidebar-active":
      overlayMenuActive && layoutMode === "overlay",
    "layout-mobile-sidebar-active": mobileMenuActive,
    // "p-input-filled": inputStyle === "filled",
    // "p-ripple-disabled": ripple === false,
  });

  const sidebarClassName = classNames("layout-sidebar", {
    "layout-sidebar-dark": layoutColorMode === "dark",
    "layout-sidebar-light": layoutColorMode === "light",
  });

  const logo =
    layoutColorMode === "dark"
      ? "assets/layout/images/logo-white.svg"
      : "assets/layout/images/logo.svg";

  const onMenuItemClick = (event) => {
    if (!event.item.items) {
      setOverlayMenuActive(false);
      setMobileMenuActive(false);
    }
  };

  const isDesktop = () => {
    return window.innerWidth > 1024;
  };

  const onToggleMenu = (event) => {
    menuClick = true;

    if (isDesktop()) {
      if (layoutMode === "overlay") {
        setOverlayMenuActive((prevState) => !prevState);
      } else if (layoutMode === "static") {
        setStaticMenuInactive((prevState) => !prevState);
      }
    } else {
      setMobileMenuActive((prevState) => !prevState);
    }
    event.preventDefault();
  };

  const onWrapperClick = (event) => {
    if (!menuClick) {
      setOverlayMenuActive(false);
      setMobileMenuActive(false);
    }
    menuClick = false;
  };

  const onSidebarClick = () => {
    menuClick = true;
  };

  const isSidebarVisible = () => {
    if (isDesktop()) {
      if (layoutMode === "static") return !staticMenuInactive;
      else if (layoutMode === "overlay") return overlayMenuActive;
      else return true;
    }

    return true;
  };

  return (
    <div className={wrapperClass} onClick={onWrapperClick}>
      <AppTopbar onToggleMenu={onToggleMenu} />

      <CSSTransition
        classNames="layout-sidebar"
        timeout={{ enter: 200, exit: 200 }}
        in={isSidebarVisible()}
        unmountOnExit
      >
        <div
          ref={sidebar}
          className={sidebarClassName}
          onClick={onSidebarClick}
        >
          <div
            className="layout-logo"
            style={{
              cursor: "pointer",
            }}
            onClick={() => history.push("/")}
          >
            <img
              alt="Logo"
              src={logo}
              style={{
                width: "200px",
                margin: "0px 0px 15px 0px",
              }}
            />
          </div>
          {/* <AppProfile /> */}
          <AppMenu model={menu} onMenuItemClick={onMenuItemClick} />
        </div>
      </CSSTransition>

      {/* <AppConfig rippleEffect={ripple} onRippleEffect={onRipple} inputStyle={inputStyle} onInputStyleChange={onInputStyleChange}
                layoutMode={layoutMode} onLayoutModeChange={onLayoutModeChange} layoutColorMode={layoutColorMode} onColorModeChange={onColorModeChange} /> */}

      <div className="layout-main">
        {/* <Router > */}
        <Route path="/" exact component={MaterialForm} />
        <Route
          path="/DemandAndInventoryAnalysis"
          exact
          component={DemandAndInventoryAnalysis}
        />
        <Route
          path="/CategoryView"
          exact
          component={CategoryView}
        />
        <Route
          path="/CostAndIndexPriceAnalysis"
          exact
          component={CostAndIndexPriceAnalysis}
        />
        <Route path="/Optimization" exact component={Demo} />
        <Route path="/Forcast" exact component={Forcast} />
        <Route path="/EditOptimize" exact component={EditOptimize} />
        <Route path="/FinalResult" exact component={FinalResult} />
        {/* </Router> */}
      </div>
    </div>
  );
};

export default App;
