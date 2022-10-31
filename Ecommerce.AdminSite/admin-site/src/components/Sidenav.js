import { Component } from 'react';
import { Sidebar, Menu, MenuItem, SubMenu, ProSidebarProvider } from 'react-pro-sidebar';

class Sidenav extends Component{
    constructor() {
        super();
    }
    render(){
        return(
            <ProSidebarProvider>
                <Sidebar className="bg-secondary h-100">
                    <Menu className="bg-secondary">
                        <MenuItem className="bg-secondary">USERS</MenuItem>
                        <SubMenu className="bg-secondary" label="CATEGORIES">
                            <MenuItem className="bg-secondary">Category 1</MenuItem>
                            <MenuItem className="bg-secondary">Category 2</MenuItem>
                        </SubMenu>
                        <MenuItem className="bg-secondary">PRODUCTS</MenuItem>
                    </Menu>   
                </Sidebar>
            </ProSidebarProvider>
        );
    }
}

export default Sidenav