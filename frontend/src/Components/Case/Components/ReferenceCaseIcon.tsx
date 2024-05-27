import { Icon, Tooltip } from "@equinor/eds-core-react"
import { bookmark_filled } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import styled from "styled-components"

const MenuIcon = styled(Icon)`
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

interface Props {
    iconPlacement?: string
}

const ReferenceCaseIcon = ({ iconPlacement }: Props) => {
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
            <MenuIcon data={bookmark_filled} size={18} />
        </Tooltip>
    )
}

export default ReferenceCaseIcon
