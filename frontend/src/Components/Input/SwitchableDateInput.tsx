import React, { ChangeEventHandler } from "react"
import { Input } from "@equinor/eds-core-react"
import InputSwitcher from "../Input/Components/InputSwitcher"
import {
    formatDate,
    dateFromString,
    isDefaultDate,
    toMonthDate,
} from "../../Utils/common"
import { ResourcePropertyKey } from "../../Models/Interfaces"
import { useAppContext } from "@/Context/AppContext"

interface SwitchableDateInputProps {
    value: string | undefined
    label: string
    resourcePropertyKey: ResourcePropertyKey
    onChange: ChangeEventHandler<HTMLInputElement>
    min?: string
    max?: string
}
const SwitchableDateInput: React.FC<SwitchableDateInputProps> = ({
    value,
    label,
    resourcePropertyKey,
    onChange,
    min,
    max,
}) => {
    const { setSnackBarMessage } = useAppContext()

    const handleBlur = (event: React.FocusEvent<HTMLInputElement>) => {
        const dateValueYear = event.target.value.substring(0, 4)
        if (Number(dateValueYear) < 1500 && dateValueYear !== "") {
            setSnackBarMessage(`The input for ${label} was not saved, because the year has to be at least 1500.`)
        }
    }

    const toScheduleValue = (date: string) => {
        const dateString = dateFromString(date)
        if (isDefaultDate(dateString)) {
            return undefined
        }
        return toMonthDate(dateString)
    }

    return (
        <InputSwitcher
            value={value ? formatDate(value) : ""}
            label={label}
        >
            <Input
                onBlur={handleBlur}
                type="month"
                id={resourcePropertyKey}
                name={resourcePropertyKey}
                onChange={(e: any) => onChange(e)}
                defaultValue={toScheduleValue(value || "")}
                min={min}
                max={max}
            />
        </InputSwitcher>
    )
}

export default SwitchableDateInput
