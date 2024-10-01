import React, { useState, useEffect } from "react"
import { useParams } from "react-router-dom"
import { Input } from "@equinor/eds-core-react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { ResourceName, ResourcePropertyKey, ResourceObject } from "../../Models/Interfaces"
import InputSwitcher from "./Components/InputSwitcher"
import { useProjectContext } from "../../Context/ProjectContext"

interface CaseEditInputProps {
    label: string;
    value: string | undefined;
    resourceName: ResourceName
    resourcePropertyKey: ResourcePropertyKey
    resourceId?: string;
    previousResourceObject: ResourceObject;
    addEdit: any;
}

const SwitchableStringInput: React.FC<CaseEditInputProps> = ({
    label,
    value,
    resourceName,
    resourcePropertyKey,
    resourceId,
    previousResourceObject,
    addEdit,
}: CaseEditInputProps) => {
    const { caseId, tab } = useParams()

    const [inputValue, setInputValue] = useState(value || "")

    useEffect(() => {
        setInputValue(value || "")
    }, [value])

    const addToEditsAndSubmit = (insertedValue: string) => {
        const { projectId } = useProjectContext()

        if (!caseId || projectId === "") { return }

        const newResourceObject: ResourceObject = structuredClone(previousResourceObject)
        newResourceObject[resourcePropertyKey as keyof ResourceObject] = insertedValue as never

        addEdit({
            newResourceObject,
            previousResourceObject,
            newDisplayValue: insertedValue,
            previousDisplayValue: value,
            inputLabel: label,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId,
            tabName: tab,
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
