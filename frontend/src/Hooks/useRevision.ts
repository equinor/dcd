import { useState } from "react"
import { useQueryClient } from "@tanstack/react-query"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { GetProjectService } from "@/Services/ProjectService"
import { useProjectContext } from "@/Context/ProjectContext"

export const useRevisions = () => {
    const { isRevision, setIsRevision, projectId } = useProjectContext()
    const { currentContext } = useModuleCurrentContext()
    const queryClient = useQueryClient()
    const { revisionId } = useParams()
    const navigate = useNavigate()

    const [isRevisionsLoading, setIsRevisionsLoading] = useState(false)

    const navigateToRevision = (
        currentRevisionId: string,
    ) => {
        setIsRevision(true)
        queryClient.invalidateQueries({ queryKey: ["projectApiData", projectId] })
        navigate(`revision/${currentRevisionId}`)
    }

    const createRevision = async (
        revision: Components.Schemas.CreateRevisionDto,
        setIsModalOpen?: React.Dispatch<React.SetStateAction<boolean>>,
    ) => {
        setIsRevisionsLoading(true)
        const projectService = await GetProjectService()
        const newRevision = await projectService.createRevision(projectId, revision)
        if (newRevision) {
            setIsRevisionsLoading(false)
            if (setIsModalOpen) {
                setIsModalOpen(false)
            }
            navigateToRevision(newRevision.id)
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
