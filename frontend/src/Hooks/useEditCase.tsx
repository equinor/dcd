import { v4 as uuidv4 } from "uuid"
import { useParams } from "react-router"
import _ from "lodash"
import { useCaseStore } from "../Store/CaseStore"
import {
    EditInstance,
    EditEntry,
    ResourceName,
    ResourcePropertyKey,
    ResourceObject,
} from "../Models/Interfaces"
import { getCurrentEditId } from "../Utils/common"
import { useAppStore } from "../Store/AppStore"
import { useSubmitToApi } from "./UseSubmitToApi"
import { useAppNavigation } from "./useNavigate"
import { createLogger } from "../Utils/logger"
import { useLocalStorage } from "./useLocalStorage"

interface AddEditParams {
    inputLabel: string;
    projectId: string;
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    resourceId?: string;
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

const editQueueLogger = createLogger({
    name: "EDIT_QUEUE",
    enabled: false, // Set to true to enable debug logging. dont leave this on for production
})

const useEditCase = () => {
    const {
        apiQueue,
        setApiQueue,
        setIsSaving,
    } = useAppStore()
    const {
        caseEdits,
        setCaseEdits,
        editIndexes,
        setEditIndexes,
        caseEditsBelongingToCurrentCase,
    } = useCaseStore()
    const { submitToApi } = useSubmitToApi()
    const { navigateToCaseTab } = useAppNavigation()
    const [storedEditIndexes, setStoredEditIndexes] = useLocalStorage<EditEntry[]>("editIndexes", [])

    const { caseId: caseIdFromParams } = useParams()

    const getActiveEditFromIndexes = () => {
        const existingEntry = _.find(storedEditIndexes, { caseId: caseIdFromParams })
        return existingEntry
    }

    const updateEditIndex = (newEditId: string) => {
        if (!caseIdFromParams) {
            editQueueLogger.warn("Not in a project case")
            return
        }

        const editEntry: EditEntry = { caseId: caseIdFromParams, currentEditId: newEditId }
        const activeEdit = getActiveEditFromIndexes()

        let updatedEditIndexes: EditEntry[]
        if (activeEdit) {
            activeEdit.currentEditId = newEditId
            const index = _.findIndex(storedEditIndexes, { caseId: caseIdFromParams })
            updatedEditIndexes = [...storedEditIndexes]
            updatedEditIndexes[index] = activeEdit
            editQueueLogger.warn("Updated existing edit index:", { newEditId, activeEdit })
        } else {
            updatedEditIndexes = [...storedEditIndexes, editEntry]
            editQueueLogger.warn("Added new edit index:", editEntry)
        }

        setStoredEditIndexes(updatedEditIndexes)
        setEditIndexes(updatedEditIndexes)
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
            wellId,
            drillingScheduleId,
            newResourceObject,
        } = editInstance

        const result = await submitToApi({
            projectId,
            caseId: caseId!,
            resourceName,
            resourceId,
            wellId,
            drillingScheduleId,
            resourceObject: newResourceObject as ResourceObject,
        })

        if (result.success && caseId) {
            const editWithProfileId = structuredClone(editInstance)
            if (result.data?.id) {
                editWithProfileId.drillingScheduleId = result.data.id
            }
            return editWithProfileId
        }

        // console.log("Error saving edit")
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
        editQueueLogger.warn("Processing queue:", apiQueue)

        try {
            const uniqueEditsQueue = _.uniqBy(
                [...apiQueue].reverse(),
                (edit) => edit.resourceName + edit.resourceId + edit.resourcePropertyKey + (edit.wellId ? edit.wellId : ""),
            )

            editQueueLogger.warn("Unique edits to process:", uniqueEditsQueue)

            const results = await Promise.all(
                uniqueEditsQueue.map(async (editInstance) => {
                    try {
                        return await HandleApiSubmissionResults(editInstance)
                    } catch (error) {
                        console.error("Error processing edit:", {
                            error,
                            editInstance,
                            resourceName: editInstance.resourceName,
                            resourceId: editInstance.resourceId,
                        })
                        editQueueLogger.warn("Issue happens here! Failed to process edit:", {
                            error,
                            editInstance,
                        })
                        return null
                    }
                }),
            )

            editQueueLogger.warn("API submission results:", results)

            if (results.some((result) => result !== null)) {
                updateHistory()
            }

            setApiQueue([])
        } catch (error) {
            console.error("Fatal error in processQueue:", error)
            editQueueLogger.warn("Fatal error in processQueue:", error)
            setApiQueue([]) // Clear queue even on error to prevent stuck state
        }
    }
    const editIsForSameResourceName = (edit1: EditInstance, edit2: EditInstance) => edit1.resourceName === edit2.resourceName && edit1.caseId === edit2.caseId

