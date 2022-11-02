import "./App.css";
import Navbar from "./components/Navbar";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Sidenav from "./components/Sidenav";
import Users from "./components/Users";
import Login from "./components/Login";
import Home from "./components/Home";
import ProductIndex from "./components/Product/ProductIndex";
import ProductEdit from "./components/Product/ProductEdit";
import React from "react";

const App = () => {
  return (
    <div className="App">
      <BrowserRouter>
        <Navbar />
        <div className="row p-0 m-0" style={{ height: 100 + "vh" }}>
          <Sidenav className="col-2 p-0 m-0" />
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/Home" element={<Home />} />
            <Route path="/Login" element={<Login />} />
            <Route path="/Users" element={<Users />} />
            <Route path="/Product/ProductIndex" element={<ProductIndex />} />
            <Route
              path="/Product/ProductEdit/:id"
              element={<ProductEdit id={this} />}
            />
          </Routes>
        </div>
      </BrowserRouter>
    </div>
  );
};

export default App;
