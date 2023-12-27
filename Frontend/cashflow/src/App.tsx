import React from 'react';
import logo from './logo.svg';
import './App.css';
import './reset.css'
import {LoginPage} from "./pages/LoginPage";
import {BrowserRouter} from "react-router-dom";
import {Routing} from "./pages/Routing";

function App() {
  return (
    <>
        <BrowserRouter>
            <Routing/>
        </BrowserRouter>
    </>
  );
}

export default App;
