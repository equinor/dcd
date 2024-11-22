import { useEffect, useState } from "react"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { GetProjectService } from "@/Services/ProjectService"
import { useProjectContext } from "@/Context/ProjectContext"
import { projectQueryFn } from "@/Services/QueryFunctions"

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

    const externalId = currentContext?.externalId
    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    useEffect(() => {
        let timeoutId: NodeJS.Timeout | undefined

        if (projectApiData && !projectApiData?.revisionsDetailsList.find((r: Components.Schemas.RevisionDetailsDto) => r.revisionId === revisionId)) {
            timeoutId = setTimeout(() => {
                exitRevisionView()
            }, 150)
        }

        return () => {
            if (timeoutId) {
                clearTimeout(timeoutId)
            }
        }
    }, [projectApiData])

    return {
        isRevisionsLoading,
        navigateToRevision,
        createRevision,
        exitRevisionView,
        disableCurrentRevision,
    }
}
