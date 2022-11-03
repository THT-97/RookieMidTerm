import React, { useEffect, useState } from "react";
import BrandService from "../../utility/BrandService";
import CategoryService from "../../utility/CategoryService";
import ProductService from "../../utility/ProductService";

const ProductAdd = () => {
  const [categories, setCategories] = useState([]);
  const [brands, setBrands] = useState([]);
  let categoryOptions;
  let brandOptions;
  const sizeInputs = [
    <select key="s1" defaultValue="M" name="sizes">
      <option value="XS">XS</option>
      <option value="S">S</option>
      <option value="M">M</option>
      <option value="L">L</option>
      <option value="XL">XL</option>
      <option value="XXL">XXL</option>
    </select>
  ];

  const colorInputs = [
    <input key="c1" name="colors" type="color" defaultValue="#ffffff"></input>
  ];

  useEffect(() => {
    CategoryService.getAll().then((response) => setCategories(response.data));
    BrandService.getAll().then((response) => setBrands(response.data));
  }, []);

  if (categories != null) {
    categoryOptions = categories.map((category) => (
      <option key={category.name} value={category.name}>
        {category.name}
      </option>
    ));
  }

  if (brands != null) {
    brandOptions = brands.map((brand) => (
      <option key={brand.name} value={brand.name}>
        {brand.name}
      </option>
    ));
  }

  async function CreateProduct(f) {
    f.preventDefault();
    const entity = {
      Name: f.target.name.value,
      Colors: f.target.colors.value,
      Sizes: f.target.sizes.value,
      Description: f.target.description.value,
      ListPrice: f.target.price.value,
      SalePrice: f.target.saleprice.value
        ? f.target.saleprice.value > f.target.price.value
          ? f.target.price.value
          : f.target.saleprice.value
        : f.target.price.value,
      Images: null,
      Quantity: f.target.quantity.value,
      Rating: 0,
      RatingCount: 0,
      Status: 0,
      CategoryName: f.target.category.value,
      BrandName: f.target.brand.value,
      Ratings: []
    };
    console.log(entity);
    await ProductService.create(entity)
      .then((response) => console.log(response))
      .catch((error) => console.log(error));
  }
  return (
    <div className="col-8">
      <h1>Create product</h1>
      <hr />
      {/* Create Form */}
      <form onSubmit={CreateProduct}>
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
                  placeholder="Product name..."
                />
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
                  defaultValue={10}
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
                  defaultValue={10}
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
                  min={10}
                  name="quantity"
                  defaultValue={10}
                />
              </td>
            </tr>
            <tr>
              <td>
                <label htmlFor="category">Category</label>
              </td>
              <td>
                <select name="category">{categoryOptions}</select>
              </td>
            </tr>
            <tr>
              <td>
                <label htmlFor="brand">Brand</label>
              </td>
              <td>
                <select name="brand">{brandOptions}</select>
              </td>
            </tr>
            <tr>
              <td>Colors</td>
              <td>{colorInputs}</td>
            </tr>
            <tr>
              <td>Sizes</td>
              <td>{sizeInputs}</td>
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

export default ProductAdd;
