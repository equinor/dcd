import React, { useEffect } from 'react'
import styled from 'styled-components'
import { useNavigate } from 'react-router-dom'
import { Icon, SingleSelect, Typography } from '@equinor/eds-core-react'
import { chevron_up, search } from '@equinor/eds-icons'
import { tokens } from '@equinor/eds-tokens'
import { UseComboboxStateChange } from 'downshift'

import { projects } from '../Components/SideMenu/SideMenu'
import { ProjectService } from '../Services/ProjectService'

const Wrapper = styled.div`
    margin: 2rem;
    width: 100%;
`

const ProjectSelect = styled.div`
    display: flex;
    align-items: center;
`

const ProjectDropdown = styled(SingleSelect)`
    width: 25rem;
    margin-left: 0.5rem;
`

const ArrowUp = styled(Icon)`
    margin-left: 7rem;
    margin-top: 0.5rem;
`

const ChooseProjectText = styled(Typography)`
    margin-left: 2rem;
`

const DashboardView = () => {
    const navigate = useNavigate()

    useEffect(() => {
        (async () => {
            try {
                const res = await ProjectService.getProjects()
                console.log(res)
            } catch (error) {
                console.error(error)
            }
        })()
    }, [])

    const onSelected = (selectedValue: string | null | undefined) => {
        const project = projects.find(p => p.name === selectedValue)
        if (project) {
            navigate(`/project/${project.id}`)
        }
    }

    const grey = tokens.colors.ui.background__scrim.rgba

    return (
        <Wrapper>
            <ProjectSelect>
                <Icon data={search} color={grey}></Icon>
                <ProjectDropdown
                    label={''}
                    placeholder={'Search projects'}
                    items={projects.map(p => p.name)}
                    handleSelectedItemChange={(changes: UseComboboxStateChange<string>) => onSelected(changes.selectedItem)}
                />
            </ProjectSelect>
            <ArrowUp data={chevron_up} color={grey}></ArrowUp>
            <ChooseProjectText>Start by choosing a project.</ChooseProjectText>
        </Wrapper>
    )
}

export default DashboardView
