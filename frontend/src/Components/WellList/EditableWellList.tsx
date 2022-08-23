/* eslint-disable max-len */
import { Button, Table, Typography } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import { WellProjectWell } from "../../models/WellProjectWell"
import { GetWellService } from "../../Services/WellService"
import { WellProject } from "../../models/assets/wellproject/WellProject"
import WellTableRow from "./EditableWellTableRow"
import { Exploration } from "../../models/assets/exploration/Exploration"
import { ExplorationWell } from "../../models/ExplorationWell"

interface Props {
    project: Project
    wellProject?: WellProject
    exploration?: Exploration
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

function CaseWellList({
    project, wellProject, exploration, setProject,
}: Props) {
    const [wells, setWells] = useState<Well[]>(project?.wells ?? [])
    const [wellProjectWells, setWellProjectWells] = useState<WellProjectWell[]>(wellProject?.wellProjectWells ?? [])
    const [explorationWells, setExplorationWells] = useState<ExplorationWell[]>(exploration?.explorationWells ?? [])

    useEffect(() => {
        if (wellProject?.wellProjectWells) {
            setWellProjectWells(wellProject.wellProjectWells)
        } else if (exploration?.explorationWells) {
            setExplorationWells(exploration.explorationWells)
        }
    }, [wellProject?.wellProjectWells, exploration?.explorationWells, project])

    const CreateWell = async () => {
        const newWell = new Well()
        newWell.wellCategory = wellProject ? 0 : 4
        newWell.name = "New well"
        newWell.projectId = project.projectId
        const newProject = await (await GetWellService()).createWell(newWell)
        setProject(newProject)
        setWells(newProject?.wells ?? [])
    }

    const isExplorationWell = (category: Components.Schemas.WellCategory | undefined) => [4, 5, 6].indexOf(category ?? -1) > -1

    const GenerateWellTableRows = () => {
        const tableRows: JSX.Element[] = []
        if (wellProject) {
            wells?.filter((w) => !isExplorationWell(w.wellCategory)).forEach((w) => {
                const wpw = wellProjectWells?.find((x) => x.wellId === w.id && x.wellProjectId === wellProject.id)

                tableRows.push((
                    <WellTableRow key={w.id} setProject={setProject} wellId={w.id!} project={project} wellProject={wellProject} wellProjectWell={wpw} />
                ))
            })
        } else if (exploration) {
            wells?.filter((w) => isExplorationWell(w.wellCategory)).forEach((w) => {
                const ew = explorationWells?.find((x) => x.wellId === w.id && x.explorationId === exploration.id)

                tableRows.push((
                    <WellTableRow key={w.id} setProject={setProject} wellId={w.id!} project={project} explorationWell={ew} exploration={exploration} />
                ))
            })
        }
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

export default CaseWellList