    const handleTableEdit = (insertedEditInstanceObject: EditInstance) => {
        editQueueLogger.warn("Current queue for this specific resource before adding table edit: ", apiQueue)
        editQueueLogger.warn("Adding table edit:", insertedEditInstanceObject)

        // Use functional update to ensure we're working with the latest state
        setApiQueue((prevQueue) => {
            const updatedQueue = [...prevQueue, insertedEditInstanceObject]
            editQueueLogger.warn("New queue after adding edit:", updatedQueue)
            return updatedQueue
        })
    }

    const handleInputFieldEdit = (
        insertedEditInstanceObject: EditInstance,
        resourcePropertyKey: ResourcePropertyKey,
        newResourceObject: ResourceObject,
    ) => {
        const existingEditsForSameResourceInQueue = apiQueue
            .slice()
            .filter((edit) => editIsForSameResourceName(edit, insertedEditInstanceObject))

        editQueueLogger.warn("Existing edits for same resource:", existingEditsForSameResourceInQueue)

        let sameFieldAlreadyInQueue = null
        const editIsForSamePropertyKey = (edit1: EditInstance, edit2: EditInstance) => edit1.resourcePropertyKey === edit2.resourcePropertyKey

        for (let i = existingEditsForSameResourceInQueue.length - 1; i >= 0; i -= 1) {
            const edit = existingEditsForSameResourceInQueue[i]
            if (editIsForSamePropertyKey(edit, insertedEditInstanceObject)) {
                sameFieldAlreadyInQueue = edit
                break
            }
        }

        editQueueLogger.warn("Same field already in queue:", sameFieldAlreadyInQueue)

        if (existingEditsForSameResourceInQueue.length > 0) {
            const latestEditInQueue = structuredClone(existingEditsForSameResourceInQueue[existingEditsForSameResourceInQueue.length - 1])
            const existingQueueItemsResourceObject = structuredClone(latestEditInQueue.newResourceObject)

            let combinedResourceObject = {} as ResourceObject

            const propertyKey = resourcePropertyKey as keyof ResourceObject
            existingQueueItemsResourceObject[propertyKey] = newResourceObject[propertyKey]
            combinedResourceObject = existingQueueItemsResourceObject

            editQueueLogger.warn("Combined resource object:", combinedResourceObject)

            if (sameFieldAlreadyInQueue) {
                editQueueLogger.warn("Updating existing edit in queue")
                const insertedEditInstanceWithCombinedResourceObject: EditInstance = {
                    ...insertedEditInstanceObject,
                    newResourceObject: combinedResourceObject,
                    previousResourceObject: latestEditInQueue.newResourceObject,
                    previousDisplayValue: sameFieldAlreadyInQueue.newDisplayValue,
                }
                editQueueLogger.warn("Updated edit instance:", insertedEditInstanceWithCombinedResourceObject)
                setApiQueue([...apiQueue, insertedEditInstanceWithCombinedResourceObject])
            } else {
                editQueueLogger.warn("Adding new edit with combined resource object")
                const insertedEditInstanceWithCombinedResourceObject: EditInstance = {
                    ...insertedEditInstanceObject,
                    newResourceObject: combinedResourceObject,
                    previousResourceObject: latestEditInQueue.newResourceObject,
                }
                editQueueLogger.warn("New edit instance:", insertedEditInstanceWithCombinedResourceObject)
                setApiQueue([...apiQueue, insertedEditInstanceWithCombinedResourceObject])
            }
        } else {
            editQueueLogger.warn("Adding new unique edit to queue")
            const updatedQueue = [...apiQueue, insertedEditInstanceObject]
            setApiQueue(updatedQueue)
            editQueueLogger.warn("Updated queue:", updatedQueue)
        }
    }

