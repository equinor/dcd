import React, { useEffect, useState } from "react"
import { default as Grid } from "@mui/material/Grid2"
import { Button, Icon, Typography } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"

import { TableWell } from "@/Models/Wells"
import { useDataFetch } from "@/Hooks/useDataFetch"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useAppContext } from "@/Context/AppContext"
import useTechnicalInputEdits from "@/Hooks/useEditTechnicalInput"
import { SectionHeader } from "./SharedWellStyles"
import WellsTable from "@/Components/Tables/ProjectTables/WellsTable"
import { useStyles } from "@equinor/fusion-react-ag-grid-styles"

interface WellsProps {
  title: string
  addButtonText: string
  defaultWellCategory: number
  wellOptions: Array<{ key: string; value: number; label: string }>
  filterWells: (well: Components.Schemas.WellOverviewDto) => boolean
}

const Wells: React.FC<WellsProps> = ({
  title,
  addButtonText,
  defaultWellCategory,
  wellOptions,
  filterWells,
}) => {
  const revisionAndProjectData = useDataFetch()
  const { addWellsEdit } = useTechnicalInputEdits()
  const { editMode } = useAppContext()
  const { isEditDisabled } = useEditDisabled()
  const styles = useStyles()

  const [rowData, setRowData] = useState<TableWell[]>([])
  const [wellStagedForDeletion, setWellStagedForDeletion] = useState<any>()


  const wellsToRowData = (wells: Components.Schemas.WellOverviewDto[]) => {
    if (!wells) return

    const tableWells: TableWell[] = wells.map((w) => {
      return {
        id: w.id!,
        name: w.name ?? "",
        wellCategory:
          w.wellCategory ||
          (defaultWellCategory as Components.Schemas.WellCategory),
        drillingDays: w.drillingDays ?? 0,
        wellCost: w.wellCost ?? 0,
        well: w,
        wells,
      }
    })

    setRowData(tableWells)
  }

  const createWell = async (category: number) => {
    if (!revisionAndProjectData) return

    const newWell: Components.Schemas.CreateWellDto = {
      wellCategory: category as Components.Schemas.WellCategory,
      name: "New well",
    }

    const createWells: Components.Schemas.UpdateWellsDto = {
      createWellDtos: [newWell],
      updateWellDtos: [],
      deleteWellDtos: [],
    }
    addWellsEdit(revisionAndProjectData.projectId, createWells)
  }

  useEffect(() => {
    const allWells =
      revisionAndProjectData?.commonProjectAndRevisionData.wells ?? []
    const filteredWells = allWells.filter(filterWells)

    if (filteredWells?.length) {
      wellsToRowData(filteredWells)
    } else {
      setRowData([])
    }
  }, [revisionAndProjectData, filterWells])

  return (
    <>
      <SectionHeader>
        <Typography variant="h2">{title}</Typography>
        {editMode && (
          <Button onClick={() => createWell(defaultWellCategory)} variant="outlined">
            <Icon data={add} size={24} />
            {addButtonText}
          </Button>
        )}
      </SectionHeader>

      <Grid container spacing={1}>
        <Grid size={12} className={styles.root}>
          <WellsTable
            rowData={rowData}
            editMode={editMode}
            isEditDisabled={isEditDisabled}
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