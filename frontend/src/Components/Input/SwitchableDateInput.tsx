import React, { ChangeEventHandler, useState } from "react"
import { Input, InputWrapper } from "@equinor/eds-core-react"
import InputSwitcher from "../Input/Components/InputSwitcher"
import {
    formatDate,
    dateFromString,
    isDefaultDate,
    toMonthDate,
    isWithinRange,
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
    const [hasError, setHasError] = useState(false)
    const [helperText, setHelperText] = useState("\u200B")
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

    const dateValueYear = (e: React.FocusEvent<HTMLInputElement>) => e.target.value.substring(0, 4)

    const handleBlur = (e: React.FocusEvent<HTMLInputElement>) => {
        if ((Number(dateValueYear(e)) <= 2010 && Number(dateValueYear(e)) >= 2110) || dateValueYear(e) !== "") {
            setSnackBarMessage(`The input for ${label} was not saved, because the year has to be between 2010 and 2110.`)
        }
    }

    const toScheduleValue = (date: string) => {
        const dateString = dateFromString(date)
        if (isDefaultDate(dateString)) {
            return undefined
        }
        return toMonthDate(dateString)
    }

    const handleDateChange = (e: React.FocusEvent<HTMLInputElement>) => {
        if ((Number(dateValueYear) <= 2010 && Number(dateValueYear) >= 2110) || dateValueYear(e) !== "") {
            validateInput(Number(dateValueYear))
        }
        onChange(e)
    }

    return (
        <InputWrapper
            color={hasError ? "error" : undefined}
            helperProps={{
                text: helperText,
            }}
        >
            <InputSwitcher
                value={value ? formatDate(value) : ""}
                label={label}
            >
                <Input
                    onBlur={handleBlur}
                    type="month"
                    id={resourcePropertyKey}
                    name={resourcePropertyKey}
                    onChange={(e: any) => handleDateChange(e)}
                    defaultValue={toScheduleValue(value || "")}
                    min={min}
                    max={max}
                />
            </InputSwitcher>
        </InputWrapper>
    )
}

export default SwitchableDateInput
