import { useQuery } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useParams } from "react-router"

import { useProjectContext } from "@/Context/ProjectContext"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"

export const useDataFetch = () => {
    const { currentContext } = useModuleCurrentContext()
    const { isRevision, projectId } = useProjectContext()
    const { revisionId } = useParams()

    const externalId = currentContext?.externalId

    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId && !isRevision,
    })

    const { data: apiRevisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!projectId && !!revisionId && !!externalId && isRevision,
    })

    const data = isRevision ? apiRevisionData : projectApiData

    return data
}
