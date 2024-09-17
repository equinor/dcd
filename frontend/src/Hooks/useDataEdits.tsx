import { v4 as uuidv4 } from "uuid"
import { useLocation, useNavigate, useParams } from "react-router"
import _ from "lodash"
import { useCaseContext } from "../Context/CaseContext"
import {
    EditInstance,
    EditEntry,
    ResourceName,
    ResourcePropertyKey,
    ResourceObject,
} from "../Models/Interfaces"
import { getCurrentEditId } from "../Utils/common"
import { useAppContext } from "../Context/AppContext"
import { useSubmitToApi } from "./UseSubmitToApi"

interface AddEditParams {
    inputLabel: string;
    projectId: string;
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    resourceId?: string;
    resourceProfileId?: string;
    wellId?: string;
    drillingScheduleId?: string;
    caseId?: string;
    newDisplayValue?: string | number | undefined;
    previousDisplayValue?: string | number | undefined;
    newResourceObject: ResourceObject;
    previousResourceObject: ResourceObject;
    tabName?: string;
    tableName?: string;
    inputFieldId?: string;
}

const useDataEdits = (): {
    addEdit: (params: AddEditParams) => void;
    undoEdit: () => void;
    redoEdit: () => void;
    processQueue: () => void;
} => {
    const {
        apiQueue,
        setApiQueue,
    } = useAppContext()
    const {
        caseEdits,
        setCaseEdits,
        editIndexes,
        setEditIndexes,
        caseEditsBelongingToCurrentCase,
    } = useCaseContext()
    const { submitToApi } = useSubmitToApi()

    const { caseId: caseIdFromParams } = useParams()
    const location = useLocation()
    const navigate = useNavigate()

    const getActiveEditFromIndexes = () => {
        const storedEditIndexes = localStorage.getItem("editIndexes")
        const editIndexesArray = storedEditIndexes ? JSON.parse(storedEditIndexes) : []

        const existingEntry = _.find(editIndexesArray, { caseId: caseIdFromParams })

        if (existingEntry) {
            return existingEntry
        }
        return undefined
    }

    const updateEditIndex = (newEditId: string) => {
        if (!caseIdFromParams) {
            console.log("you are not in a project case")
            return
        }

        const editEntry: EditEntry = { caseId: caseIdFromParams, currentEditId: newEditId }

        const storedEditIndexes = localStorage.getItem("editIndexes")
        const editIndexesArray = storedEditIndexes ? JSON.parse(storedEditIndexes) : []

        const activeEdit = getActiveEditFromIndexes()

        if (activeEdit) {
            activeEdit.currentEditId = newEditId
            const index = _.findIndex(editIndexesArray, { caseId: caseIdFromParams })
            editIndexesArray[index] = activeEdit
        } else {
            editIndexesArray.push(editEntry)
        }

        localStorage.setItem("editIndexes", JSON.stringify(editIndexesArray))
        setEditIndexes(editIndexesArray)
    }

    const updateHistory = () => {
        const currentCaseEditsWithoutObsoleteEntries = () => {
            const activeEdit = getActiveEditFromIndexes()

            if (activeEdit) {
                const indexOfActiveEdit = _.findIndex(caseEditsBelongingToCurrentCase, { uuid: activeEdit.currentEditId })

                if (indexOfActiveEdit > 0) {
                    const newCurrentCaseEdits = structuredClone(caseEditsBelongingToCurrentCase)
                    newCurrentCaseEdits.splice(0, indexOfActiveEdit)

                    return newCurrentCaseEdits
                }
            }
            return caseEditsBelongingToCurrentCase
        }
        const caseEditsNotBelongingToCurrentCase = caseEdits.filter((edit) => edit.caseId !== caseIdFromParams)
        const allEdits = [...apiQueue, ...currentCaseEditsWithoutObsoleteEntries(), ...caseEditsNotBelongingToCurrentCase]

        setCaseEdits(allEdits)
        updateEditIndex(allEdits[0].uuid)
    }

    /**
     * Submits an edit instance to the api then returns the same edit instance. In cases where the API
     * returns a new resourceProfileId, the edit instance is updated with the new resourceProfileId and returned.
     */
    const HandleApiSubmissionResults = async (editInstance: EditInstance) => {
        const {
            projectId,
            caseId,
            resourceName,
            resourceId,
            resourceProfileId,
            wellId,
            drillingScheduleId,
            newResourceObject,
        } = editInstance

        const submitted = await submitToApi({
            projectId,
            caseId: caseId!,
            resourceName,
            resourceId,
            resourceProfileId,
            wellId,
            drillingScheduleId,
            resourceObject: newResourceObject as ResourceObject,
        })

        if (submitted && caseId) {
            if (!resourceProfileId) {
                return editInstance
            }
            const editWithProfileId = structuredClone(editInstance)
            editWithProfileId.resourceProfileId = submitted.resourceProfileId
            editWithProfileId.drillingScheduleId = submitted.resourceProfileId
            return editWithProfileId
        }

        console.log("Error saving edit")
        return null
    }

    /**
     * iterates the queue from the end to the beginning and creates a new array containing only the latest
     * edit for each resource, then submits that array and updates the history tracker.
     *
     * Since each edit in the queue contains the previous edits made to the same resource object,
     * we only need to submit the the latest edit for each modified resource object to update the data the API
     */
    const processQueue = async () => {
        const uniqueEditsQueue = _.uniqBy(apiQueue.reverse(), (edit) => edit.resourceName + edit.resourceId)
        const registedEdits = await Promise.all(uniqueEditsQueue.map((editInstance) => HandleApiSubmissionResults(editInstance)))

        // todo: make sure that when the registered edit method returns an edit with a resourceProfileId,
        // the edit in history tracker is updated with the new resourceProfileId
        // update: actually, it seems that the switch between put and post works fine already?
        // idk how though we should discuss this
        updateHistory()
        setApiQueue([])
    }

    const addEdit = async ({
        inputLabel,
        projectId,
        resourceName,
        resourcePropertyKey,
        resourceId,
        resourceProfileId,
        wellId,
        drillingScheduleId,
        caseId,
        newDisplayValue,
        previousDisplayValue,
        newResourceObject,
        previousResourceObject,
        tabName,
        tableName,
        inputFieldId,
    }: AddEditParams) => {
        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            return
        }

        if (_.isEqual(newResourceObject, previousResourceObject)) {
            console.log("No changes made")
            return
        }

        const editIsForSameResourceName = (edit1: EditInstance, edit2: EditInstance) => edit1.resourceName === edit2.resourceName && edit1.caseId === edit2.caseId
        const editIsForSamePropertyKey = (edit1: EditInstance, edit2: EditInstance) => edit1.resourcePropertyKey === edit2.resourcePropertyKey

        const insertedEditInstanceObject: EditInstance = {
            uuid: uuidv4(),
            timeStamp: new Date().getTime(),
            inputLabel,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            resourceProfileId,
            wellId,
            drillingScheduleId,
            caseId,
            newDisplayValue,
            previousDisplayValue,
            newResourceObject,
            previousResourceObject,
            tabName,
            tableName,
            inputFieldId,
        }

        const existingEditsForSameResourceInQueue = apiQueue
            .slice()
            .filter((edit) => editIsForSameResourceName(edit, insertedEditInstanceObject))

        let sameFieldAlreadyInQueue = null

        for (let i = existingEditsForSameResourceInQueue.length - 1; i >= 0; i -= 1) {
            const edit = existingEditsForSameResourceInQueue[i]
            if (editIsForSamePropertyKey(edit, insertedEditInstanceObject)) {
                sameFieldAlreadyInQueue = edit
                break
            }
        }

        if (existingEditsForSameResourceInQueue.length > 0) {
            // TODO: find a more elegant way to check if the edit is for a table
            const isTableEdit = Object.prototype.hasOwnProperty.call(newResourceObject, "startYear")
                && Object.prototype.hasOwnProperty.call(newResourceObject, "values")

            const latestEditInQueue = structuredClone(existingEditsForSameResourceInQueue[existingEditsForSameResourceInQueue.length - 1])
            const existingQueueItemsResourceObject = structuredClone(latestEditInQueue.newResourceObject)

            let combinedResourceObject = {} as ResourceObject
            if (isTableEdit) {
                combinedResourceObject = structuredClone(newResourceObject)
            } else {
                const propertyKey = resourcePropertyKey as keyof ResourceObject
                existingQueueItemsResourceObject[propertyKey] = newResourceObject[propertyKey]
                combinedResourceObject = existingQueueItemsResourceObject
            }

            if (sameFieldAlreadyInQueue) {
                // add the new edit with the updated previous resource object
                const insertedEditInstanceWithCombinedResourceObject: EditInstance = {
                    ...insertedEditInstanceObject,
                    newResourceObject: combinedResourceObject,
                    previousResourceObject: latestEditInQueue.newResourceObject,
                    previousDisplayValue: sameFieldAlreadyInQueue.newDisplayValue,
                }
                setApiQueue([...apiQueue, insertedEditInstanceWithCombinedResourceObject])
            } else {
                // add new queue entry combining the new values of the previous edit with the new values of the current edit
                const insertedEditInstanceWithCombinedResourceObject: EditInstance = {
                    ...insertedEditInstanceObject,
                    newResourceObject: combinedResourceObject,
                    previousResourceObject: latestEditInQueue.newResourceObject,
                }
                setApiQueue([...apiQueue, insertedEditInstanceWithCombinedResourceObject])
            }
        } else {
            // new unique edit added with no previous edits for the same resource object. just add it to the queue
            setApiQueue([...apiQueue, insertedEditInstanceObject])
        }
    }

    const { caseId } = useParams()

    const highlightElement = (element: HTMLElement | null, duration = 3000) => {
        if (element) {
            element.classList.add("highlighted")
            setTimeout(() => {
                element.classList.remove("highlighted")
            }, duration)
        }
    }

    const delay = (ms: number) => new Promise((resolve) => { setTimeout(resolve, ms) })

    const undoEdit = async () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex(
            (edit) => edit.uuid === getCurrentEditId(editIndexes, caseIdFromParams),
        )
        if (currentEditIndex === -1) {
            return
        }

        const editThatWillBeUndone = caseEditsBelongingToCurrentCase[currentEditIndex]
        const updatedEditIndex = currentEditIndex + 1
        const updatedEdit = caseEditsBelongingToCurrentCase[updatedEditIndex]

        updateEditIndex(updatedEdit.uuid)

        if (editThatWillBeUndone) {
            const projectUrl = location.pathname.split("/case")[0]
            navigate(`${projectUrl}/case/${caseId}/${editThatWillBeUndone.tabName ?? ""}`)

            const scrollToElement = (elementId: string) => new Promise<void>((resolve, reject) => {
                const tabElement = document.getElementById(elementId) as HTMLElement | null
                if (!tabElement) {
                    reject(new Error(`Element with id ${elementId} not found`))
                    return
                }
                tabElement.scrollIntoView({ behavior: "smooth", block: "center" })
                resolve()
            })

            const rowWhereCellWillBeUndone = editThatWillBeUndone.tableName ?? editThatWillBeUndone.inputFieldId ?? editThatWillBeUndone.inputLabel

            if (!rowWhereCellWillBeUndone) {
                console.error("rowWhereCellWillBeUndone is undefined")
                return
            }

            setTimeout(async () => {
                try {
                    await scrollToElement(rowWhereCellWillBeUndone)

                    const tabElement = document.getElementById(rowWhereCellWillBeUndone) as HTMLElement | null
                    if (tabElement) {
                        // Attempt to highlight cell, doesnt work since querySelector can't find any element with data-key="${editThatWillBeUndone.resourcePropertyKey}
                        if (editThatWillBeUndone.tableName) {
                            const tableCell = tabElement.querySelector(`[data-key="${editThatWillBeUndone.resourcePropertyKey}"]`) as HTMLElement | null
                            highlightElement(tableCell ?? tabElement)
                        } else {
                            highlightElement(tabElement)
                        }
                    }

                    await delay(500)
                    await submitToApi({
                        projectId: editThatWillBeUndone.projectId,
                        caseId: editThatWillBeUndone.caseId!,
                        resourceProfileId: editThatWillBeUndone.resourceProfileId,
                        resourceName: editThatWillBeUndone.resourceName,
                        resourceId: editThatWillBeUndone.resourceId,
                        resourceObject: editThatWillBeUndone.previousResourceObject as ResourceObject,
                        wellId: editThatWillBeUndone.wellId,
                        drillingScheduleId: editThatWillBeUndone.drillingScheduleId,
                    })
                } catch (error) {
                    console.error(error)
                }
            }, 500)
        }
    }

    const redoEdit = async () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex(
            (edit) => edit.uuid === getCurrentEditId(editIndexes, caseIdFromParams),
        )
        if (currentEditIndex <= 0) {
            const lastEdit = caseEditsBelongingToCurrentCase[caseEditsBelongingToCurrentCase.length - 1]
            if (lastEdit) {
                updateEditIndex(lastEdit.uuid)
                const projectUrl = location.pathname.split("/case")[0]
                navigate(`${projectUrl}/case/${caseId}/${lastEdit.tabName ?? ""}`)

                const scrollToElement = (elementId: string) => new Promise<void>((resolve, reject) => {
                    const tabElement = document.getElementById(elementId) as HTMLElement | null
                    if (!tabElement) {
                        reject(new Error(`Element with id ${elementId} not found`))
                        return
                    }
                    tabElement.scrollIntoView({ behavior: "auto", block: "center" })
                    resolve()
                })

                const rowWhereCellWillBeUndone = lastEdit.tableName ?? lastEdit.inputFieldId ?? lastEdit.inputLabel
                if (!rowWhereCellWillBeUndone) {
                    console.error("rowWhereCellWillBeUndone is undefined")
                    return
                }

                setTimeout(async () => {
                    try {
                        await scrollToElement(rowWhereCellWillBeUndone)

                        const tabElement = document.getElementById(rowWhereCellWillBeUndone) as HTMLElement | null
                        if (tabElement) {
                            if (lastEdit.tableName) {
                                const tableCell = tabElement.querySelector(`[data-key="${lastEdit.resourcePropertyKey}"]`) as HTMLElement | null
                                highlightElement(tableCell ?? tabElement)
                            } else {
                                highlightElement(tabElement)
                            }
                        }

                        await delay(500)
                        await submitToApi({
                            projectId: lastEdit.projectId,
                            caseId: lastEdit.caseId!,
                            resourceProfileId: lastEdit.resourceProfileId,
                            resourceName: lastEdit.resourceName,
                            resourceId: lastEdit.resourceId,
                            resourceObject: lastEdit.newResourceObject as ResourceObject,
                            wellId: lastEdit.wellId,
                            drillingScheduleId: lastEdit.drillingScheduleId,
                        })
                    } catch (error) {
                        console.error(error)
                    }
                }, 500)
            }
        } else {
            const updatedEdit = caseEditsBelongingToCurrentCase[currentEditIndex - 1]
            updateEditIndex(updatedEdit.uuid)
            if (updatedEdit) {
                const projectUrl = location.pathname.split("/case")[0]
                navigate(`${projectUrl}/case/${caseId}/${updatedEdit.tabName ?? ""}`)

                const scrollToElement = (elementId: string) => new Promise<void>((resolve, reject) => {
                    const tabElement = document.getElementById(elementId) as HTMLElement | null
                    if (!tabElement) {
                        reject(new Error(`Element with id ${elementId} not found`))
                        return
                    }
                    tabElement.scrollIntoView({ behavior: "smooth", block: "center" })
                    resolve()
                })

                const rowWhereCellWillBeUndone = updatedEdit.tableName ?? updatedEdit.inputFieldId ?? updatedEdit.inputLabel
                if (!rowWhereCellWillBeUndone) {
                    return
                }

                setTimeout(async () => {
                    try {
                        await scrollToElement(rowWhereCellWillBeUndone)

                        const tabElement = document.getElementById(rowWhereCellWillBeUndone) as HTMLElement | null
                        if (tabElement) {
                            if (updatedEdit.tableName) {
                                const tableCell = tabElement.querySelector(`[data-key="${updatedEdit.resourcePropertyKey}"]`) as HTMLElement | null
                                highlightElement(tableCell ?? tabElement)
                            } else {
                                highlightElement(tabElement)
                            }
                        }

                        await delay(500)
                        await submitToApi({
                            projectId: updatedEdit.projectId,
                            caseId: updatedEdit.caseId!,
                            resourceProfileId: updatedEdit.resourceProfileId,
                            resourceName: updatedEdit.resourceName,
                            resourceId: updatedEdit.resourceId,
                            resourceObject: updatedEdit.newResourceObject as ResourceObject,
                            wellId: updatedEdit.wellId,
                            drillingScheduleId: updatedEdit.drillingScheduleId,
                        })
                    } catch (error) {
                        console.error(error)
                    }
                }, 500)
            }
        }
    }

    return {
        addEdit,
        undoEdit,
        redoEdit,
        processQueue,
    }
}

export default useDataEdits
