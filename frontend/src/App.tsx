import { useEffect } from 'react'
import { Outlet } from 'react-router-dom'
import { AppConfigurationClient } from "@azure/app-configuration"

import './styles.css'
import SideMenu from './Components/SideMenu/SideMenu'
import Header from './Components/Header'

function App() {
    const connection_string = process.env.REACT_APP_AZURE_APP_CONFIG_CONNECTION_STRING
    const client = new AppConfigurationClient(connection_string!)

    useEffect(() => {
        client.getConfigurationSetting({key: "PoC2"}).then(response => {
            console.log(response.value)
        })
    })

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
