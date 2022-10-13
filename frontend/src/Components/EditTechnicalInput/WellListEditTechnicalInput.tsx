import { Button, Table } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useState,
} from "react"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import { GetWellService } from "../../Services/WellService"
import { IsExplorationWell } from "../../Utils/common"
import WellTableRowEditTechnicalInput from "./WellTableRowEditTechnicalInput"

interface Props {
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
    wells: Well[] | undefined
    setWells: Dispatch<SetStateAction<Well[] | undefined>>
    explorationWells: boolean
}

function WellListEditTechnicalInput({
    project, setProject, explorationWells, wells, setWells,
}: Props) {
    // const [wells, setWells] = useState<Well[]>(project?.wells ?? [])

    const CreateWell = async () => {
        const newWell = new Well()
        newWell.wellCategory = !explorationWells ? 0 : 4
        newWell.name = "New well"
        newWell.projectId = project.projectId
        const newProject = await (await GetWellService()).createWell(newWell)
        setProject(newProject)
        setWells(newProject?.wells ?? [])
    }

    const GenerateWellTableRows = () => {
        const tableRows: JSX.Element[] = []
        if (!explorationWells) {
            wells?.filter((w) => !IsExplorationWell(w)).forEach((w) => {
                tableRows.push((
                    <WellTableRowEditTechnicalInput
                        key={w.id}
                        setProject={setProject}
                        wellId={w.id!}
                        project={project}
                        explorationWell={IsExplorationWell(w)}
                    />
                ))
            })
        } else {
            wells?.filter((w) => IsExplorationWell(w)).forEach((w) => {
                tableRows.push((
                    <WellTableRowEditTechnicalInput
                        key={w.id}
                        setProject={setProject}
                        wellId={w.id!}
                        project={project}
                        explorationWell={IsExplorationWell(w)}
                    />
                ))
            })
        }

        return tableRows
    }

    return (
        <>
            <Table>
                <Table.Head>
                    <Table.Row>
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
            <Button onClick={CreateWell} variant="outlined">
                {explorationWells
            ? "Add new exploration well type" : "Add new development/drilling well type"}

            </Button>
        </>
    )
}

export default WellListEditTechnicalInput
