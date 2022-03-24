import React, { Component } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import ProductService from "../services/ProductService";
import ProcService from "../services/ProcService";
import Highcharts from "highcharts";
import HighchartsReact from "highcharts-react-official";
import { MultiSelect } from "primereact/multiselect";
import { Button } from "primereact/button";

export class CostAndIndexPriceAnalysis extends Component {
  constructor(props) {
    super(props);
    this.state = {
      materialInfo: [],
      selectSerialName: null,
      icisAlertInfoData: [],
      seriesData: [],
      costDriverSeriesData: [],
      costdriverChart: true,
      costDriver: [],
      costDriverSeries: [],
      icisForecastSummaryTable: [],
      order: [],
    };

    this.productService = new ProductService();
    this.procService = new ProcService();

    this.seriesName = [
      {
        name: "LLDPE Bulk Africa E Weekly",
        code: "LLDPE Bulk Africa E Weekly",
      },
      {
        name: "PE LLDPE Film Butene CFR Peru International 0 - 6 Weeks",
        code: "PE LLDPE Film Butene CFR Peru International 0 - 6 Weeks",
      },
      {
        name: "Flat Yarn Contract China Weekly",
        code: "Flat Yarn Contract China Weekly",
      },
      {
        name: "Propylene Bulk NWE Monthly",
        code: "Propylene Bulk NWE Monthly",
      },
      {
        name: "HDPE Blow Mould Posted EU Weekly",
        code: "HDPE Blow Mould Posted EU Weekly",
      },
      {
        name: "LDPE High Grade Contract US Monthly",
        code: "LDPE High Grade Contract US Monthly",
      },
      {
        name: "Copolymer Film Contract US Monthly",
        code: "Copolymer Film Contract US Monthly",
      },
      {
        name: "LDPE Contract CFR Egypt Weekly",
        code: "LDPE Contract CFR Egypt Weekly",
      },
      {
        name: "Film Posted Bulk China Weekly",
        code: "Film Posted Bulk China Weekly",
      },
      {
        name: "HDPE Film Contract EU Weekly",
        code: "HDPE Film Contract EU Weekly",
      },
      {
        name: "LDPE High Grade Peru International Weekly",
        code: "LDPE High Grade Peru International Weekly",
      },
      {
        name: "HDPE Bulk Contract DEL US Monthly",
        code: "HDPE Bulk Contract DEL US Monthly",
      },
      {
        name: "Copolymer Domestic UK Weekly",
        code: "Copolymer Domestic UK Weekly",
      },
    ];

    this.costDrivers = [
      {
        name: "Polyethylene (Africa)",
        code: "Polyethylene (Africa)",
      },
      {
        name: "Polypropylene (Middle East)",
        code: "Polypropylene (Middle East)",
      },
      {
        name: "Polyethylene (Latin America)",
        code: "Polyethylene (Latin America)",
      },
      {
        name: "Polypropylene (Europe)",
        code: "Polypropylene (Europe)",
      },
      { name: "Polyethylene (Europe)", code: "Polyethylene (Europe)" },
      { name: "Polypropylene (US)", code: "Polypropylene (US)" },
      { name: "Polyethylene (US)", code: "Polyethylene (US)" },
    ];

    this.seriesNameForAlert = [
      {
        name: "Film Posted DEL India 0-7 Days",
        code: "Film Posted DEL India 0-7 Days",
      },
      {
        name: "HDPE Blow Mould Domestic FD EU no-data",
        code: "HDPE Blow Mould Domestic FD EU no-data",
      },
      {
        name: "Flat Yarn (Raffia) Spot DEL India W 0-7 Days",
        code: "Flat Yarn (Raffia) Spot DEL India W 0-7 Days",
      },
      {
        name: "HDPE Film Domestic FD EU no-data",
        code: "HDPE Film Domestic FD EU no-data",
      },
    ];
  }

  componentDidMount() {
    // this.procService.getOrder({ material: 7001733 }).then((res) => {
    //   return this.setState({
    //     order: res.data.order,
    //   });
    // });

    this.procService.getMaterialInfo({ material: 7001733 }).then((data) => {
      return this.setState({ materialInfo: data.data.data });
    });

    this.procService
      .getIcisForecastSummaryTable({ material: 7001733 })
      .then((data) => {
        // console.log("getIcisForecastSummaryTable  ===>", data.data.Sheet1);
        return this.setState({ icisForecastSummaryTable: data.data.Sheet1 });
      });

    this.procService.getIcisAlertInfo({ frequency: "weekly" }).then((res) => {
      let first = res.data.Sheet1;
      let second = res.data.Sheet2;
      let third = res.data.Sheet3;
      let fourth = res.data.Sheet4;
      const concatData = [].concat(...first, ...second, ...third, ...fourth);

      return this.setState({ icisAlertInfoData: concatData });
    });
  }

