import { Button, Table } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import { Project } from "../../models/Project"
import PROSPTableRow from "./PROSPTableRow"
import SharePointImport from "./SharePointImport"
import { GetProspService } from "../../Services/ProspService"
import { DriveItem } from "../../models/sharepoint/DriveItem"

interface Props {
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

function PROSPCaseList({
    project, setProject,
}: Props) {
    const [prospCases, setProspCases] = useState<SharePointImport[]>([])
    const [driveItems, setDriveItems] = useState<DriveItem[]>()

    useEffect(() => {
        (async () => {
            const newDriveItems = await (await GetProspService()).getSharePointFileNamesAndId()
            setDriveItems(newDriveItems)
            const prosp: SharePointImport[] = []

            project.cases.forEach((caseItem) => {
                const prospCase = new SharePointImport(caseItem, project)
                prosp.push(prospCase)
            })
            setProspCases(prosp)
        })()
    }, [project])

    const GenerateWellTableRows = () => {
        const tableRows: JSX.Element[] = []
        project.cases.forEach((caseItem) => {
            tableRows.push((
                <PROSPTableRow
                    key={caseItem.id!}
                    project={project}
                    prospCases={prospCases}
                    setProspCases={setProspCases}
                    caseId={caseItem.id!}
                    driveItems={driveItems}
                />
            ))
        })

        return tableRows
    }

    const save = async () => {
        const cases = prospCases.map((pc) => SharePointImport.toDto(pc))
        const newProject = await (await GetProspService()).importFromSharepoint(project.id!, cases)
        setProject(newProject)
    }

    return (
        <>
            <Table>
                <Table.Head>
                    <Table.Row>
                        <Table.Cell>
                            Checkbox
                        </Table.Cell>
                        <Table.Cell>
                            Case name
                        </Table.Cell>
                        <Table.Cell>
                            SURF
                        </Table.Cell>
                        <Table.Cell>
                            Substructure
                        </Table.Cell>
                        <Table.Cell>
                            Topside
                        </Table.Cell>
                        <Table.Cell>
                            Transport
                        </Table.Cell>
                        <Table.Cell>
                            Sharepoint file name
                        </Table.Cell>
                    </Table.Row>
                </Table.Head>
                <Table.Body>
                    {GenerateWellTableRows()}
                </Table.Body>
            </Table>
            <Button
                onClick={save}
            >
                Save
            </Button>

        </>
    )
}

export default PROSPCaseList
