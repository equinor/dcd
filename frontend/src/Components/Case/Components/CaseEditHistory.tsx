import React, { useEffect, useRef } from "react"
import { Typography, Icon } from "@equinor/eds-core-react"
import { arrow_forward } from "@equinor/eds-icons"
import styled from "styled-components"
import { useCaseContext } from "../../../Context/CaseContext"
import { formatTime, getCurrentEditId } from "../../../Utils/common"

const EditInstance = styled.div<{ $isActive: boolean }>`
    padding: 10px 5px 10px 15px;
    border-left: 2px solid ${({ $isActive }) => ($isActive ? "#007079" : "#DCDCDC")};
`
const Header = styled.div`
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 10px;
    margin-bottom: 5px;
`

const ChangeView = styled.div`
    display: flex;
    flex-wrap: nowrap;
    gap: 10px;
    align-items: center;

    & > p {
        white-space: wrap;
        overflow: hidden;
    }
`
const PreviousValue = styled(Typography)`
    color: red;
    text-decoration: line-through;
    opacity: 0.5;
    max-width: 100px;
    font-size: 12px;
`

const NextValue = styled(Typography)`
    max-width: 100px;
    font-size: 12px;
`
interface CaseEditHistoryProps {
    caseId: string
}
const CaseEditHistory: React.FC<CaseEditHistoryProps> = ({ caseId }) => {
    const { caseEdits, projectCase, editIndexes } = useCaseContext()
    const activeRef = useRef<HTMLDivElement | null>(null)

    useEffect(() => {
        if (activeRef.current) {
            activeRef.current.scrollIntoView({ behavior: "smooth", block: "center" })
        }
    }, [caseEdits, editIndexes, projectCase])

    return (
        <>
            {
                caseEdits.map((edit) => {
                    const isActive = edit.uuid === getCurrentEditId(editIndexes, projectCase)
                    return (
                        edit.caseId === caseId ? (
                            <EditInstance
                                key={edit.uuid}
                                $isActive={isActive}
                                ref={isActive ? activeRef : null}
                            >
                                <Header>
                                    <Typography variant="caption">{String(edit.inputLabel)}</Typography>
                                    <Typography variant="overline">{formatTime(edit.timeStamp)}</Typography>
                                </Header>
                                <ChangeView>
                                    <PreviousValue>
                                        {edit.previousDisplayValue ? edit.previousDisplayValue : edit.previousValue}
                                    </PreviousValue>
                                    <div>
                                        <Icon data={arrow_forward} size={16} />
                                    </div>
                                    <NextValue>
                                        {edit.newDisplayValue ? edit.newDisplayValue : edit.newValue}
                                    </NextValue>
                                </ChangeView>
                            </EditInstance>
                        ) : null
                    )
                })
            }
        </>
    )
}

export default CaseEditHistory
