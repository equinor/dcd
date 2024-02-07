import { useState } from "react"
import styled from "styled-components"
import { Link } from "react-router-dom"
import { file, folder, dashboard } from "@equinor/eds-icons"

import { useCurrentContext } from "@equinor/fusion"
import MenuItem from "./MenuItem"
import ProjectMenuItemComponent from "./ProjectMenuItemComponent"

import { ProjectPath } from "../../Utils/common"

const ExpandableDiv = styled.div`
    display: flex;
    flex-direction: column;
    padding: 0.25rem 1rem;
    cursor: pointer;
    width: 90%;
`

const Item = styled.li`
    list-style-type: none;
    margin: 0;
    display: flex;
    flex-direction: column;
    padding: 0.25rem 0 0.25rem 1.75rem;
    cursor: pointer;
`

const LinkWithoutStyle = styled(Link)`
    text-decoration: none;
`

const MenuItems = styled.ul`
    padding: 0;
    margin: 0;
    width: 100%;
`

export enum ProjectMenuItemType {
    OVERVIEW = "Overview",
    CASES = "Cases",
}

const projectMenuItems = [
    { name: ProjectMenuItemType.OVERVIEW, icon: dashboard },
    { name: ProjectMenuItemType.CASES, icon: file },
]

interface Props {
    project: Components.Schemas.ProjectDto
}

function ProjectMenu({ project }: Props) {
    const [isOpen, setIsOpen] = useState<boolean>(true)
    const currentProject = useCurrentContext()

    return (
        <ExpandableDiv>
            <nav>
                <LinkWithoutStyle to={ProjectPath(currentProject?.externalId!)}>
                    <MenuItem
                        title={project.name!}
                        isSelected={currentProject?.externalId === project.id}
                        icon={folder}
                        isOpen={isOpen}
                        onClick={() => setIsOpen(!isOpen)}
                    />
                </LinkWithoutStyle>
            </nav>
            {isOpen && (
                <MenuItems>
                    {projectMenuItems.map((projectMenuItem, index) => (
                        <Item key={`project-menu-item-${index + 1}`}>
                            {projectMenuItem.name === ProjectMenuItemType.OVERVIEW && (
                                <nav>
                                    <LinkWithoutStyle to={`/${currentProject?.id}`}>
                                        <ProjectMenuItemComponent
                                            project={project}
                                            item={projectMenuItem}
                                            projectId={project.id}
                                        />
                                    </LinkWithoutStyle>
                                </nav>
                            )}
                            {projectMenuItem.name === ProjectMenuItemType.CASES && (
                                <ProjectMenuItemComponent
                                    project={project}
                                    item={projectMenuItem}
                                    projectId={project.id}
                                    subItems={project.cases}
                                />
                            )}
                        </Item>
                    ))}
                </MenuItems>
            )}
        </ExpandableDiv>
    )
}

export default ProjectMenu
