import React, { useState } from 'react'
import { Outlet } from 'react-router-dom'
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react'
import './styles.css'
import { loginRequest } from './authConfig'
import SideMenu from './Components/SideMenu/SideMenu'
import Header from './Components/Header'

const ProfileContent = () => {
    const { instance, accounts } = useMsal()
    const [graphData, setGraphData] = useState(null)

    /*  function RequestProfileData() { */
    // Silently acquires an access token which is then attached to a request for MS Graph data
    instance
        .acquireTokenSilent({
            ...loginRequest,
            account: accounts[0],
        })
        .then(response => {
            console.log(response)
            //callMsGraph(response.accessToken).then(response => setGraphData(response));
        })
        .catch(err => {
            console.log('Error')
            console.log(err)
            // handle error
        })
    /*  } */

    return (
        <>
            <h5 className="card-title">Welcome to DCD {accounts[0].name}!</h5>
            <div className="App" style={{ display: 'flex', flexDirection: 'column', height: '100vh', width: '100vw' }}>
                <Header />
                <div style={{ display: 'flex', flexDirection: 'row', flexGrow: 1 }}>
                    <SideMenu />
                    <Outlet />
                </div>
            </div>
        </>
    )
}

function App() {
    return (
        <div className="App">
            <AuthenticatedTemplate>
                <ProfileContent />
            </AuthenticatedTemplate>

            <UnauthenticatedTemplate>
                <h5 className="card-title">Please sign-in to DCD.</h5>
            </UnauthenticatedTemplate>
        </div>
    )
}

export default App