    const addEdit = async ({
        inputLabel,
        projectId,
        resourceName,
        resourcePropertyKey,
        resourceId,
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
        editQueueLogger.warn("Adding edit:", {
            inputLabel,
            resourceName,
            resourceId,
            resourcePropertyKey,
            newDisplayValue,
            previousDisplayValue,
            newResourceObject,
            previousResourceObject,
        })

        if (resourceName !== "case" && !resourceId) {
            editQueueLogger.error("Error: asset id is required for this service", null)
            return
        }

        if (_.isEqual(newResourceObject, previousResourceObject)) {
            editQueueLogger.warn("No changes detected, skipping edit")
            return
        }

        const insertedEditInstanceObject: EditInstance = {
            uuid: uuidv4(),
            timeStamp: new Date().getTime(),
            inputLabel,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
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

        editQueueLogger.warn("Created edit instance:", insertedEditInstanceObject)

        // TODO: we need a better way to determine if the edit is a table edit
        const isTableEdit = Object.prototype.hasOwnProperty.call(newResourceObject, "startYear")
            && Object.prototype.hasOwnProperty.call(newResourceObject, "values")

        editQueueLogger.warn("Edit type:", { isTableEdit })

        if (isTableEdit) {
            handleTableEdit(insertedEditInstanceObject)
        } else {
            handleInputFieldEdit(
                insertedEditInstanceObject,
                resourcePropertyKey,
                newResourceObject,
            )
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
        setIsSaving(true)
        try {
            const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex(
                (edit) => edit.uuid === getCurrentEditId(editIndexes, caseIdFromParams),
            )

            // Handle case where we're at the first edit
            if (currentEditIndex === 0) {
                const editThatWillBeUndone = caseEditsBelongingToCurrentCase[currentEditIndex]
                if (!editThatWillBeUndone) {
                    setIsSaving(false)
                    return
                }

                // Create a special empty edit to represent the initial state
                const initialStateEdit: EditInstance = {
                    uuid: uuidv4(),
                    timeStamp: new Date().getTime(),
                    inputLabel: editThatWillBeUndone.inputLabel,
                    projectId: editThatWillBeUndone.projectId,
                    resourceName: editThatWillBeUndone.resourceName,
                    resourcePropertyKey: editThatWillBeUndone.resourcePropertyKey,
                    resourceId: editThatWillBeUndone.resourceId,
                    wellId: editThatWillBeUndone.wellId,
                    drillingScheduleId: editThatWillBeUndone.drillingScheduleId,
                    caseId: editThatWillBeUndone.caseId,
                    newResourceObject: editThatWillBeUndone.previousResourceObject,
                    previousResourceObject: editThatWillBeUndone.previousResourceObject,
                    tabName: editThatWillBeUndone.tabName,
                    tableName: editThatWillBeUndone.tableName,
                    inputFieldId: editThatWillBeUndone.inputFieldId,
                }

                updateEditIndex(initialStateEdit.uuid)
                navigateToCaseTab(caseId!, editThatWillBeUndone.tabName ?? "")

                const rowWhereCellWillBeUndone = editThatWillBeUndone.tableName ?? editThatWillBeUndone.inputFieldId ?? editThatWillBeUndone.inputLabel
                if (!rowWhereCellWillBeUndone) {
                    console.error("rowWhereCellWillBeUndone is undefined")
                    setIsSaving(false)
                    return
                }

                const tabElement = document.getElementById(rowWhereCellWillBeUndone) as HTMLElement | null
                if (tabElement) {
                    tabElement.scrollIntoView({ behavior: "smooth", block: "center" })
                    if (editThatWillBeUndone.tableName) {
                        const tableCell = tabElement.querySelector(`[data-key="${editThatWillBeUndone.resourcePropertyKey}"]`) as HTMLElement | null
                        highlightElement(tableCell ?? tabElement)
                    } else {
                        highlightElement(tabElement)
                    }
                }

                await delay(500)
                const result = await submitToApi({
                    projectId: editThatWillBeUndone.projectId,
                    caseId: editThatWillBeUndone.caseId!,
                    resourceName: editThatWillBeUndone.resourceName,
                    resourceId: editThatWillBeUndone.resourceId,
                    resourceObject: editThatWillBeUndone.previousResourceObject as ResourceObject,
                    wellId: editThatWillBeUndone.wellId,
                    drillingScheduleId: editThatWillBeUndone.drillingScheduleId,
                })

                if (!result.success) {
                    throw new Error("Failed to undo edit")
                }
                return
            }

            // Handle normal case (more than one edit)
            if (currentEditIndex === -1) {
                setIsSaving(false)
                return
            }

            const editThatWillBeUndone = caseEditsBelongingToCurrentCase[currentEditIndex]
            const updatedEditIndex = currentEditIndex + 1

            // Check if we're trying to undo beyond the last edit
            if (updatedEditIndex >= caseEditsBelongingToCurrentCase.length) {
                setIsSaving(false)
                return
            }

            const updatedEdit = caseEditsBelongingToCurrentCase[updatedEditIndex]
            if (!editThatWillBeUndone || !updatedEdit) {
                setIsSaving(false)
                return
            }

            updateEditIndex(updatedEdit.uuid)
            navigateToCaseTab(caseId!, editThatWillBeUndone.tabName ?? "")

            const rowWhereCellWillBeUndone = editThatWillBeUndone.tableName ?? editThatWillBeUndone.inputFieldId ?? editThatWillBeUndone.inputLabel
            if (!rowWhereCellWillBeUndone) {
                console.error("rowWhereCellWillBeUndone is undefined")
                setIsSaving(false)
                return
            }

            const tabElement = document.getElementById(rowWhereCellWillBeUndone) as HTMLElement | null
            if (tabElement) {
                tabElement.scrollIntoView({ behavior: "smooth", block: "center" })
                if (editThatWillBeUndone.tableName) {
                    const tableCell = tabElement.querySelector(`[data-key="${editThatWillBeUndone.resourcePropertyKey}"]`) as HTMLElement | null
                    highlightElement(tableCell ?? tabElement)
                } else {
                    highlightElement(tabElement)
                }
            }

            await delay(500)
            const result = await submitToApi({
                projectId: editThatWillBeUndone.projectId,
                caseId: editThatWillBeUndone.caseId!,
                resourceName: editThatWillBeUndone.resourceName,
                resourceId: editThatWillBeUndone.resourceId,
                resourceObject: editThatWillBeUndone.previousResourceObject as ResourceObject,
                wellId: editThatWillBeUndone.wellId,
                drillingScheduleId: editThatWillBeUndone.drillingScheduleId,
            })

            if (!result.success) {
                throw new Error("Failed to undo edit")
            }
        } catch (error) {
            console.error("Error during undo:", error)
            // Revert the edit index if the API call failed
            const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex(
                (edit) => edit.uuid === getCurrentEditId(editIndexes, caseIdFromParams),
            )
            if (currentEditIndex > 0) {
                const previousEdit = caseEditsBelongingToCurrentCase[currentEditIndex - 1]
                updateEditIndex(previousEdit.uuid)
            }
        } finally {
            setIsSaving(false)
        }
    }

    const redoEdit = async () => {
        setIsSaving(true)
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex(
            (edit) => edit.uuid === getCurrentEditId(editIndexes, caseIdFromParams),
        )
        if (currentEditIndex <= 0) {
            const lastEdit = caseEditsBelongingToCurrentCase[caseEditsBelongingToCurrentCase.length - 1]
            if (lastEdit) {
                updateEditIndex(lastEdit.uuid)
                navigateToCaseTab(caseId!, lastEdit.tabName ?? "")

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
                            resourceName: lastEdit.resourceName,
                            resourceId: lastEdit.resourceId,
                            resourceObject: lastEdit.newResourceObject as ResourceObject,
                            wellId: lastEdit.wellId,
                            drillingScheduleId: lastEdit.drillingScheduleId,
                        }).then(() => {
                            setIsSaving(false)
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
                navigateToCaseTab(caseId!, updatedEdit.tabName ?? "")

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
                            resourceName: updatedEdit.resourceName,
                            resourceId: updatedEdit.resourceId,
                            resourceObject: updatedEdit.newResourceObject as ResourceObject,
                            wellId: updatedEdit.wellId,
                            drillingScheduleId: updatedEdit.drillingScheduleId,
                        }).then(() => {
                            setIsSaving(false)
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

export default useEditCase
