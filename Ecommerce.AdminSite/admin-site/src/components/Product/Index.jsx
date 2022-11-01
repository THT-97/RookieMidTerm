import axios from "axios";
import { useEffect, useState, React } from "react";
import { Link, Route, Routes } from "react-router-dom";
import ProductService from "../../utility/ProductService";
import Edit from "./Edit";

function GetProducts(setProducts, setError, page = 0, limit = 6) {
  return axios
    .get(
      `https://localhost:7171/api/Product/GetPage?page=${page}&limit=${limit}`
    )
    .then((response) => setProducts(response.data))
    .catch((error) => setError(error.data));
}

function GetCount(setCount, setError) {
  axios
    .get(`https://localhost:7171/api/Product/Count`)
    .then((response) => setCount(response.data))
    .catch((error) => setError(error.data));
}

const Index = () => {
  const [products, setProducts] = useState(null);
  const [page, setPage] = useState(1);
  const [limit, setLimit] = useState(6);
  const [count, setCount] = useState(0);
  const [pages, setPages] = useState(1);
  const [, setError] = useState();

  // Effect when change limit
  useEffect(() => {
    setPages(Math.ceil(count / limit));
    setPage(1);
    GetProducts(setProducts, setError, 1, limit);
    GetCount(setCount, setError);
  }, [count, limit]);

  // Effect when select page
  useEffect(() => {
    ProductService.getProducts(page, limit)
      .then((response) => setProducts(response.data))
      .catch((error) => setError(error.data));
    GetCount(setCount, setError);
  }, [page, limit]);

  let table = [];
  const pageMenu = [];
  const routes = [];
  if (products != null) {
    // Add products to table
    table = products.map((product) => (
      <tr key={product.id}>
        <td>{product.id}</td>
        <td>{product.name}</td>
        <td>{product.listPrice}$</td>
        <td>{product.salePrice}$</td>
        <td>{new Date(product.createdDate).toLocaleString()}</td>
        <td>{product.quantity}</td>
        <td>
          <Link to="/Product/Edit">edit</Link>
        </td>
      </tr>
    ));
    // Create route for edit pages
    products.forEach((product) => {
      routes.push(
        <Route
          key={product.id}
          exact
          path="./Edit"
          element={<Edit id={product.id} />}
        />
      );
    });
    // Update page menu
    if (pages > 1) {
      for (let index = 1; index <= pages; index++) {
        if (page === index) pageMenu.push(<span key={index}>{index}</span>);
        else
          pageMenu.push(
            <a href="#" key={index} onClick={ChangePage.bind(this, index)}>
              {index}
            </a>
          );
      }
    }
  }

  // Change limit function
  function PageLimit(e) {
    e.preventDefault();
    setLimit(e.target.lim.value);
  }

  // Page select function
  function ChangePage(p) {
    setPage(p);
  }

  return (
    <div className="col-9 p-0 m-0">
      <h1>Product list</h1>
      <hr />
      {/* Pagination form */}
      <form onSubmit={PageLimit}>
        <label htmlFor="lim">Products per page:</label>
        <input name="lim" type="number" min="2" defaultValue={limit} />
        <button className="btn btn-sm btn-outline-info">Filter</button>
      </form>
      <hr />
      {pageMenu}
      <table className="table table-striped table-hover">
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Price</th>
            <th>Sale price</th>
            <th>Date created</th>
            <th>In stock</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {/* render table var inside tbody */}
          {table}
        </tbody>
      </table>
      {/* Add routes */}
      <Routes>{routes}</Routes>
    </div>
  );
};

export default Index;
