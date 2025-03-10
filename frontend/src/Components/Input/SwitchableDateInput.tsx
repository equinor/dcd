import React, {
    ChangeEventHandler,
    useState,
    useEffect,
    memo,
    useCallback,
    useMemo,
} from "react"
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
import { useAppStore } from "@/Store/AppStore"

interface SwitchableDateInputProps {
    value: Date | undefined
    label: string
    resourcePropertyKey: ResourcePropertyKey
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
    resourcePropertyKey,
    onChange,
    required = false,
    min,
    max,
}) => {
    const [hasError, setHasError] = useState(false)
    const [helperText, setHelperText] = useState("\u200B")
    const [localValue, setLocalValue] = useState<string | undefined>(() => toScheduleValue(value))
    const { setSnackBarMessage } = useAppStore()

    // Memoize the formatted value to prevent unnecessary re-renders
    const formattedValue = useMemo(
        () => (localValue ? formatDate(localValue) : ""),
        [localValue],
    )

    // Memoize the date value for the DatePicker
    const datePickerValue = useMemo(
        () => (localValue ? dateStringToDateUtc(localValue) : undefined),
        [localValue],
    )

    // Memoize min and max values
    const minValue = useMemo(
        () => (min ? dateStringToDateUtc(min) : undefined),
        [min],
    )

    const maxValue = useMemo(
        () => (max ? dateStringToDateUtc(max) : undefined),
        [max],
    )

    // Update local value when prop value changes
    useEffect(() => {
        const newValue = toScheduleValue(value)
        if (newValue !== localValue) {
            setLocalValue(newValue)
        }
    }, [value, localValue])

    const validateInput = useCallback((newValue: number) => {
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

        const dateString = date.toISOString()
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
                    id={resourcePropertyKey}
                    value={datePickerValue}
                    onChange={handleDateChange}
                    minValue={minValue}
                    maxValue={maxValue}
                    formatOptions={{ day: "2-digit", month: "long", year: "numeric" }}
                    locale="nb-NO"
                    timezone="Europe/Oslo"
                    hideClearButton={required}
                />
            </InputSwitcher>
        </InputWrapper>
    )
})

SwitchableDateInput.displayName = "SwitchableDateInput"

export default SwitchableDateInput
