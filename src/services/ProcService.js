import axios from "axios";

export default class ProcService {
  getMaterialInfo(params) {
    return axios
      .get("assets/demo/data/material_info.json", { params: params })
      .then((res) => {
        //console.log("getMaterialInfo ==>", res);
        return res;
      })
      .catch((e) => console.log(`error in getMaterialInfo ${e.message}`));
  }

  getDemandInfoRegression1(params) {
    return axios
      .get("assets/demo/data/demand_info_regression1.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getDemandInfoRegression1 ==>", res);
        return res.data.Sheet3;
      })
      .catch((e) =>
        console.log(`error in getDemandInfoRegression1 ${e.message}`)
      );
  }

  getDemandUITable(params) {
    return axios
      .get("assets/demo/data/demand_ui_table.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getDemandUITable ==>", res);
        return res;
      })
      .catch((e) => console.log(`error in getDemandUITable ${e.message}`));
  }

  getHistoricalUnitPrice(params) {
    return axios
      .get("assets/demo/data/historical_unit_price.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getHistoricalUnitPrice ==>", res);
        return res;
      })
      .catch((e) =>
        console.log(`error in getHistoricalUnitPrice ${e.message}`)
      );
  }

  getIcisAlertInfo(params) {
    return axios
      .get("assets/demo/data/icis_alert_info.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getIcisAlertInfo ==>", res);
        return res;
      })
      .catch((e) => console.log(`error in getIcisAlertInfo ${e.message}`));
  }

  getIcisForecastErrorInfoUpdated(params) {
    return axios
      .get("assets/demo/data/icis_forecast_error_info_updated.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getIcisForecastErrorInfoUpdated ==>", res);
        return res;
      })
      .catch((e) =>
        console.log(`error in getIcisForecastErrorInfoUpdated ${e.message}`)
      );
  }

  getIcisForecastSid(params) {
    return axios
      .get("assets/demo/data/icis_forecast_sid.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getIcisForecastSid ==>", res);
        return res;
      })
      .catch((e) => console.log(`error in getIcisForecastSid ${e.message}`));
  }

  getInventoryInfo(params) {
    return axios
      .get("assets/demo/data/inventory_info.json", { params: params })
      .then((res) => {
        //console.log("getInventoryInfo  ==>", res);
        return res;
      })
      .catch((e) => console.log(`error in getInventoryInfo ${e.message}`));
  }

  getMaterialCostDriverOutput(params) {
    return axios
      .get("assets/demo/data/material_cost_driver_output.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getMaterialCostDriverOutput ==>", res);
        return res;
      })
      .catch((e) =>
        console.log(`error in getMaterialCostDriverOutput ${e.message}`)
      );
  }

  geTonRouteInfo(params) {
    return axios
      .get("assets/demo/data/on_route_info.json", { params: params })
      .then((res) => {
        //console.log("geTonRouteInfo ==>", res);
        return res;
      })
      .catch((e) => console.log(`error in geTonRouteInfo ${e.message}`));
  }

  getDemandInfoRegression2(params) {
    return axios
      .get("assets/demo/data/demand_info_regression1.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getDemandInfoRegression2 ==>", res);
        return res;
      })
      .catch((e) =>
        console.log(`error in getDemandInfoRegression1 ${e.message}`)
      );
  }

  getPlants(params) {
    return axios
      .get("assets/demo/data/on_route_info.json", { params: params })
      .then((res) => {
        const unique = [...new Set(res.data.Sheet2.map((item) => item.plant))];
        //console.log("getPlants ==>", res);
        return unique;
      })
      .catch((e) =>
        console.log(`error in getDemandInfoRegression1 ${e.message}`)
      );
  }

  getDemandInfoRegressionSummaryTable(params) {
    return axios
      .get("assets/demo/data/demand_info_regression_summary_table.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getDemandInfoRegressionSummaryTable ==>", res);
        return res;
      })
      .catch((e) =>
        console.log(`error in getDemandInfoRegressionSummaryTable ${e.message}`)
      );
  }

  getIcisForecastSummaryTable(params) {
    return axios
      .get("assets/demo/data/icis_forecast_summary_table.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getIcisForecastSummaryTable ==>", res);
        return res;
      })
      .catch((e) =>
        console.log(`error in getIcisForecastSummaryTable ${e.message}`)
      );
  }

  getOrder(params) {
    return axios
      .get("assets/demo/data/order.json", {
        params: params,
      })
      .then((res) => {
        //console.log("getOrder ==>", res);
        return res;
      })
      .catch((e) => console.log(`error in getOrder ${e.message}`));
  }
}
