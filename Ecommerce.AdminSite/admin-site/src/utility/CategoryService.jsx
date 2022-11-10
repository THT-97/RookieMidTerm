import axios from "axios";
import AxiosClient from "./AxiosClient";
const CategoryService = {
  // Normal axios instance is used for normal requests
  // AxiosClient is used for authorized requests
  getAll() {
    return axios.get(`https://localhost:7171/api/Category/GetAll`);
  },
  getPage(page = 0, limit = 6) {
    return axios.get(
      `https://localhost:7171/api/Category/GetPage?page=${page}&limit=${limit}`
    );
  },

  getCount() {
    return axios.get(`https://localhost:7171/api/Category/Count`);
  },

  getById(id) {
    return axios.get(`https://localhost:7171/api/Category/GetById?id=${id}`);
  },

  create(entity) {
    return AxiosClient.post("Category/Create", entity);
  },

  edit(id, entity) {
    return AxiosClient.put(`Category/Update?id=${id}`, entity);
  },

  delete(id) {
    return AxiosClient.delete(`Category/Delete?id=${id}`);
  }
};
export default CategoryService;
