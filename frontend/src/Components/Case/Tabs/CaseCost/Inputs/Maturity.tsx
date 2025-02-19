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
