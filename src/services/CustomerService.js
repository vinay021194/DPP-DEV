import axios from "axios";

export default class CustomerService {
  getCustomersSmall() {
    return axios
      .get("assets/demo/data/customers-small.json")
      .then((res) => res.data.data)
      .catch((e) => console.log(e));
  }

  getCustomersMedium() {
    return axios
      .get("assets/demo/data/customers-medium.json")
      .then((res) => res.data.data)
      .catch((e) => console.log(e));
  }

  getCustomersLarge() {
    return axios
      .get("assets/demo/data/customers-large.json")
      .then((res) => res.data.data)
      .catch((e) => console.log(e));
  }

  getCustomersXLarge() {
    return axios
      .get("assets/demo/data/customers-xlarge.json")
      .then((res) => res.data.data)
      .catch((e) => console.log(e));
  }

  getCustomers(params) {
    return axios
      .get("https://www.primefaces.org/data/customers", { params: params })
      .then((res) => res.data)
      .catch((e) => console.log(e));
  }
}
