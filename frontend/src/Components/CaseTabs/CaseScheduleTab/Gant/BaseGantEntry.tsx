import { Icon } from "@equinor/eds-core-react"
import { info_circle } from "@equinor/eds-icons"
import {
    Box, Typography, Divider, Tooltip,
} from "@mui/material"
import { ReactNode } from "react"
import styled from "styled-components"

// Shared styled components
export const ChartContainer = styled(Box)`
  padding: 16px 20px;
  margin-bottom: 16px;
  width: 100%;
  overflow: visible;
  border-bottom: 1px solid #eaeaea;
`

export const SliderBox = styled(Box)`
  width: 100%;
  padding: 0 10px;
  margin-top: 1.5rem;
  position: relative;
  z-index: 1;
`

export const ContentContainer = styled(Box)`
  display: flex;
  align-items: flex-start;
  width: 100%;
`

export const HeaderSection = styled(Box)`
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
`

export const SelectSection = styled(Box)`
  flex: 0 0 300px;
  padding-right: 20px;
  display: flex;
  flex-direction: column;
  justify-content: center;
`

export const SliderSection = styled(Box)`
  flex: 1;
`

// Helper function to get display text for a period value
export const getDisplayText = (period: { quarter: number, year: number } | undefined) => (period ? `Q${period.quarter} ${period.year}` : "")

interface BaseGantEntryProps {
    title: string;
    description: string;
    children: ReactNode;
    selectComponent?: ReactNode;
}

const BaseGantEntry = ({
    title,
    description,
    children,
    selectComponent,
}: BaseGantEntryProps) => (
    <ChartContainer>
        <HeaderSection>
            <Typography variant="h6" sx={{ fontWeight: 500 }}>
                {title}
            </Typography>
            <Tooltip title={description} arrow placement="top">
                <span style={{ marginLeft: "8px", cursor: "pointer" }}>
                    <Icon data={info_circle} size={16} />
                </span>
            </Tooltip>
        </HeaderSection>

        <ContentContainer>
            {selectComponent && (
                <SelectSection>
                    {selectComponent}
                </SelectSection>
            )}

            <SliderSection>
                <SliderBox>
                    {children}
                </SliderBox>
            </SliderSection>
        </ContentContainer>
    </ChartContainer>
)

export default BaseGantEntry
