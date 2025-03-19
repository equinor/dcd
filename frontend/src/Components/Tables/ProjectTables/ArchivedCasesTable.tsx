import {
    Button, Icon, Tooltip, Typography,
} from "@equinor/eds-core-react"
import { arrow_drop_down, arrow_drop_up } from "@equinor/eds-icons"
import styled from "styled-components"
import { useState } from "react"
import { TableCase } from "@/Models/Interfaces"
import { CasesTable } from "./CasesTable"

const ArchivedTitle = styled.div`
    margin-bottom: 12px;
    display: flex;
`

const ClickableTitle = styled.div`
    cursor: pointer;
`

interface Props {
    cases: TableCase[]
    isRevision: boolean
    revisionId?: string
    onMenuClick: (caseId: string, target: HTMLElement) => void
}

export const ArchivedCasesTable = ({
    cases,
    isRevision,
    revisionId,
    onMenuClick,
}: Props) => {
    const [isExpanded, setIsExpanded] = useState<boolean>(false)

    if (cases.length === 0) { return null }

    return (
        <div>
            <ArchivedTitle>
                <ClickableTitle onClick={() => setIsExpanded(!isExpanded)}>
                    <Typography variant="h3">Archived Cases</Typography>
                </ClickableTitle>
                <Tooltip title={isExpanded ? "Collapse Archived Cases" : "Expand Archived Cases"}>
                    <Button
                        variant="ghost_icon"
                        className="GhostButton"
                        onClick={() => setIsExpanded(!isExpanded)}
                    >
                        <Icon data={isExpanded ? arrow_drop_up : arrow_drop_down} />
                    </Button>
                </Tooltip>
            </ArchivedTitle>
            {isExpanded && (
                <CasesTable
                    cases={cases}
                    isRevision={isRevision}
                    revisionId={revisionId}
                    onMenuClick={onMenuClick}
                />
            )}
        </div>
    )
}
