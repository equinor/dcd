import { BrowserRouter, Route, Routes } from "react-router-dom"
import SideMenu from "../Components/SideMenu/SideMenu"
import CaseView from "../Views/CaseView"
import Welcome from "../Views/Welcome"
import ProjectView from "../Views/ProjectView"

export const AppRouter = (): JSX.Element => (
    <BrowserRouter>
        <Routes>
            <Route
                path="/"
                element={<Welcome />}
            />
            <Route
                path="/:fusionContextId"
                element={(
                    <SideMenu>
                        <ProjectView />
                    </SideMenu>
                )}
            />
            <Route
                path=":fusionContextId/case/:caseId"
                element={(
                    <SideMenu>
                        <CaseView />
                    </SideMenu>
                )}
            />
        </Routes>
    </BrowserRouter>
)
