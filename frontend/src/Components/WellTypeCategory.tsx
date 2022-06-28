import {
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"
// import { Case } from "../models/Case"
// import { Project } from "../models/Project"
// import { Well } from "../models/Well"
// import { GetCaseService } from "../Services/CaseService"

const WellTypeDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
`

interface Props {
    // well: Well | undefined,
    setWellCategory: Dispatch<SetStateAction<Components.Schemas.WellTypeCategory | undefined>>,
    // setProject: Dispatch<SetStateAction<Project | undefined>>,
    currentValue: Components.Schemas.WellTypeCategory | undefined,
}

const WellTypeCategory = ({
    // well,
    setWellCategory,
    // setProject,
    currentValue,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        // let al:Components.Schemas.WellTypeCategory
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setWellCategory(0)
            // al = 0
            break
        case "1":
            setWellCategory(1)
            // al = 1
            break
        case "2":
            setWellCategory(2)
            // al = 2
            break
        case "3":
            setWellCategory(3)
            // al = 3
            break
        case "4":
            setWellCategory(4)
            // al = 4
            break
        case "5":
            setWellCategory(5)
            // al = 5
            break
        case "6":
            setWellCategory(6)
            // al = 6
            break
        default:
            // al = 0
            setWellCategory(0)
            break
        }
        // if (caseItem !== undefined) {
        //     const newCase = Case.Copy(caseItem)
        //     newCase.artificialLift = al
        //     const newProject = await GetCaseService().updateCase(newCase)
        //     setProject(newProject)
        // }
    }

    return (
        <WellTypeDropdown
            label="Well type"
            id="Welltype"
            placeholder="Choose a well type"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
            disabled={false}
        >
            <option key="0" value={0}>Oil producer</option>
            <option key="1" value={1}>Gas producer</option>
            <option key="2" value={2}>Water injector</option>
            <option key="3" value={3}>Gas injector</option>
            <option key="4" value={4}>Exploration well</option>
            <option key="5" value={5}>Appraisal well</option>
            <option key="6" value={6}>Sidetrack</option>
        </WellTypeDropdown>
    )
}

export default WellTypeCategory
