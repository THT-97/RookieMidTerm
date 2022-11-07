import {
  faPlusCircle,
  faEdit,
  faTrashCan
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import CategoryService from "../../utility/CategoryService";

const CategoryIndex = () => {
  const [categories, setCategories] = useState(null);
  const [page, setPage] = useState(1);
  const [limit, setLimit] = useState(6);
  const [count, setCount] = useState(0);
  const [pages, setPages] = useState(1);
  let table = [];
  const pageMenu = [];
  // On load
  useEffect(() => {
    CategoryService.getPage(1, 6).then((response) =>
      setCategories(response.data)
    );
  }, []);

  // Effect when change limit
  useEffect(() => {
    setPages(Math.ceil(count / limit));
    setPage(1);
    CategoryService.getPage(1, limit).then((response) =>
      setCategories(response.data)
    );
    CategoryService.getCount().then((response) => setCount(response.data));
  }, [count, limit]);

  // Effect when select page
  useEffect(() => {
    CategoryService.getPage(page, limit).then((response) =>
      setCategories(response.data)
    );
    CategoryService.getCount().then((response) => setCount(response.data));
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

  if (categories != null) {
    // Add categories to table
    table = categories.map((category) => (
      <tr key={category.name}>
        <td>{category.name}</td>
        <td>{category.description}$</td>
        <td>{category.status === 0 ? "Not available" : "Available"}</td>
        <td>
          <Link to="#">
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
              onClick={() => {}}
            />
          </Link>
        </td>
      </tr>
    ));
  }

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
  return (
    <div className="col-9 p-0 ms-3">
      <h1>Category list</h1>
      <hr />
      {/* Pagination form */}
      <form onSubmit={PageLimit}>
        <label htmlFor="lim">Categories per page:</label>
        <input name="lim" type="number" min="2" defaultValue={limit} />
        <button className="btn btn-sm btn-outline-info">Filter</button>
      </form>
      <Link
        to="#"
        title="Add new category"
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
            <th>Description</th>
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

export default CategoryIndex;
