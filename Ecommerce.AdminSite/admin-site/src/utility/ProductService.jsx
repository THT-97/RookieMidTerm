import axios from "axios";
const ProductService = {
  getPage(page = 0, limit = 6) {
    return axios.get(
      `https://localhost:7171/api/Product/GetPage?page=${page}&limit=${limit}`
    );
  },

  getCount() {
    return axios.get(`https://localhost:7171/api/Product/Count`);
  },

  getByID(id) {
    return axios.get(
      `https://localhost:7171/api/Product/AdminGetByID?id=${id}`
    );
  },

  create(entity) {
    return axios.post("https://localhost:7171/api/Product/Create", entity);
  },

  edit(id, entity) {
    return axios.put(
      `https://localhost:7171/api/Product/Update?id=${id}`,
      entity
    );
  }
};

export default ProductService;
