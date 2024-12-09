import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import CaseEditHistory from "../Case/Components/CaseEditHistory"
import { useCaseContext } from "@/Context/CaseContext"
import { projectQueryFn } from "@/Services/QueryFunctions"

const Container = styled.div`
    display: flex;
    flex-direction: row;
    gap: 20px;
    overflow: auto;
`

const CaseEdits = styled.div`
    display: flex;
    flex-direction: column;
    gap: 10px;
    width: 300px;
`

const EditHistoryOverviewTab = () => {
    const { caseEdits } = useCaseContext()
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    if (!apiData) {
        return null
    }

    if (apiData.commonProjectAndRevisionData.cases.length === 0) {
        return <Typography>The edit history for this project&apos;s cases will appear here once cases are created.</Typography>
    }

    if (caseEdits.length === 0) {
        return <Typography>No edits have been made to the cases recently.</Typography>
    }

    return (
        <Container>
            {apiData.commonProjectAndRevisionData.cases
                .sort((a, b) => new Date(a.createTime).getTime() - new Date(b.createTime).getTime())
                .map((projectCase, index) => {
                    const filteredEdits = caseEdits.filter((edit) => edit.caseId === projectCase.caseId)
                    return filteredEdits.length > 0 ? (
                        <CaseEdits key={projectCase.caseId}>
                            <Typography>{projectCase.name}</Typography>
                            <div>
                                <CaseEditHistory key={`menu-item-${index + 1}`} caseId={projectCase.caseId} />
                            </div>
                        </CaseEdits>
                    ) : null
                })}
        </Container>
    )
}

export default EditHistoryOverviewTab
