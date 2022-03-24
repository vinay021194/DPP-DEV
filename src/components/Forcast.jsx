import ProductService from "../services/ProductService";
import ProcService from "../services/ProcService";
import Highcharts from "highcharts";
import HighchartsReact from "highcharts-react-official";

import React, { Component } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Button } from "primereact/button";

export class Forcast extends Component {
  constructor(props) {
    super(props);
    this.state = {
      data: this.props.location.state,
      products: [],
      materialInfo: [],
      InventoryInfo: [],
      demandUITable: [],
      icisForecastErrorInfoUpdated: [],
      HistoricalChartData: [],
      icisForecastSid: [],
      supplierDetails: [],
      count: 0,
    };

    this.productService = new ProductService();
    this.procService = new ProcService();
    this.weeklyValues = {
      "Film Posted DEL India 0-7 Days": [
        1535.85, 1535.87, 1535.87, 1535.87, 1535.87, 1535.87,
      ],
      "HDPE Blow Mould Domestic FD EU no-data": [
        1743.61, 1743.61, 1743.61, 1743.61, 1743.61, 1743.61,
      ],
      "Flat Yarn (Raffia) Spot DEL India W 0-7 Days": [
        1804.49, 1804.49, 1804.49, 1804.49, 1804.49, 1804.49,
      ],
      "HDPE Film Domestic FD EU no-data": [
        1773.67, 1773.67, 1773.67, 1773.67, 1773.67, 1773.67,
      ],
    };
  }

