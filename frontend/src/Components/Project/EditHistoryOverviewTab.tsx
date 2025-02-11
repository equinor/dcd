import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"

import CaseEditHistory from "@/Components/Sidebar/Components/EditHistory/CaseEditHistory"
import { useCaseStore } from "@/Store/CaseStore"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { sortUtcDateStrings } from "@/Utils/DateUtils"

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
    const { caseEdits } = useCaseStore()
    const revisionAndProjectData = useDataFetch()

    if (!revisionAndProjectData) {
        return null
    }

    if (revisionAndProjectData.commonProjectAndRevisionData.cases.length === 0) {
        return <Typography>The edit history for this project&apos;s cases will appear here once cases are created.</Typography>
    }

    if (caseEdits.length === 0) {
        return <Typography>No edits have been made to the cases recently.</Typography>
    }

    return (
        <Container>
            {revisionAndProjectData.commonProjectAndRevisionData.cases
                .sort((a, b) => sortUtcDateStrings(a.createTime, b.createTime))
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
