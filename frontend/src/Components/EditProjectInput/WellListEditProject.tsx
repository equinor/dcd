/* eslint-disable max-len */
import { Button, Table, Typography } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useState,
} from "react"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import { GetWellService } from "../../Services/WellService"
import WellTableRowEditProject from "./WellTableRowEditProject"

interface Props {
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

function WellListEditProject({
    project, setProject,
}: Props) {
    const [wells, setWells] = useState<Well[]>(project?.wells ?? [])

    const CreateWell = async () => {
        const newWell = new Well()
        newWell.wellCategory = true ? 0 : 4
        newWell.name = "New well"
        newWell.projectId = project.projectId
        const newProject = await (await GetWellService()).createWell(newWell)
        setProject(newProject)
        setWells(newProject?.wells ?? [])
    }

    const isExplorationWell = (category: Components.Schemas.WellCategory | undefined) => [4, 5, 6].indexOf(category ?? -1) > -1

    const GenerateWellTableRows = () => {
        const tableRows: JSX.Element[] = []
            wells?.forEach((w) => {
                tableRows.push((
                    <WellTableRowEditProject key={w.id} setProject={setProject} wellId={w.id!} project={project} />
                ))
            })

        return tableRows
    }

    return (
        <>
            <Button onClick={CreateWell} variant="outlined">Add new well</Button>
            <Table>
                <Table.Caption>
                    <Typography variant="h2">
                        Wells
                    </Typography>
                </Table.Caption>
                <Table.Head>
                    <Table.Row>
                        <Table.Cell>
                            Count
                        </Table.Cell>
                        <Table.Cell>
                            Well name
                        </Table.Cell>
                        <Table.Cell>
                            Well type
                        </Table.Cell>
                        <Table.Cell>
                            Drilling days
                        </Table.Cell>
                        <Table.Cell>
                            Well cost
                        </Table.Cell>
                    </Table.Row>
                </Table.Head>
                <Table.Body>
                    {GenerateWellTableRows()}
                </Table.Body>
            </Table>
        </>
    )
}

export default WellListEditProject
