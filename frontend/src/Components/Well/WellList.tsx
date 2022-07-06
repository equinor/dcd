/* eslint-disable max-len */
import { Button, Table, Typography } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import { Project } from "../../models/Project"
import WellTableRows from "./WellTableRows"
import { Well } from "../../models/Well"
import { WellProjectWell } from "../../models/WellProjectWell"
import { GetWellService } from "../../Services/WellService"
import { WellProject } from "../../models/assets/wellproject/WellProject"
import WellTableRow from "./WellTableRow"

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

    const GenerateWellTableRows = () => {
        const tableRows: JSX.Element[] = []
        wells?.forEach((w) => {
            const wpw = wellProjectWells?.find((x) => x.wellId === w.id && x.wellProjectId === wellProject.id)
            tableRows.push((
                <WellTableRow setProject={setProject} well={w} wellProject={wellProject} wellProjectWell={wpw} />
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
                    {/* <WellTableRows wellProjectWells={wellProjectWells} wells={wells!} wellProject={wellProject} setProject={setProject} /> */}
                </Table.Body>
            </Table>
        </>
    )
}

export default WellList
