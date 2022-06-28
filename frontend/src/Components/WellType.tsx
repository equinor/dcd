/* eslint-disable max-len */
// import {
//     useEffect,
// } from "react"
import styled from "styled-components"
import { WellProject } from "../models/assets/wellproject/WellProject"
// import { WellProjectCostProfile } from "../models/assets/wellproject/WellProjectCostProfile"
import { Case } from "../models/Case"
// import { Well } from "../models/Well"
// import { GetWellService } from "../Services/WellService"
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
    caseItem: Case | undefined,
    wellProject: WellProject | undefined,
}

const WellType = ({
    caseItem,
    wellProject,
}: Props) => {
    // const [wellTypes, setWellTypes] = useState<Well[]>()
    // const wellTypeCollection = async () => {
    //     // if (caseItem?.wells !== (null || undefined)) {
    //     //     const wellsCollection = caseItem?.wells
    //     //     const wellTypes = wellsCollection?.filter((obj) => obj.wellType === true)
    //     // }
    //     if (caseItem?.wellsLink !== (null || undefined)) {
    //         // const wellTypes = allWells.find((o) => o.wellType)
    //         // console.log(wellTypes)
    //         // console.log(allWells)
    //         // if (allWells.find((o) => o.wellType) === (null || undefined)) {
    //         //     allWells[0].wellType = {
    //         //         name: "Well Type 1",
    //         //         description: "Description for well type 1",
    //         //         category: 0,
    //         //         wellCost: 5,
    //         //         drillingDays: 8,
    //         //     }
    //         // }
    //     }
    // }

    // useEffect(() => {
    //     wellTypeCollection()
    // }, [])

    // enum WellTypeLink {
    //     wellTypeLink = "wellTypeLink",
    //     explorationWellTypeLink = "explorationWellTypeLink"
    //   }

    const onSelectWellType = async (event: React.ChangeEvent<HTMLSelectElement>, link: "wellsLink") => {
        try {
            const unwrappedCase: Case = unwrapCase(caseItem)
            const caseDto = Case.Copy(unwrappedCase)

            caseDto[link] = event.currentTarget.selectedOptions[0].value

            // const newProject: Project = await GetCaseService().updateCase(caseDto)
            // setProject(newProject)
            // const caseResult: Case = unwrapCase(newProject.cases.find((o) => o.id === caseId))
            // setCase(caseResult)
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
                values={wellProject?.wellTypes?.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                // values={caseItem?.wells?.filter((o) => o.wellType).map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
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
