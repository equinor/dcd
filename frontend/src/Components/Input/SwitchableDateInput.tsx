import React, { ChangeEventHandler } from "react"
import { Input } from "@equinor/eds-core-react"
import InputSwitcher from "../Input/Components/InputSwitcher"
import {
    formatDate,
    dateFromString,
    isDefaultDate,
    toMonthDate,
} from "../../Utils/common"
import useDataEdits from "../../Hooks/useDataEdits"
import { useCaseContext } from "../../Context/CaseContext"
import { useProjectContext } from "../../Context/ProjectContext"
import { ResourcePropertyKey, ResourceName } from "../../Models/Interfaces"

interface SwitchableDateInputProps {
    value: string | undefined
    label: string
    resourceName: ResourceName
    resourcePropertyKey: ResourcePropertyKey
    resourceId?: string
    onChange: ChangeEventHandler<HTMLInputElement>
    min?: string
    max?: string
}
const SwitchableDateInput: React.FC<SwitchableDateInputProps> = ({
    value,
    label,
    resourceName,
    resourcePropertyKey,
    resourceId,
    onChange,
    min,
    max,
}) => {
    const { projectCase } = useCaseContext()
    const { project } = useProjectContext()

    if (!projectCase || !project) { return null }

    const { addEdit } = useDataEdits()

    const toScheduleValue = (date: string) => {
        const dateString = dateFromString(date)
        if (isDefaultDate(dateString)) {
            return undefined
        }
        return toMonthDate(dateString)
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        onChange(e)
        console.log(value)
        addEdit({
            newValue: e.target.value,
            previousValue: value,
            inputLabel: label,
            projectId: project.id,
            resourceName,
            resourcePropertyKey,
            resourceId,
            caseId: projectCase.id,
            newDisplayValue: formatDate(e.target.value),
            previousDisplayValue: value && formatDate((value) || ""),
        })
    }

    return (
        <InputSwitcher
            value={value ? formatDate(value) : ""
            label={label}
        >
            <Input
                type="month"
                id={objectKey}
                name={objectKey}
                onChange={handleChange}
                value={toScheduleValue(value || "")}
                min={min}
                max={max}
            />
        </InputSwitcher>
    )
}

export default SwitchableDateInput
