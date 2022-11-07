import { React } from "react";
import { Sidebar, Menu, MenuItem, ProSidebarProvider } from "react-pro-sidebar";

const Sidenav = () => {
  return (
    <ProSidebarProvider>
      <Sidebar className="bg-secondary p-0">
        <Menu className="bg-secondary">
          <MenuItem className="bg-secondary text-white" href="/Users">
            USERS
          </MenuItem>
          <hr />
          <MenuItem
            className="bg-secondary text-white"
            href="/Product/ProductIndex"
          >
            PRODUCTS
          </MenuItem>
          <hr />
          <MenuItem
            className="bg-secondary text-white"
            href="/Category/CategoryIndex"
          >
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
