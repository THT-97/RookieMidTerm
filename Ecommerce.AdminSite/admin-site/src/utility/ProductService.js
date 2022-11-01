import axios from "axios";
const ProductService = {
  getProducts(page = 0, limit = 6) {
    return axios.get(
      `https://localhost:7171/api/Product/GetPage?page=${page}&limit=${limit}`
    );
  }
};

export default ProductService;
