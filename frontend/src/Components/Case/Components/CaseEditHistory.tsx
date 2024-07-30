import React, { useEffect, useRef, useState } from "react"
import { Typography, Icon } from "@equinor/eds-core-react"
import { arrow_forward } from "@equinor/eds-icons"
import styled from "styled-components"
import { motion } from "framer-motion"
import { useCaseContext } from "../../../Context/CaseContext"
import { formatTime, getCurrentEditId } from "../../../Utils/common"
import { useAppContext } from "../../../Context/AppContext"
import { EditInstance, EditEntry, ResourceObject } from "../../../Models/Interfaces"
import useDataEdits from "../../../Hooks/useDataEdits"

const EditInstanceWrapper = styled(motion.div) <{ $isActive: boolean }>`
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
    opacity: 0.8;    max-width: 100px;
    font-size: 12px;
`

const NextValue = styled(Typography)`
    max-width: 100px;
    font-size: 12px;
    font-weight: bold;
`

interface CaseEditHistoryProps {
    caseId: string
}

const CaseEditHistory: React.FC<CaseEditHistoryProps> = ({ caseId }) => {
    const [activeEdit, setActiveEdit] = useState<string | undefined>(undefined)
    const activeRef = useRef<HTMLDivElement | null>(null)
    const {
        apiQueue,
        setIsSaving,
    } = useAppContext()
    const {
        caseEdits,
        editIndexes,
    } = useCaseContext()
    const {
        processQueue,
    } = useDataEdits()

    useEffect(() => {
        let timer: NodeJS.Timeout | undefined

        if (apiQueue.length > 0) {
            setIsSaving(true)

            console.log("Queue: ", apiQueue)

            // Clear the existing timer if `apiQueue` changes
            if (timer) {
                clearTimeout(timer)
            }

            // Set a new timer for 3 seconds
            timer = setTimeout(() => {
                console.log("processing queue")
                processQueue()
            }, 3000)
        } else {
            setIsSaving(false)
        }

        // Cleanup function to clear the timer if the component unmounts or `apiQueue` changes
        return () => {
            if (timer) {
                clearTimeout(timer)
            }
        }
    }, [apiQueue])

    useEffect(() => {
        const currentEditId = getCurrentEditId(editIndexes, caseId)
        setActiveEdit(currentEditId)
    }, [caseEdits, editIndexes, caseId])

    useEffect(() => {
        if (activeRef.current) {
            activeRef.current.scrollIntoView({ behavior: "smooth", block: "center" })
        }
    }, [activeEdit])

    return (
        <>
            {caseEdits.map((edit) => {
                const isActive = edit.uuid === activeEdit
                return edit.caseId === caseId ? (
                    <EditInstanceWrapper
                        key={edit.uuid}
                        $isActive={isActive}
                        ref={isActive ? activeRef : null}
                        initial={{ opacity: 0, x: -20 }}
                        animate={{ opacity: 1, x: 0 }}
                        transition={{ type: "spring", stiffness: 300, damping: 20 }}
                    >
                        <Header>
                            <Typography variant="caption">{String(edit.inputLabel)}</Typography>
                            <Typography variant="overline">{formatTime(edit.timeStamp)}</Typography>
                        </Header>
                        <ChangeView>
                            <PreviousValue>
                                {edit.previousDisplayValue}
                            </PreviousValue>
                            <div>
                                <Icon data={arrow_forward} size={16} />
                            </div>
                            <NextValue>
                                {edit.newDisplayValue}
                            </NextValue>
                        </ChangeView>
                    </EditInstanceWrapper>
                ) : null
            })}
        </>
    )
}

export default CaseEditHistory
