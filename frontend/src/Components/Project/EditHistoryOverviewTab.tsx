import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"
import CaseEditHistory from "../Case/Components/CaseEditHistory"
import { useProjectContext } from "../../Context/ProjectContext"
import { useCaseContext } from "../../Context/CaseContext"

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
    const { project } = useProjectContext()
    const { caseEdits } = useCaseContext()

    if (!project) {
        return null
    }

    if (project.cases.length === 0) {
        return <Typography>The edit history for this project&apos;s cases will appear here once cases are created.</Typography>
    }

    if (caseEdits.length === 0) {
        return <Typography>No edits have been made to the cases recently.</Typography>
    }

    return (
        <Container>
            {project.cases
                .sort((a, b) => new Date(a.createTime).getTime() - new Date(b.createTime).getTime())
                .map((projectCase, index) => {
                    const filteredEdits = caseEdits.filter((edit) => edit.caseId === projectCase.id)
                    return filteredEdits.length > 0 ? (
                        <CaseEdits key={projectCase.id}>
                            <Typography>{projectCase.name}</Typography>
                            <div>
                                <CaseEditHistory key={`menu-item-${index + 1}`} caseId={projectCase.id} />
                            </div>
                        </CaseEdits>
                    ) : null
                })}
        </Container>
    )
}

export default EditHistoryOverviewTab
