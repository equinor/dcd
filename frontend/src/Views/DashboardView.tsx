import React from 'react'
import { useNavigate } from 'react-router-dom'
import { Icon, SingleSelect, Typography } from '@equinor/eds-core-react'
import { chevron_up, search } from '@equinor/eds-icons'
import { tokens } from '@equinor/eds-tokens'
import { UseComboboxStateChange } from 'downshift'

import { projects } from '../Components/SideMenu/SideMenu'

const DashboardView = () => {
    const navigate = useNavigate()

    const onSelected = (selectedValue: string | null | undefined) => {
        const project = projects.find(p => p.name === selectedValue)
        if (project) {
            navigate(`/project/${project.id}`)
        }
    }

    const grey = tokens.colors.ui.background__scrim.rgba

    return (
        <div style={{ margin: '2rem', width: '100%' }}>
            <div style={{ display: 'flex', alignItems: 'center' }}>
                <Icon data={search} color={grey}></Icon>
                <SingleSelect
                    style={{ width: '25rem', marginLeft: '0.5rem' }}
                    label={''}
                    placeholder={'Search projects'}
                    items={projects.map(p => p.name)}
                    handleSelectedItemChange={(changes: UseComboboxStateChange<string>) => onSelected(changes.selectedItem)}
                />
            </div>
            <Icon data={chevron_up} color={grey} style={{ marginLeft: '7rem', marginTop: '0.5rem' }}></Icon>
            <Typography style={{ marginLeft: '2rem' }}>Start by choosing a project.</Typography>
        </div>
    )
}

export default DashboardView
