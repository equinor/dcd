import { useEffect, useState } from "react"
import styled from "styled-components"
import { Link, useParams } from "react-router-dom"
import { IconData } from "@equinor/eds-icons"

import { useCurrentContext } from "@equinor/fusion"
import { ProjectMenuItemType } from "./ProjectMenu"
import MenuItem from "./MenuItem"
import { casePath } from "../../Utils/common"
import { useAppContext } from "../../context/AppContext"

const ExpandableDiv = styled.div`
    display: flex;
    flex-direction: column;
    padding: 0.25rem 0 0.25rem 2rem;
    cursor: pointer;
`

const SubItems = styled.ul`
    padding: 0;
    margin: 0;
    width: 100%;
`

const SubItem = styled.li`
    list-style-type: none;
    margin: 0;
    display: flex;
    flex-direction: column;
    padding: 0;
    cursor: pointer;
`

const LinkWithoutStyle = styled(Link)`
    text-decoration: none;
`

export type ProjectMenuItem = {
    name: string
    icon: IconData
}

interface Props {
    item: ProjectMenuItem
}

const ProjectMenuItemComponent = ({
    item,
}: Props) => {
    const { caseId } = useParams<Record<string, string | undefined>>()
    const { project } = useAppContext()
    const currentProject = useCurrentContext()

    const isSelectedProjectMenuItem = (item.name === ProjectMenuItemType.OVERVIEW && caseId === undefined) || (item.name === ProjectMenuItemType.CASES && caseId !== undefined)
    const isSelected = currentProject?.externalId === project?.id && isSelectedProjectMenuItem
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
                onClick={project && project.cases ? () => setIsOpen(!isOpen) : undefined}
            />
            {project && project.cases && isOpen && (
                <SubItems>
                    {project.cases.map((subItem, index) => (
                        <SubItem key={`menu-sub-item-${index + 1}`}>
                            <nav>
                                <LinkWithoutStyle to={casePath(
                                    currentProject?.id!,
                                    subItem.id ? subItem.id : "",
                                )}
                                >
                                    <MenuItem
                                        title={subItem.name ? subItem.name : "Untitled"}
                                        isSelected={isSelected && caseId === subItem.id}
                                        padding="0.25rem 2rem"
                                        caseItem={subItem}
                                    />
                                </LinkWithoutStyle>
                            </nav>
                        </SubItem>
                    ))}
                </SubItems>
            )}
        </ExpandableDiv>
    )
}

export default ProjectMenuItemComponent
