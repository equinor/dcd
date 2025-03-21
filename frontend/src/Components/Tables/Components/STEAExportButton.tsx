import { Button, Icon } from "@equinor/eds-core-react"
import { archive } from "@equinor/eds-icons"
import styled from "styled-components"

import { GetProjectService } from "@/Services/ProjectService"
import { GetSTEAService } from "@/Services/STEAService"

const ButtonContainer = styled.div`
    margin-top: 12px;
    margin-bottom: 24px;
`

interface Props {
    projectOrRevisionId?: string
    setShowRevisionReminder: (show: boolean) => void
}

export const unwrapProjectId = (projectId?: string | undefined | null): string => {
    if (projectId === undefined || projectId === null) {
        throw new Error("Attempted to use a Project ID which does not exist")
    }

    return projectId
}

export const STEAExportButton = ({ projectOrRevisionId, setShowRevisionReminder }: Props) => {
    const handleSTEAExport = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault()
        if (!projectOrRevisionId) { return }

        try {
            setShowRevisionReminder(true)
            const unwrappedProjectId = unwrapProjectId(projectOrRevisionId)
            const projectResult = await GetProjectService().getProject(unwrappedProjectId)

            await GetSTEAService().excelToSTEA(projectResult)
        } catch (error) {
            console.error("[ProjectView] Error while exporting to STEA", error)
        }
    }

    return (
        <ButtonContainer>
            <Button variant="outlined" onClick={handleSTEAExport}>
                <Icon data={archive} size={18} />
                Download input to STEA
            </Button>
        </ButtonContainer>
    )
}
