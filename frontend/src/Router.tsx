import { RouterProvider, RouteObject } from "react-router-dom"
import { useRouter } from "@equinor/fusion-framework-react-app/navigation"
import { AgnosticRouteObject } from "@remix-run/router"
import Overview from "./Components/Overview"
import ProjectView from "./Views/ProjectView"
import CaseView from "./Views/CaseView"
import ProjectSelector from "./Components/ProjectSelector"
import ProjectSkeleton from "./Components/LoadingSkeletons/ProjectSkeleton"
import UserGuideView from "./Components/Guide/UserGuide"

const routes: RouteObject[] = [
    {
        path: "/guide",
        element: <UserGuideView />,
    },
    {
        path: "/",
        element: <ProjectSelector />,
        children: [
            {
                path: ":fusionContextId",
                element: <Overview />,
                children: [
                    { index: true, element: <ProjectView /> },
                    { path: ":tab", element: <ProjectView /> },
                    { path: "revision/:revisionId", element: <ProjectView /> },
                    { path: "revision/:revisionId/:tab", element: <ProjectView /> },
                    { path: "revision/:revisionId/case/:caseId", element: <CaseView /> },
                    { path: "revision/:revisionId/case/:caseId/:tab", element: <CaseView /> },
                    { path: "case/:caseId", element: <CaseView /> },
                    { path: "case/:caseId/:tab", element: <CaseView /> },
                ],
            },
        ],
    },
]

export default function AppRouter() {
    const router = useRouter(routes as AgnosticRouteObject[])
    return (
        <RouterProvider
            router={router}
            fallbackElement={<ProjectSkeleton />}
        />
    )
}
