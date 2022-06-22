import { useState } from "react"
import styled from "styled-components"
import { Link, useParams } from "react-router-dom"
import {
    file, folder, dashboard, well,
} from "@equinor/eds-icons"

import { Project } from "../../models/Project"
import MenuItem from "./MenuItem"
import ProjectMenuItemComponent from "./ProjectMenuItemComponent"

import { ProjectPath } from "../../Utils/common"
import ProjectMenuItemWellComponent from "./ProjectMenuItemWellComponent"

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
    WELLS = "Wells",
}

const projectMenuItems = [
    { name: ProjectMenuItemType.OVERVIEW, icon: dashboard },
    { name: ProjectMenuItemType.CASES, icon: file },
    { name: ProjectMenuItemType.WELLS, icon: well },
]

interface Props {
    project: Project
}

function ProjectMenu({ project }: Props) {
    const params = useParams()
    const [isOpen, setIsOpen] = useState<boolean>(params.projectId === project.id)

    return (
        <ExpandableDiv>
            <nav>
                <LinkWithoutStyle to={ProjectPath(project.id)}>
                    <MenuItem
                        title={project.name!}
                        isSelected={params.projectId === project.id}
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
                                    <LinkWithoutStyle to={`/project/${project.id}`}>
                                        <ProjectMenuItemComponent
                                            item={projectMenuItem}
                                            projectId={project.id}
                                        />
                                    </LinkWithoutStyle>
                                </nav>
                            )}
                            {projectMenuItem.name === ProjectMenuItemType.CASES && (
                                <ProjectMenuItemComponent
                                    item={projectMenuItem}
                                    projectId={project.id}
                                    subItems={project.cases}
                                />
                            )}
                            {projectMenuItem.name === ProjectMenuItemType.WELLS && (
                                <nav>
                                    <LinkWithoutStyle to={`/project/${project.id}/wells/`}>
                                        <ProjectMenuItemWellComponent
                                            item={projectMenuItem}
                                            projectId={project.id}
                                        />
                                    </LinkWithoutStyle>
                                </nav>
                            )}
                        </Item>
                    ))}
                </MenuItems>
            )}
        </ExpandableDiv>
    )
}

export default ProjectMenu
