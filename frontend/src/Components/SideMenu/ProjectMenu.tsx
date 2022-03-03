import { useState } from 'react'
import styled from 'styled-components'
import { Link, useParams } from 'react-router-dom'
import { file, folder, dashboard } from '@equinor/eds-icons'

import { useTranslation } from "react-i18next";

import { Project } from '../../models/Project'
import MenuItem from './MenuItem'
import ProjectMenuItemComponent from './ProjectMenuItemComponent'

import { ProjectPath } from '../../Utils/common'

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
//TODO, need to translate enums. 
export enum ProjectMenuItemType {
    OVERVIEW = 'Overview',
    CASES = 'Cases'
}

const projectMenuItems = [
    { name: ProjectMenuItemType.OVERVIEW, icon: dashboard },
    { name: ProjectMenuItemType.CASES, icon: file },
]

interface Props {
    project: Project
}

const ProjectMenu = ({ project }: Props) => {
    const { t } = useTranslation();
    const params = useParams()
    const [isOpen, setIsOpen] = useState<boolean>(params.projectId === project.id)

    return (
        <ExpandableDiv>
            <nav>
                <LinkWithoutStyle to={ProjectPath(project.id!)}>
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
                    {projectMenuItems.map((projectMenuItem, index) => {
                        return (
                            <Item key={index}>
                                {projectMenuItem.name === ProjectMenuItemType.OVERVIEW && (
                                    <nav>
                                        <LinkWithoutStyle to={'/project/' + project.id}>
                                            <ProjectMenuItemComponent item={projectMenuItem} projectId={project.id!} />
                                        </LinkWithoutStyle>
                                    </nav>
                                )}
                                {projectMenuItem.name === ProjectMenuItemType.CASES && (
                                    <ProjectMenuItemComponent item={projectMenuItem} projectId={project.id!} subItems={project.cases!} />
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
