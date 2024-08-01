import React from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import { useParams } from "react-router"
import InputSwitcher from "../../../../Input/Components/InputSwitcher"
import useDataEdits from "../../../../../Hooks/useDataEdits"

interface props {
    surfData: Components.Schemas.SurfWithProfilesDto
    projectId: string
}

const Maturity: React.FC<props> = ({ surfData, projectId }) => {
    const { addEdit } = useDataEdits()
    const { caseId } = useParams()

    const maturityOptions: { [key: string]: string } = {
        0: "A",
        1: "B",
        2: "C",
        3: "D",
    }

    return (
        <InputSwitcher value={maturityOptions[surfData.maturity]} label="Maturity">
            <NativeSelect
                id="maturity"
                label=""
                value={surfData.maturity}
                onChange={(e) => {
                    addEdit({
                        newValue: Number(e.currentTarget.value),
                        previousValue: surfData.maturity,
                        inputLabel: "Maturity",
                        projectId,
                        resourceName: "surf",
                        resourcePropertyKey: "maturity",
                        resourceId: surfData.id,
                        caseId,
                    })
                }}
            >
                {Object.keys(maturityOptions).map((key) => (
                    <option key={key} value={key}>{maturityOptions[key]}</option>
                ))}
            </NativeSelect>
        </InputSwitcher>
    )
}

export default Maturity
