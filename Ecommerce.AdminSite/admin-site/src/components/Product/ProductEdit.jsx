import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ProductService from "../../utility/ProductService";

const ProductEdit = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const stc = require("string-to-color");
  let colors = [];
  let colorInputs;
  useEffect(() => {
    ProductService.getByID(id).then((response) => setProduct(response.data));
  }, [id]);

  if (product != null) {
    colors = product.colors.split(" ");
    console.log(colors);
    colorInputs = colors.map((color) => (
      <tr key={color}>
        <td></td>
        <td>
          <input type="color" defaultValue={stc(color)} />
        </td>
      </tr>
    ));
  }

  function EditProduct(f) {
    f.preventDefault();
    const entity = {
      name: f.target.name.value,
      createdDate: product.createdDate,
      updatedDate: new Date().toLocaleString(),
      listPrice: f.target.price.value,
      salePrice: f.target.saleprice.value
        ? f.target.saleprice.value > product.listPrice
          ? f.target.price.value
          : f.target.saleprice.value
        : product.salePrice
    };
    console.log(entity);
  }

  return (
    <div className="col-8">
      <h1>Edit product</h1>
      <hr />
      <form onSubmit={EditProduct}>
        {product ? (
          <table className="table">
            <tbody>
              <tr>
                <td>
                  <label htmlFor="name">Product name</label>
                </td>
                <td>
                  <input
                    required
                    className="input"
                    type="text"
                    name="name"
                    defaultValue={product.name}
                  />
                </td>
              </tr>
              <tr>
                <td>Created date</td>
                <td>{new Date(product.createdDate).toLocaleString()}</td>
              </tr>
              <tr>
                <td>Updated date</td>
                <td>
                  {product.updatedDate
                    ? new Date(product.updatedDate).toLocaleString()
                    : "Not yet"}
                </td>
              </tr>
              <tr>
                <td>
                  <label htmlFor="price">Price</label>
                </td>
                <td>
                  <input
                    required
                    className="input"
                    type="number"
                    min={0}
                    step="any"
                    name="price"
                    defaultValue={product.listPrice}
                  />
                </td>
              </tr>
              <tr>
                <td>
                  <label htmlFor="saleprice">Sale Price</label>
                </td>
                <td>
                  <input
                    className="input"
                    type="number"
                    min={0}
                    step="any"
                    name="saleprice"
                    defaultValue={product.salePrice}
                  />
                </td>
              </tr>
              <tr>
                <td>
                  <label htmlFor="quantity">Quantity</label>
                </td>
                <td>
                  <input
                    required
                    className="input"
                    type="number"
                    min={0}
                    name="quantity"
                    defaultValue={product.quantity}
                  />
                </td>
              </tr>
              <tr>
                <td>
                  <label htmlFor="category">Category</label>
                </td>
                <td>
                  <input
                    required
                    className="input"
                    type="text"
                    name="category"
                    defaultValue={product.categoryName}
                  />
                </td>
              </tr>
              <tr>
                <td>
                  <label htmlFor="brand">Brand</label>
                </td>
                <td>
                  <input
                    required
                    className="input"
                    type="text"
                    name="brand"
                    defaultValue={product.brandName}
                  />
                </td>
              </tr>
              <tr>
                <td>
                  <label htmlFor="name">Description</label>
                </td>
                <td>
                  <textarea
                    name="description"
                    rows={3}
                    cols={50}
                    maxLength={255}
                    defaultValue={product.description}
                  />
                </td>
              </tr>
              <tr>
                <td>Colors</td>
              </tr>
              {colorInputs}
              <tr>
                <td></td>
                <td></td>
                <td>
                  <button className="btn btn-warning" type="submit">
                    Edit
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        ) : (
          <p>Loading...</p>
        )}
      </form>
    </div>
  );
};

export default ProductEdit;
