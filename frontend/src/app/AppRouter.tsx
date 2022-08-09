import { Route, Switch } from "react-router-dom"
import SideMenu from "../Components/SideMenu/SideMenu"
import CaseView from "../Views/CaseView"
import DrainageStrategyView from "../Views/DrainageStrategyView"
import ExplorationView from "../Views/ExplorationView"
import ProjectView from "../Views/ProjectView"
import SubstructureView from "../Views/SubstructureView"
import SurfView from "../Views/SurfView"
import TopsideView from "../Views/TopsideView"
import TransportView from "../Views/TransportView"
import Welcome from "../Views/Welcome"
import WellProjectView from "../Views/WellProjectView"

export function AppRouter(): JSX.Element {
    return (
        <Switch>
            <Route exact path="/">
                <Welcome />
            </Route>
            <Route exact path="/:fusionProjectId">
                <SideMenu>
                    <ProjectView />
                </SideMenu>
            </Route>
            <Route path="/:fusionProjectId/case/:caseId" exact>
                <SideMenu>
                    <CaseView />
                </SideMenu>
            </Route>
            <Route path="/:fusionProjectId/case/:caseId/surf/:surfId" exact>
                <SideMenu>
                    <SurfView />
                </SideMenu>
            </Route>
            <Route
                path="/:fusionProjectId/case/:caseId/drainagestrategy/:drainageStrategyId"
                exact
            >
                <SideMenu>
                    <DrainageStrategyView />
                </SideMenu>
            </Route>
            <Route
                path="/:fusionProjectId/case/:caseId/topside/:topsideId"
                exact
            >
                <SideMenu>
                    <TopsideView />
                </SideMenu>
            </Route>
            <Route
                path="/:fusionProjectId/case/:caseId/substructure/:substructureId"
                exact
            >
                <SideMenu>
                    <SubstructureView />
                </SideMenu>
            </Route>
            <Route
                path="/:fusionProjectId/case/:caseId/transport/:transportId"
                exact
            >
                <SideMenu>
                    <TransportView />
                </SideMenu>
            </Route>
            <Route
                path="/:fusionProjectId/case/:caseId/wellproject/:wellProjectId"
                exact
            >
                <SideMenu>
                    <WellProjectView />
                </SideMenu>
            </Route>
            <Route
                path="/:fusionProjectId/case/:caseId/exploration/:explorationId"
                exact
            >
                <SideMenu>
                    <ExplorationView />
                </SideMenu>
            </Route>
        </Switch>
    )
}
