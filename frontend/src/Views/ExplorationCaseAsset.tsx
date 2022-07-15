import React, {
    Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import LinkAsset from "../Components/LinkAsset"
import { GetCaseService } from "../Services/CaseService"
import { unwrapCase } from "../Utils/common"

const Wrapper = styled.div`
    margin-top: 1rem;
    margin-bottom: -1rem;
    display: flex;
    flex-direction: row;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>
    caseItem: Case | undefined,
    setCase: Dispatch<SetStateAction<Case | undefined>>
    caseId: string | undefined,
}

const ExplorationCaseAsset = ({
    project,
    setProject,
    caseItem,
    setCase,
    caseId,
}: Props) => {
    enum AssetLink {
        explorationLink = "explorationLink",
      }

    const onSelectAsset = async (event: React.ChangeEvent<HTMLSelectElement>, link: AssetLink) => {
        try {
            const unwrappedCase: Case = unwrapCase(caseItem)
            const caseDto = Case.Copy(unwrappedCase)

            caseDto[link] = event.currentTarget.selectedOptions[0].value

            const newProject: Project = await GetCaseService().updateCase(caseDto)
            setProject(newProject)
            const caseResult: Case = unwrapCase(newProject.cases.find((o) => o.id === caseId))
            setCase(caseResult)
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    return (
        <Wrapper>
            <LinkAsset
                assetName="Exploration"
                linkAsset={onSelectAsset}
                values={project.explorations.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                currentValue={caseItem?.explorationLink}
                link={AssetLink.explorationLink}
            />
        </Wrapper>
    )
}

export default ExplorationCaseAsset
