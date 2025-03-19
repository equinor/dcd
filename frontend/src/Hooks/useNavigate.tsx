import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useNavigate as useRouterNavigate, useLocation } from "react-router-dom"

import { projectTabNames } from "@/Utils/Config/constants"

export const useAppNavigation = () => {
    const navigate = useRouterNavigate()
    const { currentContext } = useModuleCurrentContext()
    const location = useLocation()

    const projectPath = (projectId?: string) => `/${projectId || currentContext?.id || ""}`

    const navigateToProject = (projectId?: string, tabName?: string) => {
        const path = tabName
            ? `${projectPath(projectId)}/${tabName}`
            : projectPath(projectId)

        navigate(path)
    }

    const navigateToCase = (caseId: string, tabName?: string, options?: { replace?: boolean }) => {
        if (!currentContext?.id) { return }
        // Get revisionId from current path if we're in a revision
        const revisionMatch = location.pathname.match(/\/revision\/([^/]+)/)
        const revisionId = revisionMatch ? revisionMatch[1] : null

        // Only keep the tab if navigating from another case
        const shouldKeepTab = location.pathname.includes("/case/")
        const finalTabName = shouldKeepTab ? tabName : undefined

        const path = revisionId
            ? `${projectPath()}/revision/${revisionId}/case/${caseId}${finalTabName ? `/${finalTabName}` : ""}`
            : `${projectPath()}/case/${caseId}${finalTabName ? `/${finalTabName}` : ""}`

        navigate(path, { replace: options?.replace })
    }

    const navigateToRevision = (revisionId: string, tabName?: string) => {
        if (!currentContext?.id) { return }
        const path = tabName
            ? `${projectPath()}/revision/${revisionId}/${tabName}`
            : `${projectPath()}/revision/${revisionId}`

        navigate(path)
    }

    const navigateToRevisionCase = (revisionId: string, caseId: string, tabName?: string, options?: { replace?: boolean }) => {
        if (!currentContext?.id) { return }
        // Only keep the tab if navigating from another case
        const shouldKeepTab = location.pathname.includes("/case/")
        const finalTabName = shouldKeepTab ? tabName : undefined

        const path = `${projectPath()}/revision/${revisionId}/case/${caseId}${finalTabName ? `/${finalTabName}` : ""}`

        navigate(path, { replace: options?.replace })
    }

    const navigateToProjectTab = (tabIndex: number, revisionId?: string): number => {
        if (!currentContext?.id) { return tabIndex }
        const tabName = projectTabNames[tabIndex]

        if (revisionId) {
            navigateToRevision(revisionId, tabName)
        } else {
            navigateToProject(undefined, tabName)
        }

        return tabIndex
    }

    const navigateToCaseTab = (caseId: string, tabName: string, revisionId?: string) => {
        if (!currentContext?.id) { return }
        if (revisionId) {
            navigateToRevisionCase(revisionId, caseId, tabName)
        } else {
            navigateToCase(caseId, tabName)
        }
    }

    return {
        navigateToProject,
        navigateToCase,
        navigateToRevision,
        navigateToRevisionCase,
        navigateToProjectTab,
        navigateToCaseTab,
        projectPath,
    }
}
