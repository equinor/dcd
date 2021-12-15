import React from 'react'
import styled from 'styled-components'
import ProjectMenu from './ProjectMenu'

const SidebarDiv = styled.div`
    min-width: 15rem;
    display: flex;
    border-right: 1px solid lightgrey;
`

export type Project = {
    name: string
    id: string
    cases: Case[]
}

export type Case = {
    title: string
    id: string
    capex: number
    drillex: number
    ur: number
}

export const projects = [
    {
        name: 'Project 1',
        id: '78hdkssjd7c73ndks38shsn',
        cases: [
            { title: 'Case 1', id: '7jssgs62hajk', capex: 1400, drillex: 900, ur: 120 },
            { title: 'Case 2', id: 'hdgsiwksjs6l', capex: 1200, drillex: 700, ur: 110 },
            { title: 'Case 3', id: 'i83uhdgdte73', capex: 1300, drillex: 500, ur: 140 },
            { title: 'Case 4', id: 'ksstegdb83jk', capex: 1800, drillex: 200, ur: 155 },
            { title: 'Case 5', id: '9ked63hsvdgd', capex: 900, drillex: 100, ur: 110 },
            { title: 'Case 6', id: '73jshdgfyegd', capex: 1400, drillex: 900, ur: 90 },
        ],
    },
    {
        name: 'Project 2',
        id: 'js83hdytdgsdhffh63hsfs',
        cases: [
            { title: 'Case 1', id: 'gdhj63dhdjkd', capex: 1200, drillex: 700, ur: 150 },
            { title: 'Case 2', id: 'dhhdj3dhdjkd', capex: 1400, drillex: 900, ur: 90 },
        ],
    },
    {
        name: 'Project 3',
        id: 'h63fdt3d63a8jfgyd-73isgs',
        cases: [
            { title: 'Case 1', id: 'hd63hdjd8sjs', capex: 1200, drillex: 700, ur: 150 },
            { title: 'Case 2', id: 'hfgded7edsgs', capex: 1300, drillex: 500, ur: 140 },
            { title: 'Case 3', id: '83jdhfftehss', capex: 1400, drillex: 800, ur: 120 },
            { title: 'Case 4', id: 'fkfjetsshdye', capex: 1500, drillex: 600, ur: 110 },
        ],
    },
    {
        name: 'Project 4',
        id: 'hsye7362jdkhfg73hsgdf73',
        cases: [
            { title: 'Case 1', id: '83jshddtwgsj', capex: 1400, drillex: 900, ur: 150 },
            { title: 'Case 2', id: 'kdjeuhdgfyeh', capex: 1200, drillex: 700, ur: 140 },
            { title: 'Case 3', id: 'ieje83shsh6d', capex: 1100, drillex: 500, ur: 130 },
            { title: 'Case 4', id: '93kshdgteshs', capex: 1500, drillex: 400, ur: 120 },
            { title: 'Case 5', id: '93kfyehdsnsh', capex: 1600, drillex: 700, ur: 110 },
        ],
    },
]

const SideMenu = () => {
    return (
        <SidebarDiv>
            <ul style={{ padding: 0, margin: 0, width: '100%', paddingTop: '2rem' }}>
                {projects.map((project, index) => (
                    <li style={{ listStyleType: 'none', margin: 0, padding: 0 }} key={index}>
                        <ProjectMenu project={project} />
                    </li>
                ))}
            </ul>
        </SidebarDiv>
    )
}

export default SideMenu
