import { useState } from "react"
import { useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router-dom"

import { GetProjectService } from "@/Services/ProjectService"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppNavigation } from "@/Hooks/useNavigate"

export const useRevisions = () => {
    const {
        isRevision,
        setIsRevision,
        projectId,
        setIsCreateRevisionModalOpen,
    } = useProjectContext()
    const queryClient = useQueryClient()
    const { revisionId } = useParams()
    const { navigateToRevision: navigateToRevisionPath, navigateToProject } = useAppNavigation()

    const [isRevisionsLoading, setIsRevisionsLoading] = useState(false)

    const navigateToRevision = async (currentRevisionId: string) => {
        if (revisionId !== currentRevisionId) {
            setIsRevision(true)
            await queryClient.invalidateQueries({ queryKey: ["projectApiData", projectId] })
            await queryClient.invalidateQueries({ queryKey: ["revisionApiData", currentRevisionId] })
        }
        navigateToRevisionPath(currentRevisionId)
    }

    const createRevision = async (
        revision: Components.Schemas.CreateRevisionDto,
    ) => {
        setIsRevisionsLoading(true)
        const projectService = await GetProjectService()
        const newRevision = await projectService.createRevision(projectId, revision)
        if (newRevision) {
            setIsRevisionsLoading(false)
            setIsCreateRevisionModalOpen(false)
            navigateToRevision(newRevision.revisionId)
        }
    }

    const exitRevisionView = () => {
        setIsRevision(false)
        queryClient.invalidateQueries({ queryKey: ["projectApiData", projectId] })
        navigateToProject()
    }

    const disableCurrentRevision = (
        currentRevisionId: string | undefined,
    ) => isRevision && currentRevisionId === revisionId

    return {
        isRevisionsLoading,
        navigateToRevision,
        createRevision,
        exitRevisionView,
        disableCurrentRevision,
    }
}
