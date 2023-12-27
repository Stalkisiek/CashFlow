import React from 'react';
import './App.css';
import './reset.css'
import './fonts/ZilapMonograma-L2J4.ttf'
import {BrowserRouter} from "react-router-dom";
import {Routing} from "./pages/Routing";

function App() {
    const elementStyle = {
        fontFamily: '"Roboto", sans-serif', // Replace 'Arial' with your desired font
        // fontFamily: "Light" //"Medium"
    };
  return (
      <div style={elementStyle}>
          <BrowserRouter>
              <Routing/>
          </BrowserRouter>
      </div>

  );
}

export default App;
