/* eslint-disable max-len */
import {
    Input, NativeSelect, Table,
} from "@equinor/eds-core-react"
import {
    ChangeEvent,
    Dispatch, SetStateAction, useState,
} from "react"
import { Well } from "../../models/Well"
import { Project } from "../../models/Project"
import { GetWellService } from "../../Services/WellService"

interface Props {
    wellId: string
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
    explorationWell: boolean
}

function WellTableRowEditProject({
    wellId, project, setProject, explorationWell,
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

    if (!well) return null

    console.log("From table row: ", explorationWell)

    return (

        <Table.Row key={well.id}>
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
                    {!explorationWell ? (
                        <>
                            <option key="0" value={0}>Oil producer</option>
                            <option key="1" value={1}>Gas producer</option>
                            <option key="2" value={2}>Water injector</option>
                            <option key="3" value={3}>Gas Injector</option>
                        </>
                    )
                        : (
                            <>
                                <option key="4" value={4}>Exploration well</option>
                                <option key="5" value={5}>Appraisal well</option>
                                <option key="6" value={6}>Sidetrack</option>
                            </>
                        )}
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
export default WellTableRowEditProject
