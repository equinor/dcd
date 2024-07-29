import React from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import { useParams } from "react-router"
import InputSwitcher from "../../../../Input/Components/InputSwitcher"

interface props {
    surfData: Components.Schemas.SurfWithProfilesDto
    projectId: string
    addEdit: any
}

const Maturity: React.FC<props> = ({ surfData, projectId, addEdit }) => {
    const { caseId } = useParams()

    const maturityOptions: { [key: string]: string } = {
        0: "A",
        1: "B",
        2: "C",
        3: "D",
    }

    const addMaturityEdit = (e: any) => {
        const newValue = e.currentTarget.value

        const previousResourceObject = structuredClone(surfData)
        const newResourceObject = structuredClone(surfData)
        newResourceObject.maturity = newValue

        addEdit({
            newDisplayValue: newValue,
            previousDisplayValue: previousResourceObject.maturity,
            newResourceObject,
            previousResourceObject,
            inputLabel: "Maturity",
            projectId,
            resourceName: "surf",
            resourcePropertyKey: "maturity",
            resourceId: surfData.id,
            caseId,
        })
    }

    return (
        <InputSwitcher value={maturityOptions[surfData.maturity]} label="Maturity">
            <NativeSelect
                id="maturity"
                label=""
                value={surfData.maturity}
                onChange={(e) => addMaturityEdit(e)}
            >
                {Object.keys(maturityOptions).map((key) => (
                    <option key={key} value={key}>{maturityOptions[key]}</option>
                ))}
            </NativeSelect>
        </InputSwitcher>
    )
}

export default Maturity
