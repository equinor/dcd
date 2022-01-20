import { ApplicationInsights } from '@microsoft/applicationinsights-web'
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal, useMsalAuthentication } from '@azure/msal-react'
import { createBrowserHistory } from 'history'
import { InteractionType, PublicClientApplication } from '@azure/msal-browser'
import { MsalProvider } from '@azure/msal-react'
import { Outlet } from 'react-router-dom'
import { ReactPlugin } from '@microsoft/applicationinsights-react-js'
import { useEffect, useState } from 'react'
import styled from 'styled-components'

import Header from './Components/Header'
import SideMenu from './Components/SideMenu/SideMenu'

import { loginRequest } from './auth/authContextProvider'
import { fusionApiScope, RetrieveConfigFromAzure } from './config'

import './styles.css'

const browserHistory = createBrowserHistory()

const reactPlugin = new ReactPlugin()

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
        ;(async () => {
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
    const [msalInstance, setMsalInstance] = useState<PublicClientApplication>()

    useMsalAuthentication(InteractionType.Redirect)

    useEffect(() => {
        ;(async () => {
            try {
                const config = await RetrieveConfigFromAzure()

                const appInsights = new ApplicationInsights({
                    config: {
                        instrumentationKey: config.applicationInsightInstrumentationKey,
                        extensions: [reactPlugin],
                        extensionConfig: {
                            [reactPlugin.identifier]: { history: browserHistory },
                        },
                    },
                })

                appInsights.loadAppInsights()

                console.log('[App] config', config)

                setMsalInstance(new PublicClientApplication(config.msal))
            } catch (error) {
                console.error(error)
            }
        })()
    }, [])

    // TODO: display spinner
    if (!msalInstance) return null

    return (
        <div className="App">
            <MsalProvider instance={msalInstance}>
                <AuthenticatedTemplate>
                    <ProfileContent />
                </AuthenticatedTemplate>

                <UnauthenticatedTemplate>
                    <h5 className="card-title">Please sign-in to DCD.</h5>
                </UnauthenticatedTemplate>
            </MsalProvider>
        </div>
    )
}

export default App
