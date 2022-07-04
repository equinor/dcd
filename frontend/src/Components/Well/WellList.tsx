/* eslint-disable max-len */
import { tokens } from "@equinor/eds-tokens"
import { Button, Table, Typography } from "@equinor/eds-core-react"
import { well } from "@equinor/eds-icons"
import { useState } from "react"
import { Project } from "../../models/Project"
import WellTableRow from "./WellTableRow"
import { Well } from "../../models/Well"
import { WellProjectWell } from "../../models/WellProjectWell"
import { Case } from "../../models/Case"
import { GetWellService } from "../../Services/WellService"
import { WellProject } from "../../models/assets/wellproject/WellProject"

interface Props {
    project: Project
    wellProject: WellProject
}

function WellList({ project, wellProject }: Props) {
    const [wells, setWells] = useState<Well[]>(project?.wells ?? [])
    const [wellProjectWells, setWellCases] = useState<WellProjectWell[]>(wellProject?.wellProjectWells ?? [])

    console.log("WellList WellProject: ", wellProject.wellProjectWells)

    const CreateWell = () => {
        const newWell = new Well()
        newWell.category = 0
        newWell.name = "New well"
        newWell.projectId = project.projectId
        GetWellService().createWell(newWell)
        const existingWells = [...wells]
        if (existingWells !== undefined && existingWells !== null) {
            existingWells.push(newWell)
            setWells(existingWells)
        }

        console.log(wells?.length)
    }

    if (!wellProjectWells) return null

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
                    <WellTableRow wellProjectWells={wellProject?.wellProjectWells ?? []} wells={wells!} wellProject={wellProject} />
                </Table.Body>
            </Table>
        </>
    )
}

export default WellList
