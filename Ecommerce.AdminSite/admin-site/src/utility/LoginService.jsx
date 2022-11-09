import axios from "axios";
const LoginService = {
  signIn(login) {
    return axios.post("https://localhost:7171/api/Auth/SignIn", login);
  },

  getRole(username) {
    return axios.get(
      `https://localhost:7171/api/Auth/GetRoles?username=${username}`
    );
  }
};
export default LoginService;
