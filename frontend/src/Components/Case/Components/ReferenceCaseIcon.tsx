import { Button, Icon, Tooltip } from "@equinor/eds-core-react"
import { bookmark_filled, bookmark_outlined } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import styled from "styled-components"

export const SideBarRefCaseWrapper = styled.div`
    justify-content: center;
    align-items: center;
    display: inline-flex;
    margin-left: 1.1rem;
`

const CaseViewIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 0.2rem;
`

const CasesTableIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: -0.5rem;
    margin-left: -0.5rem;
`

const SideBarIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 0.2rem;
    margin-left: -1.2rem;
`

interface PropsChooseRefCase {
    handleReferenceCaseChange: () => void
    projectRefCaseId: any
    projectCaseId: any
}

export const ChooseReferenceCase = ({ handleReferenceCaseChange, projectRefCaseId, projectCaseId }: PropsChooseRefCase) => {
    if (projectRefCaseId !== projectCaseId) {
        return (
            <Tooltip title="Set as reference case">
                <Button variant="ghost" onClick={handleReferenceCaseChange}>
                    <CaseViewIcon data={bookmark_outlined} size={18} />
                </Button>
            </Tooltip>
        )
    }
    return (
        <Tooltip title="Remove as reference case">
            <Button variant="ghost" onClick={handleReferenceCaseChange}>
                <CaseViewIcon data={bookmark_filled} size={18} />
            </Button>
        </Tooltip>
    )
}

interface PropsShowIcon {
    iconPlacement?: string
}

export const ReferenceCaseIcon = ({ iconPlacement }: PropsShowIcon) => {
    if (iconPlacement === "sideBar") {
        return (
            <Tooltip title="Reference case">
                <SideBarIcon data={bookmark_filled} size={16} />
            </Tooltip>
        )
    }
    if (iconPlacement === "casesTable") {
        return (
            <Tooltip title="Reference case">
                <CasesTableIcon data={bookmark_filled} size={16} />
            </Tooltip>
        )
    }
    return (
        <Tooltip title="Reference case">
            <CaseViewIcon data={bookmark_filled} size={18} />
        </Tooltip>
    )
}
