import axios from "axios";
const ProductService = {
  getProducts(page = 0, limit = 6) {
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
  }
};

export default ProductService;
