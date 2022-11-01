import "./App.css";
import Navbar from "./components/Navbar";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Sidenav from "./components/Sidenav";
import Users from "./components/Users";
import Login from "./components/Login";
import Home from "./components/Home";
import Index from "./components/Product/Index";

const App = () => {
  return (
    <div className="App">
      <BrowserRouter>
        <Navbar />
        <div className="row p-0 m-0" style={{ height: 100 + "vh" }}>
          <Sidenav className="col-2 p-0 m-0" />
          <Routes>
            <Route exact path="/Login" element={<Login />} />
            <Route exact path="/Users" element={<Users />} />
            <Route exact path="/Home" element={<Home />} />
            <Route exact path="/Product/Index" element={<Index />} />
          </Routes>
        </div>
      </BrowserRouter>
    </div>
  );
};

export default App;
