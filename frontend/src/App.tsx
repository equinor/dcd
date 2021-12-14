import React from 'react';
import './styles.css';
import SideMenu from './Components/SideMenu/SideMenu'
import Header from './Components/Header'

function App() {
  return (
    <div className="App" style={{display: 'flex', flexDirection: 'column', height: '100vh', width: '100vw'}}>
        <Header/>
        <SideMenu/>
    </div>
  );
}

export default App;
