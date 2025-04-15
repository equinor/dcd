import { Progress, Typography } from "@equinor/eds-core-react"
import React, { useState } from "react"

import ActionButton from "./ActionButton"
import { SharePointFileStatus } from "./PROSPTab"
import {
    Container,
    LeftColumn,
    MiddleColumn,
    RightColumn,
    StyledSelect,
    TextDisplay,
} from "./SharedStyledComponents"

import { useFeedbackStatus } from "@/Utils/ProspUtils"

export interface SharePointFileDropdownProps {
    caseItem: Components.Schemas.CaseOverviewDto;
    value: string | null;
    options: Record<string, string>;
    onChange: (caseId: string, fileId: string) => Promise<void>;
    onRefresh?: (caseId: string, fileId: string | null) => Promise<boolean>;
    disabled: boolean;
    sharePointFileStatus: SharePointFileStatus;
}

/**
 * SharePointFileDropdown component that displays a dropdown for selecting SharePoint files
 * and an action button for importing data from the selected file.
 * In view mode (disabled=true), it shows a text element instead of a dropdown for better readability.
 */
const SharePointFileDropdown: React.FC<SharePointFileDropdownProps> = ({
    caseItem,
    value,
    options,
    onChange,
    onRefresh,
    disabled,
    sharePointFileStatus,
}) => {
    const [isChanging, setIsChanging] = useState(false)
    const { isLoading: isRefreshing, feedbackStatus, withFeedback } = useFeedbackStatus()
    const [tempSharePointFileStatus, setTempSharePointFileStatus] = useState<SharePointFileStatus | null>(null)

    const isDisabled = disabled || isChanging

    const handleChange = async (e: React.ChangeEvent<HTMLSelectElement>) => {
        const newValue = e.target.value

        if (value === newValue) { return }

        setIsChanging(true)
        setTempSharePointFileStatus(SharePointFileStatus.IMPORTABLE)

        console.log(caseItem.sharepointUpdatedTimestampUtc)

        try {
            await onChange(caseItem.caseId, newValue)
        } catch (error) {
            console.error("[SharePointFileDropdown] Error changing file", error)
        } finally {
            setIsChanging(false)
        }
    }

    const handleRefresh = async () => {
        if (!value || isDisabled || isRefreshing || !onRefresh) { return }

        try {
            await withFeedback(onRefresh(caseItem.caseId, value))
            setTempSharePointFileStatus(null) // Reset temp state to original status
        } catch (error) {
            console.error("[SharePointFileDropdown] Error refreshing file", error)
        }
    }

    const selectedOptionText = value ? options[value] || "" : ""
    const effectiveSharePointFileStatus = tempSharePointFileStatus ?? sharePointFileStatus

    return (
        <Container>
            <LeftColumn>
                <Typography>{caseItem.name || "Case"}</Typography>
            </LeftColumn>

            <MiddleColumn>
                {isDisabled ? (
                    <TextDisplay>
                        {selectedOptionText || "No file selected"}
                    </TextDisplay>
                ) : (
                    <StyledSelect
                        id={`case-sharepointFileId-${caseItem.caseId}`}
                        label=""
                        value={value || ""}
                        onChange={handleChange}
                        disabled={isDisabled}
                    >
                        {Object.entries(options).map(([key, optionValue]) => (
                            <option key={key} value={key}>{optionValue}</option>
                        ))}
                    </StyledSelect>
                )}
            </MiddleColumn>
            <RightColumn>
                {{
                    [SharePointFileStatus.IMPORTING]:
    <>
        <ActionButton
            isLoading
            feedbackStatus={feedbackStatus}
            onClick={() => { }}
            disabled
        />
        <Typography>
            Importing...
        </Typography>
    </>,
                    [SharePointFileStatus.NO_FILE_SELECTED]:
    <ActionButton
        isLoading={isRefreshing}
        feedbackStatus={feedbackStatus}
        onClick={handleRefresh}
        disabled={isDisabled || !value}
    />,
                    [SharePointFileStatus.IMPORTABLE]:
    <>
        <ActionButton
            isLoading={isRefreshing}
            feedbackStatus={feedbackStatus}
            onClick={handleRefresh}
            disabled={isDisabled || !value}
        />
        <Typography>
            Changes found
        </Typography>
    </>,
                    [SharePointFileStatus.UNCHANGED_IN_SHAREPOINT]:
    <>
        <ActionButton
            isLoading={isRefreshing}
            feedbackStatus={feedbackStatus}
            onClick={() => { }}
            disabled
        />
        <Typography>
            No changes found
        </Typography>
    </>,
                }[effectiveSharePointFileStatus]}
            </RightColumn>
        </Container>
    )
}

export default SharePointFileDropdown
