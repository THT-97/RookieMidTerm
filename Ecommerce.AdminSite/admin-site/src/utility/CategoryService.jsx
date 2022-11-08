import axios from "axios";
const CategoryService = {
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
    return axios.post("https://localhost:7171/api/Category/Create", entity);
  },

  edit(id, entity) {
    return axios.put(
      `https://localhost:7171/api/Category/Update?id=${id}`,
      entity
    );
  },

  delete(id) {
    return axios.delete(`https://localhost:7171/api/Category/Delete?id=${id}`);
  }
};
export default CategoryService;
