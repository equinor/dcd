import React from "react"
import { QueryClient } from "@tanstack/react-query"
import { NavigateFunction } from "react-router"
import { ContextItem } from "@equinor/fusion-framework-react-module-context"
import { GetProjectService } from "@/Services/ProjectService"

export const openRevisionModal = (setCreatingRevision: React.Dispatch<React.SetStateAction<boolean>>) => {
    console.log("Creating revision")
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
    externalId: string | undefined,
    navigate: NavigateFunction,
) => {
    setIsRevision(true)
    queryClient.invalidateQueries(
        { queryKey: ["projectApiData", externalId] },
    )
    navigate(`revision/${revisionId}`)
}

// export this function
export const exitRevisionView = (
    setIsRevision: React.Dispatch<React.SetStateAction<boolean>>,
    queryClient: QueryClient,
    externalId: string | undefined,
    currentContext: ContextItem | null | undefined,
    navigate: NavigateFunction,
) => {
    setIsRevision(false)
    queryClient.invalidateQueries(
        { queryKey: ["projectApiData", externalId] },
    )

    if (currentContext) {
        navigate(`/${currentContext.id}`)
    } else {
        navigate("/")
    }
    console.log("Exiting revision view")
}

export const disableCurrentRevision = (revisionId: string, isRevision: boolean, location: any) => {
    // this is stupid
    const currentRevisionId = location.pathname.split("/revision/")[1]
    if (isRevision && currentRevisionId === revisionId) {
        return true
    } return false
}
