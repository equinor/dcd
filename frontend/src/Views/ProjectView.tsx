import {
    Tabs, Typography,
} from "@equinor/eds-core-react"
import React, {
    useEffect,
    useMemo,
    useState,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"

import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"

import { unwrapProjectId } from "../Utils/common"

import { Case } from "../models/Case"
import OverviewView from "./OverviewView"
import CompareCasesView from "./CompareCasesView"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const StyledTabPanel = styled(Panel)`
    padding-top: 0px;
    border-top: 1px solid LightGray;
`

const Wrapper = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const ProjectView = () => {
    const [activeTab, setActiveTab] = React.useState(0)
    const params = useParams()
    const [project, setProject] = useState<Project>()
    const [physicalUnit, setPhysicalUnit] = useState<Components.Schemas.PhysUnit>(0)
    const [currency, setCurrency] = useState<Components.Schemas.Currency>(1)

    const [capexYearXLabels, setCapexYearXLabels] = useState<number[]>([])
    const [capexYearYDatas, setCapexYearYDatas] = useState<number[][]>([[]])
    const [capexYearCaseTitles, setCapexYearCaseTitles] = useState<string[]>([])

    useEffect(() => {
        (async () => {
            try {
                const projectId: string = unwrapProjectId(params.projectId)
                const res: Project = await GetProjectService().getProjectByID(projectId)
                if (res !== undefined) {
                    setPhysicalUnit(res?.physUnit)
                    setCurrency(res?.currency)
                }
                console.log("[ProjectView]", res)
                setProject(res)
            } catch (error) {
                console.error(`[ProjectView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId])

    useEffect(() => {
        (async () => {
            try {
                if (project !== undefined) {
                    const projectDto = Project.Copy(project)
                    projectDto.physUnit = physicalUnit
                    projectDto.currency = currency
                    projectDto.projectId = params.projectId!
                    const cases: Case[] = []
                    project.cases.forEach((c) => cases.push(Case.Copy(c)))
                    projectDto.cases = cases
                    const res: Project = await GetProjectService().updateProject(projectDto)
                    setProject(res)
                }
            } catch (error) {
                console.error(`[ProjectView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [physicalUnit, currency])

    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const generateChartDataForCapexYear = useMemo(() => {
        const years: number[] = []
        const values: number[][] = []
        const caseTitles: string[] = []
        project?.cases.forEach((casee) => {
            if (casee.capexYear?.startYear !== null) {
                years.push(casee.capexYear?.startYear!)
            }
            if (casee.capexYear?.values?.length !== 0) {
                values.push(casee.capexYear?.values!)
                caseTitles?.push(casee.name!)
            }
        })

        setCapexYearXLabels(years)
        setCapexYearYDatas(values)
        setCapexYearCaseTitles(caseTitles)
    }, [project])

    if (!project) return null

    return (
        <div>
            <Wrapper>
                <Typography variant="h4">{project.name}</Typography>
            </Wrapper>
            <Wrapper>
                <Tabs activeTab={activeTab} onChange={setActiveTab}>
                    <List>
                        <Tab>Overview </Tab>
                        <Tab>Compare cases</Tab>
                        <Tab>Workflow</Tab>
                        <Tab>Settings</Tab>
                    </List>
                    <Panels>
                        <StyledTabPanel>
                            <OverviewView
                                project={project}
                                setProject={setProject}
                                physicalUnit={physicalUnit}
                                setPhysicalUnit={setPhysicalUnit}
                                currency={currency}
                                setCurrency={setCurrency}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>

                            <CompareCasesView
                                capexYearX={capexYearXLabels}
                                capexYearY={capexYearYDatas}
                                caseTitles={capexYearCaseTitles}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <p>Workflow</p>
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <p>Setting</p>
                        </StyledTabPanel>
                    </Panels>
                </Tabs>
            </Wrapper>
        </div>
    )
}

export default ProjectView
