import {
    Slider, Autocomplete, TextField,
} from "@mui/material"

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
}

const MileStoneEntry = ({
    title,
    description,
    value,
    periodsData,
    onChange,
}: MileStoneEntryProps) => {
    const handleChange = (_event: Event, newValue: number | number[]) => {
        onChange(newValue as number)
    }

    const handleSelectChange = (_event: React.SyntheticEvent, option: { value: number; quarter: number; year: number } | null) => {
        if (option) {
            onChange(option.value)
        }
    }

    const getValueText = (val: number) => getDisplayText(periodsData[val])

    const selectedPeriod = periodsData[value]

    const selectComponent = (
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
                    size="medium"
                />
            )}
            disableClearable
            fullWidth
        />
    )

    return (
        <BaseGantEntry
            title={title}
            description={description}
            selectComponent={selectComponent}
        >
            <Slider
                getAriaLabel={() => "Milestone date"}
                value={value}
                onChange={handleChange}
                valueLabelDisplay="on"
                valueLabelFormat={getValueText}
                getAriaValueText={getValueText}
                marks={periodsData}
                min={0}
                max={periodsData.length - 1}
                step={1}
                track={false}
                sx={{
                    "& .MuiSlider-thumb": {
                        backgroundColor: "#d32f2f",
                        borderRadius: "2px",
                        width: 20,
                        height: 20,
                    },
                }}
            />
        </BaseGantEntry>
    )
}

export default MileStoneEntry
