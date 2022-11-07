import React, { useEffect, useState } from "react";
import BrandService from "../../utility/BrandService";
import CategoryService from "../../utility/CategoryService";
import Uploader from "../../utility/Uploader";
import ProductService from "../../utility/ProductService";
import { Button } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMinusCircle, faPlusCircle } from "@fortawesome/free-solid-svg-icons";

const ProductAdd = () => {
  const [categories, setCategories] = useState([]);
  const [brands, setBrands] = useState([]);
  const [colorInputs, setColorInputs] = useState([
    <input
      key="c0"
      name="colors"
      className="me-1"
      type="color"
      defaultValue="#ffffff"
    ></input>
  ]);
  const [sizeInputs, setSizeInputs] = useState([
    <select key="s0" name="sizes" className="me-1" defaultValue="M">
      <option value="XS">XS</option>
      <option value="S">S</option>
      <option value="M">M</option>
      <option value="L">L</option>
      <option value="XL">XL</option>
      <option value="XXL">XXL</option>
    </select>
  ]);
  let categoryOptions;
  let brandOptions;

  // Load categories and brands menu options
  useEffect(() => {
    CategoryService.getAll().then((response) => setCategories(response.data));
    BrandService.getAll().then((response) => setBrands(response.data));
  }, []);
  // When categories and brands are loaded
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
  // Addcolor function
  function AddColor() {
    if (colorInputs.length < 6) {
      setColorInputs((prevInputs) => [
        ...prevInputs,
        <input
          key={`c${colorInputs.length}`}
          name="colors"
          className="me-1"
          type="color"
          defaultValue="#ffffff"
        ></input>
      ]);
    }
  }
  // Popcolor function
  function PopColor() {
    if (colorInputs.length > 1) {
      setColorInputs(colorInputs.slice(0, colorInputs.length - 1));
    }
  }
  // Addsize function
  function AddSize() {
    if (sizeInputs.length < 6) {
      setSizeInputs((prevInputs) => [
        ...prevInputs,
        <select key={`s${sizeInputs.length}`} name="sizes" className="me-1">
          <option key="xs" value="XS">
            XS
          </option>
          <option key="s" value="S">
            S
          </option>
          <option key="m" value="M">
            M
          </option>
          <option key="l" value="L">
            L
          </option>
          <option key="xl" value="XL">
            XL
          </option>
          <option key="xxl" value="XXL">
            XXL
          </option>
        </select>
      ]);
    }
  }
  // Popsize function
  function PopSize() {
    if (sizeInputs.length > 1) {
      setSizeInputs(sizeInputs.slice(0, sizeInputs.length - 1));
    }
  }
  // Submit function
  async function CreateProduct(f) {
    f.preventDefault();
    if (confirm("Create new product?")) {
      // Get files from form
      const files = Array.from(f.target.images.files);
      // Call uploader to upload files and get urls
      const filenames = await Uploader.upload(files);
      // Force submit function to wait for Uploader
      Promise.resolve().then(() => {
        // console.log("filenames: " + filenames);
        const entity = {
          Name: f.target.name.value,
          Description: f.target.description.value,
          ListPrice: f.target.price.value,

          // create a set of unique colors
          // then iterate the set as an array and reduce to a string with specific format
          Colors:
            Array.from(f.target.colors).length > 1
              ? [
                  ...new Set(
                    Array.from(f.target.colors).map((color) => color.value)
                  )
                ].reduce((total, color) => {
                  return total ? `${total} ${color}` : color;
                }, null)
              : f.target.colors.value,

          // similar operation for sizes
          Sizes:
            // type check in case array.from makes an array of options instead of array of selects
            f.target.sizes.type !== "select-one" &&
            // check array of selects length and create string from set
            Array.from(f.target.sizes).length > 1
              ? [
                  ...new Set(
                    Array.from(f.target.sizes).map((size) => size.value)
                  )
                ].reduce((total, size) => {
                  return total ? `${total} ${size}` : size;
                }, null)
              : f.target.sizes.value,

          // sale price will not be more than list price
          SalePrice: f.target.saleprice.value
            ? f.target.saleprice.value > f.target.price.value
              ? f.target.price.value
              : f.target.saleprice.value
            : f.target.price.value,

          Images: filenames,
          Quantity: f.target.quantity.value,
          CategoryName: f.target.category.value,
          BrandName: f.target.brand.value
        };

        // console.log(entity);
        // console.log(filenames);
        // console.log(Array.from([f.target.sizes]));
        ProductService.create(entity)
          .then(() => {
            alert("Product created");
            window.location.href = "../ProductIndex";
          })
          .catch((error) => alert(`Error${error}`));
      });
    }
  }
  return (
    <div className="col-8 ms-5">
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
                <label htmlFor="img">Upload Image</label>
              </td>
              <td>
                <input
                  id="img"
                  type="file"
                  name="images"
                  accept="image/*"
                  title="Upload Image"
                  multiple
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
              <td>
                Colors{" "}
                <Button variant="none" size="sm" onClick={AddColor}>
                  <FontAwesomeIcon
                    className="text-primary"
                    icon={faPlusCircle}
                  ></FontAwesomeIcon>
                </Button>
                <Button variant="none" size="sm" onClick={PopColor}>
                  <FontAwesomeIcon
                    className="text-secondary"
                    icon={faMinusCircle}
                  ></FontAwesomeIcon>
                </Button>
              </td>
              <td>{colorInputs}</td>
            </tr>
            <tr>
              <td>
                Sizes
                <Button variant="none" size="sm" onClick={AddSize}>
                  <FontAwesomeIcon
                    className="text-primary m-0"
                    icon={faPlusCircle}
                  ></FontAwesomeIcon>
                </Button>
                <Button variant="none" size="sm" onClick={PopSize}>
                  <FontAwesomeIcon
                    className="text-secondary"
                    icon={faMinusCircle}
                  ></FontAwesomeIcon>
                </Button>
              </td>
              <td>{sizeInputs}</td>
            </tr>
            <tr>
              <td>
                <label htmlFor="description">Description</label>
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
