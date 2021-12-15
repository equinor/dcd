import React from 'react'
import { Outlet } from 'react-router-dom'
import './styles.css'

import SideMenu from './Components/SideMenu/SideMenu'
import Header from './Components/Header'

function App() {
    return (
        <div className="App" style={{ display: 'flex', flexDirection: 'column', height: '100vh', width: '100vw' }}>
            <Header />
            <div style={{ display: 'flex', flexDirection: 'row', flexGrow: 1 }}>
                <SideMenu />
                <Outlet />
            </div>
        </div>
    )
}

export default App
