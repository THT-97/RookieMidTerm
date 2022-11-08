import { React } from "react";
import { Sidebar, Menu, MenuItem, ProSidebarProvider } from "react-pro-sidebar";

const Sidenav = () => {
  return (
    <ProSidebarProvider>
      <Sidebar className="bg-secondary p-0">
        <div className="w-100 h-100 bg-secondary m-0 p-0">
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
          </Menu>
        </div>
      </Sidebar>
    </ProSidebarProvider>
  );
};

export default Sidenav;
