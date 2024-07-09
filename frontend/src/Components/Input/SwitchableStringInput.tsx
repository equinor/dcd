import React, { useState, useEffect } from "react"
import { useParams } from "react-router-dom"
import { Input } from "@equinor/eds-core-react"
import useDataEdits from "../../Hooks/useDataEdits"
import { useProjectContext } from "../../Context/ProjectContext"
import { ResourceName, ResourcePropertyKey } from "../../Models/Interfaces"
import InputSwitcher from "./Components/InputSwitcher"

interface CaseEditInputProps {
    label: string;
    value: string | undefined;
    resourceName: ResourceName
    resourcePropertyKey: ResourcePropertyKey
    resourceId?: string;
}

const SwitchableStringInput: React.FC<CaseEditInputProps> = ({
    label,
    value,
    resourceName,
    resourcePropertyKey,
    resourceId,
}: CaseEditInputProps) => {
    const { addEdit } = useDataEdits()
    const { project } = useProjectContext()
    const { caseId } = useParams()

    const [inputValue, setInputValue] = useState(value || "")

    useEffect(() => {
        setInputValue(value || "")
    }, [value])

    const addToEditsAndSubmit = (insertedValue: string) => {
        if (!caseId || !project) { return }

        addEdit({
            newValue: insertedValue,
            previousValue: value,
            inputLabel: label,
            projectId: project.id,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId,
        })
    }

    return (
        <InputSwitcher
            label={label}
            value={`${inputValue}`}
        >
            <Input
                label={label}
                value={inputValue}
                onChange={(e: any) => setInputValue(e.target.value)}
                onBlur={(e: any) => addToEditsAndSubmit(e.target.value)}
            />
        </InputSwitcher>
    )
}

export default SwitchableStringInput