  onSeriesChange = (e) => {
    //console.log("on change event  ==>", e);
    const { icisAlertInfoData } = this.state;

    let exampleData = e.value.map((sr) =>
      icisAlertInfoData
        .filter((el) => el.serial_name === sr.name)
        .map((d) => {
          //console.log("data in map ===>", d);
          let date = d.date
            .split("/") // 3/23/04  ===>
            .map((d, i) => (i === 2 ? 20 + d : d)) //  20 +"04" == 2004
            .join("/"); //  [3, 23, 04] ==> 3/23/2004
          date = new Date(date);
          let milliseconds = date.getTime();

          var dataObj = [milliseconds, Number(d.actual_values)];
          return dataObj;
        })
    );

    const chartData1 = e.value.map((sr, i) => {
      return {
        name: sr.name,
        data: exampleData[i].splice(-150),
      };
    });

    this.setState({ selectSerialName: e.value, seriesData: chartData1 });
  };

  oncostDriverSeriesChange = (e) => {
    //console.log("on oncostDriverSeriesChange event  ==>", e);
    const { icisForecastSummaryTable } = this.state;

    let exampleData = e.value.map((sr) =>
      icisForecastSummaryTable
        .filter((el) => el.serial_name === sr.name)
        .map((d) => {
          // console.log("data in map ===>", d);
          let date = d.date
            .split("/") // 3/23/04  ===>
            .map((d, i) => (i === 2 ? 20 + d : d)) //  20 +"04" == 2004
            .join("/"); //  [3, 23, 04] ==> 3/23/2004
          date = new Date(date);
          let milliseconds = date.getTime();

          var dataObj = [milliseconds, Number(d.price)];
          return dataObj;
        })
    );

    const chartData1 = e.value.map((sr, i) => {
      return {
        name: sr.name,
        data: exampleData[i].splice(-150),
      };
    });

    this.setState({
      costDriverSeries: e.value,
      costDriverSeriesData: chartData1,
    });
  };

  onCostDriverChange = (e) => {
    //console.log("onCostDriverChange event ====>", e);
    this.setState({ costDriver: e.value });
  };

