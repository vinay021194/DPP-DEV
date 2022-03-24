import React, { Component } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import ProductService from "../services/ProductService";
import ProcService from "../services/ProcService";
import { Button } from "primereact/button";

export class FinalResult extends Component {
  constructor(props) {
    super(props);
    this.state = {
      costDriver: this.props.location.state.costDriver,
      seriesName: this.props.location.state.seriesName,
      previousProducts: this.props.location.state.previousProducts,
      plant: this.props.location.state.plant,
      editPageData: this.props.location.state.products,
      customers: this.props.location.state.customers,
      editPageMaterialInfo: this.props.location.state.materialInfo,
      InventoryInfo: this.props.location.state.InventoryInfo,
      demandUITable: this.props.location.state.demandUITable,
      products: [],
      materialInfo: [],
      supplierDetails: this.props.location.state
        ? this.props.location.state.supplierDetails
        : [],
    };

    this.productService = new ProductService();
    this.procService = new ProcService();
  }

  componentDidMount() {
    this.productService
      .getProductsSmall()
      .then((data) => this.setState({ products: data }));

    this.procService.getMaterialInfo({ material: 7001733 }).then((data) => {
      return this.setState({ materialInfo: data.data.data });
    });
  }

  back = () => {
    const {
      costDriver,
      seriesName,
      previousProducts,
      plant,
      editPageData,
      customers,
      editPageMaterialInfo,
      InventoryInfo,
      demandUITable,
    } = this.state;

    this.props.history.push("/EditOptimize", {
      costDriver,
      seriesName,
      previousProducts,
      plant,
      editPageData,
      customers,
      InventoryInfo,
      demandUITable,
      editPageMaterialInfo,
    });
  };

  render() {
    //console.log("state in final =====>", this.state);

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
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Ordering Schedule for Plant - 1000</h4>
          <DataTable
            value={this.state.products}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column field="code" header="Supler Name" />
            <Column field="quantity" header={`${month1}`} />
            <Column field="quantity" header={`${month2}`} />
            <Column field="quantity" header={`${month3}`} />
            <Column field="quantity" header={`${month4}`} />
            <Column field="quantity" header={`${month5}`} />
            <Column field="quantity" header={`${month6}`} />
            <Column field="quantity" header="Total Quantities From Supplier" />
          </DataTable>
        </div>

        <div className="card">
          <DataTable
            value={this.state.products}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column field="code" header="" />
            <Column field="quantity" header={`${month1}`} />
            <Column field="quantity" header={`${month2}`} />
            <Column field="quantity" header={`${month3}`} />
            <Column field="quantity" header={`${month4}`} />
            <Column field="quantity" header={`${month5}`} />
          </DataTable>
        </div>

        <div className="card">
          <DataTable value={this.state.products}>
            <Column field="code" header="Error Information" />
          </DataTable>
        </div>

        <div style={{ display: "flex", margin: "5px 10px", float: "right" }}>
          <Button
            label="Back"
            style={{ margin: "5px 10px" }}
            onClick={this.back}
          />
        </div>
      </div>
    );
  }
}
