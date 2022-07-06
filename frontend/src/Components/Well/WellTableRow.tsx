import {
    Button, Input, Label, NativeSelect, Table,
} from "@equinor/eds-core-react"
import {
    ChangeEvent,
    Dispatch, ReactElement, SetStateAction, useState,
} from "react"
import { Well } from "../../models/Well"
import { WellProjectWell } from "../../models/WellProjectWell"
import { GetWellProjectWellService } from "../../Services/WellProjectWellService"
import { WellProject } from "../../models/assets/wellproject/WellProject"
import { Project } from "../../models/Project"
import { GetWellService } from "../../Services/WellService"

interface Props {
    well: Well
    wellProjectWell: WellProjectWell | undefined
    wellProject: WellProject
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

function WellTableRow({
    well, wellProjectWell, wellProject, setProject,
}: Props) {
    const [wellName, setWellName] = useState<string>(well.name ?? "")

    const UpdateWellName = async () => {
        if (well.name !== wellName) {
            const newWell = { ...well }
            newWell.name = wellName
            const newProject = await GetWellService().updateWell(newWell)
            setProject(newProject)
        }
        console.log("Yeet")
    }

    const onNameChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.value !== wellName && event.target.value !== "") {
            setWellName(event.target.value)
        }
    }

    const ChangeWellCase = async (w: Well, wpw: WellProjectWell | undefined, increase: boolean) => {
        if (!wpw) {
            const newWellCase = new WellProjectWell()
            newWellCase.wellId = w.id
            newWellCase.wellProjectId = wellProject.id
            newWellCase.count = 1
            const newProject = await GetWellProjectWellService().createWellProjectWell(newWellCase)
            setProject(newProject)
        } else {
            const newWellCase = { ...wpw }
            newWellCase.count! = increase ? wpw.count! + 1 : wpw.count! - 1
            const newProject = await GetWellProjectWellService().updateWellProjectWell(newWellCase)
            setProject(newProject)
        }
    }

    return (

        <Table.Row key={well.id}>
            <Table.Cell>
                {wellProjectWell?.count ?? 0}
                <Button onClick={() => ChangeWellCase(well, wellProjectWell, true)}>Increase</Button>
                <Button onClick={() => ChangeWellCase(well, wellProjectWell, false)}>Decrease</Button>
                {console.log("WellProjectWell.WellId: ", wellProjectWell?.wellId)}
                {wellProjectWell?.wellId}
            </Table.Cell>
            <Table.Cell>
                <Input
                    id="textfield-normal"
                    placeholder="Placeholder text"
                    autoComplete="off"
                    value={wellName ?? ""}
                    onBlur={UpdateWellName}
                    onChange={onNameChange}
                />
                {well.name}
            </Table.Cell>
            <Table.Cell>
                {well.wellCategory}
                <NativeSelect id="wellCategory" label="Well category" value={well.wellCategory}>
                    <option key="0" value={0}>Oil producer</option>
                    <option key="1" value={1}>Gas producer</option>
                    <option key="2" value={2}>Water injector</option>
                    <option key="3" value={3}>Gas Injector</option>
                    <option key="4" value={4}>Exploration well</option>
                    <option key="5" value={5}>Appraisal well</option>
                    <option key="6" value={6}>Sidetrack</option>
                    <option key="7" value={7}>Rig mob/demob</option>
                </NativeSelect>
            </Table.Cell>
            <Table.Cell>
                {well.drillingDays}
            </Table.Cell>
            <Table.Cell>
                {well.wellCost}
            </Table.Cell>
        </Table.Row>
    )
}
export default WellTableRow
