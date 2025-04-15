import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"

import { caseQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Store/ProjectContext"

export const useCaseApiData = () => {
    const { caseId, revisionId } = useParams()
    const { projectId, isRevision } = useProjectContext()

    const {
        data: apiData, isLoading, error, ...rest
    } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
        staleTime: 5000,
        gcTime: 30000,
    })

    return {
        apiData, isLoading, error, ...rest,
    }
}
