import { Button, Tooltip } from "@equinor/eds-core-react"
import styled from "styled-components"

import { ReferenceCaseIcon } from "../CellRenderers/ReferenceCaseIcon"

import { useAppNavigation } from "@/Hooks/useNavigate"
import { useAppStore } from "@/Store/AppStore"
import { useCaseStore } from "@/Store/CaseStore"
import { caseTabNames } from "@/Utils/Config/constants"

const Wrapper = styled.div`
    justify-content: center;
    align-items: center;
    display: inline-flex;
`

interface CaseNameCellProps {
    value: string
    data: {
        id: string
        referenceCaseId: string
    }
    isRevision: boolean
    revisionId?: string
}

export const CaseNameCell = ({
    value, data, isRevision, revisionId,
}: CaseNameCellProps) => {
    const { navigateToCase, navigateToRevisionCase } = useAppNavigation()
    const { setEditMode } = useAppStore()
    const { activeTabCase } = useCaseStore()

    const navigateToSelectedCase = () => {
        setEditMode(false)
        const currentTab = caseTabNames[activeTabCase]

        if (isRevision && revisionId) {
            navigateToRevisionCase(revisionId, data.id, currentTab)
        } else {
            navigateToCase(data.id, currentTab)
        }
    }

    return (
        <Tooltip title={value} placement="bottom-start">
            <Wrapper>
                {data.referenceCaseId === data.id && (
                    <ReferenceCaseIcon iconPlacement="casesTable" />
                )}
                <Button
                    as="span"
                    variant="ghost"
                    className="GhostButton"
                    onClick={navigateToSelectedCase}
                >
                    {value}
                </Button>
            </Wrapper>
        </Tooltip>
    )
}
