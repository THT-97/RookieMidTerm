import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import CategoryService from "../../utility/CategoryService";

const CategoryEdit = () => {
  const { id } = useParams();
  const [category, setCategory] = useState(null);

  useEffect(() => {
    CategoryService.getById(id).then((response) => setCategory(response.data));
  }, [id]);

  // Edit function
  async function EditCategory(f) {
    f.preventDefault();
    if (confirm("Edit category?")) {
      const entity = {
        id,
        name: f.target.name.value,
        description: f.target.description.value,
        status: category.status
      };

      CategoryService.edit(id, entity)
        .then(() => {
          alert("Category updated");
          window.location.href = "../CategoryIndex";
        })
        .catch((error) => alert(`Error${error}`));
    }
  }
  return (
    <div className="col-8 ms-5">
      <h1>Edit category</h1>
      <hr />
      {/* Edit Form */}
      {category ? (
        <form onSubmit={EditCategory} className="d-flex justify-content-center">
          <table className="table w-50">
            <tbody>
              <tr>
                <td className="text-end">
                  <label htmlFor="name">Category name</label>
                </td>
                <td className="d-flex justify-content-start">
                  <input
                    required
                    className="input"
                    type="text"
                    name="name"
                    defaultValue={category.name}
                  />
                </td>
              </tr>
              <tr>
                <td className="text-end">
                  <label htmlFor="description">Description</label>
                </td>
                <td className="d-flex justify-content-start">
                  <textarea
                    name="description"
                    rows={4}
                    cols={50}
                    maxLength={1000}
                    defaultValue={category.description}
                  />
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
              <tr>
                <td></td>
                <td>
                  <button className="btn btn-warning" type="submit">
                    Edit
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </form>
      ) : (
        <h4 className="text-info">Loading...</h4>
      )}
    </div>
  );
};

export default CategoryEdit;
