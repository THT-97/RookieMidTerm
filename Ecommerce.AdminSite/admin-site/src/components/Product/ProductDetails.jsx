import { faEdit } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useState } from "react";
import { Carousel } from "react-responsive-carousel";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { Link, useParams } from "react-router-dom";
import { commonUrl } from "../../firebase";
import ProductService from "../../utility/ProductService";

const ProductDetails = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [sizeInputs, setSizeInputs] = useState([]);
  const [colorInputs, setColorInputs] = useState([]);
  const [slideImages, setSlideImages] = useState([]);
  // Get product by id then get categories and brands
  useEffect(() => {
    ProductService.getByID(id).then((response) => setProduct(response.data));
  }, [id]);

  useEffect(() => {
    // When product is loaded
    let colors = [];
    let sizes = [];
    // Load old images and add to carousel
    if (product != null) {
      // Get images
      if (product.images != null) {
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
          <span key={color} style={{ width: "10%" }} className="me-3">
            <button
              style={{ backgroundColor: `${color}` }}
              className="w-100 h-100"
            ></button>
          </span>
        ))
      );
      setSizeInputs(
        sizes.map((size) => (
          <span key={size} className="me-3">
            <button>{size}</button>
          </span>
        ))
      );
    }
  }, [product]);

  return (
    <div className="col-8 row ms-5">
      <h1>
        Product details{" "}
        <Link to={`/Product/productEdit/${id}`}>
          <FontAwesomeIcon
            icon={faEdit}
            title="Edit"
            className="text-warning ms-3"
          />
        </Link>
      </h1>
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
            </div>
          ) : (
            "No images"
          )
        ) : (
          ""
        )}
      </div>

      {/* Edit Form */}
      {product ? (
        <div className="col-8">
          <table className="table table-striped table-hover">
            <tbody>
              <tr>
                <td className="text-end">Product name</td>
                <td className="d-flex justify-content-start">{product.name}</td>
              </tr>
              <tr>
                <td className="text-end">Created date</td>
                <td className="d-flex justify-content-start">
                  {new Date(product.createdDate).toLocaleString()}
                </td>
              </tr>
              <tr>
                <td className="text-end">Updated date</td>
                <td className="d-flex justify-content-start">
                  {product.updatedDate
                    ? new Date(product.updatedDate).toLocaleString()
                    : "Not yet"}
                </td>
              </tr>
              <tr>
                <td className="text-end">Price</td>
                <td className="d-flex justify-content-start">
                  {product.listPrice}$
                </td>
              </tr>
              <tr>
                <td className="text-end">Sale Price</td>
                <td className="d-flex justify-content-start">
                  {product.salePrice}$
                </td>
              </tr>
              <tr>
                <td className="text-end">Quantity</td>
                <td className="d-flex justify-content-start">
                  {product.quantity}
                </td>
              </tr>
              <tr>
                <td className="text-end">Category</td>
                <td className="d-flex justify-content-start">
                  {product.categoryName}
                </td>
              </tr>
              <tr>
                <td className="text-end">Brand</td>
                <td className="d-flex justify-content-start">
                  {product.brandName}
                </td>
              </tr>
              <tr>
                <td className="text-end">Colors</td>
                <td className="d-flex justify-content-start">{colorInputs}</td>
              </tr>
              <tr>
                <td className="text-end">Sizes</td>
                <td className="d-flex justify-content-start">{sizeInputs}</td>
              </tr>
              <tr>
                <td className="w-25 text-end">Description</td>
                <td className="d-flex justify-content-start">
                  <p className="text-wrap text-start">{product.description}</p>
                </td>
              </tr>
              <tr>
                <td className="text-end">Status</td>
                <td className="d-flex justify-content-start">
                  {product.status === 0
                    ? "Not available"
                    : product.status === 3
                    ? "Suspended"
                    : product.status === 1
                    ? "Available"
                    : "Out of stock"}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      ) : (
        <h4 className="text-info">Loading...</h4>
      )}
    </div>
  );
};

export default ProductDetails;
