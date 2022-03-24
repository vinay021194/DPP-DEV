import React, { Component } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { InputText } from "primereact/inputtext";
// import { InputNumber } from "primereact/inputnumber";
import { Button } from "primereact/button";

import ProductService from "../services/ProductService";
import CustomerService from "../services/CustomerService";
import ProcService from "../services/ProcService";

export class EditOptimize extends Component {
  constructor(props) {
    super(props);
    this.state = {
      costDriver: this.props.location.state
        ? this.props.location.state.costDriver
        : null,
      seriesName: this.props.location.state
        ? this.props.location.state.seriesName
        : [],
      previousProducts: this.props.location.state
        ? this.props.location.state.products
        : [],
      plant: this.props.location.state ? this.props.location.state.plant : null,
      products: [],
      customers: [],
      materialInfo: [],
      InventoryInfo: [],
      demandUITable: [],
      supplierDetails: this.props.location.state
        ? this.props.location.state.supplierDetails
        : [],
    };
    this.editingCellRows = {};
    this.originalRows = {};
    this.originalRows2 = {};
    this.productService = new ProductService();
    this.customerService = new CustomerService();
    this.procService = new ProcService();
  }

  componentDidMount() {
    this.productService
      .getProductsSmall()
      .then((data) => this.setState({ products: data }));

    this.customerService
      .getCustomersMedium()
      .then((data) => this.setState({ customers: data }));

    this.procService.getMaterialInfo({ material: 7001733 }).then((data) => {
      return this.setState({ materialInfo: data.data.data });
    });

    this.procService.getInventoryInfo({ material: 7001733 }).then((data) => {
      return this.setState({ InventoryInfo: data.data.Sheet2 });
    });

    this.procService.getDemandUITable({ material: 7001733 }).then((data) => {
      return this.setState({ demandUITable: data.data.Sheet2 });
    });
  }

  onRowEditInit = (event) => {
    const { data, InventoryInfo } = this.state;

    // console.log("event in edit =====>", event);
    const filterData = data
      ? InventoryInfo.filter((d) => d.plant === data?.data?.plant)
      : InventoryInfo;
    this.originalRows[event.index] = { ...filterData[event.index] };
  };

  onRowEditCancel = (event) => {
    const { data, InventoryInfo } = this.state;

    // console.log("event in cancel =====>", event);
    const filterData = data
      ? InventoryInfo.filter((d) => d.plant === data?.data?.plant)
      : InventoryInfo;
    let products = [...filterData];
    products[event.index] = this.originalRows[event.index];
    delete this.originalRows[event.index];

    this.setState({ InventoryInfo: products });
  };

  onRowEditInit1 = (event) => {
    // console.log("event in edit1 =====>", event);

    this.originalRows[event.index] = {
      ...this.state.demandUITable[event.index],
    };
  };

  onRowEditCancel1 = (event) => {
    // console.log("event in cancel1 =====>", event);

    let products = [...this.state.demandUITable];
    products[event.index] = this.originalRows[event.index];
    delete this.originalRows[event.index];

    this.setState({ demandUITable: products });
  };

  onRowEditInit2 = (event) => {
    // console.log("event in edit2 =====>", event);

    this.originalRows[event.index] = { ...this.state.customers[event.index] };
  };

  onRowEditCancel2 = (event) => {
    // console.log("event in cancel2 =====>", event);

    let products = [...this.state.customers];
    products[event.index] = this.originalRows[event.index];
    delete this.originalRows[event.index];

    this.setState({ customers: products });
  };

  onEditorValueChange = (productKey, props, value) => {
    let updatedProducts = [...props.value];
    updatedProducts[props.rowIndex][props.field] = value;
    this.setState({ [`${productKey}`]: updatedProducts });
  };

  inputTextEditor = (productKey, props, field) => {
    //console.log("productKey, props, field =====>", productKey, props, field);
    return (
      <InputText
        style={{ width: "100%" }}
        type="text"
        value={props.rowData[field]}
        onChange={(e) =>
          this.onEditorValueChange(productKey, props, e.target.value)
        }
      />
    );
  };

  codeEditor = (productKey, props, field) => {
    return this.inputTextEditor(productKey, props, field);
  };

  back = () => {
    const {
      costDriver,
      seriesName,
      previousProducts,
      plant,
      supplierDetails,
      products,
    } = this.state;

    this.props.history.push("/Forcast", {
      // data: materialCostDriverOutput,
      costDriver,
      seriesName,
      previousProducts,
      plant,
      supplierDetails,
      products,
    });
  };

  optimize = () => {
    const {
      products,
      customers,
      materialInfo,
      InventoryInfo,
      demandUITable,
      costDriver,
      seriesName,
      previousProducts,
      plant,
    } = this.state;

    this.props.history.push("/FinalResult", {
      costDriver,
      seriesName,
      previousProducts,
      plant,
      products,
      customers,
      materialInfo,
      InventoryInfo,
      demandUITable,
    });
  };

