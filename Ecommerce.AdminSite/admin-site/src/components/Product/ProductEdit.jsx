import React, { useEffect, useState } from "react";
import { Carousel } from "react-responsive-carousel";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { useParams } from "react-router-dom";
import { commonUrl } from "../../firebase";
import ProductService from "../../utility/ProductService";

const ProductEdit = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  let colors = [];
  let sizes = [];
  let slideImages;
  let sizeInputs;
  let colorInputs;
  // Get product by id
  useEffect(() => {
    ProductService.getByID(id).then((response) => setProduct(response.data));
  }, [id]);
  // When product is loaded
  if (product != null) {
    // Get images
    product.images
      ? (slideImages = product.images
          .split(" ")
          .map((image) => <img src={`${commonUrl}${image}`} key={image} />))
      : (slideImages = <p>No image</p>);
    // Get colors and size
    colors = product.colors.split(" ");
    sizes = product.sizes.split(" ");
    // Create inputs
    colorInputs = colors.map((color) => (
      <span key={color} className="me-3">
        <input name="colors" type="color" defaultValue={color} />
      </span>
    ));
    sizeInputs = sizes.map((size) => (
      <span key={size} className="me-3">
        <select defaultValue={size} name="sizes">
          <option value="XS">XS</option>
          <option value="S">S</option>
          <option value="M">M</option>
          <option value="L">L</option>
          <option value="XL">XL</option>
          <option value="XXL">XXL</option>
        </select>
      </span>
    ));
  }

  // Edit form on submit
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
        : product.salePrice,
      quantity: f.target.quantity.value,
      colors: Array.from(f.target.colors).reduce((total, color) => {
        return total ? `${total} ${color.value}` : color.value;
      }, null),
      sizes: Array.from(f.target.sizes).reduce((total, size) => {
        return total ? `${total} ${size.value}` : size.value;
      }, null),
      categoryName: f.target.category.value,
      brandName: f.target.brand.value
    };

    console.log(entity);
  }

  return (
    <div className="col-8 row">
      <h1>Edit product</h1>
      <hr />
      {/* Image carousel */}
      <div className="col-4 p-0 m-0">
        <Carousel
          className="p-1 bg-secondary"
          dynamicHeight={true}
          showThumbs={false}
          infiniteLoop={true}
        >
          {slideImages}
        </Carousel>
      </div>

      {/* Edit Form */}
      <form onSubmit={EditProduct} className="col-8">
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
                    min={10}
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
                    defaultValue={product.description}
                  />
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
        ) : (
          <p>Loading...</p>
        )}
      </form>
    </div>
  );
};

export default ProductEdit;
