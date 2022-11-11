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
        await LoginService.getRole(login.username).then((subr) => {
          // console.log(subr.data.toString());
          if (subr.data.toString() === "SysAdmin") {
            Cookies.set("role", response.data);
            Cookies.set("token", response.data);
            Cookies.set("username", login.username);
            window.location.href = "/";
          } else alert("Unauthorized");
        });
      })
      .catch((error) => alert(error));
  }
  return (
    <div className="col-8 text-center p-5">
      <h1>ADMIN SIGN IN</h1>
      <hr />
      <form
        className="h-50 pt-2 d-flex justify-content-center"
        onSubmit={Submit}
      >
        <table className="w-50 h-100 d-flex justify-content-center shadow rounded">
          <tbody className="m-4">
            <tr>
              <td>
                <label htmlFor="username">Username</label>
              </td>
              <td>
                <input
                  required
                  className="input form-control shadow rounded"
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
                  className="input form-control"
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
