import axios from "axios";
const CategoryService = {
  getPage(page = 0, limit = 6) {
    return axios.get(
      `https://localhost:7171/api/Category/GetPage?page=${page}&limit=${limit}`
    );
  },

  getCount() {
    return axios.get(`https://localhost:7171/api/Category/Count`);
  },

  create(entity) {
    return axios.post("https://localhost:7171/api/Category/Create", entity);
  }
};
export default CategoryService;
