import React, { useState } from 'react'
import styled from 'styled-components'
import { file, folder, dashboard } from '@equinor/eds-icons'
import { MenuItem } from './SideMenu'
import MenuItemHeader from './MenuItemHeader'
import ExpandableProjectMenuItem from './ExpandableProjectMenuItem'

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

const subItems = [
    { name: 'Overview', icon: dashboard },
    { name: 'Cases', icon: file },
]

interface Props {
    item: MenuItem
}

const ExpandableProject = ({ item }: Props) => {
    const [isOpen, setIsOpen] = useState<boolean>(false)

    return (
        <ExpandableDiv>
            <MenuItemHeader title={item.name} icon={folder} isOpen={isOpen} onClick={() => setIsOpen(!isOpen)} />
            {isOpen && (
                <ul style={{ padding: 0, margin: 0, width: '100%' }}>
                    {subItems.map((projectMenuItem, index) => {
                        return (
                            <StyledLi style={{ listStyleType: 'none', margin: 0, padding: 0 }} key={index}>
                                <ExpandableProjectMenuItem
                                    item={projectMenuItem}
                                    subItems={projectMenuItem.name === 'Cases' ? item.cases : undefined}
                                />
                            </StyledLi>
                        )
                    })}
                </ul>
            )}
        </ExpandableDiv>
    )
}

export default ExpandableProject
