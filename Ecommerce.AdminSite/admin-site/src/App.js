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
import CategoryIndex from "./components/Category/CategoryIndex";
import CategoryEdit from "./components/Category/CategoryEdit.jsx";
import CategoryAdd from "./components/Category/CategoryAdd";
import ProductDetails from "./components/Product/ProductDetails";
import CategoryDetails from "./components/Category/CategoryDetails";

const App = () => {
  // const [token, setToken] = useState(null);
  // useEffect(() => {
  //   setToken(Cookies.get("token"));
  // }, []);
  // console.log({ token });
  return (
    <div className="App">
      <BrowserRouter>
        <Navbar />
        <div className="row p-0 m-0" style={{ height: 100 + "vh" }}>
          <Sidenav className="col-2 p-0 m-0" />
          <Routes>
            <Route index element={<Home />} />
            <Route path="/Login" element={<Login />} />
            <Route path="/Users" element={<Users />} />
            {/* Routes for product */}
            <Route path="/Product/ProductIndex" element={<ProductIndex />} />
            <Route path="/Product/ProductAdd" element={<ProductAdd />} />
            <Route
              path="/Product/ProductDetails/:id"
              element={<ProductDetails />}
            />
            <Route path="/Product/ProductEdit/:id" element={<ProductEdit />} />
            {/* Routes for category */}
            <Route path="/Category/CategoryIndex" element={<CategoryIndex />} />
            <Route path="/Category/CategoryAdd" element={<CategoryAdd />} />
            <Route
              path="/Category/CategoryDetails/:id"
              element={<CategoryDetails />}
            />
            <Route
              path="/Category/CategoryEdit/:id"
              element={<CategoryEdit />}
            />
          </Routes>
        </div>
      </BrowserRouter>
    </div>
  );
};

export default App;
