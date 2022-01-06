import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import App from './App'
import DashboardView from './Views/DashboardView'
import ProjectView from './Views/ProjectView'
import CaseView from './Views/CaseView'
import MainView from './Components/SideMenu/MainView'

ReactDOM.render(
    <React.StrictMode>
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<App />}>
                    <Route index element={<DashboardView />} />
                    <Route path="project" element={<MainView />}>
                        <Route path=":projectId" element={<ProjectView />} />
                        <Route path=":projectId/case/:caseId" element={<CaseView />} />
                    </Route>
                </Route>
            </Routes>
        </BrowserRouter>
    </React.StrictMode>,
    document.getElementById('root')
)
