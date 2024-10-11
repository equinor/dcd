import React from "react"
import { QueryClient } from "@tanstack/react-query"
import { NavigateFunction } from "react-router"
import { ContextItem } from "@equinor/fusion-framework-react-module-context"
import { GetProjectService } from "@/Services/ProjectService"

export const openRevisionModal = (setCreatingRevision: React.Dispatch<React.SetStateAction<boolean>>) => {
    setCreatingRevision(true)
}

export const createRevision = async (
    projectId: string,
    setCreatingRevision: React.Dispatch<React.SetStateAction<boolean>>,
) => {
    const projectService = await GetProjectService()
    const newRevision = await projectService.createRevision(projectId)
    if (newRevision) {
        setCreatingRevision(false)
    }
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
