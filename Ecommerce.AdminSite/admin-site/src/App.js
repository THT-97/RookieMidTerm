import './App.css';
import Navbar from './components/Navbar';
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Component } from 'react';

class App extends Component {
  render() {
    return(
      <div className="App">
        <BrowserRouter>
          <div>
            <Navbar/>
            <Routes>
            <Route exact path="*" element={<h1>Welcome to Admin site</h1>}/>
              <Route exact path="*" element={<h1>Nothing here</h1>}/>
            </Routes>
          </div>
        </BrowserRouter>
      </div>
    );
  }
}

export default App;
