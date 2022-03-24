// import React, { useState, useEffect } from "react";
// import ProductService from "../services/ProductService";
// import ssCharts from "soothsayercharts";

// export const BarCharts = () => {
//   const productService = new ProductService();
//   const [products, setProducts] = useState([]);

//   useEffect(() => {
//     productService.getProductsSmall().then((data) => setProducts(data));
//   }, []);

//   const data = () => {
//     return products.map((col) => {
//       return { name: col.name, value: col.price };
//     });
//   };

//   // const scatterData = () => {
//   //   return products.map((col) => {
//   //     let name = col.name;
//   //     let x = col.price;
//   //     let y = col.quantity;
//   //     return { name, x, y };
//   //   });
//   // };
//   // console.log("scatterData ===> ", { scatterData });

//   // const data = () => {
//   //   return "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
//   //     .split("")
//   //     .map((name) => {
//   //       let value = Math.random();
//   //       return { name, value };
//   //     })
//   //     .filter((p) => {
//   //       // if(p.value*100<20)
//   //       //     console.log(p);
//   //       return p.value * 100 > 20;
//   //     })
//   //     .sort((a, b) => a.value - b.value);
//   // };

//   // const scatterData = () => {
//   //   return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("").map((name) => {
//   //     let x = Math.random();
//   //     let y = Math.round(Math.random() * 1000);
//   //     return { name, x, y };
//   //   });
//   // };

//   ssCharts.barChart({
//     element: document.getElementById("barChart"),
//     data: data(),
//     height: 400,
//     width: 1000,
//     color: "steelblue",
//     tooltip: false,
//     xAxis: {
//       label: "Products Name",
//     },
//     yAxis: {
//       label: "Products Price",
//     },
//     margin: {
//       top: 20,
//       right: 20,
//       bottom: 20,
//       left: 40,
//     },
//     backgroundColor: "#f4f4f4",
//     barPadding: 3,
//   });
//   // ssCharts.lineChart({
//   //   element: document.getElementById("lineChart"),
//   //   data: data(),
//   //   height: 400,
//   //   width: 1000,
//   //   color: "orange",
//   //   tooltip: {
//   //     format: "{{name}} = {{value}}",
//   //   },
//   //   xAxis: {
//   //     label: "Occurances",
//   //   },
//   //   margin: {
//   //     top: 20,
//   //     right: 20,
//   //     bottom: 20,
//   //     left: 40,
//   //   },
//   //   backgroundColor: "#f4f4f4",
//   //   barPadding: 1,
//   // });
//   // ssCharts.scatterChart({
//   //   element: document.getElementById("scatterChart"),
//   //   data: scatterData(),
//   //   height: 400,
//   //   width: 1000,
//   //   color: "green",
//   //   tooltip: {
//   //     format: "{{name}} = ({{x}},{{y}})",
//   //   },
//   //   xAxis: {
//   //     label: "Probability",
//   //   },
//   //   yAxis: {
//   //     label: "Output",
//   //   },
//   //   margin: {
//   //     top: 20,
//   //     right: 20,
//   //     bottom: 20,
//   //     left: 40,
//   //   },
//   //   backgroundColor: "#f4f4f4",
//   //   barPadding: 1,
//   // });

//   return (
//     <>
//       <div id="barChart"></div>
//       {/* <div id="lineChart"></div>
//       <div id="scatterChart"></div> */}
//     </>
//   );

//   /*ssCharts.lineChart({
//         element: document.getElementById('lineChart2'),
//         data: data(),
//         height: 400,
//         width: 400,
//         color: "lightgreen",
//         tooltip:{
//             format:"{{name}} = {{value}}"
//         },
//         xAxis:{
//             label:"Occurances"
//         },
//         backgroundColor:"#f4f4f4",
//         barPadding: 1,
//     });*/
// };
