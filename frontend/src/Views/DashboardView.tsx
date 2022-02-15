import { search } from '@equinor/eds-icons'
import { Icon, SingleSelect, Typography, Card } from '@equinor/eds-core-react'
import { tokens } from '@equinor/eds-tokens'
import { UseComboboxStateChange } from 'downshift'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import styled from 'styled-components'

import { projectService } from '../Services/ProjectService'

const Wrapper = styled.div`
    margin: 2rem;
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    margin-top: 4rem;
`

const ProjectSelect = styled.div`
    display: flex;
    align-items: center;
`

const ProjectDropdown = styled(SingleSelect)`
    width: 25rem;
    margin-left: 0.5rem;
`

const FindProjectText = styled(Typography)`
    width: 25rem;
    margin-left: 2rem;
    margin-bottom: 1rem;
`

const RecentProjectCard = styled(Card)`
`

const DashboardView = () => {
    const navigate = useNavigate()

    const [projects, setProjects] = useState<any[]>()

    useEffect(() => {
        if (projectService) {
            (async () => {
                try {
                    const res = await projectService.getProjects()
                    console.log(res)
                    setProjects(res)
                } catch (error) {
                    console.error(error)
                }
            })()
        }
    }, [])

    const onSelected = (selectedValue: string | null | undefined) => {
        const project = projects?.find(p => p.name === selectedValue)
        if (project) {
            navigate(`/project/${project.projectId}`)
        }
    }

    const grey = tokens.colors.ui.background__scrim.rgba

    if (!projects) return null

    return (
        <Wrapper>
            <FindProjectText variant="h2">Find a project</FindProjectText>
            <ProjectSelect>
                <Icon data={search} color={grey}></Icon>
                <ProjectDropdown
                    label={''}
                    placeholder={'Search projects'}
                    items={projects.map(p => p.name)}
                    handleSelectedItemChange={(changes: UseComboboxStateChange<string>) => onSelected(changes.selectedItem)}
                />
            </ProjectSelect>
            <RecentProjectCard>
				<Card.Header>
				  <Card.HeaderTitle>
					<Typography variant="h5">
					  Helo.
					</Typography>
					<Typography variant="body_short">
					  Hi.
					</Typography>
				  </Card.HeaderTitle>
				</Card.Header>
            </RecentProjectCard>
        </Wrapper>
    )
}

export default DashboardView
