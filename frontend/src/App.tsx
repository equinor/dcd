import React from 'react'
import './styles.css'
import { Route, Routes } from 'react-router-dom'

import SideMenu from './Components/SideMenu/SideMenu'
import Header from './Components/Header'
import ProjectView from './Views/ProjectView'
import DashboardView from './Views/DashboardView'
import CaseView from './Views/CaseView'

function App() {
    return (
        <div className="App" style={{ display: 'flex', flexDirection: 'column', height: '100vh', width: '100vw' }}>
            <Header />
            <div style={{ display: 'flex', flexDirection: 'row', flexGrow: 1 }}>
                <SideMenu />
                <Routes>
                    <Route path="/" element={<DashboardView />} />
                    <Route path="/project/:projectId" element={<ProjectView />} />
                    <Route path="/project/:projectId/case/:caseId" element={<CaseView />} />
                </Routes>
            </div>
        </div>
    )
}

export default App
