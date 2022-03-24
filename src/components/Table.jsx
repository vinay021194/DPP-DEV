import React, { useState, useEffect, useRef } from "react";
import { Toolbar } from "primereact/toolbar";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import ProductService from "../services/ProductService";
import { Rating } from "primereact/rating";
import { Button } from "primereact/button";
import { Toast } from "primereact/toast";
import { MultiSelect } from "primereact/multiselect";
import { InputText } from "primereact/inputtext";
// import "./DataTableDemo.css";

export const Table = () => {
  const [products, setProducts] = useState([]);
  const [expandedRows, setExpandedRows] = useState(null);
  const [selectedProducts, setSelectedProducts] = useState([]);
  const [selectedRepresentative, setSelectedRepresentative] = useState(null);
  const [globalFilter, setGlobalFilter] = useState(null);
  const dt = useRef(null);
  const toast = useRef(null);
  const isMounted = useRef(false);
  const productService = new ProductService();

  // useEffect(() => {
  //   if (isMounted.current) {
  //     const summary =
  //       expandedRows !== null ? "All Rows Expanded" : "All Rows Collapsed";
  //     //   toast.current.show({
  //     //     severity: "success",
  //     //     summary: `${summary}`,
  //     //     life: 3000,
  //     //   });
  //   }
  // }, [expandedRows]);

  useEffect(() => {
    isMounted.current = true;
    productService
      .getProductsWithOrdersSmall()
      .then((data) => {
        return setProducts(data);
      })
      .catch((e) => console.log(e));
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  //   const onRowExpand = (event) => {
  //     toast.current.show({
  //       severity: "info",
  //       summary: "Product Expanded",
  //       detail: event.data.name,
  //       life: 3000,
  //     });
  //   };

  //   const onRowCollapse = (event) => {
  //     toast.current.show({
  //       severity: "success",
  //       summary: "Product Collapsed",
  //       detail: event.data.name,
  //       life: 3000,
  //     });
  //   };

  const expandAll = () => {
    let _expandedRows = {};
    products.forEach((p) => (_expandedRows[`${p.id}`] = true));

    setExpandedRows(_expandedRows);
  };

  const collapseAll = () => {
    setExpandedRows(null);
  };

  const formatCurrency = (value) => {
    return value.toLocaleString("en-US", {
      style: "currency",
      currency: "USD",
    });
  };

  const amountBodyTemplate = (rowData) => {
    return formatCurrency(rowData.amount);
  };

  const statusOrderBodyTemplate = (rowData) => {
    return (
      <span className={`order-badge order-${rowData.status.toLowerCase()}`}>
        {rowData.status}
      </span>
    );
  };

  // const searchBodyTemplate = () => {
  //   return <Button icon="pi pi-search" />;
  // };

  // const imageBodyTemplate = (rowData) => {
  //   return (
  //     <img
  //       src={`showcase/demo/images/product/${rowData.image}`}
  //       onError={(e) =>
  //         (e.target.src =
  //           "https://www.primefaces.org/wp-content/uploads/2020/05/placeholder.png")
  //       }
  //       alt={rowData.image}
  //       className="product-image"
  //     />
  //   );
  // };

  const onRepresentativesChange = (e) => {
    setSelectedRepresentative(e.value);
    console.log("dt.current ==> ", dt.current);
    // dt.current.filter(e.value, "representative.name", "in");

    // const arr1 = [{id:'1',name:'A'},{id:'2',name:'B'},{id:'3',name:'C'},{id:'4',name:'D'}];
    // const arr2 = [{id:'1',name:'A',state:'healthy'},{id:'3',name:'C',state:'healthy'}];
    // const filterByReference = (arr1, arr2) => {
    //    let res = [];
    //    res = arr1.filter(el => {
    //       return !arr2.find(element => {
    //          return element.id === el.id;
    //       });
    //    });
    //    return res;
    // }
    // console.log(filterByReference(arr1, arr2));
  };

  const priceBodyTemplate = (rowData) => {
    return formatCurrency(rowData.price);
  };

  const ratingBodyTemplate = (rowData) => {
    return <Rating value={rowData.rating} readOnly cancel={false} />;
  };

  const statusBodyTemplate = (rowData) => {
    return (
      <span
        className={`product-badge status-${rowData.inventoryStatus.toLowerCase()}`}
      >
        {rowData.inventoryStatus}
      </span>
    );
  };

  const rowExpansionTemplate = (data) => {
    console.log(data);
    return (
      <div className="orders-subtable">
        <h5 style={{ fontWeight:"bolder", fontFamily:'revert' }}>Orders for {data.name}</h5>
        <DataTable value={data.orders}>
          <Column field="id" header="Id" sortable></Column>
          <Column field="customer" header="Customer" sortable></Column>
          <Column field="date" header="Date" sortable></Column>
          <Column
            field="amount"
            header="Amount"
            body={amountBodyTemplate}
            sortable
          ></Column>
          <Column
            field="status"
            header="Status"
            body={statusOrderBodyTemplate}
            sortable
          ></Column>
          {/* <Column
            headerStyle={{ width: "4rem" }}
            body={searchBodyTemplate}
          ></Column> */}
        </DataTable>
      </div>
    );
  };

  const cols = [
    // { field: 'code', header: 'Code' },
    { field: "name", header: "Name" },
    { field: "price", header: "Price" },
    { field: "category", header: "Category" },
    { field: "rating", header: "Reviews" },
    { field: "inventoryStatus", header: "Status" },
    // { field: 'quantity', header: 'Quantity' }
  ];

  const exportColumns = cols.map((col) => ({
    title: col.header,
    dataKey: col.field,
  }));

  // const exportCSV = (selectionOnly) => {
  //   console.log("selectionOnly ===> ", selectionOnly);
  //   console.log("dt.current ===> ", dt.current);
  //   // dt.current.exportCSV({ selectionOnly });
  // };

  const exportPdf = () => {
    import("jspdf").then((jsPDF) => {
      import("jspdf-autotable").then(() => {
        const doc = new jsPDF.default(0, 0);
        doc.autoTable(exportColumns, products);
        doc.save("products.pdf");
      });
    });
  };

  const exportExcel = () => {
    import("xlsx").then((xlsx) => {
      const worksheet = xlsx.utils.json_to_sheet(products);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array",
      });
      saveAsExcelFile(excelBuffer, "products");
    });
  };

  const saveAsExcelFile = (buffer, fileName) => {
    import("file-saver").then((FileSaver) => {
      let EXCEL_TYPE =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8";
      let EXCEL_EXTENSION = ".xlsx";
      const data = new Blob([buffer], {
        type: EXCEL_TYPE,
      });
      FileSaver.saveAs(
        data,
        fileName + "_export_" + new Date().getTime() + EXCEL_EXTENSION
      );
    });
  };

  const representatives = [
    { name: "INSTOCK", image: "amyelsner.png" },
    { name: "LOWSTOCK", image: "annafali.png" },
    { name: "OUTOFSTOCK", image: "asiyajavayant.png" },
  ];

  // const representativesItemTemplate = (option) => {
  //   return (
  //     <div className="p-multiselect-representative-option">
  //       {/* <img alt={option.name} src={`showcase/demo/images/avatar/${option.image}`} onError={(e) => e.target.src='https://www.primefaces.org/wp-content/uploads/2020/05/placeholder.png'} width={32} style={{verticalAlign: 'middle'}} /> */}
  //       <span className="image-text">{option.name}</span>
  //     </div>
  //   );
  // };

  const header = (
    <div className="table-header-container">
      <Button
        icon="pi pi-plus"
        label="Expand All"
        onClick={expandAll}
        className="p-mr-2"
      />
      <Button icon="pi pi-minus" label="Collapse All" onClick={collapseAll} />
    </div>
  );

  const leftToolbarTemplate = () => {
    return (
      <React.Fragment>
        {/* <Button
          type="button"
          icon="pi pi-file-o"
          onClick={() => exportCSV(false)}
          className="p-mr-2"
          tooltip="CSV"
        /> */}
        <Button
          type="button"
          icon="pi pi-file-excel"
          onClick={exportExcel}
          className="p-button-success p-mr-2"
          tooltip="XLS"
        />
        <Button
          type="button"
          icon="pi pi-file-pdf"
          onClick={exportPdf}
          className="p-button-warning p-mr-2"
          tooltip="PDF"
        />
        {/* <Button
          type="button"
          icon="pi pi-filter"
          onClick={() => exportCSV(true)}
          className="p-button-info p-ml-auto p-mr-2"
          tooltip="Selection Only"
        /> */}
      </React.Fragment>
    );
  };

  const rightToolbarTemplate = () => {
    return (
      <div className="p-input-icon-left">
        <i className="pi pi-search" />
        <InputText
          type="search"
          onInput={(e) => setGlobalFilter(e.target.value)}
          placeholder="Search..."
        />
      </div>
    );
  };

  // const representativeFilter = (
  //   <MultiSelect
  //     value={selectedRepresentative}
  //     options={representatives}
  //     onChange={onRepresentativesChange}
  //     optionLabel="name"
  //     placeholder="All"
  //   />
  // );

  console.log("selectedRepresentative", selectedRepresentative);
  return (
    <div className="datatable-rowexpansion-demo">
      <Toast ref={toast} />
      <Toolbar
        className="p-mb-4"
        left={leftToolbarTemplate}
        right={rightToolbarTemplate}
      ></Toolbar>
      <div className="card">
        <DataTable
          value={products}
          expandedRows={expandedRows}
          paginator
          rows={5}
          rowsPerPageOptions={[5, 10, 20]}
          paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          onRowToggle={(e) => setExpandedRows(e.data)}
          //   onRowExpand={onRowExpand}
          //   onRowCollapse={onRowCollapse}
          rowExpansionTemplate={rowExpansionTemplate}
          dataKey="id"
          header={header}
          selectionMode="multiple"
          selection={selectedProducts}
          onSelectionChange={(e) => setSelectedProducts(e.value)}
          globalFilter={globalFilter}
        >
          <Column expander style={{ width: "3em" }} />
          <Column field="name" header="Name" sortable filter />
          {/* <Column header="Image" body={imageBodyTemplate} /> */}
          <Column
            field="price"
            header="Price"
            sortable
            filter
            body={priceBodyTemplate}
          />
          <Column field="category" header="Category" sortable filter />
          <Column
            field="rating"
            header="Reviews"
            sortable
            filter
            body={ratingBodyTemplate}
            // filterElement={representativeFilter}
          />
          <Column
            field="inventoryStatus"
            header="Status"
            sortable
            filter
            body={statusBodyTemplate}
            // filterElement={representativeFilter}
          />
        </DataTable>
      </div>
    </div>
  );
};
