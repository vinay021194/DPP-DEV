import React, { Component } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import ProductService from "../services/ProductService";
import ProcService from "../services/ProcService";
import Highcharts from "highcharts";
import HighchartsReact from "highcharts-react-official";
import { MultiSelect } from "primereact/multiselect";
export class CategoryView extends Component {
  constructor(props) {
    super(props);
    this.state = {
      inventoryInfo: [],
      demandUITable: [],
      materialInfo: [],
      demandInfoRegressionSummaryTable: [],
      HistoricalConsumptionSeriesData: [],
      plants: [],
    };

    this.productService = new ProductService();
    this.procService = new ProcService();

    this.plants = [
      { label: "1200", value: "1200" },
      { label: "1500", value: "1500" },
      { label: "1800", value: "1800" },
      { label: "2000", value: "2000" },
      { label: "3300", value: "3300" },
      { label: "4100", value: "4100" },
    ];
  }

  componentDidMount() {
    this.procService
      .getInventoryInfo({ material: 7001733 })
      .then((data) => this.setState({ inventoryInfo: data.data.Sheet2 }));

    this.procService.getDemandUITable({ material: 7001733 }).then((data) => {
      return this.setState({ demandUITable: data.data.Sheet2 });
    });

    this.procService.getMaterialInfo({ material: 7001733 }).then((data) => {
      return this.setState({ materialInfo: data.data.data });
    });

    this.procService
      .getDemandInfoRegressionSummaryTable({ material: 7001733 })
      .then((data) => {
        return this.setState({
          demandInfoRegressionSummaryTable: data.data.data,
        });
      });
  }

  onPlantChange = (e) => {
    //console.log("event ===>", e);

    const { demandInfoRegressionSummaryTable } = this.state;

    // console.log(
    //   "demandInfoRegressionSummaryTable in map ===>",
    //   demandInfoRegressionSummaryTable
    // );

    let convertedData = demandInfoRegressionSummaryTable.map((el) => {
      let date = new Date(el.period);
      let milliseconds = date.getTime();

      return {
        executedOn: el.executed_on,
        plant: el.plant,
        x: milliseconds,
        y: Number(el.quantity),
        total_cons_converted_mp_level: el.total_cons_converted_mp_level,
      };
    });

    //console.log("convertedData in map ===>", convertedData);

    let exampleData = e.value.map((sr) =>
      convertedData.filter((el) => el.plant === sr)
    );

    //console.log("exampleData in map ===>", exampleData);

    const chartData1 = e.value.map((sr, i) => {
      return {
        name: sr,
        data: exampleData[i],
      };
    });

    this.setState({
      plants: e.value,
      HistoricalConsumptionSeriesData: chartData1,
    });
  };

  render() {
    // console.log("state Data  =>", this.state);
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
      "Dec",
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

    const chart3 = {
      chart: {
        zoomType: "x",
      },

      title: {
        text: "Historical Consumption and Forcasted Demand ",
      },
      subtitle: {
        // text: "Source: thesolarfoundation.com",
      },
      yAxis: {
        title: {
          text: "Quantity",
        },
      },
      xAxis: {
        title: {
          text: "Period",
        },
        plotBands: [
          {
            color: "#C8FDFB",
            from: month1,
            to: month6,
          },
        ],

        type: "datetime",
      },
      legend: {
        layout: "horizontal",
        align: "center",
        verticalAlign: "bottom",
      },
      tooltip: {
        //layout: 'horizontal',
        //align: 'center',
        //verticalAlign: 'bottom',
        formatter: function () {
          return (
            "Color :  <b>" +
            this.point.color +
            "</b> </br> Executed on :  <b>" +
            this.point.executedOn +
            "</b> </br>  Period : <b>" +
            new Date(this.x).toUTCString() +
            " </b> </br> Plant :  <b>" +
            this.series.name +
            "</b> </br> Quantity :  <b>" +
            this.y +
            "</b>"
          );
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
      series: this.state.HistoricalConsumptionSeriesData,
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
        {/* <div className="card">
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
        </div> */}

        <div className="card">
          {/* <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Inventory Analysis</h4> */}
          <DataTable 
            value={this.state.inventoryInfo}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column field="Material Number" header="Material Number" />
            <Column field="Alert Category" header="Alert Category" />
            <Column field="ROP" header="ROP" />
            <Column
              field="Average Anual consumption"
              header="Average Anual consumption"
            />
            <Column field="Potential Consumption" header="Potential Consumption" />
            <Column field="Material Type" header="Material Type" />
             <Column field="UOM" header="UOM" />
             <Column field="PR Qty" header="PR Qty" />
             <Column field="On Route Qty" header="On Route Qty" />
             <Column field="Forcasted Period" header="Forcasted Period" />
             <Column field="Action" header="Action" />
          </DataTable>
        </div>

        {/* <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Demand Prediction for next 6 months across all plants</h4>
          <DataTable
            value={this.state.demandUITable}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column field="plant" header="Plant" />
            <Column
              field="avg_total_consumption"
              header="Avg Annual Consumption"
            />
            <Column field="5/1/21" header={`${month1}`} />
            <Column field="6/1/21" header={`${month2}`} />
            <Column field="7/1/21" header={`${month3}`} />
            <Column field="8/1/21" header={`${month4}`} />
            <Column field="9/1/21" header={`${month5}`} />
            <Column field="10/1/21" header={`${month6}`} />
            <Column field="prediction_error" header="Prediction Accuracy" />
          </DataTable>
        </div> */}

        {/* <div className="card">
          <div>
            <div>
              <MultiSelect
                style={{ width: "99.9%" }}
                value={this.state.plants}
                options={this.plants}
                onChange={(e) => this.onPlantChange(e)}
                optionLabel="label"
                placeholder="Select a Plant"
                display="chip"
              />
            </div>
          </div>

          <div style={{ width: "99%" }}>
            <HighchartsReact highcharts={Highcharts} options={chart3} />
          </div>
        </div> */}
      </div>
    );
  }
}
