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
  margin-top: ${({ $isReadOnly }): string => ($isReadOnly ? "12px" : "26px")};
  width: 100%;
  overflow: visible;
`

export const SliderBox = styled(Box)<{ $isReadOnly?: boolean }>`
  width: 100%;
  padding: ${({ $isReadOnly }): string => ($isReadOnly ? "0 0 0 10px" : "0 10px")};
  margin-top: ${({ $isReadOnly }): string => ($isReadOnly ? "0" : "0")};
  position: relative;
  z-index: 1;
  display: flex;
  align-items: center;
  height: ${({ $isReadOnly }): string => ($isReadOnly ? "32px" : "auto")};
`

export const ContentContainer = styled(Box)<{ $isReadOnly?: boolean }>`
  display: flex;
  align-items: center;
  width: 100%;
`

export const EntryTitleSection = styled(Box)`
  display: flex;
  align-items: center;
  margin-right: 16px;
  gap: 8px;
  margin-bottom: 16px;
`

export const SliderSection = styled(Box)`
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

export const getDisplayText = (period: { quarter: number, year: number } | undefined): string => (period ? `Q${period.quarter} ${period.year}` : "")

interface BaseGantEntryProps {
    title: string;
    children: ReactNode;
    onClear?: () => void;
    isRequired?: boolean;
    disabled?: boolean;
    readOnly?: boolean;
}

const BaseGantEntry = ({
    title,
    children,
    onClear,
    isRequired = false,
    disabled = false,
    readOnly = false,
}: BaseGantEntryProps): JSX.Element => (
    <ChartContainer $isReadOnly={readOnly}>

        <ContentContainer $isReadOnly={readOnly}>
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

            <SliderSection>
                <SliderBox $isReadOnly={readOnly}>
                    {children}
                </SliderBox>
            </SliderSection>
        </ContentContainer>
    </ChartContainer>
)

export default BaseGantEntry
