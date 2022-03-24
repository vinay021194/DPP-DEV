import axios from "axios";

export default class ProductService {
  getProductsSmall() {
    return axios
      .get("assets/demo/data/products-small.json")
      .then((res) => res.data.data)
      .catch((e) => console.log(e));
  }

  getProducts() {
    return axios
      .get("assets/demo/data/products.json")
      .then((res) => res.data.data)
      .catch((e) => console.log(e));
  }

  getProductsWithOrdersSmall() {
    return axios
      .get("assets/demo/data/products-orders-small.json")
      .then((res) => res.data.data)
      .catch((e) => console.log(e));
  }

  getGlobalProducts(params) {
    return axios
      .get("assets/demo/data/products-small", { params: params })
      .then((res) => res.data)
      .catch((e) => console.log(e));
  }
}
