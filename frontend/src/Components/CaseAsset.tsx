import React from "react"
import { Button } from "@equinor/eds-core-react"
import styled from "styled-components"
import { useNavigate } from "react-router"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import LinkAsset from "./LinkAsset"
import { GetCaseService } from "../Services/CaseService"

const Wrapper = styled.div`
    display: flex;
    flex-direction: row;
`

const AssetButton = styled(Button)`
    margin-top: 1rem;
    margin-left: 2rem;
    &:disabled {
        margin-top: 1rem;
        margin-left: 2rem;
    }
`

interface Props {
    project: Project,
    setProject: React.Dispatch<React.SetStateAction<Project | undefined>>
    caseItem: Case | undefined,
    setCase: React.Dispatch<React.SetStateAction<Case | undefined>>
    caseId: string | undefined,
}

const CaseAsset = ({
    project,
    setProject,
    caseItem,
    setCase,
    caseId,
}: Props) => {
    const navigate = useNavigate()

    const emptyGuid = "00000000-0000-0000-0000-000000000000"
    interface CaseDto {
        capex?: number
        createdAt?: Date | null
        description?: string
        DG1Date?: Date | null
        DG2Date?: Date | null
        DG3Date?: Date | null
        DG4Date?: Date | null
        id?: string
        projectId: string
        updatedAt?: Date | null
        name?: string
        isRef: boolean
        DrainageStrategyLink?: string
        ExplorationLink?: string
        WellProjectLink?: string
        SurfLink?: string
        TopsideLink?: string
        SubstructureLink?: string
        TransportLink?: string
        [key: string]: any
    }

    const onSelectAsset = async (event: React.ChangeEvent<HTMLSelectElement>, link: string) => {
        try {
            const caseDto: CaseDto = {
                capex: caseItem?.capex,
                createdAt: caseItem?.createdAt,
                id: caseItem?.id ?? "",
                projectId: project.id,
                name: caseItem?.name ?? "",
                description: caseItem?.description ?? "",
                DG1Date: caseItem?.DG1Date,
                DG2Date: caseItem?.DG2Date,
                DG3Date: caseItem?.DG3Date,
                DG4Date: caseItem?.DG4Date,
                updatedAt: caseItem?.updatedAt,
                isRef: caseItem?.isRef ?? false,
                DrainageStrategyLink: caseItem?.links?.drainageStrategyLink,
                ExplorationLink: caseItem?.links?.explorationLink,
                WellProjectLink: caseItem?.links?.wellProjectLink,
                SurfLink: caseItem?.links?.surfLink,
                TopsideLink: caseItem?.links?.topsideLink,
                SubstructureLink: caseItem?.links?.substructureLink,
                TransportLink: caseItem?.links?.transportLink,
            }
            caseDto[link] = event.currentTarget.selectedOptions[0].value

            const newProject = await GetCaseService().updateCase(caseDto)
            setProject(newProject)
            const caseResult = newProject.cases.find((o) => o.id === caseId)
            setCase(caseResult)
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    const submitOpenAsset = async (
        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
        type: string,
        id?: string,
    ) => {
        event.preventDefault()

        try {
            navigate(`${type}/${id}`)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    return (
        <>
            <Wrapper>
                <LinkAsset
                    assetName="Drainage strategy"
                    linkAsset={onSelectAsset}
                    values={project.drainageStrategies.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.links?.drainageStrategyLink}
                    link="drainageStrategyLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "drainagestrategy", caseItem?.links?.drainageStrategyLink)}
                    disabled={caseItem?.links?.drainageStrategyLink === emptyGuid}
                >
                    Open
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Exploration"
                    linkAsset={onSelectAsset}
                    values={project.explorations.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.links?.explorationLink}
                    link="explorationLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "exploration", caseItem?.links?.explorationLink)}
                    disabled={caseItem?.links?.explorationLink === emptyGuid}
                >
                    Open
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Well project"
                    linkAsset={onSelectAsset}
                    values={project.wellProjects.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.links?.wellProjectLink}
                    link="wellProjectLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "wellproject", caseItem?.links?.wellProjectLink)}
                    disabled={caseItem?.links?.wellProjectLink === emptyGuid}
                >
                    Open
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="SURF"
                    linkAsset={onSelectAsset}
                    values={project.surfs.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.links?.surfLink}
                    link="surfLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "surf", caseItem?.links?.surfLink)}
                    disabled={caseItem?.links?.surfLink === emptyGuid}
                >
                    Open
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Topside"
                    linkAsset={onSelectAsset}
                    values={project.topsides.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.links?.topsideLink}
                    link="topsideLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "topside", caseItem?.links?.topsideLink)}
                    disabled={caseItem?.links?.topsideLink === emptyGuid}
                >
                    Open
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Substructure"
                    linkAsset={onSelectAsset}
                    values={project.substructures.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.links?.substructureLink}
                    link="substructureLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "substructure", caseItem?.links?.substructureLink)}
                    disabled={caseItem?.links?.substructureLink === emptyGuid}
                >
                    Open
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Transport"
                    linkAsset={onSelectAsset}
                    values={project.transports.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.links?.transportLink}
                    link="transportLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "transport", caseItem?.links?.transportLink)}
                    disabled={caseItem?.links?.transportLink === emptyGuid}
                >
                    Open
                </AssetButton>
            </Wrapper>
        </>
    )
}

export default CaseAsset
