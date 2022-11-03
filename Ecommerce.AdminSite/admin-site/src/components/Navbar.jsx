import React from "react";
import { Link } from "react-router-dom";

const Navbar = () => {
  return (
    <div className="ReactApp">
      <nav id="navbar">
        <ul>
          <Link to="/Home">
            <li>
              <img
                style={{ width: "10%" }}
                src={require("../../../CustomerSite/wwwroot/Images/logo.jpg")}
              />
              AdminSite
            </li>
          </Link>
        </ul>
        <ul>
          <Link to="/Login">
            <li>Sign In</li>
          </Link>
          <Link to="*">
            <li>About</li>
          </Link>
        </ul>
      </nav>
    </div>
  );
};

export default Navbar;
