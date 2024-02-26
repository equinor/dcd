import { RouterProvider, RouteObject } from "react-router-dom"
import { useRouter } from "@equinor/fusion-framework-react-app/navigation"
import Overview from "./Components/Overview"
import ProjectView from "./Views/ProjectView"
import CaseView from "./Views/CaseView"
import Header from "./Components/Header"

const routes: RouteObject[] = [
    {
        path: "/",
        element: <Header />,
        children: [
            {
                path: ":fusionContextId",
                element: <Overview />,
                children: [
                    { index: true, element: <ProjectView /> },
                    { path: "case/:caseId", element: <CaseView /> },
                ],
            },
        ],
    },
]

export default function AppRouter() {
    const router = useRouter(routes)
    return <RouterProvider router={router} fallbackElement={<div>Loading...</div>} />
}
