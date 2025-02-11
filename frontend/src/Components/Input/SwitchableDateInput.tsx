import React, { ChangeEventHandler, useState } from "react"
import { InputWrapper, DatePicker } from "@equinor/eds-core-react"
import InputSwitcher from "../Input/Components/InputSwitcher"
import {
    formatDate,
    dateStringToDateUtc,
} from "@/Utils/DateUtils"
import {
    isWithinRange,
} from "../../Utils/common"
import { ResourcePropertyKey } from "../../Models/Interfaces"
import { useAppContext } from "@/Context/AppContext"

interface SwitchableDateInputProps {
    value: Date | undefined
    label: string
    resourcePropertyKey: ResourcePropertyKey
    onChange: ChangeEventHandler<HTMLInputElement>
    min?: string
    max?: string
}

const toScheduleValue = (value: Date | undefined): string | undefined => {
    if (!value) { return undefined }
    const dateString = value.toISOString()
    if (dateString === "0001-01-01T00:00:00.000Z") { return undefined }
    return dateString
}

const SwitchableDateInput: React.FC<SwitchableDateInputProps> = ({
    value,
    label,
    resourcePropertyKey,
    onChange,
    min,
    max,
}) => {
    const [hasError, setHasError] = useState(false)
    const [helperText, setHelperText] = useState("\u200B")
    const [localValue, setLocalValue] = useState<string | undefined>(toScheduleValue(value))
    const { setSnackBarMessage } = useAppContext()

    const validateInput = (newValue: number) => {
        if (!isWithinRange(newValue, 2010, 2110)) {
            setHelperText(`(min: ${2010}, max: ${2110})`)
            setHasError(true)
            return false
        }
        setHasError(false)
        setHelperText("\u200B")

        return true
    }

    const handleDateChange = (date: Date | null) => {
        if (!date) {
            setLocalValue(undefined)
            onChange({ target: { value: "" } } as React.ChangeEvent<HTMLInputElement>)
            return
        }

        if (!validateInput(date.getFullYear())) {
            setSnackBarMessage(`The input for ${label} was not saved, because the year has to be between 2010 and 2110.`)
            return
        }

        const dateString = date.toISOString()
        setLocalValue(dateString)
        onChange({ target: { value: dateString } } as React.ChangeEvent<HTMLInputElement>)
    }

    return (
        <InputWrapper
            color={hasError ? "error" : undefined}
            helperProps={{
                text: helperText,
            }}
        >
            <InputSwitcher
                value={localValue ? formatDate(localValue) : ""}
                label={label}
            >
                <DatePicker
                    id={resourcePropertyKey}
                    value={localValue ? dateStringToDateUtc(localValue) : undefined}
                    onChange={handleDateChange}
                    minValue={min ? dateStringToDateUtc(min) : undefined}
                    maxValue={max ? dateStringToDateUtc(max) : undefined}
                    formatOptions={{ year: "numeric", month: "2-digit", day: "2-digit" }}
                />
            </InputSwitcher>
        </InputWrapper>
    )
}

export default SwitchableDateInput
