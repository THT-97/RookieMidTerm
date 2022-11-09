import React from "react";
import Cookies from "js-cookie";
import LoginService from "../utility/LoginService";

const Login = () => {
  async function Submit(f) {
    f.preventDefault();
    const login = {
      username: f.target.username.value,
      password: f.target.password.value
    };
    console.log(login);
    await LoginService.signIn(login)
      .then(async (response) => {
        await LoginService.getRole(login.username)
          .then((subr) => {
            // console.log(subr.data.toString());
            if (subr.data.toString() === "SysAdmin") {
              Cookies.set("role", response.data);
              Cookies.set("token", response.data);
              Cookies.set("username", login.username);
              window.location.href = "/";
            } else alert("Unauthorized");
          })
          .catch((error) => alert(error));
      })
      .catch((error) => alert(error));
  }
  return (
    <div className="col-8 text-center p-5">
      <h1>ADMIN SIGN IN</h1>
      <hr />
      <form onSubmit={Submit}>
        <table className="d-flex justify-content-center">
          <tbody>
            <tr>
              <td>
                <label htmlFor="username">Username</label>
              </td>
              <td>
                <input
                  required
                  className="input"
                  type="text"
                  name="username"
                  placeholder="Username..."
                />
              </td>
            </tr>
            <tr>
              <td>
                <label htmlFor="password">Password</label>
              </td>
              <td>
                <input
                  required
                  type="password"
                  name="password"
                  placeholder="Password..."
                />
              </td>
            </tr>
            <tr className="d-flex justify-content-end">
              <td>
                <button className="btn btn-outline-primary" type="submit">
                  Sign in
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </form>
    </div>
  );
};

export default Login;