  render() {
    // const { order, icisAlertInfoData } = this.state;
    // console.log("order====>", order);
    // console.log("data ====>", icisAlertInfoData);

    // let orderdData = icisAlertInfoData.map(el=>)

    let date = new Date();
    let month = date.getMonth();
    let year = date.getFullYear();
    let month1 = Date.UTC(year, month, 1);
    let month6 = Date.UTC(year, month + 5, 1);

    const alertChartOptions = {
      chart: {
        zoomType: "x",
      },
      title: {
        text: "Alert Analysis",
      },
      subtitle: {
        // text: "Source: thesolarfoundation.com",
      },
      yAxis: {
        title: {
          text: "Actual Values",
        },
      },
      xAxis: {
        type: "datetime",
        title: {
          text: "Dates",
        },
        plotBands: [
          {
            color: "#C8FDFB",
            from: month1,
            to: month6,
          },
        ],
        // accessibility: {
        //   rangeDescription: "Range: 2010 to 2013",
        // },
      },
      legend: {
        // layout: "horizontal",
        // align: "center",
        // verticalAlign: "bottom",
      },
      tooltip: {
        //layout: 'horizontal',

        //align: 'center',

        //verticalAlign: 'bottom',

        valueDecimals: 2,

        formatter: function () {
          // var timeInterwal = $("#chart").val();

          // console.log("timeInterwal ===>", timeInterwal)

          // var chart = timeInterwal == "Weekly" ? 'Weekly Returns :  <b>' + this.point.weeklyReturn + '</b>' : 'Monthly Returns :  <b>' + this.point.monthly_returns + '</b>'

          // var Percentile = timeInterwal == "Weekly" ? ' </br> Percentile of Weekly Returns :  <b>' +

          //     this.point.weeklyPercentageReturn + '</b> ' : ' </br> Percentile of Monthly Returns :  <b>' +

          //     this.point.mothlyPercentageReturn + '</b> '

          return (
            "Date of Date (Icis Alert info) : " +
            new Date(this.x).toUTCString() +
            " </br> Actual Values :  <b>" +
            this.y +
            "</b> </br>"
          );
          //  + chart + Percentile
        },
      },

      plotOptions: {
        series: {
          label: {
            connectorAllowed: false,
          },
          pointStart: 2010,
        },
      },
      series: this.state.seriesData,
      responsive: {
        rules: [
          {
            condition: {
              maxWidth: 600,
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

    const costDriverAnalysisChart = {
      chart: {
        zoomType: "x",
      },
      title: {
        text: "Cost Driver Analysis",
      },

      yAxis: {
        title: {
          text: "Unit Price (USD)",
        },
      },

      xAxis: {
        title: {
          text: "Day of Date",
        },
        //categories: data2,
        plotBands: [
          {
            color: "#C8FDFB",
            from: month1,
            to: month6,
          },
        ],

        type: "datetime",
      },

      // legend: {
      //   layout: "horizontal",
      //   align: "center",
      //   verticalAlign: "bottom",
      // },

      tooltip: {
        // layout: "horizontal",
        // align: "center",
        // verticalAlign: "bottom",
        valueDecimals: 2,
        formatter: function () {
          //var timeInterwal = $("#chart").val();
          //console.log("timeInterwal ===>", timeInterwal)
          //var chart = timeInterwal == "Weekly" ? 'Weekly Returns :  <b>' + this.point.weeklyReturn + '</b>' : 'Monthly Returns :  <b>' + this.point.monthly_returns + '</b>'
          //var Percentile = timeInterwal == "Weekly" ? ' </br> Percentile of Weekly Returns :  <b>' +
          //    this.point.weeklyPercentageReturn + '</b> ' : ' </br> Percentile of Monthly Returns :  <b>' +
          //    this.point.mothlyPercentageReturn + '</b> '
          return (
            "Series name :  <b>" +
            this.series.name +
            "</b> </br> Avg Price :  <b>" +
            this.y +
            "</b> </br> Day of Date : <b>" +
            new Date(this.x).toUTCString() +
            "</b>"
          );
        },
      },

      series: this.state.costDriverSeriesData,

      responsive: {
        rules: [
          {
            condition: {
              maxWidth: 600,
            },
            chartOptions: {
              // legend: {
              //   layout: "horizontal",
              //   align: "center",
              //   verticalAlign: "bottom",
              // },
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

        <div style={{ display: "flex", margin: "5px 10px", float: "right" }}>
          <Button
            label="Costdriver Analysis"
            style={{ margin: "5px 10px" }}
            onClick={() => {
              this.setState({ costdriverChart: true });
            }}
          />

          <Button
            label="Alert Analysis"
            style={{ margin: "5px 10px" }}
            onClick={() => {
              this.setState({ costdriverChart: false });
            }}
          />
        </div>
        <div style={{ clear: "both" }}>
          {!this.state.costdriverChart && (
            <div className="card">
              <MultiSelect
                style={{ width: "95%" }}
                value={this.state.selectSerialName}
                options={this.seriesNameForAlert}
                onChange={(e) => this.onSeriesChange(e)}
                optionLabel="name"
                placeholder="Select a Series"
                display="chip"
              />

              <div style={{ width: "90%" }}>
                <HighchartsReact
                  highcharts={Highcharts}
                  options={alertChartOptions}
                />
              </div>
            </div>
          )}

          {this.state.costdriverChart && (
            <div className="card">
              <div style={{ display: "flex", margin: "5px 10px" }}>
                <MultiSelect
                  style={{ width: "49%", margin: "5px 10px" }}
                  value={this.state.costDriver}
                  options={this.costDrivers}
                  onChange={(e) => this.onCostDriverChange(e)}
                  optionLabel="name"
                  placeholder="Select a Cost Driver"
                  display="chip"
                />

                <MultiSelect
                  style={{ width: "49%", margin: "5px 10px" }}
                  value={this.state.costDriverSeries}
                  options={this.seriesName}
                  onChange={(e) => this.oncostDriverSeriesChange(e)}
                  optionLabel="name"
                  placeholder="Select a Series"
                  display="chip"
                />
              </div>

              <div style={{ width: "100%" }}>
                <HighchartsReact
                  highcharts={Highcharts}
                  options={costDriverAnalysisChart}
                />
              </div>
            </div>
          )}
        </div>
        <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Accuracy On Forcast Horizon-Monthly(percetage)</h4>
          <DataTable value={this.state.materialInfo}>
            <Column
              field="material"
              header="Coast Driver"
              style={{ width: "14%" }}
            />
            <Column field="type" header="Type" style={{ width: "14%" }} />
            <Column
              field="description"
              header="Series Name"
              style={{ width: "30%" }}
            />
            <Column field="group" header="March22" style={{ width: "14%" }} />
            <Column field="class" header="April22" style={{ width: "14%" }} />
            <Column
              field="criticality"
              header="May22"
              style={{ width: "14%" }}
            />
             <Column
              field="criticality"
              header="June22"
              style={{ width: "14%" }}
            />
             <Column
              field="criticality"
              header="July22"
              style={{ width: "14%" }}
            />
             <Column
              field="criticality"
              header="Aug22"
              style={{ width: "14%" }}
            />
          </DataTable>
        </div>
        <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Predicted Values On Forecast Horizon-Weekly</h4>
          <DataTable 
            value={this.state.inventoryInfo}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column field="Series Name" header="Series Name" />
            <Column field="Week" header="Alert Category" />
            <Column field="group" header="March22" style={{ width: "14%" }} />
            <Column field="class" header="April22" style={{ width: "14%" }} />
            <Column
              field="criticality"
              header="May22"
              style={{ width: "14%" }}
            />
             <Column
              field="criticality"
              header="June22"
              style={{ width: "14%" }}
            />
             <Column
              field="criticality"
              header="July22"
              style={{ width: "14%" }}
            />
             <Column
              field="criticality"
              header="Aug22"
              style={{ width: "14%" }}
            />
          </DataTable>
        </div>
      </div>
    );
  }
}
