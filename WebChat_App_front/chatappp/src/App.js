import React, { Component } from 'react';

import Chat from './component/Chat';

import logo from './logo.svg';
import './App.css';


class App extends Component {
  render(){
    return (
      <div className="App">
        <header className="App-header">
            <h1 className="App-title">Welcome to React</h1>
            <hr/>
            <hr/>
            <hr/>
            <br/>
            <br/>
            <br/>
          </header>
        <div>
          <Chat />
        </div>
      </div>
    );
  }
}
export default App;
