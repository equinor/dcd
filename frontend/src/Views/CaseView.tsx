
import { Tabs, Typography } from "@equinor/eds-core-react"
import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import styled from 'styled-components'
import { useTranslation } from "react-i18next";

import { Project } from "../models/Project"
import { Case } from "../models/Case"
import DrainageStrategyView from "./DrainageStrategyView"
import ExplorationView from "./ExplorationView"
import OverviewView from "./OverviewView"
import { GetProjectService } from "../Services/ProjectService"

const {
    List, Tab, Panels, Panel,
} = Tabs

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const CaseHeader = styled(Typography)`
    margin-bottom: 2rem;
`

const CaseView = () => {
    const { t } = useTranslation();
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const params = useParams()

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const handleTabChange = (index: number) => {
        setActiveTab(index)
    }

    if (!project) return null

    return (
        <CaseViewDiv>
            <CaseHeader variant="h2">
                {project.name}
                {" "}
                -
                {caseItem?.name}
            </CaseHeader>
            <Tabs activeTab={activeTab} onChange={handleTabChange}>
                <List>
                    <Tab>{t('CaseView.Overview')}</Tab>
                    <Tab>{t('CaseView.InputData')}</Tab>
                    <Tab>{t('CaseView.TechnicalInput')}</Tab>
                    <Tab>{t('CaseView.SateliteProduction')}</Tab>
                    <Tab>{t('CaseView.PipelinesUmbilicals')}</Tab>
                    <Tab>{t('CaseView.FlexibleRiser')}</Tab>
                </List>
                <Panels>
                    <Panel>
                        <OverviewView />
                    </Panel>
                    <Panel>
                        <DrainageStrategyView />
                    </Panel>
                    <Panel>
                        <ExplorationView />
                    </Panel>
                    <Panel>{t('CaseView.SateliteProduction')}</Panel>
                    <Panel>{t('CaseView.PipelinesUmbilicals')}</Panel>
                </Panels>
            </Tabs>
        </CaseViewDiv>
    )
}

export default CaseView
