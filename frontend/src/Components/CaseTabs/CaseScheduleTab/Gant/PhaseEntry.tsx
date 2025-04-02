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
    readOnly?: boolean;
}

const PhaseEntry = ({
    title,
    description,
    value,
    periodsData,
    onChange,
    onClear,
    disabled = false,
    readOnly = false,
}: PhaseEntryProps) => {
    // Local state to track the slider value during dragging
    const [localValue, setLocalValue] = useState<number[]>(value)

    // Update local value when prop value changes
    useEffect(() => {
        setLocalValue(value)
    }, [value])

    // Handle local state updates during dragging (doesn't trigger API call)
    const handleDragChange = (_event: Event, newValue: number | number[]) => {
        // Only update local state during drag, don't call the API
        // No-op in readOnly mode
        if (!readOnly) {
            setLocalValue(newValue as number[])
        }
    }

    // Only commit the change to the API when the slider drag is complete
    const handleChangeCommitted = (_event: React.SyntheticEvent | Event, newValue: number | number[]) => {
        // Only after drag is complete do we update the parent
        // No-op in readOnly mode
        if (!readOnly) {
            onChange(newValue as number[])
        }
    }

    const handleStartDateChange = (_event: React.SyntheticEvent, option: { value: number; quarter: number; year: number } | null) => {
        if (option && !readOnly) {
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
        if (option && !readOnly) {
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

    // Only show the select component in edit mode
    const selectComponent = !readOnly ? (
        <Stack spacing={1.5}>
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
                        size="small"
                        sx={{ minWidth: "150px" }}
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
                        size="small"
                        sx={{ minWidth: "150px" }}
                    />
                )}
                disableClearable
                fullWidth
                disabled={disabled}
            />
        </Stack>
    ) : null

    return (
        <BaseGantEntry
            title={title}
            description={description}
            selectComponent={selectComponent}
            onClear={!readOnly && onClear ? onClear : undefined}
            canClear={Boolean(onClear)}
            disabled={disabled || readOnly}
            readOnly={readOnly}
        >
            <Slider
                getAriaLabel={() => "Quarter range"}
                value={localValue}
                onChange={handleDragChange}
                onChangeCommitted={handleChangeCommitted}
                valueLabelDisplay="on"
                valueLabelFormat={getValueText}
                getAriaValueText={getValueText}
                marks={periodsData.filter((p) => p.label !== "")}
                min={0}
                max={periodsData.length - 1}
                step={1}
                disabled={disabled || readOnly}
                sx={{
                    "& .MuiSlider-thumb": {
                        width: 16,
                        height: 16,
                    },
                    "& .MuiSlider-markLabel": {
                        fontSize: "0.75rem",
                    },
                    // Make the slider look more presentable in view mode
                    ...(readOnly && {
                        pointerEvents: "none",
                        height: 32, // More compact height
                        marginTop: 0,
                        "& .MuiSlider-valueLabel": {
                            backgroundColor: "transparent",
                            color: "#000",
                            fontSize: "0.75rem",
                            fontWeight: "bold",
                            top: -2,
                            padding: "0 4px",
                        },
                        "& .MuiSlider-rail": {
                            opacity: 0.5,
                            height: 4,
                        },
                        "& .MuiSlider-mark": {
                            opacity: 0.4,
                            height: 8,
                        },
                    }),
                }}
            />
        </BaseGantEntry>
    )
}

export default PhaseEntry
