import {
    NativeSelect,
} from "@equinor/eds-core-react"
import React, {
    ChangeEvent,
} from "react"
import styled from "styled-components"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetCaseService } from "../Services/CaseService"

const ArtificialLiftDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
`

interface Props {
    caseItem: Case | undefined,
    setArtificialLift: React.Dispatch<React.SetStateAction<Components.Schemas.ArtificialLift | undefined>>,
    setProject: React.Dispatch<React.SetStateAction<Project | undefined>>,
    currentValue: Components.Schemas.ArtificialLift | undefined,
}

const CaseArtificialLift = ({
    caseItem,
    setArtificialLift,
    setProject,
    currentValue,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        let al:Components.Schemas.ArtificialLift | undefined
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setArtificialLift(0)
            al = 0
            break
        case "1":
            setArtificialLift(1)
            al = 1
            break
        case "2":
            setArtificialLift(2)
            al = 2
            break
        case "3":
            setArtificialLift(3)
            al = 3
            break
        default:
            al = undefined
            setArtificialLift(undefined)
            break
        }
        if (caseItem !== undefined) {
            const newCase = Case.Copy(caseItem)
            newCase.artificialLift = al
            const newProject = await GetCaseService().updateCase(newCase)
            setProject(newProject)
        }
    }
    return (
        <ArtificialLiftDropdown
            label="Artificial Lift"
            id="ArtificialLift"
            placeholder="Choose an artificial lift"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
        >
            <option key="0" value={0}>NoArtificialLift </option>
            <option key="1" value={1}>GasLift </option>
            <option key="2" value={2}>ElectricalSubmergedPumps </option>
            <option key="3" value={3}>SubseaBoosterPumps </option>
        </ArtificialLiftDropdown>
    )
}

export default CaseArtificialLift
