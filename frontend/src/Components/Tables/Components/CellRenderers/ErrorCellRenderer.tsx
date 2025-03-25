import { Tooltip, Icon } from "@equinor/eds-core-react"
import { error_outlined } from "@equinor/eds-icons"
import styled from "styled-components"

import { roundToDecimals } from "@/Utils/FormatingUtils"

const ErrorCellContainer = styled.div<{ $hasError: boolean }>`
    display: flex;
    justify-content: space-between;
    padding-right: 6px;
    background-color: ${(props) => (props.$hasError ? "#FFCCCC" : "transparent")};

    & > div {
        position: relative;
        top: 4px;
        padding: 0 6px;

        & > svg {
            cursor: pointer;
        }
    }
`

const CellRenderer = styled.div`
    padding: 0 6px;
`

interface ErrorCellRendererProps {
    errorMsg: null | string
    value: string | number
    isEditMode?: boolean
    precision?: number
}

const ErrorCellRenderer = ({
    errorMsg, value, isEditMode = false, precision = 2,
}: ErrorCellRendererProps) => {
    let displayValue = value

    if (!isEditMode && typeof value === "number") {
        displayValue = roundToDecimals(value, precision).toString()
    }

    return errorMsg ? (
        <ErrorCellContainer $hasError={!!errorMsg}>
            <Tooltip title={errorMsg} placement="top">
                <div>
                    <Icon data={error_outlined} size={18} color="red" />
                </div>
            </Tooltip>
            {displayValue}
        </ErrorCellContainer>
    ) : (
        <CellRenderer>{displayValue}</CellRenderer>
    )
}

export default ErrorCellRenderer
