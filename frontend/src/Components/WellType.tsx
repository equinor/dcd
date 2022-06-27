/* eslint-disable max-len */
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"
// import { WellProjectCostProfile } from "../models/assets/wellproject/WellProjectCostProfile"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { Well } from "../models/Well"
import { GetCaseService } from "../Services/CaseService"
import { GetWellService } from "../Services/WellService"
import { unwrapCase } from "../Utils/common"
// import { EMPTY_GUID } from "../Utils/constants"
// import { Wrapper } from "../Views/Asset/StyledAssetComponents"
import LinkWellType from "./LinkWellType"

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
    margin-bottom: 2rem;
`

// const WellTypeDropdown = styled(NativeSelect)`
// width: 20rem;
// margin-top: -0.5rem;
// margin-left: 1rem;
// `

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case | undefined,
    setCase: Dispatch<SetStateAction<Case | undefined>>
    caseId: string | undefined,
}

const WellType = ({
    setProject,
    caseItem,
    setCase,
    caseId,
}: Props) => {
    const [wells, setWells] = useState<Well[]>()
    const wellTypeCollection = async () => {
        // if (caseItem?.wells !== (null || undefined)) {
        //     const wellsCollection = caseItem?.wells
        //     const wellTypes = wellsCollection?.filter((obj) => obj.wellType === true)
        // }
        if (caseItem?.wellsLink !== (null || undefined)) {
            const allWells = await (await GetWellService().getWellsByProjectId(caseItem.projectId!)).filter((obj) => obj.id === caseItem.wellsLink)
            setWells(allWells)
        }
    }

    useEffect(() => {
        wellTypeCollection()
    })

    // enum WellTypeLink {
    //     wellTypeLink = "wellTypeLink",
    //     explorationWellTypeLink = "explorationWellTypeLink"
    //   }

    const onSelectWellType = async (event: React.ChangeEvent<HTMLSelectElement>, link: "wellsLink") => {
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
        <WrapperColumn>
            <LinkWellType
                caseItem={caseItem}
                linkWellType={onSelectWellType}
                link="wellsLink"
                currentValue={caseItem?.wellsLink}
                values={wells?.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
            />
            {/* <Typography>
                Description:
                {currentValue?.description}
            </Typography>
            <Typography>
                Category:
                {currentValue?.category}
            </Typography>
            <Typography>
                Drilling days:
                {currentValue?.drillingDays}
            </Typography>
            <Typography>
                Well cost:
                {currentValue?.wellCost}
            </Typography> */}
        </WrapperColumn>
    )
}

export default WellType
