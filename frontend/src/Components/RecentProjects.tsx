import { Typography, Card } from '@equinor/eds-core-react'
import styled from 'styled-components'
import { ProjectPhaseNumberToText } from '../Utils/common'

const Wrapper = styled.div``

const RecentProjectTitle = styled(Typography)`
    margin: 4rem 2rem 0rem .5rem;
    align-self: flex-start;
`

const RecentProjectCardWrapper = styled.div`
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

interface Props {
    projects: Components.Schemas.ProjectDto[]
}

const RecentProjects = ({ projects }: Props) => {

    const renderCard = (project: Components.Schemas.ProjectDto,
        index: number) => {
            return (
                <RecentProjectCard key={index}>
                    <Card.Header>
                        <CardHeaderTitle>
                            <CardHeaderTitleText variant="h5">
                                {project.name}
                            </CardHeaderTitleText>
                            <ProjectDG variant="caption">
                                {ProjectPhaseNumberToText(project.projectPhase!)}
                            </ProjectDG>
                        </CardHeaderTitle>
                    </Card.Header>
                    <Card.Content>
                        <Typography variant="caption" lines={4}>
                            {project.description}
                        </Typography>
                        <CardFooter variant="meta">
                            Created Feb 15, 2022
                        </CardFooter>
                    </Card.Content>
                </RecentProjectCard>
            )
        }

    return (
        <Wrapper>
            <RecentProjectTitle variant="h3">
                Recently used Projects
            </RecentProjectTitle>
            <RecentProjectCardWrapper>
                {projects.map((project, index) => renderCard(project, index))}
            </RecentProjectCardWrapper>
        </Wrapper>
    )
}

export default RecentProjects
