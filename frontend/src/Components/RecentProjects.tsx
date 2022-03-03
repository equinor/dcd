import { tokens } from '@equinor/eds-tokens'
import { Typography, Card } from '@equinor/eds-core-react'
import { useCallback } from 'react'
import styled from 'styled-components'
import { useTranslation } from "react-i18next";

import { Project } from '../models/Project'

import { ProjectPhaseNumberToText, ProjectPath } from '../Utils/common'

const Wrapper = styled.div`
    margin-top: 4rem;
    width: 90%;
`

const RecentProjectTitle = styled(Typography)`
    margin: 0rem 2rem 0rem .5rem;
`

const RecentProjectCardWrapper = styled.div`
    width: 100%;
    display: flex;
    flex-wrap: wrap;
`

const RecentProjectCard = styled(Card)`
    margin: .5rem;
    width: 20rem;
    box-shadow: ${tokens.elevation.raised};
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
    border-radius: 3px;
    display: flex;
    flex-direction: row;
    align-items: flex-start;
    padding: 6px 7px 5px 8px;
`

const CardFooter = styled(Typography)`
    margin: 1rem 0rem;
    color: ${tokens.colors.text.static_icons__tertiary.rgba}
`

const OpenProject = styled(Typography)`
    width: fit-content;
`

interface Props {
    projects: Project[]
}

const RecentProjects = ({ projects }: Props) => {

    const { t } = useTranslation();

    const renderProjectDG = useCallback((phase?: string) => {
        const backgroundColor = phase
            ? tokens.colors.ui.background__info.rgba
            : tokens.colors.ui.background__danger.rgba

        const labelText = phase ?? 'TBD'

        return (
            <ProjectDG style={{background: backgroundColor}} variant="caption">
                {labelText}
            </ProjectDG>
        )
    }, [])

    const renderDescription = useCallback((description?: string | null) => {
        if (!description) {
            return null
        }
        return (
            <Typography variant="caption" lines={4}>
                {description}
            </Typography>
        )
    }, [])

    if (projects?.length === 0) return null

    return (
        <Wrapper>
            <RecentProjectTitle variant="h3">
                {t('RecentProjects.RecentlyUsedProjects')}
            </RecentProjectTitle>
            <RecentProjectCardWrapper>
                {projects?.map((project) => (
                    <RecentProjectCard key={project.id}>
                        <Card.Header>
                            <CardHeaderTitle>
                                <CardHeaderTitleText variant="h5">
                                    {project.name}
                                </CardHeaderTitleText>
                                {renderProjectDG(project.phase?.toString())}
                            </CardHeaderTitle>
                        </Card.Header>
                        <Card.Content>
                            {renderDescription(project.description)}
                            <CardFooter variant="meta">
                                {/* TODO replace once projectDto has date*/}
                                {t('RecentProjects.CreatedDate')}
                            </CardFooter>
                            <OpenProject link
                                href={ProjectPath(project.id!)}>
                                {('RecentProjects.Open')}
                            </OpenProject>
                        </Card.Content>
                    </RecentProjectCard>
                    )
                )}
            </RecentProjectCardWrapper>
        </Wrapper>
    )
}

export default RecentProjects
