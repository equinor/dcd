import { Icon, Typography } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import {
    Box, Slider,
} from "@mui/material"
import styled from "styled-components"

const RangeSliderContainer = styled(Box)`
  display: flex;
  flex-direction: column;
  margin-bottom: 16px;
  padding: 8px 24px;
  border: 1px solid #eaeaea;
  border-radius: 4px;
  background-color: #fafafa;
`

const Header = styled(Box)`
  display: flex;
  align-items: center;
  gap: 8px;
  padding-bottom: 46px;
`

interface TimelineRangeSliderProps {
    visualSliderValues: [number, number];
    minAllowedYear: number;
    maxAllowedYear: number;
    currentYear: number;
    isLoading: boolean;
    maxRangeYears: number;
    onSliderDrag: (event: Event | React.SyntheticEvent<Element, Event>, newValue: number | number[]) => void;
    onSliderCommit: (event: Event | React.SyntheticEvent<Element, Event>, newValue: number | number[]) => void;
}

const TimelineRangeSlider: React.FC<TimelineRangeSliderProps> = ({
    visualSliderValues,
    minAllowedYear,
    maxAllowedYear,
    currentYear,
    isLoading,
    maxRangeYears,
    onSliderDrag,
    onSliderCommit,
}) => (
    <RangeSliderContainer>
        <Header>
            <Icon data={info_circle} size={18} />
            <Typography>
                Adjust Timeline Range (Max
                {" "}
                {maxRangeYears}
                {" "}
                Years)
            </Typography>
        </Header>

        <Slider
            value={visualSliderValues}
            onChange={onSliderDrag}
            onChangeCommitted={onSliderCommit}
            min={minAllowedYear}
            max={maxAllowedYear}
            step={1}
            disabled={isLoading}
            marks={[
                { value: minAllowedYear, label: `${minAllowedYear}` },
                { value: currentYear, label: `${currentYear}` },
                { value: maxAllowedYear, label: `${maxAllowedYear}` },
            ]}
            valueLabelDisplay="on"
            getAriaValueText={(value): string => `${value}`}
            valueLabelFormat={(value): string => `${value}`}
        />
    </RangeSliderContainer>
)

export default TimelineRangeSlider
