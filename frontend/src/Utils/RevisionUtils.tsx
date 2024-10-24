import React from "react"
import { QueryClient } from "@tanstack/react-query"
import { NavigateFunction } from "react-router"
import { ContextItem } from "@equinor/fusion-framework-react-module-context"
import { GetProjectService } from "@/Services/ProjectService"

export const openRevisionModal = (setCreatingRevision: React.Dispatch<React.SetStateAction<boolean>>) => {
    setCreatingRevision(true)
}

export const navigateToRevision = (
    revisionId: string,
    setIsRevision: React.Dispatch<React.SetStateAction<boolean>>,
    queryClient: QueryClient,
    projectId: string | undefined,
    navigate: NavigateFunction,
) => {
    setIsRevision(true)
    queryClient.invalidateQueries(
        { queryKey: ["projectApiData", projectId] },
    )
    navigate(`revision/${revisionId}`)
}

export const createRevision = async (
    projectId: string,
    project: Components.Schemas.CreateRevisionDto,
    setCreatingRevision: React.Dispatch<React.SetStateAction<boolean>>,
    queryClient: QueryClient,
    setIsRevision: React.Dispatch<React.SetStateAction<boolean>>,
    navigate: NavigateFunction,
) => {
    const projectService = await GetProjectService()
    const newRevision = await projectService.createRevision(projectId, project)
    if (newRevision) {
        setCreatingRevision(false)
    }
    navigateToRevision(newRevision.id, setIsRevision, queryClient, projectId, navigate)
}

export const exitRevisionView = (
    setIsRevision: React.Dispatch<React.SetStateAction<boolean>>,
    queryClient: QueryClient,
    projectId: string | undefined,
    currentContext: ContextItem | null | undefined,
    navigate: NavigateFunction,
) => {
    setIsRevision(false)
    queryClient.invalidateQueries(
        { queryKey: ["projectApiData", projectId] },
    )

    if (currentContext) {
        navigate(`/${currentContext.id}`)
    } else {
        navigate("/")
    }
}

export const disableCurrentRevision = (
    revisionId: string,
    isRevision: boolean,
    currentRevisionId: string | undefined,
) => isRevision && currentRevisionId === revisionId
