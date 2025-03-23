import { useRouter } from "@equinor/fusion-framework-react-app/navigation"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { AgnosticRouteObject } from "@remix-run/router"
import { RouterProvider, RouteObject } from "react-router-dom"

import ChangeLogView from "./Components/ChangeLog/ProjectChangeLog"
import UserGuideView from "./Components/Guide/UserGuide"
import ProjectSkeleton from "./Components/LoadingSkeletons/ProjectSkeleton"
import ProjectLayout from "./Components/ProjectTabs/ProjectLayout"
import CaseView from "./Views/CaseView"
import IndexView from "./Views/IndexView"
import NotFoundView from "./Views/NotFoundView"
import ProjectView from "./Views/ProjectView"

const ProjectRoute = (): JSX.Element => {
    const { currentContext } = useModuleCurrentContext()

    if (!currentContext) {
        return <NotFoundView />
    }

    return <ProjectLayout />
}

const routes: RouteObject[] = [
    {
        path: "/guide",
        element: <UserGuideView />,
    },
    {
        path: "/",
        children: [
            { index: true, element: <IndexView /> },
            {
                path: ":fusionContextId",
                element: <ProjectRoute />,
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
                    { path: "*", element: <NotFoundView /> },
                ],
            },
            { path: "*", element: <NotFoundView /> },
        ],
    },
]

const AppRouter = (): JSX.Element => {
    const router = useRouter(routes as AgnosticRouteObject[])

    return (
        <RouterProvider
            router={router}
            fallbackElement={<ProjectSkeleton />}
        />
    )
}

export default AppRouter
