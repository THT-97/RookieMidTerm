import axios from "axios";
import AxiosClient from "./AxiosClient";
const ProductService = {
  // Normal axios instance is used for normal requests
  // AxiosClient is used for authorized requests
  getPage(page = 0, limit = 6) {
    return AxiosClient.get(`Product/GetPage?page=${page}&limit=${limit}`);
  },

  getCount() {
    return axios.get(`https://localhost:7171/api/Product/Count`);
  },

  getByID(id) {
    return AxiosClient.get(`Product/AdminGetByID?id=${id}`);
  },

  create(entity) {
    return AxiosClient.post("Product/Create", entity);
  },

  edit(id, entity) {
    return AxiosClient.put(`Product/Update?id=${id}`, entity);
  },

  delete(id) {
    return AxiosClient.delete(`Product/Delete?id=${id}`);
  }
};

export default ProductService;
