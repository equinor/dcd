import {
    Slider, Autocomplete, TextField,
} from "@mui/material"
import { useState, useEffect, useCallback } from "react"

import BaseGantEntry, { getDisplayText } from "./BaseGantEntry"

export interface MileStoneEntryProps {
    title: string;
    description: string;
    value: number;
    periodsData: {
        value: number;
        label: string;
        quarter: number;
        year: number;
    }[];
    onChange: (newValue: number) => void;
    onClear?: () => void;
    disabled?: boolean;
    readOnly?: boolean;
}

const MileStoneEntry = ({
    title,
    description,
    value,
    periodsData,
    onChange,
    onClear,
    disabled = false,
    readOnly = false,
}: MileStoneEntryProps) => {
    // Local state to track the slider value during dragging
    const [localValue, setLocalValue] = useState<number>(value)

    // Update local value when prop value changes
    useEffect(() => {
        setLocalValue(value)
    }, [value])

    // Handle local state updates during dragging (doesn't trigger API call)
    const handleDragChange = (_event: Event, newValue: number | number[]) => {
        // Only update local state during drag, don't call the API
        // No-op in readOnly mode
        if (!readOnly) {
            setLocalValue(newValue as number)
        }
    }

    // Only commit the change to the API when the slider drag is complete
    const handleChangeCommitted = (_event: React.SyntheticEvent | Event, newValue: number | number[]) => {
        // Only after drag is complete do we update the parent
        // No-op in readOnly mode
        if (!readOnly) {
            onChange(newValue as number)
        }
    }

    const handleSelectChange = (_event: React.SyntheticEvent, option: { value: number; quarter: number; year: number } | null) => {
        if (option && !readOnly) {
            onChange(option.value)
        }
    }

    const getValueText = (val: number) => getDisplayText(periodsData[val])

    const selectedPeriod = periodsData[value]

    // Only show the select component in edit mode
    const selectComponent = !readOnly ? (
        <Autocomplete
            value={selectedPeriod}
            onChange={handleSelectChange}
            options={periodsData}
            getOptionLabel={(option) => `Q${option.quarter} ${option.year}`}
            renderInput={(params) => (
                <TextField
                    {...params}
                    label="Select date"
                    variant="outlined"
                    size="small"
                    sx={{ width: "130px" }}
                />
            )}
            disableClearable
            fullWidth
            disabled={disabled}
        />
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
                getAriaLabel={() => "Milestone date"}
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
                track={false}
                disabled={disabled || readOnly}
                sx={{
                    "& .MuiSlider-thumb": {
                        backgroundColor: "#d32f2f",
                        borderRadius: "2px",
                        width: 20,
                        height: 20,
                    },
                    "& .MuiSlider-markLabel": {
                        fontSize: "0.75rem",
                    },
                    // Make the slider look more presentable in view mode
                    ...(readOnly && {
                        pointerEvents: "none",
                        height: 32, // More compact height
                        marginTop: 0,
                        "& .MuiSlider-thumb": {
                            backgroundColor: "#d32f2f",
                            borderRadius: "2px",
                            width: 12,
                            height: 12,
                        },
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

export default MileStoneEntry
