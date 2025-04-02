import { Icon } from "@equinor/eds-core-react"
import { info_circle, delete_to_trash } from "@equinor/eds-icons"
import {
    Box, Typography, IconButton, Tooltip, Button,
} from "@mui/material"
import { ReactNode } from "react"
import styled from "styled-components"

// Shared styled components
export const ChartContainer = styled(Box)<{ isReadOnly?: boolean }>`
  padding: ${({ isReadOnly }) => (isReadOnly ? "8px 20px" : "16px 20px")};
  margin-bottom: ${({ isReadOnly }) => (isReadOnly ? "8px" : "16px")};
  width: 100%;
  overflow: visible;
  border-bottom: 1px solid #eaeaea;
`

export const SliderBox = styled(Box)<{ isReadOnly?: boolean }>`
  width: 100%;
  padding: ${({ isReadOnly }) => (isReadOnly ? "0 0 0 10px" : "0 10px")};
  margin-top: ${({ isReadOnly }) => (isReadOnly ? "0" : "1.5rem")};
  position: relative;
  z-index: 1;
  display: flex;
  align-items: center;
  height: ${({ isReadOnly }) => (isReadOnly ? "32px" : "auto")};
`

export const ContentContainer = styled(Box)<{ isReadOnly?: boolean }>`
  display: flex;
  align-items: ${({ isReadOnly }) => (isReadOnly ? "center" : "flex-start")};
  width: 100%;
`

export const HeaderSection = styled(Box)<{ isReadOnly?: boolean }>`
  margin-bottom: ${({ isReadOnly }) => (isReadOnly ? "0" : "1rem")};
  display: ${({ isReadOnly }) => (isReadOnly ? "none" : "flex")};
  align-items: center;
  justify-content: space-between;
`

export const HeaderLeft = styled(Box)`
  display: flex;
  align-items: center;
`

export const HeaderRight = styled(Box)`
  display: flex;
  align-items: center;
`

export const SelectSection = styled(Box)`
  flex: 0 0 200px;
  padding-right: 16px;
  display: flex;
  flex-direction: column;
  justify-content: center;
`

export const SliderSection = styled(Box)<{ isFullWidth?: boolean; isReadOnly?: boolean }>`
  flex: 1;
  ${({ isFullWidth }) => isFullWidth && `
    flex: 1 0 100%;
  `}
`

export const ViewModeHeader = styled(Box)`
  display: flex;
  align-items: center;
  width: 80px;
  min-width: 80px;
  margin-right: 16px;
  padding-left: 8px;
  height: 32px; /* Match the slider height */
`

// Helper function to get display text for a period value
export const getDisplayText = (period: { quarter: number, year: number } | undefined) => (period ? `Q${period.quarter} ${period.year}` : "")

interface BaseGantEntryProps {
    title: string;
    description: string;
    children: ReactNode;
    selectComponent?: ReactNode;
    onClear?: () => void;
    canClear?: boolean;
    disabled?: boolean;
    readOnly?: boolean;
}

const BaseGantEntry = ({
    title,
    description,
    children,
    selectComponent,
    onClear,
    canClear = false,
    disabled = false,
    readOnly = false,
}: BaseGantEntryProps) => (
    <ChartContainer isReadOnly={readOnly}>
        <HeaderSection isReadOnly={readOnly}>
            <HeaderLeft>
                <Typography variant="h6" sx={{ fontWeight: 500 }}>
                    {title}
                </Typography>
                <Tooltip title={description} arrow placement="top">
                    <span style={{ marginLeft: "8px", cursor: "pointer" }}>
                        <Icon data={info_circle} size={16} />
                    </span>
                </Tooltip>
            </HeaderLeft>

            {onClear && canClear && (
                <HeaderRight>
                    <Button
                        onClick={onClear}
                        variant="outlined"
                        color="error"
                        size="small"
                        disabled={disabled}
                        startIcon={<Icon data={delete_to_trash} size={16} />}
                        sx={{
                            minWidth: "auto",
                            "&:hover": {
                                backgroundColor: "rgba(211, 47, 47, 0.08)",
                            },
                        }}
                    >
                        Remove
                    </Button>
                </HeaderRight>
            )}
        </HeaderSection>

        <ContentContainer isReadOnly={readOnly}>
            {readOnly && (
                <ViewModeHeader>
                    <Typography variant="body2" sx={{ fontWeight: 500 }}>
                        {title}
                    </Typography>
                    <Tooltip title={description} arrow placement="top">
                        <span style={{ marginLeft: "4px", cursor: "help" }}>
                            <Icon data={info_circle} size={16} />
                        </span>
                    </Tooltip>
                </ViewModeHeader>
            )}

            {!readOnly && selectComponent && (
                <SelectSection>
                    {selectComponent}
                </SelectSection>
            )}

            <SliderSection isFullWidth={!selectComponent && !readOnly} isReadOnly={readOnly}>
                <SliderBox isReadOnly={readOnly}>
                    {children}
                </SliderBox>
            </SliderSection>
        </ContentContainer>
    </ChartContainer>
)

export default BaseGantEntry
