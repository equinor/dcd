import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter, Route, Routes } from 'react-router-dom'

import App from './App'

import DashboardView from './Views/DashboardView'
import ProjectView from './Views/ProjectView'
import CaseView from './Views/CaseView'

ReactDOM.render(
    <React.StrictMode>
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<App />}>
                    <Route index element={<DashboardView />} />
                    <Route path="project/:projectId" element={<ProjectView />} />
                    <Route path="project/:projectId/case/:caseId" element={<CaseView />} />
                </Route>
            </Routes>
        </BrowserRouter>
    </React.StrictMode>,
    document.getElementById('root')
)
