import { Button, Icon, Typography } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import { useStyles } from "@equinor/fusion-react-ag-grid-styles"
import Grid from "@mui/material/Grid2"
import React, { useEffect, useState } from "react"
import styled from "styled-components"

import WellsTable from "@/Components/Tables/ProjectTables/WellsTable"
import { useDataFetch } from "@/Hooks"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import useTechnicalInputEdits from "@/Hooks/useEditTechnicalInput"
import { TableWell } from "@/Models/Interfaces"
import { WellCategory } from "@/Models/enums"
import { useAppStore } from "@/Store/AppStore"

const ButtonWrapper = styled.div`
    display: flex;
    justify-content: flex-end;
    margin-bottom: 16px;
`

const Header = styled(Grid)`
    margin-bottom: 44px;
    gap: 16px;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
`

interface WellsProps {
    title: string
    addButtonText: string
    defaultWellCategory: WellCategory
    wellOptions: Array<{ key: string; value: WellCategory; label: string }>
    filterWells: (well: Components.Schemas.WellOverviewDto) => boolean
    isExplorationWellTable: boolean
}

const Wells: React.FC<WellsProps> = ({
    title,
    addButtonText,
    defaultWellCategory,
    wellOptions,
    filterWells,
    isExplorationWellTable,
}) => {
    const revisionAndProjectData = useDataFetch()
    const { addWellsEdit } = useTechnicalInputEdits()
    const { editMode } = useAppStore()
    const { canEdit, isEditDisabled } = useCanUserEdit()
    const styles = useStyles()

    const [rowData, setRowData] = useState<TableWell[]>([])
    const [wellStagedForDeletion, setWellStagedForDeletion] = useState<any>()

    const wellsToRowData = (wells: Components.Schemas.WellOverviewDto[]) => {
        if (!wells) {
            return
        }

        const tableWells: TableWell[] = wells.map((w) => ({
            id: w.id,
            name: w.name ?? "",
            wellCategory: w.wellCategory || defaultWellCategory,
            drillingDays: w.drillingDays ?? 0,
            wellCost: w.wellCost ?? 0,
            wellInterventionCost: w.wellInterventionCost ?? 0,
            plugingAndAbandonmentCost: w.plugingAndAbandonmentCost ?? 0,
            well: w,
            wells,
        }))

        setRowData(tableWells)
    }

    const createWell = async (category: WellCategory) => {
        if (!revisionAndProjectData) {
            return
        }

        const newWell: Components.Schemas.CreateWellDto = {
            wellCategory: category,
            name: "New well",
            wellInterventionCost: 0,
            plugingAndAbandonmentCost: 0,
            wellCost: 0,
            drillingDays: 0,
        }

        const createWells: Components.Schemas.UpdateWellsDto = {
            createWellDtos: [newWell],
            updateWellDtos: [],
            deleteWellDtos: [],
        }

        addWellsEdit(revisionAndProjectData.projectId, revisionAndProjectData.commonProjectAndRevisionData.fusionProjectId, createWells)
    }

    useEffect(() => {
        const allWells = revisionAndProjectData?.commonProjectAndRevisionData.wells ?? []
        const filteredWells = allWells.filter(filterWells)

        if (filteredWells?.length) {
            wellsToRowData(filteredWells)
        } else {
            setRowData([])
        }
    }, [revisionAndProjectData, filterWells])

    return (
        <>
            <Header>
                <Typography variant="h3">{title}</Typography>
                <Typography variant="body_long">
                    This input is used to calculate each case&apos;s well costs based on their drilling schedules.
                </Typography>
            </Header>
            {canEdit() && (
                <ButtonWrapper>
                    <Button onClick={() => createWell(defaultWellCategory)} variant="outlined">
                        <Icon data={add} size={24} />
                        {addButtonText}
                    </Button>
                </ButtonWrapper>
            )}
            <Grid container spacing={1}>
                <Grid size={12} className={styles.root}>
                    <WellsTable
                        rowData={rowData}
                        editMode={editMode}
                        isEditDisabled={isEditDisabled}
                        isExplorationWellTable={isExplorationWellTable}
                        wellOptions={wellOptions}
                        revisionAndProjectData={revisionAndProjectData && revisionAndProjectData}
                        addWellsEdit={addWellsEdit}
                        defaultWellCategory={defaultWellCategory}
                        wellStagedForDeletion={wellStagedForDeletion}
                        setWellStagedForDeletion={setWellStagedForDeletion}
                    />
                </Grid>
            </Grid>
        </>
    )
}

export default Wells
