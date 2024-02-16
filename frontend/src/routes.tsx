import { RouteObject } from "react-router-dom"
import SideMenu from "./Components/SideMenu/SideMenu"
import ProjectView from "./Views/ProjectView"
import CaseView from "./Views/CaseView"
import RouteCoordinator from "./Components/RouteCoordinator"

export const routes: RouteObject[] = [
    {
        path: "/",
        element: <RouteCoordinator />,
        children: [
            {
                path: ":fusionContextId",
                element: <SideMenu />,
                children: [
                    { index: true, element: <ProjectView /> },
                    { path: "case/:caseId", element: <CaseView /> },
                ],
            },
        ],
    },
]

export default routes
