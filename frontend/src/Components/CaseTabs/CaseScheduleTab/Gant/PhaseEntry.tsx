import {
    Slider, Autocomplete, TextField, Stack,
} from "@mui/material"
import { useState, useEffect, useCallback } from "react"

import BaseGantEntry, { getDisplayText } from "./BaseGantEntry"

export interface PhaseEntryProps {
    title: string;
    description: string;
    value: number[];
    periodsData: {
        value: number;
        label: string;
        quarter: number;
        year: number;
    }[];
    onChange: (newValue: number[]) => void;
    onClear?: () => void;
    disabled?: boolean;
}

const PhaseEntry = ({
    title,
    description,
    value,
    periodsData,
    onChange,
    onClear,
    disabled = false,
}: PhaseEntryProps) => {
    // Local state to track the slider value during dragging
    const [localValue, setLocalValue] = useState<number[]>(value)

    // Update local value when prop value changes
    useEffect(() => {
        setLocalValue(value)
    }, [value])

    // Handle local state updates during dragging (doesn't trigger API call)
    const handleDragChange = (_event: Event, newValue: number | number[]) => {
        setLocalValue(newValue as number[])
    }

    // Only commit the change to the API when the slider drag is complete
    const handleChangeCommitted = (_event: React.SyntheticEvent | Event, newValue: number | number[]) => {
        onChange(newValue as number[])
    }

    const handleStartDateChange = (_event: React.SyntheticEvent, option: { value: number; quarter: number; year: number } | null) => {
        if (option) {
            const newValue = [...value]

            newValue[0] = option.value
            // Ensure start date is not after end date
            if (newValue[0] <= newValue[1]) {
                onChange(newValue)
            } else {
                onChange([option.value, option.value])
            }
        }
    }

    const handleEndDateChange = (_event: React.SyntheticEvent, option: { value: number; quarter: number; year: number } | null) => {
        if (option) {
            const newValue = [...value]

            newValue[1] = option.value
            // Ensure end date is not before start date
            if (newValue[1] >= newValue[0]) {
                onChange(newValue)
            } else {
                onChange([option.value, option.value])
            }
        }
    }

    const getValueText = (val: number) => getDisplayText(periodsData[val])

    const startPeriod = periodsData[value[0]]
    const endPeriod = periodsData[value[1]]

    const selectComponent = (
        <Stack spacing={2}>
            <Autocomplete
                value={startPeriod}
                onChange={handleStartDateChange}
                options={periodsData}
                getOptionLabel={(option) => `Q${option.quarter} ${option.year}`}
                renderInput={(params) => (
                    <TextField
                        {...params}
                        label="Start date"
                        variant="outlined"
                        size="medium"
                    />
                )}
                disableClearable
                fullWidth
                disabled={disabled}
            />
            <Autocomplete
                value={endPeriod}
                onChange={handleEndDateChange}
                options={periodsData}
                getOptionLabel={(option) => `Q${option.quarter} ${option.year}`}
                renderInput={(params) => (
                    <TextField
                        {...params}
                        label="End date"
                        variant="outlined"
                        size="medium"
                    />
                )}
                disableClearable
                fullWidth
                disabled={disabled}
            />
        </Stack>
    )

    return (
        <BaseGantEntry
            title={title}
            description={description}
            selectComponent={selectComponent}
            onClear={onClear}
            canClear={Boolean(onClear)}
            disabled={disabled}
        >
            <Slider
                getAriaLabel={() => "Quarter range"}
                value={localValue}
                onChange={handleDragChange}
                onChangeCommitted={handleChangeCommitted}
                valueLabelDisplay="on"
                valueLabelFormat={getValueText}
                getAriaValueText={getValueText}
                marks={periodsData}
                min={0}
                max={periodsData.length - 1}
                step={1}
                disabled={disabled}
                sx={{
                    "& .MuiSlider-thumb": {
                        width: 16,
                        height: 16,
                    },
                }}
            />
        </BaseGantEntry>
    )
}

export default PhaseEntry
