import React from "react";

const Login = () => {
  function Submit(f) {
    f.preventDefault();
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
                  name="name"
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
