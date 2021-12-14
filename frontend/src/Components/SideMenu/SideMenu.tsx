import React from 'react'
import styled from 'styled-components'
import ExpandableProject from './ExpandableProject'

const SidebarDiv = styled.div`
    width: 15rem;
    display: flex;
    flex-grow: 1;
    border-right: 1px solid lightgrey;
`

export type MenuItem = {
    name: string
    cases: string[]
}

const menuItems = [
    { name: 'Project 1', cases: ['Case 1', 'Case 2', 'Case 3', 'Case 4', 'Case 5', 'Case 6'] },
    { name: 'Project 2', cases: ['Case 1', 'Case 2'] },
    { name: 'Project 3', cases: ['Case 1', 'Case 2', 'Case 3', 'Case 4'] },
    { name: 'Project 4', cases: ['Case 1', 'Case 2', 'Case 3', 'Case 4', 'Case 5'] },
]

const SideMenu = () => {
    return (
        <SidebarDiv>
            <ul style={{ padding: 0, margin: 0, width: '100%', paddingTop: '2rem' }}>
                {menuItems.map((item, index) => (
                    <li style={{ listStyleType: 'none', margin: 0, padding: 0 }} key={index}>
                        <ExpandableProject item={item} />
                    </li>
                ))}
            </ul>
        </SidebarDiv>
    )
}

export default SideMenu
