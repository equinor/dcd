import React from 'react'
import { Outlet } from 'react-router-dom'
import styled from 'styled-components'
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal, useMsalAuthentication } from '@azure/msal-react'
import { ApplicationInsights } from '@microsoft/applicationinsights-web'
import { ReactPlugin } from '@microsoft/applicationinsights-react-js'
import { InteractionType } from '@azure/msal-browser'

import './styles.css'
import { createBrowserHistory } from 'history'
import { appInsightsInstrumentationKey } from './config'
import { loginRequest } from './auth/authContextProvider'
import SideMenu from './Components/SideMenu/SideMenu'
import Header from './Components/Header'

const browserHistory = createBrowserHistory()
const reactPlugin = new ReactPlugin()
const appInsights = new ApplicationInsights({
    config: {
        instrumentationKey: appInsightsInstrumentationKey,
        extensions: [reactPlugin],
        extensionConfig: {
            [reactPlugin.identifier]: { history: browserHistory },
        },
    },
})
appInsights.loadAppInsights()

const MainViewWrapper = styled.div`
    width: calc(100% - 15rem);
    overflow: scroll;
`

const ProfileContent = () => {
    const { instance, accounts } = useMsal()

    /*  function RequestProfileData() { */
    // Silently acquires an access token which is then attached to a request for MS Graph data
    instance
        .acquireTokenSilent({
            ...loginRequest,
            account: accounts[0],
        })
        .then(response => {
            console.log(response)
        })
        .catch(err => {
            console.log('Error')
            console.log(err)
        })
    /*  } */

    return (
        <>
            <h5 className="card-title">Welcome to DCD {accounts[0].name}!</h5>
            <div className="App" style={{ display: 'flex', flexDirection: 'column', height: '100vh', width: '100vw' }}>
                <Header />
                <div style={{ display: 'flex', flexDirection: 'row', flexGrow: 1, width: '100%' }}>
                    <SideMenu />
                    <MainViewWrapper>
                        <Outlet />
                    </MainViewWrapper>
                </div>
            </div>
        </>
    )
}

function App() {
    useMsalAuthentication(InteractionType.Redirect)
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
