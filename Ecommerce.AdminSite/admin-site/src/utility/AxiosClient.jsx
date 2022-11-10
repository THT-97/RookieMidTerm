import axios from "axios";
import Cookies from "js-cookie";

const AxiosClient = () => {
  const defaultOptions = {
    baseURL: "https://localhost:7171/api/",
    headers: {
      "Content-Type": "application/json"
    }
  };

  // Create instance
  const instance = axios.create(defaultOptions);

  // Set authorization header with interceptors
  instance.interceptors.request.use(function (config) {
    const token = Cookies.get("token");
    config.headers.Authorization = token ? `Bearer ${token}` : "";
    return config;
  });

  return instance;
};
export default AxiosClient();
