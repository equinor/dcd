import React from "react"
import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"

interface props {
    surfData: Components.Schemas.SurfDto
}

const Maturity: React.FC<props> = ({ surfData }) => {
    const maturityOptions: { [key: string]: string } = {
        0: "A",
        1: "B",
        2: "C",
        3: "D",
    }
    /*
    const addMaturityEdit = (e: any) => {
        const newValue = Number(e.newResourceObject.maturity)
        const previousResourceObject = structuredClone(surfData)
        const newResourceObject: Components.Schemas.SurfDto = structuredClone(surfData)
        newResourceObject.maturity = newValue as Components.Schemas.Maturity

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
        */

    return (
        <SwitchableDropdownInput
            resourceName="surf"
            resourcePropertyKey="maturity"
            resourceId={surfData.id}
            value={surfData.maturity}
            previousResourceObject={structuredClone(surfData)}
            options={maturityOptions}
            label="Gas solution"
        />
    )
}

export default Maturity
