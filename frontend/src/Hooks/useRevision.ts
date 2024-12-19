import { useState } from "react"
import { useQueryClient } from "@tanstack/react-query"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { GetProjectService } from "@/Services/ProjectService"
import { useProjectContext } from "@/Context/ProjectContext"

export const useRevisions = () => {
    const {
        isRevision,
        setIsRevision,
        projectId,
        setIsCreateRevisionModalOpen,
    } = useProjectContext()
    const { currentContext } = useModuleCurrentContext()
    const queryClient = useQueryClient()
    const { revisionId } = useParams()
    const navigate = useNavigate()

    const [isRevisionsLoading, setIsRevisionsLoading] = useState(false)

    const navigateToRevision = async (currentRevisionId: string) => {
        if (revisionId !== currentRevisionId) {
            setIsRevision(true)
            await queryClient.invalidateQueries({ queryKey: ["projectApiData", projectId] })
            await queryClient.invalidateQueries({ queryKey: ["revisionApiData", currentRevisionId] })
        }
        navigate(`revision/${currentRevisionId}`)
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

        if (currentContext) {
            navigate(`/${currentContext.id}`)
        } else {
            navigate("/")
        }
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
