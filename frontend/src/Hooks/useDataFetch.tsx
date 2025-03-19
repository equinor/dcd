import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import { useEffect, useState } from "react"
import { useParams } from "react-router"

import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Store/ProjectContext"

export const useDataFetch = () => {
    const { currentContext } = useModuleCurrentContext()
    const { projectId, isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const [data, setData] = useState<Components.Schemas.ProjectDataDto | Components.Schemas.RevisionDataDto | undefined | null>(null)

    const externalId = currentContext?.externalId

    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId && !isRevision,
    })

    const { data: apiRevisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!revisionId && isRevision && !!projectId,
    })

    useEffect(() => {
        if (isRevision) {
            setData(apiRevisionData as Components.Schemas.RevisionDataDto)
        } else {
            setData(projectApiData as Components.Schemas.ProjectDataDto)
        }
    }, [projectApiData, apiRevisionData, isRevision])

    return data
}
