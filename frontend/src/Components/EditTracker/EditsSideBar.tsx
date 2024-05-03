import React from "react"
import { useCaseContext } from "../../Context/CaseContext"

// ignore this component for now it will gain functionality in later pr
const EditsSideBar: React.FC = () => {
    const { caseEdits, setCaseEdits } = useCaseContext()

    return (
        <div></div>
    )
}

export default EditsSideBar
