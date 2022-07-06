/* eslint-disable max-len */
import { Button, Table, Typography } from "@equinor/eds-core-react"
import {
 Dispatch, SetStateAction, useEffect, useState,
} from "react"
import { Project } from "../../models/Project"
import WellTableRow from "./WellTableRow"
import { Well } from "../../models/Well"
import { WellProjectWell } from "../../models/WellProjectWell"
import { GetWellService } from "../../Services/WellService"
import { WellProject } from "../../models/assets/wellproject/WellProject"

interface Props {
    project: Project
    wellProject: WellProject
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

function WellList({ project, wellProject, setProject }: Props) {
    const [wells, setWells] = useState<Well[]>(project?.wells ?? [])
    const [wellProjectWells, setWellProjectWells] = useState<WellProjectWell[]>(wellProject?.wellProjectWells ?? [])

    useEffect(() => {
        if (wellProject.wellProjectWells) {
            setWellProjectWells(wellProject.wellProjectWells)
        }
    }, [wellProject.wellProjectWells, project])

    const CreateWell = async () => {
        const newWell = new Well()
        newWell.wellCategory = 0
        newWell.name = "New well"
        newWell.projectId = project.projectId
        const newProject = await GetWellService().createWell(newWell)
        setProject(newProject)
        setWells(newProject?.wells ?? [])
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
                    <WellTableRow wellProjectWells={wellProjectWells} wells={wells!} wellProject={wellProject} setProject={setProject} />
                </Table.Body>
            </Table>
        </>
    )
}

export default WellList
