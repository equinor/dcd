import { useEffect, useState } from "react"
import styled from "styled-components"
import { Link, useParams } from "react-router-dom"
import { IconData } from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { SidebarItemType } from "./Sidebar"
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

export type ProjectMenuItemType = {
    name: string
    icon: IconData
}

interface Props {
    item: ProjectMenuItemType
}

const ProjectMenuItem = ({
    item,
}: Props) => {
    const { caseId } = useParams<Record<string, string | undefined>>()
    const { project } = useAppContext()
    const { currentContext } = useModuleCurrentContext()

    const isSelectedProjectMenuItem = (item.name === SidebarItemType.OVERVIEW && caseId === undefined) || (item.name === SidebarItemType.CASES && caseId !== undefined)
    const isSelected = currentContext?.externalId === project?.id && isSelectedProjectMenuItem
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
                                    currentContext?.id!,
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

export default ProjectMenuItem
