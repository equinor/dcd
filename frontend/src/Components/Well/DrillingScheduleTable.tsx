/* eslint-disable max-len */
import { Typography } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import DataTable, { CellValue } from "../DataTable/DataTable"
import {
    buildGridData, buildZeroGridData, getColumnAbsoluteYears, replaceOldData,
} from "../DataTable/helpers"
import Import from "../Import/Import"
import { ITimeSeries } from "../../models/ITimeSeries"
import {
    DeleteButton, ImportButton, Wrapper, WrapperColumn,
} from "../../Views/Asset/StyledAssetComponents"
import { WellProjectWell } from "../../models/WellProjectWell"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import { GetWellProjectWellService } from "../../Services/WellProjectWellService"

interface Props {
    wellCase1: WellProjectWell,
    setWellCases: Dispatch<SetStateAction<WellProjectWell[] | null | undefined>>,
    wellCases: WellProjectWell[] | null | undefined
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
    well: Well
}

const DrillingScheduleTable = ({
    wellCase1,
    setWellCases,
    wellCases,
    setProject,
    project,
    well,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [dialogOpen, setDialogOpen] = useState(false)
    const [wellProjectWell, setWellCase] = useState<WellProjectWell>(wellCase1)

    const buildAlignedGrid = (updatedTimeSeries: ITimeSeries) => {
        if (updatedTimeSeries !== undefined) {
            const columnTitles: string[] = ["2022", "2023", "2024"]
            // for (let i = updatedTimeSeries.startYear!;
            //     i < updatedTimeSeries.startYear!
            //     + updatedTimeSeries.values!.length; i += 1) {
            //         columnTitles.push(i.toString())
            // }
            setColumns(columnTitles)
            const newGridData = buildGridData(wellProjectWell.drillingSchedule)
            setGridData(newGridData)
        } else {
            setColumns([])
            setGridData([[]])
        }
    }

    useEffect(() => {
        buildAlignedGrid(wellProjectWell!.drillingSchedule!)
    }, [wellProjectWell, wellCases])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData: CellValue[][] = replaceOldData(gridData, changes)
        setGridData(newGridData)
    }

    const onImport = async (input: string, year: number) => {
        const newDrillingSchedule: ITimeSeries = { ...wellProjectWell.drillingSchedule }
        newDrillingSchedule.startYear = year
        newDrillingSchedule.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i)) // parseInt
        buildAlignedGrid(newDrillingSchedule)
        setDialogOpen(!dialogOpen)
        const newWellProjectWell: WellProjectWell = { ...wellProjectWell }
        newWellProjectWell.drillingSchedule = newDrillingSchedule
        console.log("newWellCase: ", newWellProjectWell)
        setWellCase(newWellProjectWell)
        const newProject = await GetWellProjectWellService().updateWellProjectWell(newWellProjectWell)
        setProject(newProject)
        // if (Array.isArray(wellCases)) {
        //     // const newWellCases = [...wellCases]
        //     const newWellCases = wellCases.map((wc) => (wc.wellId === wellCase.wellId
        //         ? { ...wc, drillingSchedule: newDrillingSchedule }
        //         : wc))
        //         setWellCases(newWellCases)
        // }
    }

    const deleteTimeseries = async () => {
        const newWellProjectWell: WellProjectWell = { ...wellProjectWell }
        newWellProjectWell.drillingSchedule = undefined
        const newProject = await GetWellProjectWellService().updateWellProjectWell(newWellProjectWell)
        setColumns([])
        setGridData([[]])
    }

    return (
        <>
            <Wrapper>
                <Typography variant="h4">
                    {well.name}
                </Typography>
            </Wrapper>
            <Wrapper>
                <ImportButton
                    onClick={() => { setDialogOpen(true) }}
                >
                    {wellProjectWell !== undefined ? "Edit" : "Import"}
                </ImportButton>
                <DeleteButton
                    disabled={wellProjectWell === undefined}
                    color="danger"
                    onClick={deleteTimeseries}
                >
                    Delete
                </DeleteButton>
            </Wrapper>

            <WrapperColumn>
                <DataTable
                    columns={columns}
                    gridData={gridData}
                    onCellsChanged={onCellsChanged}
                    dG4Year="2022"
                />
            </WrapperColumn>
            {!dialogOpen ? null
                : <Import onClose={() => { setDialogOpen(!dialogOpen) }} onImport={onImport} />}
        </>
    )
}

export default DrillingScheduleTable
