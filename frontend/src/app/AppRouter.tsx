import { Route, Switch } from "react-router-dom"
import SideMenu from "../Components/SideMenu/SideMenu"
import CaseView from "../Views/CaseView"
import UnauthorizedView from "../Views/ErrorPages/UnauthorizedView"
import Welcome from "../Views/Welcome"
import InternalServerErrorView from "../Views/ErrorPages/InternalServerErrorView"
import ProjectView from "../Views/ProjectView"

export function AppRouter(): JSX.Element {
    return (
        <Switch>
            <Route
                path="/"
                exact
            >
                <Welcome />
            </Route>
            <Route
                path="/403"
                exact
            >
                <UnauthorizedView />
            </Route>
            <Route
                path="/500"
                exact
            >
                <InternalServerErrorView />
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
