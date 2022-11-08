import React from "react";
import CategoryService from "../../utility/CategoryService";

const CategoryAdd = () => {
  async function AddCategory(f) {
    f.preventDefault();
    if (confirm("Add new category?")) {
      const entity = {
        name: f.target.name.value,
        description: f.target.description.value,
        status: 1
      };

      CategoryService.create(entity)
        .then(() => {
          alert("Category added");
          window.location.href = "./CategoryIndex";
        })
        .catch((error) => alert(`Error${error}`));
    }
  }
  return (
    <div className="col-8 ms-5">
      <h1>Add category</h1>
      <hr />
      {/* Add Form */}
      <form onSubmit={AddCategory} className="col-8">
        <table className="table">
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
                  placeholder="Category Name..."
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
                  placeholder="Description..."
                />
              </td>
            </tr>
            <tr>
              <td></td>
              <td>
                <button className="btn btn-primary" type="submit">
                  Create
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </form>
    </div>
  );
};

export default CategoryAdd;
