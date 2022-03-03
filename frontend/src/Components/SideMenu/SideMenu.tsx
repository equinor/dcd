import { chevron_left } from '@equinor/eds-icons'
import { Divider, Icon, Typography } from '@equinor/eds-core-react'
import { useNavigate, useParams } from 'react-router-dom'
import { useEffect, useState } from 'react'
import styled from 'styled-components'

import { useTranslation } from "react-i18next";

import ProjectMenu from './ProjectMenu'
import { Project } from '../../models/Project'
import { ProjectService } from '../../Services/ProjectService'

const SidebarDiv = styled.div`
    width: 15rem;
    display: flex;
    border-right: 1px solid lightgrey;
    display: flex;
    flex-direction: column;
`

const ReturnToSearch = styled.div`
    display: flex;
    align-items: center;
    padding: 1rem 1rem 0 1rem;
    cursor: pointer;
`

const StyledDivider = styled(Divider)`
    width: 80%;
`

export const projects = [
    {
        name: 'Project 1',
        id: '78hdkssjd7c73ndks38shsn',
        createdDate: 1641387278188,
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
        createdDate: 1633434946771,
        cases: [
            { title: 'Case 1', id: 'gdhj63dhdjkd', capex: 1200, drillex: 700, ur: 150 },
            { title: 'Case 2', id: 'dhhdj3dhdjkd', capex: 1400, drillex: 900, ur: 90 },
        ],
    },
    {
        name: 'Project 3',
        id: 'h63fdt3d63a8jfgyd-73isgs',
        createdDate: 1622894187502,
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
        createdDate: 1641387546683,
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
    const { t } = useTranslation();
    const [project, setProject] = useState<Project>()
    const navigate = useNavigate()
    const params = useParams()

    useEffect(() => {
        if (params.projectId) {
            (async () => {
                try {
                    setProject(await ProjectService.getProjectByID(params.projectId!))
                } catch (error) {
                    console.error()
                }
            })()
        }
    }, [params])

    const returnToSearch = () => {
        navigate('/')
    }

    if (project) {
        return (
            <SidebarDiv>
                <ReturnToSearch onClick={returnToSearch}>
                    <Icon data={chevron_left} size={24} />
                    <Typography>{t('SideMenu.BackToSearch')}</Typography>
                </ReturnToSearch>
                <StyledDivider />
                <ProjectMenu project={project} />
            </SidebarDiv>
        )
    } else {
        return <></>
    }
}

export default SideMenu
