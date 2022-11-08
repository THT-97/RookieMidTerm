import React from "react";
import { Link } from "react-router-dom";
import { commonUrl } from "../firebase";

const Navbar = () => {
  return (
    <div>
      <nav id="navbar" className="bg-black">
        <ul>
          <Link to="/Home" className="text-decoration-none">
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
