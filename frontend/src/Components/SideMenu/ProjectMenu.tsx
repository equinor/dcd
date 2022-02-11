import React, { useState } from 'react'
import styled from 'styled-components'
import { Link, useParams } from 'react-router-dom'
import { file, folder, dashboard } from '@equinor/eds-icons'

import MenuItem from './MenuItem'
import ProjectMenuItemComponent from './ProjectMenuItemComponent'

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
    OVERVIEW = 'Overview',
    CASES = 'Cases',
}

const projectMenuItems = [
    { name: ProjectMenuItemType.OVERVIEW, icon: dashboard },
    { name: ProjectMenuItemType.CASES, icon: file },
]

interface Props {
    project: Components.Schemas.ProjectDto
}

const ProjectMenu = ({ project }: Props) => {
    const params = useParams()
    const [isOpen, setIsOpen] = useState<boolean>(params.projectId === project.projectId)

    return (
        <ExpandableDiv>
            <nav>
                <LinkWithoutStyle to={'/project/' + project.projectId}>
                    <MenuItem
                        title={project.name!}
                        isSelected={params.projectId === project.projectId}
                        icon={folder}
                        isOpen={isOpen}
                        onClick={() => setIsOpen(!isOpen)}
                    />
                </LinkWithoutStyle>
            </nav>
            {isOpen && (
                <MenuItems>
                    {projectMenuItems.map((projectMenuItem, index) => {
                        return (
                            <Item key={index}>
                                {projectMenuItem.name === ProjectMenuItemType.OVERVIEW && (
                                    <nav>
                                        <LinkWithoutStyle to={'/project/' + project.projectId}>
                                            <ProjectMenuItemComponent item={projectMenuItem} projectId={project.projectId!} />
                                        </LinkWithoutStyle>
                                    </nav>
                                )}
                                {projectMenuItem.name === ProjectMenuItemType.CASES && (
                                    <ProjectMenuItemComponent item={projectMenuItem} projectId={project.projectId!} subItems={project.cases!} />
                                )}
                            </Item>
                        )
                    })}
                </MenuItems>
            )}
        </ExpandableDiv>
    )
}

export default ProjectMenu
