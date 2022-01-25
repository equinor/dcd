import { ApplicationInsights } from '@microsoft/applicationinsights-web'
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal, useMsalAuthentication } from '@azure/msal-react'
import { createBrowserHistory } from 'history'
import { InteractionType } from '@azure/msal-browser'
import { Outlet } from 'react-router-dom'
import { ReactPlugin } from '@microsoft/applicationinsights-react-js'
import { useEffect, useState } from 'react'
import styled from 'styled-components'

import SideMenu from './Components/SideMenu/SideMenu'
import Header from './Components/Header'

import { ServicesContextProvider } from './Services'

import { loginRequest } from './auth/authContextProvider'
import { appInsightsInstrumentationKey } from './config'

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
    const [accessToken, setAccessToken] = useState<string>()
    const { instance, accounts } = useMsal()

    useEffect(() => {
        (async () => {
            // Silently acquires an access token which is then attached to a request for MS Graph data
            try {
                const { accessToken } = await instance.acquireTokenSilent({
                    ...loginRequest,
                    account: accounts[0],
                })
                setAccessToken(accessToken)
            } catch (error) {
                console.error("[ProfileContent] Login failed", error)
            }
        })()
    }, [])

    if (!accessToken) return null

    return (
        <ServicesContextProvider accessToken={accessToken}>
            <Wrapper className="App">
                <Header name={accounts[0].name} />
                <Body>
                    <SideMenu />
                    <MainView>
                        <Outlet />
                    </MainView>
                </Body>
            </Wrapper>
        </ServicesContextProvider>
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
