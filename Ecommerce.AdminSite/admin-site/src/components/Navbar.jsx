import Cookies from "js-cookie";
import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { commonUrl } from "../firebase";

const Navbar = () => {
  const [username, setUsername] = useState(null);
  useEffect(() => {
    console.log(Cookies.get("username"));
    if (
      Cookies.get("username") === undefined &&
      Cookies.get("role") !== "SysAdmin" &&
      window.location.href !== "http://localhost:3000/Login"
    ) {
      window.location.href = "/Login";
    }
    setUsername(Cookies.get("username"));
  }, []);

  return (
    <div>
      <nav id="navbar" className="bg-black">
        <ul>
          <Link to="/" className="text-decoration-none">
            <li className="d-flex mt-3">
              <img
                className="me-2"
                style={{ width: "20%" }}
                src={`${commonUrl}2Flogo.jpg?alt=media&token=ea64ee08-6951-4927-8849-d08a5994486d`}
                alt="shop-logo"
              />
              <div className="text-start m-0 border border-3 border-danger ps-1 pe-2 pb-4">
                <h6 className="text-danger m-0">Nash</h6>
                <h6 className="text-danger m-0">Shoes.</h6>
              </div>
            </li>
          </Link>
        </ul>
        <ul>
          {username ? (
            <ul>
              <Link to="#">
                <li>{username}</li>
              </Link>
              <Link
                to="/"
                onClick={() => {
                  setUsername(null);
                  Cookies.remove("username");
                  Cookies.remove("token");
                }}
              >
                <li>SignOut</li>
              </Link>
            </ul>
          ) : (
            <Link to="/Login">
              <li>Sign In</li>
            </Link>
          )}
        </ul>
      </nav>
    </div>
  );
};

export default Navbar;
