import React from 'react';
import logo from './logo.svg';
import './App.css';
import Greetings from "./Greeting";
import Counter from "./Counter.";

function App() {
  const onClick = (name:string) =>{
    console.log(`${name} say hello`);
  };

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
          <div>
              <Greetings onClick={onClick} name="HELLO"></Greetings>
          </div>
          <div>
              <Counter></Counter>
          </div>
      </header>

    </div>
  );
}

export default App;
