import React, { useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import styled from 'styled-components'
import { file, folder, dashboard } from '@equinor/eds-icons'

import { Project } from './SideMenu'
import MenuItem from './MenuItem'
import ProjectMenuItemComponent from './ProjectMenuItemComponent'

const ExpandableDiv = styled.div`
    display: flex;
    flex-direction: column;
    padding: 0.25rem 1rem;
    cursor: pointer;
`

const StyledLi = styled.li`
    list-style-type: none;
    margin: 0;
    display: flex;
    flex-direction: column;
    padding: 0.25rem 0 0.25rem 1.75rem;
    cursor: pointer;
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
    project: Project
}

const ProjectMenu = ({ project }: Props) => {
    const params = useParams()
    const [isOpen, setIsOpen] = useState<boolean>(params.projectId === project.id)

    return (
        <ExpandableDiv>
            <nav>
                <Link to={'/project/' + project.id} style={{ textDecoration: 'none' }}>
                    <MenuItem
                        title={project.name}
                        isSelected={params.projectId === project.id}
                        icon={folder}
                        isOpen={isOpen}
                        onClick={() => setIsOpen(!isOpen)}
                    />
                </Link>
            </nav>
            {isOpen && (
                <ul style={{ padding: 0, margin: 0, width: '100%' }}>
                    {projectMenuItems.map((projectMenuItem, index) => {
                        return (
                            <StyledLi style={{ listStyleType: 'none', margin: 0, padding: 0 }} key={index}>
                                {projectMenuItem.name === ProjectMenuItemType.OVERVIEW && (
                                    <nav>
                                        <Link to={'/project/' + project.id} style={{ textDecoration: 'none' }}>
                                            <ProjectMenuItemComponent item={projectMenuItem} projectId={project.id} />
                                        </Link>
                                    </nav>
                                )}
                                {projectMenuItem.name === ProjectMenuItemType.CASES && (
                                    <ProjectMenuItemComponent item={projectMenuItem} projectId={project.id} subItems={project.cases} />
                                )}
                            </StyledLi>
                        )
                    })}
                </ul>
            )}
        </ExpandableDiv>
    )
}

export default ProjectMenu
