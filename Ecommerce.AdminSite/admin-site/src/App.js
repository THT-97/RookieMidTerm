import './App.css';
import Navbar from './components/Navbar';
import { BrowserRouter, Route, Routes } from "react-router-dom";

import { Component } from 'react';
import Sidenav from './components/Sidenav';
import { ProSidebarProvider } from 'react-pro-sidebar';
import Home from './components/Home';

class App extends Component {
  render() {
    return(
      <div className="App">
        <BrowserRouter>
            <Navbar/>
            <div className='row' style={{height:100+'vh'}}>
              <Sidenav className='col-2'/>
              <Routes>
                <Route exact path="/Home" element={<Home />}/>
                <Route exact path="*" element={<h1 className='col-8'>Nothing here</h1>}/>
              </Routes>
            </div>
        </BrowserRouter>
      </div>
    );
  }
}

export default App;
