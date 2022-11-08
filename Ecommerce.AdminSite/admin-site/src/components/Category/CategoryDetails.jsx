import { faEdit } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import CategoryService from "../../utility/CategoryService";

const CategoryDetails = () => {
  const { id } = useParams();
  const [category, setCategory] = useState(null);

  useEffect(() => {
    CategoryService.getById(id).then((response) => setCategory(response.data));
  }, [id]);

  return (
    <div className="col-8 ms-5">
      <h1>
        Category details{" "}
        <Link to={`/Category/CategoryEdit/${id}`}>
          <FontAwesomeIcon
            icon={faEdit}
            title="Edit"
            className="text-warning ms-3"
          />
        </Link>
      </h1>
      <hr />
      {category ? (
        <table className="table d-flex justify-content-center w-100">
          <tbody>
            <tr>
              <td className="text-end">Category name</td>
              <td className="d-flex justify-content-start">{category.name}</td>
            </tr>
            <tr>
              <td className="w-50 text-end">Description</td>
              <td className="d-flex justify-content-start">
                <p className="text-wrap text-start">{category.description}</p>
              </td>
            </tr>
            <tr>
              <td className="text-end">
                <label htmlFor="status">Status</label>
              </td>
              <td className="d-flex justify-content-start">
                {category.status === 0 ? "Not available" : "Available"}
              </td>
            </tr>
          </tbody>
        </table>
      ) : (
        <h4 className="text-info">Loading...</h4>
      )}
    </div>
  );
};

export default CategoryDetails;
