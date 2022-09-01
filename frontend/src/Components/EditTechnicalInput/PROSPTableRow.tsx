/* eslint-disable max-len */
import { Checkbox, NativeSelect, Table } from "@equinor/eds-core-react"
import {
    ChangeEvent,
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import { Case } from "../../models/case/Case"
import { Project } from "../../models/Project"
import { DriveItem } from "../../models/sharepoint/DriveItem"
import Import from "../Import/Import"
import { ImportStatusEnum } from "./ImportStatusEnum"
import SharePointImport from "./SharePointImport"

interface Props {
    project: Project
    setProspCases: Dispatch<SetStateAction<SharePointImport[]>>
    prospCases: SharePointImport[]
    caseId: string
    driveItems: DriveItem[] | undefined
}

function PROSPTableRow({
    project, setProspCases, prospCases, caseId, driveItems,
}: Props) {
    const [caseItem, setCaseItem] = useState<Case>()
    const [prospCase, setProspCase] = useState<SharePointImport>()
    const [surf, setSurf] = useState<ImportStatusEnum>()
    const [topside, setTopside] = useState<ImportStatusEnum>()
    const [substructure, setSubstructure] = useState<ImportStatusEnum>()
    const [transport, setTransport] = useState<ImportStatusEnum>()
    const [sharePointFileId, setSharePointFileId] = useState<string>("")
    const [sharePointFileName, setSharePointFileName] = useState<string>("")
    const [selected, setSelected] = useState<boolean>()

    useEffect(() => {
        const selectedCase = project.cases.find((c) => c.id === caseId)
        setCaseItem(selectedCase)
        if (prospCases && prospCases.length > 0) {
            const selectedProspCase = prospCases.find((pc) => pc.id === caseId)
            setProspCase(selectedProspCase)
            setSurf(selectedProspCase?.surfState)
            setSubstructure(selectedProspCase?.substructureState)
            setTopside(selectedProspCase?.topsideState)
            setTransport(selectedProspCase?.transportState)
            setSelected(selectedProspCase?.selected)
            setSharePointFileId(selectedProspCase?.sharePointFileId!)
            setSharePointFileName(selectedProspCase?.sharePointFileName!)
        }
    }, [project, prospCases])

    const updateProspCases = (newProspCase: SharePointImport) => {
        // const newProspCases: PROSPCase[] = { ...prospCases }
        // newProspCases.find((pc) => pc.id === caseId)

        const newProspCases = prospCases.map((pc) => {
            if (pc.id === caseId) {
                return newProspCase
            }
            return pc
        })
        setProspCases(newProspCases)
        console.log("Prospcases: ", newProspCases)
    }

    useEffect(() => {
        const newProspCase: SharePointImport = { ...prospCase }
        newProspCase.id = prospCase?.id!
        newProspCase.selected = selected!
        newProspCase.surfState = surf!
        newProspCase.substructureState = substructure!
        newProspCase.topsideState = topside!
        newProspCase.transportState = transport!
        newProspCase.sharePointFileId = sharePointFileId
        newProspCase.sharePointFileName = sharePointFileName
        setProspCase(newProspCase)
        updateProspCases(newProspCase)
    }, [selected, surf, substructure, topside, transport, sharePointFileId, sharePointFileName])

    if (!prospCase) { return null }

    const checkBoxStatus = (status: ImportStatusEnum, changeStatus: Dispatch<SetStateAction<ImportStatusEnum | undefined>>) => {
        if (status === ImportStatusEnum.PROSP) { return <Checkbox disabled defaultChecked /> }
        if (status === ImportStatusEnum.Selected) { return <Checkbox checked onChange={() => changeStatus(ImportStatusEnum.NotSelected)} /> }
        return <Checkbox onChange={() => changeStatus(ImportStatusEnum.Selected)} />
    }

    if (!caseItem) { return null }

    const sharePointFileDropdownOptions = () => {
        if (!driveItems) { return null }
        const options: JSX.Element[] = []

        driveItems.forEach((item) => {
            options.push((<option key={item.id} value={item.id!}>{item.name}</option>))
        })
        return options
    }

    const onSharePointFileChange = (event: ChangeEvent<HTMLSelectElement>) => {
        setSharePointFileId(event.currentTarget.selectedOptions[0].value)
    }

    return (

        <Table.Row key={1}>
            <Table.Cell>
                Checkbox
            </Table.Cell>
            <Table.Cell>
                {caseItem.name}
            </Table.Cell>
            <Table.Cell>
                {checkBoxStatus(surf!, setSurf)}
            </Table.Cell>
            <Table.Cell>
                {checkBoxStatus(substructure!, setSubstructure)}
            </Table.Cell>
            <Table.Cell>
                {checkBoxStatus(topside!, setTopside)}
            </Table.Cell>
            <Table.Cell>
                {checkBoxStatus(transport!, setTransport)}
            </Table.Cell>
            <Table.Cell>
                <NativeSelect
                    id="sharePointFile"
                    label=""
                    value={prospCase.sharePointFileId}
                    onChange={onSharePointFileChange}
                >
                    {sharePointFileDropdownOptions()}
                </NativeSelect>
            </Table.Cell>
        </Table.Row>
    )
}
export default PROSPTableRow
