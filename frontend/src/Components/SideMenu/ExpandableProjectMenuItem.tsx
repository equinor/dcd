import React, { useState } from 'react'
import styled from 'styled-components'
import { IconData } from '@equinor/eds-icons'
import MenuItemHeader from './MenuItemHeader'

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
    subItems?: string[]
}

const ExpandableProjectMenuItem = ({ item, subItems }: Props) => {
    const [isOpen, setIsOpen] = useState<boolean>(false)

    return (
        <ExpandableDiv>
            <MenuItemHeader title={item.name} icon={item.icon} isOpen={isOpen} onClick={subItems ? () => setIsOpen(!isOpen) : undefined} />
            {subItems && isOpen && (
                <ul style={{ padding: 0, margin: 0, width: '100%' }}>
                    {subItems.map((subItem, index) => (
                        <StyledLi style={{ listStyleType: 'none', margin: 0, padding: 0 }} key={index}>
                            <MenuItemHeader title={subItem} style={{ padding: '0.25rem 2rem' }} />
                        </StyledLi>
                    ))}
                </ul>
            )}
        </ExpandableDiv>
    )
}

export default ExpandableProjectMenuItem
