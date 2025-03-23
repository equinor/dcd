import { InputWrapper, DatePicker } from "@equinor/eds-core-react"
import React, {
    ChangeEventHandler,
    useState,
    useEffect,
    memo,
    useCallback,
    useMemo,
} from "react"

import InputSwitcher from "../Input/Components/InputSwitcher"

import { useAppStore } from "@/Store/AppStore"
import {
    formatDate,
    dateStringToDateUtc,
    dateToUtcDateStringWithZeroTimePart,
} from "@/Utils/DateUtils"

interface SwitchableDateInputProps {
    value: Date | undefined
    label: string
    onChange: ChangeEventHandler<HTMLInputElement>
    required?: boolean
    min?: string
    max?: string
}

const toScheduleValue = (value: Date | undefined): string | undefined => {
    if (!value) { return undefined }
    const dateString = value.toISOString()

    if (dateString === "0001-01-01T00:00:00.000Z") { return undefined }

    return dateString
}

const SwitchableDateInput: React.FC<SwitchableDateInputProps> = memo(({
    value,
    label,
    onChange,
    required = false,
    min,
    max,
}) => {
    const [hasError, setHasError] = useState(false)
    const [helperText, setHelperText] = useState("\u200B")
    const [localValue, setLocalValue] = useState<string | undefined>(() => toScheduleValue(value))
    const { setSnackBarMessage } = useAppStore()

    const formattedValue = useMemo(
        () => (localValue ? formatDate(localValue) : ""),
        [localValue],
    )

    const datePickerValue = useMemo(
        () => (localValue ? dateStringToDateUtc(localValue) : undefined),
        [localValue],
    )

    const minValue = useMemo(
        () => (min ? dateStringToDateUtc(min) : undefined),
        [min],
    )

    const maxValue = useMemo(
        () => (max ? dateStringToDateUtc(max) : undefined),
        [max],
    )

    useEffect(() => {
        const newValue = toScheduleValue(value)

        if (newValue !== localValue) {
            setLocalValue(newValue)
        }
    }, [value, localValue])

    const validateInput = useCallback((newValue: number): boolean => {
        const isWithinRange = (num: number, upper: number, lower: number): boolean => num >= lower && num <= upper

        if (!isWithinRange(newValue, 2010, 2110)) {
            setHelperText(`(min: ${2010}, max: ${2110})`)
            setHasError(true)

            return false
        }
        setHasError(false)
        setHelperText("\u200B")

        return true
    }, [])

    const handleDateChange = useCallback((date: Date | null) => {
        if (!date) {
            setLocalValue(undefined)
            onChange({ target: { value: "" } } as React.ChangeEvent<HTMLInputElement>)

            return
        }

        if (!validateInput(date.getFullYear())) {
            setSnackBarMessage(`The input for ${label} was not saved, because the year has to be between 2010 and 2110.`)

            return
        }

        const dateString = dateToUtcDateStringWithZeroTimePart(date)

        setLocalValue(dateString)
        onChange({ target: { value: dateString } } as React.ChangeEvent<HTMLInputElement>)
    }, [onChange, validateInput, label, setSnackBarMessage])

    return (
        <InputWrapper
            color={hasError ? "error" : undefined}
            helperProps={{
                text: helperText,
            }}
        >
            <InputSwitcher
                value={formattedValue}
                label={label}
            >
                <DatePicker
                    value={datePickerValue}
                    onChange={handleDateChange}
                    minValue={minValue}
                    maxValue={maxValue}
                    formatOptions={{ day: "2-digit", month: "long", year: "numeric" }}
                    locale="nb-NO"
                    hideClearButton={required}
                />
            </InputSwitcher>
        </InputWrapper>
    )
})

SwitchableDateInput.displayName = "SwitchableDateInput"

export default SwitchableDateInput
