import {
    Button, Input, NativeSelect, Table,
} from "@equinor/eds-core-react"
import {
    ChangeEvent,
    Dispatch, SetStateAction, useState,
} from "react"
import { Well } from "../../models/Well"
import { WellProjectWell } from "../../models/WellProjectWell"
import { GetWellProjectWellService } from "../../Services/WellProjectWellService"
import { WellProject } from "../../models/assets/wellproject/WellProject"
import { Project } from "../../models/Project"
import { GetWellService } from "../../Services/WellService"

interface Props {
    wellId: string
    project: Project
    wellProjectWell: WellProjectWell | undefined
    wellProject: WellProject
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

function WellTableRow({
    wellId, project, wellProjectWell, wellProject, setProject,
}: Props) {
    const [well, setWell] = useState<Well | undefined>(project.wells?.find((w) => w.id === wellId))
    const [wellName, setWellName] = useState<string>(well?.name ?? "")
    const [drillingDays, setDrillingDays] = useState<number>(well?.drillingDays ?? 0)
    const [wellCost, setWellCost] = useState<number>(well?.wellCost ?? 0)
    const [wellCategory, setWellCategory] = useState<Components.Schemas.WellCategory>(well?.wellCategory ?? 0)

    const updateWell = async () => {
        if (well && (wellName !== well.name || drillingDays !== well.drillingDays
            || wellCost !== well.wellCost || wellCategory !== well.wellCategory)) {
            const newWell = { ...well }
            newWell.name = wellName
            newWell.drillingDays = drillingDays
            newWell.wellCost = wellCost
            newWell.wellCategory = wellCategory
            const newProject = await (await GetWellService()).updateWell(newWell)
            setProject(newProject)
            const updatedWell = newProject.wells?.find((w) => w.id === wellId)
            if (updatedWell) {
                setWell(updatedWell)
            }
        }
    }

    const onNameChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.value !== wellName && event.target.value !== "") {
            setWellName(event.target.value)
        }
    }

    const onWellCategoryChange = (event: ChangeEvent<HTMLSelectElement>) => {
        switch (event.currentTarget.selectedOptions[0].value) {
        case "0":
            setWellCategory(0)
            break
        case "1":
            setWellCategory(1)
            break
        case "2":
            setWellCategory(2)
            break
        case "3":
            setWellCategory(3)
            break
        case "4":
            setWellCategory(4)
            break
        case "5":
            setWellCategory(5)
            break
        case "6":
            setWellCategory(6)
            break
        case "7":
            setWellCategory(7)
            break
        default:
            setWellCategory(0)
            break
        }
    }

    const changeWellCase = async (w: Well, wpw: WellProjectWell | undefined, increase: boolean) => {
        if (!wpw) {
            const newWellCase = new WellProjectWell()
            newWellCase.wellId = w.id
            newWellCase.wellProjectId = wellProject.id
            newWellCase.count = 1
            const newProject = await (await GetWellProjectWellService()).createWellProjectWell(newWellCase)
            setProject(newProject)
        } else {
            const newWellCase = { ...wpw }
            newWellCase.count! = increase ? wpw.count! + 1 : wpw.count! - 1
            const newProject = await (await GetWellProjectWellService()).updateWellProjectWell(newWellCase)
            setProject(newProject)
        }
    }

    if (!well) return null

    return (

        <Table.Row key={well.id}>
            <Table.Cell>
                {wellProjectWell?.count ?? 0}
                <Button onClick={() => changeWellCase(well, wellProjectWell, true)}>Increase</Button>
                <Button onClick={() => changeWellCase(well, wellProjectWell, false)}>Decrease</Button>
            </Table.Cell>
            <Table.Cell>
                <Input
                    id="textfield-normal"
                    placeholder="Placeholder text"
                    autoComplete="off"
                    value={wellName ?? ""}
                    onBlur={updateWell}
                    onChange={onNameChange}
                />
            </Table.Cell>
            <Table.Cell>
                <NativeSelect
                    id="wellCategory"
                    label="Well category"
                    value={wellCategory}
                    onChange={onWellCategoryChange}
                    onBlur={updateWell}
                >
                    <option key="0" value={0}>Oil producer</option>
                    <option key="1" value={1}>Gas producer</option>
                    <option key="2" value={2}>Water injector</option>
                    <option key="3" value={3}>Gas Injector</option>
                    {/* <option key="4" value={4}>Exploration well</option>
                    <option key="5" value={5}>Appraisal well</option>
                    <option key="6" value={6}>Sidetrack</option> */}
                    <option key="7" value={7}>Rig mob/demob</option>
                </NativeSelect>
            </Table.Cell>
            <Table.Cell>
                <Input
                    id="DrillingDays"
                    type="number"
                    value={drillingDays}
                    onChange={(e: ChangeEvent<HTMLInputElement>) => setDrillingDays(Number(e.target.value))}
                    onBlur={updateWell}
                />
            </Table.Cell>
            <Table.Cell>
                <Input
                    id="WellCost"
                    type="number"
                    value={wellCost}
                    onChange={(e: ChangeEvent<HTMLInputElement>) => setWellCost(Number(e.target.value))}
                    onBlur={updateWell}
                />
            </Table.Cell>
        </Table.Row>
    )
}
export default WellTableRow
