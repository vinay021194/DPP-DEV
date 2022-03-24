import React, { Component } from "react";

import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { MultiSelect } from "primereact/multiselect";
import { AutoComplete } from "primereact/autocomplete";

import ProductService from "../services/ProductService";
import ProcService from "../services/ProcService";
import { Button } from "primereact/button";
import "./Optimization.css";

export class Optimization extends Component {
  constructor(props) {
    super(props);
    this.state = {
      materialCostDriverOutput: [],
      materialInfo: [],
      costDriver: "",
      costDrivers: [
        { name: "New York", code: "NY" },
        { name: "Rome", code: "RM" },
        { name: "London", code: "LDN" },
        { name: "Istanbul", code: "IST" },
        { name: "Paris", code: "PRS" },
      ],
      filteredCostDriver: [],
      selectedRepresentative: null,
    };

    this.representatives = [
      { name: "Amy Elsner", image: "amyelsner.png" },
      { name: "Anna Fali", image: "annafali.png" },
      { name: "Asiya Javayant", image: "asiyajavayant.png" },
      { name: "Bernardo Dominic", image: "bernardodominic.png" },
    ];

    this.productService = new ProductService();
    this.procService = new ProcService();
  }

  componentDidMount() {
    this.procService
      .getMaterialCostDriverOutput({ material: 7001733 })
      .then((data) =>
        this.setState({ materialCostDriverOutput: data.data.Sheet3 })
      );

    this.procService.getMaterialInfo({ material: 7001733 }).then((data) => {
      return this.setState({ materialInfo: data.data.data });
    });
  }

  searchCountry = (event) => {
    let countries = this.state.costDrivers;
    setTimeout(() => {
      let _filteredCountries;
      if (!event.query.trim().length) {
        _filteredCountries = [...countries];
      } else {
        _filteredCountries = countries.filter((country) => {
          return country.name
            .toLowerCase()
            .startsWith(event.query.toLowerCase());
        });
      }
      console.log("_filteredCountries ===>", _filteredCountries);
      this.setState({ filteredCostDriver: _filteredCountries });
    }, 250);
  };

  onRepresentativesChange = (e) => {
    console.log(e);

    this.setState({ selectedRepresentative: e.value });
    // this.dt.filter(e.value, "representative.name", "in");
  };

  onSave = () => {
    const { materialCostDriverOutput, materialInfo, filteredCostDriver } =
      this.state;
    this.props.history.push("/Forcast", {
      // data: materialCostDriverOutput,
      data: { materialInfo, filteredCostDriver, materialCostDriverOutput },
    });
  };

  render() {
    console.log("props ===>", this.props);

    // let months = [
    //   "Jan",
    //   "Feb",
    //   "Mar",
    //   "Apr",
    //   "May",
    //   "Jun",
    //   "Jul",
    //   "Aug",
    //   "Sep",
    //   "Oct",
    //   "Nov",
    //   "Dec",
    // ];
    // let date = new Date();
    // let month = date.getMonth();
    // let year = date.getFullYear();
    // let month1 =
    //   month > 11
    //     ? months[month % 11] + "-" + year + 1
    //     : months[month] + "-" + year;
    // let month2 =
    //   month + 1 > 11
    //     ? months[(month + 1) % 11] + "-" + year + 1
    //     : months[month + 1] + "-" + year;
    // let month3 =
    //   month + 2 > 11
    //     ? months[(month + 2) % 11] + "-" + year + 1
    //     : months[month + 2] + "-" + year;
    // let month4 =
    //   month + 3 > 11
    //     ? months[(month + 3) % 11] + "-" + year + 1
    //     : months[month + 3] + "-" + year;
    // let month5 =
    //   month + 4 > 11
    //     ? months[(month + 4) % 11] + "-" + year + 1
    //     : months[month + 4] + "-" + year;
    // let month6 =
    //   month + 5 > 11
    //     ? months[(month + 5) % 11] + "-" + year + 1
    //     : months[month + 5] + "-" + year;
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
          <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Price Prediction Cost Driver Output</h4>
          <DataTable
            value={this.state.materialCostDriverOutput}
            paginator
            rows={5}
            rowsPerPageOptions={[5, 10, 20]}
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          >
            <Column field="materialcode" header="Material Number" />
            <Column field="item_composition_in_english" header="Composition" />
            <Column field="concentration_percentage" header="Percentage" />
          </DataTable>
        </div>

        <div className="card">
          <MultiSelect
            value={this.state.selectedRepresentative}
            options={this.representatives}
            onChange={this.onRepresentativesChange}
            optionLabel="name"
            optionValue="name"
            placeholder="All"
            className="p-column-filter"
          />
          ;
        </div>

        <div style={{ display: "flex" }}>
          <div style={{ width: "66%" }}>
            <div className="card">
              <h4 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Price Prediction Cost Driver Output</h4>
              <DataTable
                value={this.state.materialCostDriverOutput}
                paginator
                rows={5}
                rowsPerPageOptions={[5, 10, 20]}
                paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
              >
                <Column field="materialcode" header="Material Number" />
                <Column
                  field="item_composition_in_english"
                  header="Composition"
                />
                <Column field="concentration_percentage" header="Percentage" />
              </DataTable>
            </div>
          </div>

          <div style={{ width: "33%" }}>
            <h5 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Cost Driver</h5>
            <AutoComplete
              // className="p-fluid"
              style={{ width: "100%" }}
              value={this.state.costDriver}
              suggestions={this.state.filteredCostDriver}
              completeMethod={this.searchCountry}
              onChange={(e) => this.setState({ costDriver: e.value.name })}
            />

            <MultiSelect
              style={{ width: "97%", margin: "10px 20px 10px 10px" }}
              value={this.state.costDriver}
              options={this.state.costDrivers}
              onChange={(e) => this.setState({ costDriver: e.value.name })}
              optionLabel="name"
              placeholder="Select a costDriver"
            />
          </div>
        </div>
        <Button label="Save" onClick={(e) => this.onSave()} />
      </div>
    );
  }
}
