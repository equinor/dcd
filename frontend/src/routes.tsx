import {
    RouteObject,
} from "react-router-dom"
import Welcome from "./Views/Welcome"
import SideMenu from "./Components/SideMenu/SideMenu"
import ProjectView from "./Views/ProjectView"
import CaseView from "./Views/CaseView"

export const routes: RouteObject[] = [
    {
        path: "/",
        element: <Welcome />,
        children: [

        ],
    },
    {
        path: "/:fusionContextId",
        element: (
            <SideMenu>
                <ProjectView />
            </SideMenu>
        ),
        children: [

        ],
    },
    {
        path: "/:fusionContextId/case/:caseId",
        element: (
            <SideMenu>
                <CaseView />
            </SideMenu>
        ),
        children: [

        ],
    },
]

export default routes
