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
import { ServiceKey, ServiceName } from "../../Models/Interfaces"

interface SwitchableDateInputProps {
    value: string | undefined
    label: string
    serviceName: ServiceName
    serviceKey: ServiceKey
    serviceId?: string
    onChange: ChangeEventHandler<HTMLInputElement>
    min?: string
    max?: string
}
const SwitchableDateInput: React.FC<SwitchableDateInputProps> = ({
    value,
    label,
    serviceName,
    serviceKey,
    serviceId,
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
        addEdit(
            e.target.value, // newValue
            value, // previousValue
            label, // inputLabel
            project.id, // projectId
            serviceName, // serviceName
            serviceKey, // serviceKey
            serviceId, // serviceId
            projectCase.id, // caseId
            formatDate(e.target.value), // newDisplayValue
            value && formatDate((value) || ""), // previousDisplayValue
        )
    }

    return (
        <InputSwitcher
            value={formatDate(value || "")}
            label={label}
        >
            <Input
                type="month"
                id="dgaDate"
                name="dgaDate"
                onChange={handleChange}
                value={toScheduleValue(value || "")}
                min={min}
                max={max}
            />
        </InputSwitcher>
    )
}

export default SwitchableDateInput
