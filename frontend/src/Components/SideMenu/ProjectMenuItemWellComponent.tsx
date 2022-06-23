import { useParams } from "react-router"
import { useEffect, useState } from "react"
import styled from "styled-components"
import { IconData } from "@equinor/eds-icons"

import { ProjectMenuItemType } from "./ProjectMenu"
import MenuItem from "./MenuItem"

const ExpandableDiv = styled.div`
    display: flex;
    flex-direction: column;
    padding: 0.25rem 0 0.25rem 2rem;
    cursor: pointer;
`

export type ProjectMenuItem = {
    name: string
    icon: IconData
}

interface Props {
    item: ProjectMenuItem
    projectId: string
}

function ProjectMenuItemWellComponent({ item, projectId }: Props) {
    const params = useParams()
    // eslint-disable-next-line max-len
    const isSelectedProjectMenuItem = (item.name === ProjectMenuItemType.OVERVIEW)
        || (item.name === ProjectMenuItemType.WELLS)
    const isSelected = params.projectId === projectId && isSelectedProjectMenuItem
    const [isOpen, setIsOpen] = useState<boolean>(isSelected)

    useEffect(() => {
        setIsOpen(isSelected)
    }, [isSelected])

    return (
        <ExpandableDiv>
            <MenuItem
                title={item.name}
                isSelected={isSelected}
                icon={item.icon}
                isOpen={isOpen}
            />
        </ExpandableDiv>
    )
}

export default ProjectMenuItemWellComponent
