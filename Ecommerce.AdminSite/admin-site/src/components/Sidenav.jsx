import axios from "axios";
import { useEffect, useState } from "react";
import {
  Sidebar,
  Menu,
  MenuItem,
  SubMenu,
  ProSidebarProvider
} from "react-pro-sidebar";

const Sidenav = () => {
  const [categories, setCategories] = useState(null);
  const [brands, setBrands] = useState(null);
  useEffect(() => {
    // get list of categories
    axios
      .get("https://localhost:7171/api/Category/GetAll")
      .then((response) => setCategories(response.data))
      .catch((error) => setCategories(error.data));

    // get list of brands
    axios
      .get("https://localhost:7171/api/Brand/GetAll")
      .then((response) => setBrands(response.data))
      .catch((error) => setBrands(error.data));
  }, []);

  let caTable = [];
  let brandTable = [];
  if (categories != null) {
    caTable = categories.map((category) => (
      <MenuItem
        key={category.name}
        className="bg-secondary text-white text-start ps-3"
      >
        {category.name}
      </MenuItem>
    ));
  }

  if (brands != null) {
    brandTable = brands.map((brand) => (
      <MenuItem
        key={brand.name}
        className="bg-secondary text-white text-start ps-4"
      >
        {brand.name}
      </MenuItem>
    ));
  }

  return (
    <ProSidebarProvider>
      <Sidebar className="bg-secondary h-100 p-0">
        <Menu className="bg-secondary">
          <MenuItem className="bg-secondary text-white" href="/Users">
            USERS
          </MenuItem>
          <hr />
          <SubMenu className="bg-secondary text-white" label="PRODUCTS">
            <MenuItem className="bg-secondary text-white" href="/Product/Index">
              All Products
            </MenuItem>
            <SubMenu
              className="bg-secondary text-white"
              label="View by categories"
            >
              {caTable}
            </SubMenu>
            <SubMenu className="bg-secondary text-white" label="View by brands">
              {brandTable}
            </SubMenu>
          </SubMenu>
          <hr />
          <MenuItem className="bg-secondary text-white" href="*">
            CATEGORIES
          </MenuItem>
          <hr />
          <MenuItem className="bg-secondary text-white" href="*">
            BRAND
          </MenuItem>
          <hr />
        </Menu>
      </Sidebar>
    </ProSidebarProvider>
  );
};

export default Sidenav;
