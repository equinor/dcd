import { Icon, Tooltip } from "@equinor/eds-core-react"
import { bookmark_filled } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import styled from "styled-components"

const MenuIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 0.5rem;
    margin-bottom: -0.2rem;
`

const ReferenceCaseIcon = (project: any, projectCase: any) => {
    if (project?.referenceCaseId === projectCase?.id) {
        return (
            <Tooltip title="Reference case">
                <MenuIcon data={bookmark_filled} size={18} />
            </Tooltip>
        )
    }
    return <></>
}

export default ReferenceCaseIcon
