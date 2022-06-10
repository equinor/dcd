import {
    NativeSelect,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction,
} from "react"
import styled from "styled-components"
import { Project } from "../models/Project"

const CurrencyDropdown = styled(NativeSelect)`
width: 20rem;
padding-bottom: 2em;
`

interface Props {
    project: Project
    setCurrency: Dispatch<SetStateAction<Components.Schemas.Currency>>,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    currentValue: Components.Schemas.Currency,
}

const Currency = ({
    project,
    setCurrency,
    setProject,
    currentValue,
}: Props) => {
    const onChange = async (event: ChangeEvent<HTMLSelectElement>) => {
        let pu:Components.Schemas.Currency
        switch (event.currentTarget.selectedOptions[0].value) {
        case "2":
            setCurrency(2)
            pu = 2
            break
        default:
            setCurrency(1)
            pu = 1
            break
        }
        if (project !== undefined) {
            const newProject = project
            newProject.currency = pu
            setProject(newProject)
        }
    }

    return (
        <CurrencyDropdown
            label="Currency"
            id="currency"
            placeholder="Choose a currency"
            onChange={(event: ChangeEvent<HTMLSelectElement>) => onChange(event)}
            value={currentValue}
            disabled={false}
        >
            <option key="0" value={0}>USD</option>
            <option key="1" value={1}>NOK</option>
        </CurrencyDropdown>
    )
}

export default Currency
