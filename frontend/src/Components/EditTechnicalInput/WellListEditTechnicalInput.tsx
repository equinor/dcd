import { Button, Table } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useState,
} from "react"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import { GetWellService } from "../../Services/WellService"
import WellTableRowEditTechnicalInput from "./WellTableRowEditTechnicalInput"

interface Props {
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
    explorationWells: boolean
}

function WellListEditTechnicalInput({
    project, setProject, explorationWells,
}: Props) {
    const [wells, setWells] = useState<Well[]>(project?.wells ?? [])

    const CreateWell = async () => {
        const newWell = new Well()
        newWell.wellCategory = !explorationWells ? 0 : 4
        newWell.name = "New well"
        newWell.projectId = project.projectId
        const newProject = await (await GetWellService()).createWell(newWell)
        setProject(newProject)
        setWells(newProject?.wells ?? [])
    }

    // eslint-disable-next-line max-len
    const isExplorationWell = (category: Components.Schemas.WellCategory | undefined) => [4, 5, 6].indexOf(category ?? -1) > -1

    const GenerateWellTableRows = () => {
        const tableRows: JSX.Element[] = []
        if (!explorationWells) {
            wells?.filter((w) => !isExplorationWell(w.wellCategory)).forEach((w) => {
                tableRows.push((
                    <WellTableRowEditTechnicalInput
                        key={w.id}
                        setProject={setProject}
                        wellId={w.id!}
                        project={project}
                        explorationWell={isExplorationWell(w.wellCategory)}
                    />
                ))
            })
        } else {
            wells?.filter((w) => isExplorationWell(w.wellCategory)).forEach((w) => {
                tableRows.push((
                    <WellTableRowEditTechnicalInput
                        key={w.id}
                        setProject={setProject}
                        wellId={w.id!}
                        project={project}
                        explorationWell={isExplorationWell(w.wellCategory)}
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
