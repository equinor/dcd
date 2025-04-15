import { Icon } from "@equinor/eds-core-react"
import { delete_to_trash } from "@equinor/eds-icons"
import {
    Box,
    Typography,
    IconButton,
    Tooltip,
} from "@mui/material"
import { ReactNode } from "react"
import styled from "styled-components"

export const ChartContainer = styled(Box)<{ $isReadOnly?: boolean }>`
  padding: ${({ $isReadOnly }): string => ($isReadOnly ? "8px 20px" : "16px 20px")};
  margin-top: ${({ $isReadOnly }): string => ($isReadOnly ? "18px" : "26px")};
  width: 100%;
  overflow: visible;
  ${({ $isReadOnly }): string => ($isReadOnly ? "" : "border: 1px solid #7c7c7c33;")}
  background-color: ${({ $isReadOnly }): string => ($isReadOnly ? "transparent" : "#d8d8d833")};
  border-radius: 8px;
`

export const SliderBox = styled(Box)<{ $isReadOnly?: boolean }>`
  width: 100%;
  z-index: 1;
  display: flex;
`

export const ContentContainer = styled(Box)<{ $isReadOnly?: boolean }>`
  display: flex;
  align-items: flex-start;
  width: 100%;
`

export const EntryTitleSection = styled(Box)`
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 8px;
`

export const LeftSection = styled(Box)`
  margin-right: 24px;
  display: flex;
  flex-direction: column;
`

export const SliderSection = styled(Box)`
  margin-top: 32px;
  flex: 1;
  display: flex;
  align-items: center;
`

export const DeleteIconButton = styled(IconButton)`
  && {
    color: #d32f2f;
    padding: 2px;
    &:hover {
      background-color: rgba(211, 47, 47, 0.08);
    }
    &.Mui-disabled {
      color: rgba(211, 47, 47, 0.4);
    }
  }
`

export const getDisplayText = (period: { quarter: number, year: number }): string => (`Q${period.quarter} ${period.year}`)

interface BaseGantEntryProps {
    title: string;
    children: ReactNode;
    leftControls: ReactNode;
    onClear?: () => void;
    isRequired?: boolean;
    disabled?: boolean;
    readOnly?: boolean;
}

const BaseGantEntry = ({
    title,
    children,
    leftControls,
    onClear,
    isRequired = false,
    disabled = false,
    readOnly = false,
}: BaseGantEntryProps): JSX.Element => (
    <ChartContainer $isReadOnly={readOnly}>
        <ContentContainer $isReadOnly={readOnly}>
            <LeftSection>
                <EntryTitleSection>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>
                        {title}
                    </Typography>
                    {!readOnly && onClear && (
                        <Tooltip title={isRequired ? "Required milestone cannot be removed" : "Remove milestone"} placement="top">
                            <span>
                                <DeleteIconButton
                                    onClick={onClear}
                                    disabled={disabled || isRequired}
                                >
                                    <Icon data={delete_to_trash} size={16} />
                                </DeleteIconButton>
                            </span>
                        </Tooltip>
                    )}
                </EntryTitleSection>
                {leftControls}
            </LeftSection>

            <SliderSection>
                <SliderBox $isReadOnly={readOnly}>
                    {children}
                </SliderBox>
            </SliderSection>
        </ContentContainer>
    </ChartContainer>
)

export default BaseGantEntry
