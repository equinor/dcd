import React from "react"
import styled from "styled-components"
import { Tooltip, Icon } from "@equinor/eds-core-react"
import { error_outlined } from "@equinor/eds-icons"

const ErrorCellContainer = styled.div<{ hasError: boolean }>`
    display: flex;
    justify-content: space-between;
    padding-right: 6px;
    background-color: ${(props) => (props.hasError ? "#FFCCCC" : "transparent")};

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
    value: string
}

const ErrorCellRenderer = ({ errorMsg, value }: ErrorCellRendererProps) => (
    errorMsg ? (
        <ErrorCellContainer hasError={!!errorMsg}>
            <Tooltip title={errorMsg} placement="top">
                <div>
                    <Icon data={error_outlined} size={18} color="red" />
                </div>
            </Tooltip>
            {value}
        </ErrorCellContainer>
    ) : (
        <CellRenderer>{value}</CellRenderer>
    )
)

export default ErrorCellRenderer
