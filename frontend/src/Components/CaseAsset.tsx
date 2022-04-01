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

    enum AssetLink {
        drainageStrategyLink = "drainageStrategyLink",
        explorationLink = "explorationLink",
        substructureLink = "substructureLink",
        surfLink = "surfLink",
        topsideLink = "topsideLink",
        transportLink = "transportLink",
        wellProjectLink = "wellProjectLink",
      }

    const onSelectAsset = async (event: React.ChangeEvent<HTMLSelectElement>, link: AssetLink) => {
        try {
            const caseDto = Case.Copy(caseItem!)

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
            navigate(`${type?.toLowerCase()}/${id}`)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const submitCreateAsset = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>, type: string) => {
        event?.preventDefault()
        navigate(`${type.toLowerCase()}/${emptyGuid}`)
    }

    return (
        <>
            <Wrapper>
                <LinkAsset
                    assetName="Drainage strategy"
                    linkAsset={onSelectAsset}
                    values={project.drainageStrategies.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.drainageStrategyLink}
                    link="drainageStrategyLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "drainagestrategy", caseItem?.drainageStrategyLink)}
                    disabled={caseItem?.drainageStrategyLink === emptyGuid}
                >
                    Open
                </AssetButton>
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitCreateAsset(event, "DrainageStrategy")}
                >
                    Create new
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Exploration"
                    linkAsset={onSelectAsset}
                    values={project.explorations.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.explorationLink}
                    link="explorationLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "exploration", caseItem?.explorationLink)}
                    disabled={caseItem?.explorationLink === emptyGuid}
                >
                    Open
                </AssetButton>
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitCreateAsset(event, "Exploration")}
                >
                    Create new
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Well project"
                    linkAsset={onSelectAsset}
                    values={project.wellProjects.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.wellProjectLink}
                    link="wellProjectLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "wellproject", caseItem?.wellProjectLink)}
                    disabled={caseItem?.wellProjectLink === emptyGuid}
                >
                    Open
                </AssetButton>
                <AssetButton
                    type="submit"
                    onClick={
                        (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => submitCreateAsset(e, "wellproject")
                    }
                >
                    Create new
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="SURF"
                    linkAsset={onSelectAsset}
                    values={project.surfs.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.surfLink}
                    link="surfLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "surf", caseItem?.surfLink)}
                    disabled={caseItem?.surfLink === emptyGuid}
                >
                    Open
                </AssetButton>
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitCreateAsset(event, "SURF")}
                >
                    Create new
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Topside"
                    linkAsset={onSelectAsset}
                    values={project.topsides.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.topsideLink}
                    link="topsideLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "topside", caseItem?.topsideLink)}
                    disabled={caseItem?.topsideLink === emptyGuid}
                >
                    Open
                </AssetButton>
                <AssetButton
                    type="submit"
                    onClick={
                        (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => submitCreateAsset(e, "topside")
                    }
                >
                    Create new
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Substructure"
                    linkAsset={onSelectAsset}
                    values={project.substructures.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.substructureLink}
                    link="substructureLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "substructure", caseItem?.substructureLink)}
                    disabled={caseItem?.substructureLink === emptyGuid}
                >
                    Open
                </AssetButton>
                <AssetButton
                    type="submit"
                    onClick={
                        (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => submitCreateAsset(e, "substructure")
                    }
                >
                    Create new
                </AssetButton>
            </Wrapper>
            <Wrapper>
                <LinkAsset
                    assetName="Transport"
                    linkAsset={onSelectAsset}
                    values={project.transports.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                    currentValue={caseItem?.transportLink}
                    link="transportLink"
                />
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitOpenAsset(event, "transport", caseItem?.transportLink)}
                    disabled={caseItem?.transportLink === emptyGuid}
                >
                    Open
                </AssetButton>
                <AssetButton
                    type="submit"
                    onClick={(
                        event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
                    ) => submitCreateAsset(event, "Transport")}
                >
                    Create new
                </AssetButton>
            </Wrapper>
        </>
    )
}

export default CaseAsset
