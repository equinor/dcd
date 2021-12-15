import React from 'react'
import styled from 'styled-components'
import ExpandableProject from './ExpandableProject'

const SidebarDiv = styled.div`
    width: 15rem;
    display: flex;
    border-right: 1px solid lightgrey;
`

export type MenuItem = {
    name: string
    id: string
    cases: CaseItem[]
}

export type CaseItem = {
    title: string
    id: string
}

const menuItems = [
    {
        name: 'Project 1',
        id: '78hdkssjd7c73ndks38shsn',
        cases: [
            { title: 'Case 1', id: '7jssgs62hajk' },
            { title: 'Case 2', id: 'hdgsiwksjs6l' },
            { title: 'Case 3', id: 'i83uhdgdte73' },
            { title: 'Case 4', id: 'ksstegdb83jk' },
            { title: 'Case 5', id: '9ked63hsvdgd' },
            { title: 'Case 6', id: '73jshdgfyegd' },
        ],
    },
    {
        name: 'Project 2',
        id: 'js83hdytdgsdhffh63hsfs',
        cases: [
            { title: 'Case 1', id: 'gdhj63dhdjkd' },
            { title: 'Case 2', id: 'dhhdj3dhdjkd' },
        ],
    },
    {
        name: 'Project 3',
        id: 'h63fdt3d63a8jfgyd-73isgs',
        cases: [
            { title: 'Case 1', id: 'hd63hdjd8sjs' },
            { title: 'Case 2', id: 'hfgded7edsgs' },
            { title: 'Case 3', id: '83jdhfftehss' },
            { title: 'Case 4', id: 'fkfjetsshdye' },
        ],
    },
    {
        name: 'Project 4',
        id: 'hsye7362jdkhfg73hsgdf73',
        cases: [
            { title: 'Case 1', id: '83jshddtwgsj' },
            { title: 'Case 2', id: 'kdjeuhdgfyeh' },
            { title: 'Case 3', id: 'ieje83shsh6d' },
            { title: 'Case 4', id: '93kshdgteshs' },
            { title: 'Case 5', id: '93kfyehdsnsh' },
        ],
    },
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
