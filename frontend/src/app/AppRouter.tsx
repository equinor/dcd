import { Route, Switch } from "react-router-dom"
import SideMenu from "../Components/SideMenu/SideMenu"
import CaseView from "../Views/CaseView"
import Welcome from "../Views/Welcome"
import ProjectView from "../Views/ProjectView"

export const AppRouter = (): JSX.Element => {
    return (
        <Switch>
            <Route
                path="/"
                exact
            >
                <Welcome />
            </Route>
            <Route
                path="/:fusionContextId"
                exact
            >
                <SideMenu>
                    <ProjectView />
                </SideMenu>
            </Route>
            <Route
                path="/:fusionContextId/case/:caseId"
                exact
            >
                <SideMenu>
                    <CaseView />
                </SideMenu>
            </Route>
        </Switch>
    )
}