  componentDidMount() {
    // console.log("props in componentDidMount forcast ===>", this.props);

    // this.productService
    //   .getProductsSmall()
    //   .then((data) => this.setState({ products: data }));

    this.procService.getMaterialInfo({ material: 7001733 }).then((data) => {
      return this.setState({ materialInfo: data.data.data });
    });
    this.procService.getInventoryInfo({ material: 7001733 }).then((data) => {
      return this.setState({ InventoryInfo: data.data.Sheet2 });
    });
    this.procService.getDemandUITable({ material: 7001733 }).then((data) => {
      return this.setState({ demandUITable: data.data.Sheet2 });
    }); //icis_forecast_error_info_updated

    this.procService
      .getHistoricalUnitPrice({ material: 7001733 })
      .then((res) => {
        const { plant } = this.state.data && this.state.data;
        let resData = res.data.Sheet2;

        const filterByPlantData = resData.filter((el) => el.plant === plant);

        const unitPriceUSD = filterByPlantData.map((el) => {
          let date = el.posting_date
            .split("/") // 3/23/04  ===>
            .map((d, i) => (i === 2 ? 20 + d : d)) //  20 +"04" == 2004
            .join("/"); //  [3, 23, 04] ==> 3/23/2004

          date = new Date(date);
          let milliseconds = date.getTime();

          // console.log("date ==>", milliseconds);

          return [milliseconds, Number(el.unit_price_usd)];
        });

        const chartData = [
          {
            name: plant,
            data: unitPriceUSD.slice(-12),
          },
        ];

        // console.log("HistoricalUnitPrice chartData ====> ", chartData);
        return this.setState({ HistoricalChartData: chartData });
      });

    this.procService
      .getIcisForecastErrorInfoUpdated({ material: 7001733 })
      .then((data) => {
        let res = data.data;
        const dt = [];
        dt.push(res.Sheet1[res.Sheet1.length - 1]);
        dt.push(res.Sheet2[res.Sheet2.length - 1]);
        dt.push(res.Sheet3[res.Sheet3.length - 1]);
        dt.push(res.Sheet4[res.Sheet4.length - 1]);
        // console.log("icisForecastErrorInfoUpdated Data ===>", dt);

        return this.setState({ icisForecastErrorInfoUpdated: dt });
      }); //icis_forecast_error_info_updated

    this.procService.getIcisForecastSid({ material: 7001733 }).then((data) => {
      return this.setState({ icisForecastSid: data.data });
    });
  }
  convertData = () => {
    const { data } = this.state;
    if (data) {
      let suppliers =
        (data !== undefined && data.products) ||
        (this.props.location.state && this.props.location.state.suppliers);
      let forecastedObj = {};
      let leadTimeObj = {};
      let supplierMaxCapacity = {};
      //console.log("suppliers  =====>", suppliers);
      let convertedData = suppliers.map((el) => {
        if (Number(el.quantity)) {
          //console.log("its a Number");

          forecastedObj = {
            name: el.name,

            field: "Forecasted Price",

            month1: Number(el.quantity),

            month2: Number(el.quantity),

            month3: Number(el.quantity),

            month4: Number(el.quantity),

            month5: Number(el.quantity),

            month6: Number(el.quantity),
          };

          leadTimeObj = {
            name: el.name,

            field: "Lead Time",

            month1: el.Percentage,

            month2: el.Percentage,

            month3: el.Percentage,

            month4: el.Percentage,

            month5: el.Percentage,

            month6: el.Percentage,
          };

          supplierMaxCapacity = {
            name: el.name,

            field: "Supplier Max. Capacity",

            //OptimizeName : "Capacity",

            month1: el.price,

            month2: el.price,

            month3: el.price,

            month4: el.price,

            month5: el.price,

            month6: el.price,
          };
        } else {
          //console.log("its not a Number");

          //2*[1]+[2]
          const { data } = this.state;

          let seriesArr = data.seriesName.map((sr) => sr.name);

          // console.log("seriesArr in func   =======>", seriesArr);

          let str = el.quantity;

          var regex = /\[/gi,
            result,
            indices = [];
          while ((result = regex.exec(str))) {
            indices.push(result.index);
          }

          // let seriesUsedInFormula = indices.map((el) => Number(str[el + 1])); // 2, 1, 3

          let res = [];
          for (let i = 0; i < 6; i++) {
            let duplicate = el.quantity;
            //console.log("duplicate el.quantity ===>", duplicate);
            let duplicateSeriesArr = [...seriesArr];
            let strArr = duplicate.split("");
            while (strArr.indexOf("[") !== -1) {
              let avgMonthData = this.weeklyValues[duplicateSeriesArr[0]][i];

              let index = strArr.indexOf("[");
              strArr.splice(index, 3, avgMonthData);
              duplicateSeriesArr.shift();
            }
            //console.log("eval formul value " + i + "=====>", strArr.join(""));
            res.push(Number(eval(strArr.join("")).toFixed(2)));
          }

          forecastedObj = {
            name: el.name,

            field: "Forecasted Price",

            month1: res[0],

            month2: res[1],

            month3: res[2],

            month4: res[3],

            month5: res[4],

            month6: res[5],
          };

          leadTimeObj = {
            name: el.name,

            field: "Lead Time",

            month1: el.Percentage,

            month2: el.Percentage,

            month3: el.Percentage,

            month4: el.Percentage,

            month5: el.Percentage,

            month6: el.Percentage,
          };

          supplierMaxCapacity = {
            name: el.name,

            field: "Supplier Max. Capacity",

            //OptimizeName : "Capacity",

            month1: el.price,

            month2: el.price,

            month3: el.price,

            month4: el.price,

            month5: el.price,

            month6: el.price,
          };
        }
        return { forecastedObj, supplierMaxCapacity, leadTimeObj };
      });

      //console.log("convertedData =====>", convertedData);

      return this.setState({
        supplierDetails: convertedData,
        count: this.state.count + 1,
      });
    }
  };

  back = () => {
    const { data } = this.state;
    const { costDriver, seriesName, products, plant } = data;

    this.props.history.push("/Optimization", {
      // data: materialCostDriverOutput,
      costDriver,
      seriesName,
      products,
      plant,
    });
  };

  edit = () => {
    const {
      materialInfoproducts,
      InventoryInfoproducts,
      demandUITableproducts,
      icisForecastErrorInfoUpdatedproducts,
      data,
      supplierDetails,
    } = this.state;
    const { costDriver, seriesName, products, plant } = data;

    this.props.history.push("/EditOptimize", {
      costDriver,
      seriesName,
      products,
      plant,
      materialInfoproducts,
      InventoryInfoproducts,
      demandUITableproducts,
      icisForecastErrorInfoUpdatedproducts,
      supplierDetails,
    });
  };

  optimize = () => {
    const {
      materialInfoproducts,
      InventoryInfoproducts,
      demandUITableproducts,
      icisForecastErrorInfoUpdatedproducts,
      supplierDetails,
      data,
    } = this.state;
    const { costDriver, seriesName, products, plant } = data;

    this.props.history.push("/FinalResult", {
      costDriver,
      seriesName,
      products,
      plant,
      materialInfoproducts,
      InventoryInfoproducts,
      demandUITableproducts,
      icisForecastErrorInfoUpdatedproducts,
      supplierDetails,
    });
  };

  render() {
    //console.log("state in forcast =====>", this.state);
    //console.log("props in forcast =====>", this.props);

    let months = [
      "Jan",
      "Feb",
      "Mar",
      "Apr",
      "May",
      "Jun",
      "Jul",
      "Aug",
      "Sep",
      "Oct",
      "Nov",
      "month2",
    ];

    let date = new Date();
    let month = date.getMonth();
    let year = date.getFullYear();
    let month1 =
      month > 11
        ? months[month % 11] + "-" + year + 1
        : months[month] + "-" + year;
    let month2 =
      month + 1 > 11
        ? months[(month + 1) % 11] + "-" + year + 1
        : months[month + 1] + "-" + year;
    let month3 =
      month + 2 > 11
        ? months[(month + 2) % 11] + "-" + year + 1
        : months[month + 2] + "-" + year;
    let month4 =
      month + 3 > 11
        ? months[(month + 3) % 11] + "-" + year + 1
        : months[month + 3] + "-" + year;
    let month5 =
      month + 4 > 11
        ? months[(month + 4) % 11] + "-" + year + 1
        : months[month + 4] + "-" + year;
    let month6 =
      month + 5 > 11
        ? months[(month + 5) % 11] + "-" + year + 1
        : months[month + 5] + "-" + year;

    const {
      data,
      InventoryInfo,
      icisForecastErrorInfoUpdated,
      supplierDetails,
      count,
    } = this.state;

    if (count < 2 && data) this.convertData();

    const forcastedValues =
      supplierDetails.length > 0
        ? supplierDetails.map((supplier) => supplier.forecastedObj)
        : [];

    const forcastSeriesData =
      supplierDetails.length > 0
        ? supplierDetails.map((supplier) => {
            let objData = {
              name: supplier.forecastedObj.name,
              data: [
                [month1, supplier.forecastedObj.month1],
                [month2, supplier.forecastedObj.month2],
                [month3, supplier.forecastedObj.month3],
                [month4, supplier.forecastedObj.month4],
                [month5, supplier.forecastedObj.month5],
                [month6, supplier.forecastedObj.month6],
              ],
            };
            //console.log("forcastSeriesData ====>", objData);
            return objData;
          })
        : [];

    const filterData =
      (data != null || data !== undefined) && InventoryInfo
        ? InventoryInfo.filter((d) => d.plant === data.plant)
        : InventoryInfo;

    const filterBySeries =
      data != null || data !== undefined
        ? data.seriesName.map((d) =>
            icisForecastErrorInfoUpdated.filter((el) => el.series === d.name)
          )
        : [];
    const concatFilterBySeries =
      data != null || data !== undefined ? [].concat(...filterBySeries) : [];
    const d1 = data ? concatFilterBySeries : icisForecastErrorInfoUpdated;

    // console.log("d1 in forcast ===>", d1);
    // console.log("state in forcast ===>", this.state);

    const forecastedSupplierPriceOpthin = {
      chart: {
        zoomType: "x",
      },
      title: {
        // text: "Sabic Historical Prices",
        text: "Forecasted Supplier Price Trend",
        align: "center",
      },
      yAxis: {
        // type: "datetime",
        title: {
          text: "USD/Ton",
        },
      },
      xAxis: {
        categories: [month1, month2, month3, month4, month5, month6],
        title: {
          text: "Dates",
        },
      },
      legend: {
        layout: "horizontal",
        align: "center",
        verticalAlign: "bottom",
      },
      plotOptions: {
        series: {
          label: {
            connectorAllowed: false,
          },
        },
      },
      series: forcastSeriesData,
      responsive: {
        rules: [
          {
            condition: {
              maxWidth: 500,
            },
            chartOptions: {
              legend: {
                layout: "horizontal",
                align: "center",
                verticalAlign: "bottom",
              },
            },
          },
        ],
      },
    };

    // +++++++++++++++++++++++++++++++++++++=

    const historicalPricesOpthion = {
      chart: {
        zoomType: "x",
      },
      title: {
        text: "Sabic Historical Prices",

        align: "center",
      },
      yAxis: {
        // type: "datetime",
        title: {
          text: "USD/Ton",
        },
      },
      xAxis: {
        type: "datetime",
      },
      legend: {
        layout: "horizontal",
        align: "center",
        verticalAlign: "bottom",
      },
      plotOptions: {
        series: {
          label: {
            connectorAllowed: false,
          },
          pointStart: 2010,
        },
      },
      series: this.state.HistoricalChartData,
      responsive: {
        rules: [
          {
            condition: {
              maxWidth: 500,
            },
            chartOptions: {
              legend: {
                layout: "horizontal",
                align: "center",
                verticalAlign: "bottom",
              },
            },
          },
        ],
      },
    };

    return (
      <div>
        <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Material Information</h4>
          <DataTable value={this.state.materialInfo}>
            <Column
              field="material"
              header="Material Number"
              style={{ width: "14%" }}
            />
            <Column field="type" header="Type" style={{ width: "14%" }} />
            <Column
              field="description"
              header="Description"
              style={{ width: "30%" }}
            />
            <Column field="group" header="Group" style={{ width: "14%" }} />
            <Column field="class" header="Class" style={{ width: "14%" }} />
            <Column
              field="criticality"
              header="Criticality"
              style={{ width: "14%" }}
            />
          </DataTable>
        </div>

        <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}> Plant Data (in Ton) {data && data.plant}</h4>
          <DataTable value={filterData}>
            <Column
              field="warehouse_capacity"
              header="Warehouse Capacity(Tons)"
            />

            <Column field="safety_stock" header="Safety Stock" />
            <Column field="opening_stock" header="Closing Stock" />
          </DataTable>
        </div>

        <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Demand Prediction for next 6 months across all plants</h4>
          <DataTable
            value={this.state.demandUITable}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column field="avg_total_consumption" header="Demand Details" />
            <Column field="5/1/21" header={`${month1}`} />
            <Column field="6/1/21" header={`${month2}`} />
            <Column field="7/1/21" header={`${month3}`} />
            <Column field="8/1/21" header={`${month4}`} />
            <Column field="9/1/21" header={`${month5}`} />
            <Column field="10/1/21" header={`${month6}`} />
          </DataTable>
        </div>

        <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Forcasted Price (in USD) For Suppliers</h4>
          <DataTable
            value={forcastedValues}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column field="name" header="Supplier Name" />
            <Column field="month1" header={`${month1}`} />
            <Column field="month2" header={`${month2}`} />
            <Column field="month3" header={`${month3}`} />
            <Column field="month4" header={`${month4}`} />
            <Column field="month4" header={`${month5}`} />
            <Column field="month5" header={`${month6}`} />
          </DataTable>
        </div>

        <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Cost Driver Accuracy</h4>
          <DataTable
            value={d1}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column field="material" header="Cost Driver" />
            <Column field="series" header="Series" />
            <Column field="first_month_accuracy" header={`${month1}`} />
            <Column field="second_month_accuracy" header={`${month2}`} />
            <Column field="third_month_accuracy" header={`${month3}`} />
            <Column field="fourth_month_accuracy" header={`${month4}`} />
            <Column field="fifth_month_accuracy" header={`${month5}`} />
            <Column field="sixth_month_accuracy" header={`${month6}`} />
          </DataTable>
        </div>

        <div
          style={{
            display: "flex",
            flexBasis: "row",
            justifyContent: "space-between",
          }}
        >
          <div style={{ width: "49%", justifyContent: "center" }}>
            <HighchartsReact
              highcharts={Highcharts}
              options={historicalPricesOpthion}
            />
          </div>
          <div style={{ width: "49%", justifyContent: "center" }}>
            <HighchartsReact
              highcharts={Highcharts}
              options={forecastedSupplierPriceOpthin}
            />
          </div>
        </div>

        <div style={{ display: "flex", margin: "5px 10px", float: "right" }}>
          <Button
            label="Back"
            style={{ margin: "5px 10px" }}
            onClick={this.back}
          />
          <Button
            label="Edit"
            style={{ margin: "5px 10px" }}
            onClick={this.edit}
          />
          <Button
            label="Optimize"
            style={{ margin: "5px 10px" }}
            onClick={this.optimize}
          />
        </div>
      </div>
    );
  }
}
