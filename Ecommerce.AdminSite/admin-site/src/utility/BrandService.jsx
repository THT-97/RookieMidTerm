import axios from "axios";
const BrandService = {
  getAll() {
    return axios.get("https://localhost:7171/api/Brand/GetAll");
  }
};
export default BrandService;
