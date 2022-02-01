import { ApplicationInsights } from '@microsoft/applicationinsights-web'
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal, useMsalAuthentication } from '@azure/msal-react'
import { createBrowserHistory } from 'history'
import { InteractionType } from '@azure/msal-browser'
import { Outlet } from 'react-router-dom'
import { ReactPlugin } from '@microsoft/applicationinsights-react-js'
import { useEffect } from 'react'
import styled from 'styled-components'

import SideMenu from './Components/SideMenu/SideMenu'
import Header from './Components/Header'

import { loginRequest } from './auth/authContextProvider'
import { appInsightsInstrumentationKey, fusionApiScope } from './config'

import './styles.css'

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

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
    height: 100vh;
    width: 100vw;
`

const Body = styled.div`
    display: flex;
    flex-direction: row;
    flex-row: 1;
    width: 100%;
    height: 100%;
`

const MainView = styled.div`
    width: calc(100% - 15rem);
    overflow: scroll;
`

const ProfileContent = () => {
    const { instance, accounts } = useMsal()

    useEffect(() => {
        (async () => {
            // Silently acquires an access token which is then attached to a request for MS Graph data
            try {
                const { accessToken } = await instance.acquireTokenSilent({
                    ...loginRequest,
                    account: accounts[0],
                })
                window.sessionStorage.setItem('loginAccessToken', accessToken)
            } catch (error) {
                console.error('[ProfileContent] Login failed', error)
            }
            try {
                const { accessToken } = await instance.acquireTokenSilent({
                    scopes: fusionApiScope,
                    account: accounts[0],
                })
                window.sessionStorage.setItem('fusionAccessToken', accessToken)
            } catch (error) {
                console.error('[Fusion] Failed to get fusion token', error)
            }
        })()
    }, [])

    if (!window.sessionStorage.getItem('loginAccessToken')) return null

    return (
        <Wrapper className="App">
            <Header name={accounts[0].name} />
            <Body>
                <SideMenu />
                <MainView>
                    <Outlet />
                </MainView>
            </Body>
        </Wrapper>
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
