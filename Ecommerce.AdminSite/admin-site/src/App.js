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
import ProductAdd from "./components/Product/ProductAdd";

const App = () => {
  return (
    <div className="App">
      <BrowserRouter>
        <Navbar />
        <div className="row p-0 m-0" style={{ height: 100 + "vh" }}>
          <Sidenav className="col-2 p-0 m-0" />
          <Routes>
            <Route index element={<Home />} />
            <Route path="/Home" element={<Home />} />
            <Route path="/Login" element={<Login />} />
            <Route path="/Users" element={<Users />} />
            {/* Routes for product */}
            <Route path="/Product/ProductIndex" element={<ProductIndex />} />
            <Route path="/Product/ProductAdd" element={<ProductAdd />} />
            <Route path="/Product/ProductEdit/:id" element={<ProductEdit />} />
          </Routes>
        </div>
      </BrowserRouter>
    </div>
  );
};

export default App;