  render() {
    const {
      plant,
      InventoryInfo,

      supplierDetails,
    } = this.state;

    //console.log("state in Edit =====>", this.state);
    //console.log("props in Edit =====>", this.props);

    const filterData =
      (plant != null || plant !== undefined) && InventoryInfo
        ? InventoryInfo.filter((d) => d.plant === plant)
        : InventoryInfo;

    let modifiedSupplierDetails = [];
    supplierDetails.map((element) => {
      return modifiedSupplierDetails.push(
        element.forecastedObj,
        element.supplierMaxCapacity,
        element.leadTimeObj
      );
    });

    //console.log("modifiedSupplierDetails  =====>", modifiedSupplierDetails);

    // const filterBySeries =
    //   seriesName != null || seriesName !== undefined
    //     ? seriesName.map((d) =>
    //         icisForecastErrorInfoUpdated.filter((el) => el.series === d.name)
    //       )
    //     : [];
    // const concatFilterBySeries =
    //   data != null || data !== undefined ? [].concat(...filterBySeries) : [];
    // const d1 = data ? concatFilterBySeries : icisForecastErrorInfoUpdated;

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
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}> Plant Data (in Ton) {plant && plant}</h4>
          <DataTable
            value={filterData}
            editMode="row"
            dataKey="id"
            onRowEditInit={this.onRowEditInit}
            onRowEditCancel={this.onRowEditCancel}
          >
            <Column
              field="warehouse_capacity"
              header="Warehouse Capacity(Tons)"
              editor={(props) =>
                this.codeEditor("filterData", props, "warehouse_capacity")
              }
            />

            <Column
              field="safety_stock"
              header="Safety Stock"
              editor={(props) =>
                this.codeEditor("filterData", props, "safety_stock")
              }
            />
            <Column
              field="opening_stock"
              header="Closing Stock"
              editor={(props) =>
                this.codeEditor("filterData", props, "opening_stock")
              }
            />
            <Column
              rowEditor
              headerStyle={{ width: "7rem" }}
              bodyStyle={{ textAlign: "center", width: "7rem" }}
            ></Column>
          </DataTable>
        </div>

        <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Demand Prediction for next 6 months across all plants</h4>
          <DataTable
            editMode="row"
            dataKey="id"
            onRowEditInit={this.onRowEditInit1}
            onRowEditCancel={this.onRowEditCancel1}
            value={this.state.demandUITable}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column
              field="avg_total_consumption"
              header="Demand Details"
              editor={(props) =>
                this.codeEditor("filterData", props, "avg_total_consumption")
              }
            />
            <Column
              field="5/1/21"
              header={`${month1}`}
              editor={(props) =>
                this.codeEditor("demandUITable", props, "5/1/21")
              }
            />
            <Column
              field="6/1/21"
              header={`${month2}`}
              editor={(props) =>
                this.codeEditor("demandUITable", props, "6/1/21")
              }
            />
            <Column
              field="7/1/21"
              header={`${month3}`}
              editor={(props) =>
                this.codeEditor("demandUITable", props, "7/1/21")
              }
            />
            <Column
              field="8/1/21"
              header={`${month4}`}
              editor={(props) =>
                this.codeEditor("demandUITable", props, "8/1/21")
              }
            />
            <Column
              field="9/1/21"
              header={`${month5}`}
              editor={(props) =>
                this.codeEditor("demandUITable", props, "9/1/21")
              }
            />
            <Column
              field="10/1/21"
              header={`${month6}`}
              editor={(props) =>
                this.codeEditor("demandUITable", props, "10/1/21")
              }
            />
            <Column
              rowEditor
              headerStyle={{ width: "7rem" }}
              bodyStyle={{ textAlign: "center", width: "7rem" }}
            ></Column>
          </DataTable>
        </div>

        <div className="card">
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Plant Data</h4>
          <DataTable
            value={modifiedSupplierDetails}
            rowGroupMode="rowspan"
            groupField="name"
            sortMode="single"
            sortField="name"
            sortOrder={1}
            editMode="row"
            dataKey="id"
            onRowEditInit={this.onRowEditInit2}
            onRowEditCancel={this.onRowEditCancel2}
          >
            <Column
              field="name"
              header="Supplier Name"
              editor={(props) =>
                this.codeEditor("supplierDetails", props, "name")
              }
            />
            <Column
              field="field"
              header="Field"
              editor={(props) =>
                this.codeEditor("supplierDetails", props, "field")
              }
            />
            <Column
              field="month1"
              header={`${month1}`}
              editor={(props) =>
                this.codeEditor("supplierDetails", props, "month1")
              }
            />
            <Column
              field="month2"
              header={`${month2}`}
              editor={(props) =>
                this.codeEditor("supplierDetails", props, "month2")
              }
            />
            <Column
              field="month3"
              header={`${month3}`}
              editor={(props) =>
                this.codeEditor("supplierDetails", props, "month3")
              }
            />
            <Column
              field="month4"
              header={`${month4}`}
              editor={(props) =>
                this.codeEditor("supplierDetails", props, "month4")
              }
            />
            <Column
              field="month5"
              header={`${month5}`}
              editor={(props) =>
                this.codeEditor("supplierDetails", props, "month5")
              }
            />
            <Column
              field="month6"
              header={`${month6}`}
              editor={(props) =>
                this.codeEditor("supplierDetails", props, "month6")
              }
            />
            <Column
              rowEditor
              headerStyle={{ width: "7rem" }}
              bodyStyle={{ textAlign: "center", width: "7rem" }}
            ></Column>
          </DataTable>
        </div>

        <div style={{ display: "flex", margin: "5px 10px", float: "right" }}>
          {/* <Button
            label="Back"
            style={{ margin: "5px 10px" }}
            onClick={this.back}
          /> */}

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
