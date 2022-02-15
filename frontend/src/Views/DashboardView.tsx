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

const RecentProjectsWrapper = styled.div``

const RecentProjectTitle = styled(Typography)`
    margin: 4rem 2rem 0rem .5rem;
    align-self: flex-start;
`

const RecentProjects = styled.div`
    width: 100%;
    display: flex;
    flex-wrap: wrap;
    height: 10rem;
    justify-content: start;
`

const RecentProjectCard = styled(Card)`
    margin: .5rem;
    width: 20rem;
    fill: rgba(255, 255, 255, 1.0);
    box-shadow: 0.0px 1.0px 5.0px 0px rgba(0, 0, 0, 0.2),0.0px 3.0px 4.0px 0px rgba(0, 0, 0, 0.12),0.0px 2.0px 4.0px 0px rgba(0, 0, 0, 0.14);
`

const CardHeaderTitle = styled(Card.HeaderTitle)`
    width: 100%;
    display: flex;
    flex-wrap: wrap;
    flex-direction: column;
    height: 1.5rem;
    align-content: space-between;
`

const CardHeaderTitleText = styled(Typography)`
    padding-top: 2px;
`

const ProjectDG = styled(Typography)`
    background: #D5EAF4;
    border-radius: 3px;
    display: flex;
    flex-direction: row;
    align-items: flex-start;
    padding: 6px 7px 5px 8px;
`

const CardFooter = styled(Typography)`
    margin-top: 1rem;
    color: rgb(111, 111, 111);
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
            <RecentProjectsWrapper>
                <RecentProjectTitle variant="h3">
                    Recently used Projects
                </RecentProjectTitle>
                <RecentProjects>
                    <RecentProjectCard>
                        <Card.Header>
                            <CardHeaderTitle>
                                <CardHeaderTitleText variant="h5">
                                    Some Project Name
                                </CardHeaderTitleText>
                                <ProjectDG variant="caption">
                                    DG1
                                </ProjectDG>
                            </CardHeaderTitle>
                        </Card.Header>
                        <Card.Content>
                            <Typography variant="caption" lines={4}>
                                Lorem ipsum dolor sit amet, consectetur
                                adipiscing elit, sed do eiusmod tempor
                                incididunt ut labore et dolore magna aliqua.
                            </Typography>
                            <CardFooter variant="meta">
                                Created Feb 15, 2022
                            </CardFooter>
                        </Card.Content>
                    </RecentProjectCard>
                    <RecentProjectCard>
                        <Card.Header>
                            <CardHeaderTitle>
                                <CardHeaderTitleText variant="h5">
                                    Some Project Name
                                </CardHeaderTitleText>
                                <ProjectDG variant="caption">
                                    DG1
                                </ProjectDG>
                            </CardHeaderTitle>
                        </Card.Header>
                        <Card.Content>
                            <Typography variant="caption" lines={4}>
                                Lorem ipsum dolor sit amet, consectetur
                                adipiscing elit, sed do eiusmod tempor
                                incididunt ut labore et dolore magna aliqua.
                            </Typography>
                            <CardFooter variant="meta" color="tertiary">
                                Created Feb 15, 2022
                            </CardFooter>
                        </Card.Content>
                    </RecentProjectCard>
                    <RecentProjectCard>
                        <Card.Header>
                            <CardHeaderTitle>
                                <CardHeaderTitleText variant="h5">
                                    Some Project Name
                                </CardHeaderTitleText>
                                <ProjectDG variant="caption">
                                    DG1
                                </ProjectDG>
                            </CardHeaderTitle>
                        </Card.Header>
                        <Card.Content>
                            <Typography variant="caption" lines={4}>
                                Lorem ipsum dolor sit amet, consectetur
                                adipiscing elit, sed do eiusmod tempor
                                incididunt ut labore et dolore magna aliqua.
                            </Typography>
                            <CardFooter variant="meta" color="tertiary">
                                Created Feb 15, 2022
                            </CardFooter>
                        </Card.Content>
                    </RecentProjectCard>
                    <RecentProjectCard>
                        <Card.Header>
                            <CardHeaderTitle>
                                <CardHeaderTitleText variant="h5">
                                    Some Project Name
                                </CardHeaderTitleText>
                                <ProjectDG variant="caption">
                                    DG1
                                </ProjectDG>
                            </CardHeaderTitle>
                        </Card.Header>
                        <Card.Content>
                            <Typography variant="caption" lines={4}>
                                Lorem ipsum dolor sit amet, consectetur
                                adipiscing elit, sed do eiusmod tempor
                                incididunt ut labore et dolore magna aliqua.
                            </Typography>
                            <CardFooter variant="meta" color="tertiary">
                                Created Feb 15, 2022
                            </CardFooter>
                        </Card.Content>
                    </RecentProjectCard>
                </RecentProjects>
            </RecentProjectsWrapper>
        </Wrapper>
    )
}

export default DashboardView
