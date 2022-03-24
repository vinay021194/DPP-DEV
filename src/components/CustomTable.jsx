import React, { useState, useEffect } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import ProductService from "../services/ProductService";
import { MultiSelect } from "primereact/multiselect";

export const CustomTable = () => {
  const columns = [
    { field: "name", header: "Name" },
    { field: "category", header: "Category" },
    { field: "price", header: "Price" },
    { field: "quantity", header: "Quantity" },
    { field: "inventoryStatus", header: "Inventory Status" },
    { field: "rating", header: "Rating" },
  ];

  const [selectedColumns, setSelectedColumns] = useState(columns);
  const [products, setProducts] = useState([]);
  const productService = new ProductService();

  useEffect(() => {
    productService.getProductsSmall().then((data) => setProducts(data));
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  const onColumnToggle = (event) => {
    let selectedColumns = event.value;
    let orderedSelectedColumns = columns.filter((col) =>
      selectedColumns.some((sCol) => sCol.field === col.field)
    );
    setSelectedColumns(orderedSelectedColumns);
  };

  const header = (
    <div style={{ textAlign: "left" }}>
      <MultiSelect
        value={selectedColumns}
        options={columns}
        optionLabel="header"
        onChange={onColumnToggle}
        style={{ width: "20em" }}
      />
    </div>
  );

  const columnComponents = selectedColumns.map((col) => {
    return (
      <Column
        key={col.field}
        field={col.field}
        header={col.header}
        sortable
        // filter
      />
    );
  });

  return (
    <div>
      <div className="card">
        <DataTable
          className="p-datatable-sm"
          value={products}
          header={header}
          paginator
          rows={5}
          rowsPerPageOptions={[5, 10, 20]}
          paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
        >
          <Column field="code" header="Code" />
          {columnComponents}
        </DataTable>
      </div>
    </div>
  );
};
