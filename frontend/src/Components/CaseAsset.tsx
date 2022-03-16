import React from "react"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import LinkAsset from "./LinkAsset"
import { GetCaseService } from "../Services/CaseService"

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
                DG1Date: caseItem?.DG1Date ?? new Date(),
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

    return (
        <>
            <LinkAsset
                assetName="Drainage strategy"
                linkAsset={onSelectAsset}
                values={project.drainageStrategies.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                currentValue={caseItem?.links?.drainageStrategyLink}
                link="drainageStrategyLink"
            />
            <LinkAsset
                assetName="Exploration"
                linkAsset={onSelectAsset}
                values={project.explorations.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                currentValue={caseItem?.links?.explorationLink}
                link="explorationLink"
            />
            <LinkAsset
                assetName="Well project"
                linkAsset={onSelectAsset}
                values={project.wellProjects.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                currentValue={caseItem?.links?.wellProjectLink}
                link="wellProjectLink"
            />
            <LinkAsset
                assetName="SURF"
                linkAsset={onSelectAsset}
                values={project.surfs.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                currentValue={caseItem?.links?.surfLink}
                link="surfLink"
            />
            <LinkAsset
                assetName="Topside"
                linkAsset={onSelectAsset}
                values={project.topsides.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                currentValue={caseItem?.links?.topsideLink}
                link="topsideLink"
            />
            <LinkAsset
                assetName="Substructure"
                linkAsset={onSelectAsset}
                values={project.substructures.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                currentValue={caseItem?.links?.substructureLink}
                link="substructureLink"
            />
            <LinkAsset
                assetName="Transport"
                linkAsset={onSelectAsset}
                values={project.transports.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                currentValue={caseItem?.links?.transportLink}
                link="transportLink"
            />
        </>
    )
}

export default CaseAsset
