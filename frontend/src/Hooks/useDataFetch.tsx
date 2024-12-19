import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useParams } from "react-router"
import { useEffect, useState } from "react"

import { useProjectContext } from "@/Context/ProjectContext"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"

export const useDataFetch = () => {
    const { currentContext } = useModuleCurrentContext()
    const { isRevision } = useProjectContext()
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
        queryFn: () => revisionQueryFn(externalId, revisionId),
        enabled: !!revisionId && !!externalId && isRevision,
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
