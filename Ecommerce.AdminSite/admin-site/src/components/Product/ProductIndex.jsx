import { useEffect, useState, React } from "react";
import { Link } from "react-router-dom";
import ProductService from "../../utility/ProductService";
import "font-awesome/css/font-awesome.min.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faTrashCan,
  faEdit,
  faPlusCircle
} from "@fortawesome/free-solid-svg-icons";

const ProductIndex = () => {
  const [products, setProducts] = useState(null);
  const [page, setPage] = useState(1);
  const [limit, setLimit] = useState(6);
  const [count, setCount] = useState(0);
  const [pages, setPages] = useState(1);
  let table = [];
  const pageMenu = [];
  // On load
  useEffect(() => {
    ProductService.getPage(1, 6).then((response) => setProducts(response.data));
  }, []);

  // Effect when change limit
  useEffect(() => {
    setPages(Math.ceil(count / limit));
    setPage(1);
    ProductService.getPage(1, limit).then((response) =>
      setProducts(response.data)
    );
    ProductService.getCount().then((response) => setCount(response.data));
  }, [count, limit]);

  // Effect when select page
  useEffect(() => {
    ProductService.getPage(page, limit).then((response) =>
      setProducts(response.data)
    );
    ProductService.getCount().then((response) => setCount(response.data));
  }, [page, limit]);

  // Change limit function
  function PageLimit(e) {
    e.preventDefault();
    setLimit(e.target.lim.value);
  }

  // Page select function
  function ChangePage(p) {
    setPage(p);
  }

  if (products != null) {
    // Add products to table
    table = products.map((product) => (
      <tr key={product.id}>
        <td>{product.name}</td>
        <td>{product.listPrice}$</td>
        <td>{product.salePrice}$</td>
        <td>{new Date(product.createdDate).toLocaleString()}</td>
        <td>{product.quantity}</td>
        <td>{product.status}</td>
        <td>
          <Link to={`/Product/productEdit/${product.id}`}>
            <FontAwesomeIcon
              icon={faEdit}
              title="Edit"
              className="text-warning me-3"
            />
          </Link>
          <Link to="#">
            <FontAwesomeIcon
              icon={faTrashCan}
              title="Delete"
              className="text-danger"
            />
          </Link>
        </td>
      </tr>
    ));

    // Update page menu
    if (pages > 1) {
      for (let index = 1; index <= pages; index++) {
        if (page === index)
          pageMenu.push(
            <span key={index} className="me-1">
              {index}
            </span>
          );
        else
          pageMenu.push(
            <a
              href="#"
              className="me-1"
              key={index}
              onClick={ChangePage.bind(this, index)}
            >
              {index}
            </a>
          );
      }
    }
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
      <Link
        to="/Product/ProductAdd"
        title="Add new product"
        className="text-primary d-flex ms-5"
      >
        <h2>
          <FontAwesomeIcon icon={faPlusCircle}></FontAwesomeIcon>
        </h2>
      </Link>
      <hr />
      {pageMenu}
      <table className="table table-striped table-hover">
        <thead>
          <tr>
            <th>Name</th>
            <th>Price</th>
            <th>Sale price</th>
            <th>Date created</th>
            <th>In stock</th>
            <th>Status</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {/* render table var inside tbody */}
          {table}
        </tbody>
      </table>
    </div>
  );
};

export default ProductIndex;
