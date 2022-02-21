import React, { useEffect, useState } from 'react'
import styled from 'styled-components'
import { Link, useParams } from 'react-router-dom'
import { IconData } from '@equinor/eds-icons'

import { ProjectMenuItemType } from './ProjectMenu'
import MenuItem from './MenuItem'
import { CasePath } from '../../Utils/common'

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
    projectId: string
    subItems?: Components.Schemas.CaseDto[]
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
                <SubItems>
                    {subItems.map((subItem, index) => (
                        <SubItem key={index}>
                            <nav>
                                <LinkWithoutStyle to={CasePath(projectId, subItem.id!)}>
                                    <MenuItem
                                        title={subItem.name!}
                                        isSelected={isSelected && params.caseId === subItem.id}
                                        padding={'0.25rem 2rem'}
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
