import {
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"
import { Project } from "../models/Project"

const PhysicalUnitDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
`

interface Props {
    project: Project
    setPhysicalUnit: Dispatch<SetStateAction<Components.Schemas.PhysUnit>>,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    currentValue: Components.Schemas.PhysUnit,
}

const PhysicalUnit = ({
    project,
    setPhysicalUnit,
    setProject,
    currentValue,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        let pu:Components.Schemas.PhysUnit
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setPhysicalUnit(0)
            pu = 0
            break
        case "1":
            setPhysicalUnit(1)
            pu = 1
            break
        default:
            pu = 0
            setPhysicalUnit(0)
            break
        }
        if (project !== undefined) {
            // const newCase = Case.Copy(caseItem)
            // newCase.productionStrategyOverview = pu
            const newProject = project
            console.log(pu)
            newProject.physUnit = pu
            setProject(newProject)
        }
    }

    return (
        <PhysicalUnitDropdown
            label="Physical Unit"
            id="physicalUnit"
            placeholder="Choose a physical unit"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
            disabled={false}
        >
            <option key="0" value={0}>SI</option>
            <option key="1" value={1}>Oil field</option>
        </PhysicalUnitDropdown>
    )
}

export default PhysicalUnit
