import { Component } from "react";

class Home extends Component{
    constructor() {
        super();
        this.state = {username: "THT"};
    }
    render(){
        return(
            <h1 className='col-8'>Welcome to Admin site</h1>
        )

    }
}

export default Home;