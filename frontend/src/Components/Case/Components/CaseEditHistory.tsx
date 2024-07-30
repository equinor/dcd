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
        setIsSaving,
        apiQueue,
        setApiQueue,
    } = useAppContext()
    const {
        caseEdits,
        setCaseEdits,
        editIndexes,
        caseEditsBelongingToCurrentCase,
        setEditIndexes,
    } = useCaseContext()
    const { submitToApi } = useDataEdits()

    useEffect(() => {
        const currentEditId = getCurrentEditId(editIndexes, caseId)
        setActiveEdit(currentEditId)
    }, [caseEdits, editIndexes, caseId])

    useEffect(() => {
        if (activeRef.current) {
            activeRef.current.scrollIntoView({ behavior: "smooth", block: "center" })
        }
    }, [activeEdit])

    const updateEditIndex = (newEditId: string) => {
        if (!caseId) {
            console.log("you are not in a project case")
            return
        }

        const editEntry: EditEntry = { caseId, currentEditId: newEditId }
        const storedEditIndexes = localStorage.getItem("editIndexes")
        const editIndexesArray = storedEditIndexes ? JSON.parse(storedEditIndexes) : []
        const currentCasesEditIndex = editIndexesArray.findIndex((entry: { caseId: string }) => entry.caseId === caseId)

        if (currentCasesEditIndex !== -1) {
            editIndexesArray[currentCasesEditIndex].currentEditId = newEditId
        } else {
            editIndexesArray.push(editEntry)
        }

        localStorage.setItem("editIndexes", JSON.stringify(editIndexesArray))
        setEditIndexes(editIndexesArray)
    }

    const addToHistoryTracker = async (editInstanceObject: EditInstance, newCaseId: string) => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, newCaseId))
        const caseEditsNotBelongingToCurrentCase = caseEdits.filter((edit) => edit.caseId !== newCaseId)

        let edits = caseEditsBelongingToCurrentCase

        if (currentEditIndex > 0) {
            edits = caseEditsBelongingToCurrentCase.slice(currentEditIndex)
        }

        if (currentEditIndex === -1) {
            edits = []
        }

        edits = [editInstanceObject, ...edits, ...caseEditsNotBelongingToCurrentCase]
        setCaseEdits(edits)
        updateEditIndex(editInstanceObject.uuid)
    }

    const submitQueueItem = async (editInstance: EditInstance) => {
        console.log("submitting edit", editInstance)
        const success = await submitToApi({
            projectId: editInstance.projectId,
            caseId: editInstance.caseId!,
            resourceName: editInstance.resourceName,
            resourcePropertyKey: editInstance.resourcePropertyKey,
            resourceId: editInstance.resourceId,
            resourceProfileId: editInstance.resourceProfileId,
            wellId: editInstance.wellId,
            drillingScheduleId: editInstance.drillingScheduleId,
            resourceObject: editInstance.newResourceObject as ResourceObject,
        })
        return success
    }

    const addEditToHistoryTracker = async (editInstance: EditInstance) => {
        console.log("Processing edit", editInstance)
        const submitted = await submitQueueItem(editInstance)

        if (submitted && editInstance.caseId) {
            if (!editInstance.resourceProfileId) {
                console.log("Edit saved successfully")
                addToHistoryTracker(editInstance, editInstance.caseId)
            } else {
                const editWithProfileId = structuredClone(editInstance)
                editWithProfileId.resourceProfileId = submitted.resourceProfileId
                editWithProfileId.drillingScheduleId = submitted.resourceProfileId
                addToHistoryTracker(editWithProfileId, editInstance.caseId)
                console.log("Edit with resourceProfileId saved successfully")
            }
        } else {
            console.log("Error saving edit")
        }
    }

    const processQueue = () => {
        console.log("Processes queue method running")
        setApiQueue((prevQueue) => {
            console.log("Queue length", prevQueue.length)

            if (prevQueue.length > 0) {
                setIsSaving(true)
                const editInstance = prevQueue[0]
                addEditToHistoryTracker(editInstance).then(() => {
                    setIsSaving(false)
                    setApiQueue(prevQueue.slice(1))
                })
            }
            return prevQueue
        })
    }

    useEffect(() => {
        if (apiQueue.length > 0) {
            console.log("Queue: ", apiQueue)
            processQueue()
        }
    }, [apiQueue])

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
