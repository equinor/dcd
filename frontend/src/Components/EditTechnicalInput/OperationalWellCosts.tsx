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
    title: string
}

function OperationalWellCosts({
    project, setProject, title,
}: Props) {
    const [wells, setWells] = useState<Well[]>(project?.wells ?? [])

    // eslint-disable-next-line max-len
    const isExplorationWell = (category: Components.Schemas.WellCategory | undefined) => [4, 5, 6].indexOf(category ?? -1) > -1

    return (
        <Table>
            <Table.Head>
                <Table.Row>
                    <Table.Cell>
                        {title}
                    </Table.Cell>
                    <Table.Cell>
                        Cost (MUSD)
                    </Table.Cell>
                </Table.Row>
            </Table.Head>
            <Table.Body>
                <p />
                {" "}

            </Table.Body>
        </Table>
    )
}

export default OperationalWellCosts
