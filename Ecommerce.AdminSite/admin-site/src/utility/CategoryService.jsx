import axios from "axios";
const CategoryService = {
  getAll() {
    return axios.get("https://localhost:7171/api/Category/GetAll");
  },

  create(entity) {
    return axios.post("https://localhost:7171/api/Category/Create", entity);
  }
};
export default CategoryService;
