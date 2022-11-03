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
                className="me-2"
                style={{ width: "10%" }}
                src="https://localhost:7091/Images/logo.jpg"
                alt="shop-logo"
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
