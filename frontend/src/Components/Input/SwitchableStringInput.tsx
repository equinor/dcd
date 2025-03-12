import React, { useState, useEffect } from "react"
import { useParams } from "react-router-dom"
import { Input } from "@equinor/eds-core-react"
import { ResourceName, ResourcePropertyKey, ResourceObject } from "../../Models/Interfaces"
import InputSwitcher from "./Components/InputSwitcher"
import { useProjectContext } from "../../Store/ProjectContext"
import useEditCase from "@/Hooks/useEditCase"

interface CaseEditInputProps {
    label: string;
    value: string | undefined;
    resourceName: ResourceName
    resourcePropertyKey: ResourcePropertyKey
    resourceId?: string;
    previousResourceObject: ResourceObject;
}

const SwitchableStringInput: React.FC<CaseEditInputProps> = ({
    label,
    value,
    resourceName,
    resourcePropertyKey,
    resourceId,
    previousResourceObject,
}: CaseEditInputProps) => {
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { addEdit } = useEditCase()

    const [inputValue, setInputValue] = useState(value || "")

    useEffect(() => {
        setInputValue(value || "")
    }, [value])

    const addToEditsAndSubmit = (insertedValue: string) => {
        if (!caseId || projectId === "") { return }

        const resourceObject: ResourceObject = structuredClone(previousResourceObject)
        resourceObject[resourcePropertyKey as keyof ResourceObject] = insertedValue as never

        addEdit({
            resourceObject,
            projectId,
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
