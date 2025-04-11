import React from "react"

import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"
import { useSurfMutation } from "@/Hooks/Mutations"

interface props {
    surfData: Components.Schemas.SurfDto
}

const Maturity: React.FC<props> = ({ surfData }) => {
    const { updateMaturity } = useSurfMutation()

    const maturityOptions: { [key: string]: string } = {
        0: "A",
        1: "B",
        2: "C",
        3: "D",
    }

    return (
        <SwitchableDropdownInput
            value={surfData.maturity}
            options={maturityOptions}
            label="Maturity"
            id={`surf-maturity-${surfData.id}`}
            onSubmit={(newValue) => updateMaturity(newValue)}
        />
    )
}

export default Maturity
