import React, { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import styled from 'styled-components'
import { IconData } from '@equinor/eds-icons'

import { Case } from '../../types'
import { ProjectMenuItemType } from './ProjectMenu'
import MenuItem from './MenuItem'

const ExpandableDiv = styled.div`
    display: flex;
    flex-direction: column;
    padding: 0.25rem 0 0.25rem 2rem;
    cursor: pointer;
`

const StyledLi = styled.li`
    list-style-type: none;
    margin: 0;
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
    subItems?: Case[]
}

const ProjectMenuItemComponent = ({ item, projectId, subItems }: Props) => {
    const params = useParams()
    const isSelectedProjectMenuItem =
        (item.name === ProjectMenuItemType.OVERVIEW && params.caseId === undefined) ||
        (item.name === ProjectMenuItemType.CASES && params.caseId !== undefined)
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
                onClick={subItems ? () => setIsOpen(!isOpen) : undefined}
            />
            {subItems && isOpen && (
                <ul style={{ padding: 0, margin: 0, width: '100%' }}>
                    {subItems.map((subItem, index) => (
                        <StyledLi style={{ listStyleType: 'none', margin: 0, padding: 0 }} key={index}>
                            <nav>
                                <Link to={'/project/' + projectId + '/case/' + subItem.id} style={{ textDecoration: 'none' }}>
                                    <MenuItem
                                        title={subItem.title}
                                        style={{ padding: '0.25rem 2rem' }}
                                        isSelected={isSelected && params.caseId === subItem.id}
                                    />
                                </Link>
                            </nav>
                        </StyledLi>
                    ))}
                </ul>
            )}
        </ExpandableDiv>
    )
}

export default ProjectMenuItemComponent
