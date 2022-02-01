import { ApplicationInsights } from '@microsoft/applicationinsights-web'
import { AuthenticationResult, EventMessage, EventType, IPublicClientApplication, PublicClientApplication } from '@azure/msal-browser'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { createBrowserHistory } from 'history'
import { MsalProvider } from '@azure/msal-react'
import { ReactPlugin } from '@microsoft/applicationinsights-react-js'
import React, { useEffect, useState, VoidFunctionComponent } from 'react'

import { ViewsContainer } from './Views/ViewsContainer'
import CaseView from './Views/CaseView'
import DashboardView from './Views/DashboardView'
import ProjectView from './Views/ProjectView'

import { RetrieveConfigFromAzure } from './config'

import './styles.css'

const browserHistory = createBrowserHistory()

const reactPlugin = new ReactPlugin()

const App: VoidFunctionComponent = () => {
    const [instance, setMsalInstance] = useState<IPublicClientApplication>()

    useEffect(() => {
        ;(async () => {
            try {
                const appConfig = await RetrieveConfigFromAzure()

                // Set up MSAL
                const instance = new PublicClientApplication(appConfig.msal)

                const accounts = instance.getAllAccounts();
                if (accounts.length > 0) {
                    instance.setActiveAccount(accounts[0]);
                }

                instance.addEventCallback((event: EventMessage) => {
                    if (event.eventType === EventType.LOGIN_SUCCESS && event.payload) {
                        const payload = event.payload as AuthenticationResult;
                        const account = payload.account;
                        instance.setActiveAccount(account);
                    }
                });

                setMsalInstance(instance)

                // Set up AppInsights
                const appInsights = new ApplicationInsights({
                    config: {
                        instrumentationKey: appConfig.applicationInsightInstrumentationKey,
                        extensions: [reactPlugin],
                        extensionConfig: {
                            [reactPlugin.identifier]: { history: browserHistory },
                        },
                    },
                })

                appInsights.loadAppInsights()
            } catch (error) {
                console.error('[App] Error while retreiving AppConfig from Azure', error)
            }
        })()
    }, [])

    if (!instance) return null

    return (
        <React.StrictMode>
            <MsalProvider instance={instance}>
                <BrowserRouter>
                    <Routes>
                        <Route path="/" element={<ViewsContainer />}>
                            <Route index element={<DashboardView />} />
                            <Route path="project/:projectId" element={<ProjectView />} />
                            <Route path="project/:projectId/case/:caseId" element={<CaseView />} />
                        </Route>
                    </Routes>
                </BrowserRouter>
            </MsalProvider>
        </React.StrictMode>
    )
}

export default App
