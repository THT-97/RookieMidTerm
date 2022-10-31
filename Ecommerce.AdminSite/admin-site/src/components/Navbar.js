import { Component } from "react";
import { Link } from "react-router-dom";

class Navbar extends Component{
    constructor() {
        super();
        this.state = {username: "THT"};
    }
    render(){
        return(
            <div className="ReactApp">
                <nav id='navbar'>
                    <ul>
                        <Link to="*"><li>
                            <img className="w-25" src="https://localhost:7091/Images/logo.jpg" alt="shop-logo"/>
                            AdminSite
                        </li></Link>
                    </ul>
                    <ul>
                        <Link to="/Home"><li>Home</li></Link>
                        <Link to="*"><li>About</li></Link>
                    </ul>
                </nav>
            </div>
        )

    }
}

export default Navbar;