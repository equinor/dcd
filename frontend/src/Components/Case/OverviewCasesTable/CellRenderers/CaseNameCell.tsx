import { Button, Tooltip } from "@equinor/eds-core-react"
import styled from "styled-components"
import { ReferenceCaseIcon } from "../../Components/ReferenceCaseIcon"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { useAppContext } from "@/Context/AppContext"

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
    const { setEditMode } = useAppContext()

    const navigateToSelectedCase = () => {
        setEditMode(false)
        if (isRevision && revisionId) {
            navigateToRevisionCase(revisionId, data.id)
        } else {
            navigateToCase(data.id)
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
