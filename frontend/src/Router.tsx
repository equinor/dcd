import { RouterProvider, RouteObject } from "react-router-dom"
import { useRouter } from "@equinor/fusion-framework-react-app/navigation"
import { AgnosticRouteObject } from "@remix-run/router"
import ProjectLayout from "./Components/Project/ProjectLayout"
import ProjectView from "./Views/ProjectView"
import CaseView from "./Views/CaseView"
import UserGuideView from "./Components/Guide/UserGuide"
import ChangeLogView from "./Components/ChangeLog/ProjectChangeLog"
import ProjectSkeleton from "./Components/LoadingSkeletons/ProjectSkeleton"

const routes: RouteObject[] = [
    {
        path: "/guide",
        element: <UserGuideView />,
    },
    {
        path: "/",
        children: [
            {
                path: ":fusionContextId",
                element: <ProjectLayout />,
                children: [
                    { index: true, element: <ProjectView /> },
                    { path: "change-log", element: <ChangeLogView /> },
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
