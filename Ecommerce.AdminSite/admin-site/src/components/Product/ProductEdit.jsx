import React, { useEffect, useState } from "react";
import { Carousel } from "react-responsive-carousel";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { useParams } from "react-router-dom";
import { commonUrl } from "../../firebase";
import BrandService from "../../utility/BrandService";
import CategoryService from "../../utility/CategoryService";
import ProductService from "../../utility/ProductService";
import { Button } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faMinusCircle,
  faPlusCircle,
  faXmarkCircle
} from "@fortawesome/free-solid-svg-icons";
import Uploader from "../../utility/Uploader";

const ProductEdit = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [categories, setCategories] = useState([]);
  const [brands, setBrands] = useState([]);
  const [sizeInputs, setSizeInputs] = useState([]);
  const [colorInputs, setColorInputs] = useState([]);
  const [slideImages, setSlideImages] = useState([]);
  const [oldImages, setOldImages] = useState([]);
  const [oldImagesInputs, setOldImagesInputs] = useState([]);
  let categoryOptions;
  let brandOptions;
  // Get product by id then get categories and brands
  useEffect(() => {
    ProductService.getByID(id).then((response) => setProduct(response.data));
    CategoryService.getAll().then((response) => setCategories(response.data));
    BrandService.getAll().then((response) => setBrands(response.data));
  }, [id]);

  useEffect(() => {
    // When product is loaded
    let colors = [];
    let sizes = [];
    // Load old images and add to carousel
    if (product != null) {
      // Get images
      if (product.images != null) {
        setOldImages(product.images.split(" "));
        setSlideImages(
          product.images
            .split(" ")
            .map((image) => <img src={`${commonUrl}${image}`} key={image} />)
        );
      } else setSlideImages(<p>No image</p>);

      // Get colors and size
      colors = product.colors.split(" ");
      sizes = product.sizes.split(" ");
      // Create inputs
      setColorInputs(
        colors.map((color) => (
          <span key={color} className="me-3">
            <input name="colors" type="color" defaultValue={color} />
          </span>
        ))
      );
      setSizeInputs(
        sizes.map((size) => (
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
        ))
      );
    }
  }, [product]);

  // Add old image thumbnails (removable)
  useEffect(() => {
    setOldImagesInputs(
      <div className="row mt-2">
        {oldImages.map((image) => (
          <div key={image} className="col-4">
            <img className="w-100" src={`${commonUrl}${image}`} />
            <Button
              variant="none"
              size="sm"
              className="m-0"
              onClick={() => setOldImages(oldImages.filter((i) => i !== image))}
            >
              <FontAwesomeIcon icon={faXmarkCircle}></FontAwesomeIcon>
            </Button>
          </div>
        ))}
      </div>
    );
  }, [oldImages]);

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

  // Edit form on submit
  async function EditProduct(f) {
    f.preventDefault();
    if (confirm("Edit product?")) {
      // Get files from form
      const files = Array.from(f.target.images.files);
      // Call uploader to upload files and get urls
      const filenames = await Uploader.upload(files);
      // Force submit function to wait for Uploader
      Promise.resolve().then(() => {
        const entity = {
          id: product.id,
          name: f.target.name.value,
          images:
            oldImages.reduce((total, img) => {
              return total ? `${total} ${img}` : img;
            }, null) +
            " " +
            filenames,
          description: f.target.description.value,
          listPrice: f.target.price.value,

          // sale price will not be more than list price
          salePrice: f.target.saleprice.value
            ? f.target.saleprice.value > f.target.price.value
              ? f.target.price.value
              : f.target.saleprice.value
            : product.salePrice,

          quantity: f.target.quantity.value,

          status: f.target.status.value
            ? // check if product is out of stock
              f.target.quantity.value <= 0
              ? 2
              : f.target.status.value
            : product.status,

          // create a set of unique colors
          // then iterate the set as an array and reduce to a string with specific format
          colors:
            Array.from(f.target.colors).length > 1
              ? [
                  ...new Set(
                    Array.from(f.target.colors).map((color) => color.value)
                  )
                ].reduce((total, color) => {
                  return total ? `${total} ${color}` : color;
                }, null)
              : f.target.colors.value,

          // type check in case array.from makes an array of options instead of array of selects
          sizes:
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

          categoryName: f.target.category.value,
          brandName: f.target.brand.value
        };
        console.log(entity);
        ProductService.edit(id, entity)
          .then(() => {
            alert("Product updated");
            window.location.href = "../ProductIndex";
          })
          .catch((error) => alert(`Error${error}`));
      });
    }
  }

  return (
    <div className="col-8 row ms-5">
      <h1>Edit product</h1>
      <hr />
      {/* Image carousel and thumbnails for old images */}
      <div className="col-4 p-0 m-0">
        {product ? (
          product.images ? (
            <div>
              {/* // Image carousel */}
              <Carousel
                className="p-1 bg-secondary"
                dynamicHeight={true}
                showThumbs={false}
                infiniteLoop={true}
              >
                {slideImages}
              </Carousel>
              {oldImagesInputs}
            </div>
          ) : (
            "No images"
          )
        ) : (
          ""
        )}
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
                <td>
                  <label htmlFor="images">Images</label>
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
                  <select name="category" defaultValue={product.categoryName}>
                    {categoryOptions}
                  </select>
                </td>
              </tr>
              <tr>
                <td>
                  <label htmlFor="brand">Brand</label>
                </td>
                <td>
                  <select name="brand" defaultValue={product.brandName}>
                    {brandOptions}
                  </select>
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
                    defaultValue={product.description}
                  />
                </td>
              </tr>
              <tr>
                <td>
                  <label htmlFor="status">Status</label>
                </td>
                <td>
                  <select name="status" defaultValue={product.status}>
                    <option key="0" value="0">
                      Not Available
                    </option>
                    <option key="1" value="1">
                      Available
                    </option>
                    <option key="2" value="2">
                      Out of stock
                    </option>
                    <option key="3" value="3">
                      Suspended
                    </option>
                  </select>
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
