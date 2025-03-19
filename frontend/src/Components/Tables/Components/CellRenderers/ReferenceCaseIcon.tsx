import { Button, Icon, Tooltip } from "@equinor/eds-core-react"
import { bookmark_filled, bookmark_outlined } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import styled from "styled-components"

import { useAppStore } from "@/Store/AppStore"

const CaseViewIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 4px;
`

const CasesTableIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: -8px;
    margin-left: -8px;
`

const SideBarIcon = styled(Icon)<{ $sidebarOpen?: boolean }>`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: ${({ $sidebarOpen }) => (!$sidebarOpen ? "0px" : "6px")};
    margin-left: ${({ $sidebarOpen }) => ($sidebarOpen ? "0px" : "6px")};
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
    const { sidebarOpen } = useAppStore()

    if (iconPlacement === "sideBar") {
        return (
            <Tooltip title="Reference case">
                <SideBarIcon data={bookmark_filled} size={16} $sidebarOpen={sidebarOpen} />
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
