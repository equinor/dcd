import { ColDef } from "@ag-grid-community/core"
import { AgGridReact } from "@ag-grid-community/react"
import { Typography } from "@equinor/eds-core-react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { useQuery } from "@tanstack/react-query"
import { useMemo, useState } from "react"
import { styled } from "styled-components"

import ProjectSkeleton from "../LoadingSkeletons/ProjectSkeleton"

import { useDataFetch } from "@/Hooks/useDataFetch"
import { ChangeLogCategory } from "@/Models/enums"
import { changeLogQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Store/ProjectContext"
import { formatDateAndTime } from "@/Utils/DateUtils"
import { getCustomContextMenuItems } from "@/Utils/TableUtils"

interface IRow {
    entityDescription: string | null;
    entityName: string;
    propertyName: string | null;
    fieldName: string;
    oldValue: string | null;
    newValue: string | null;
    username: string | null;
    timestampUtc: string;
    entityState: string;
    category: string;
}

const ChangeLogWrapper = styled.div`
    height: calc(100vh - 250px);
    padding: 40px;
`

const ProjectChangeLog = () => {
    useStyles()
    const { projectId } = useProjectContext()
    const revisionAndProjectData = useDataFetch()

    const { data: projectChangeLogData } = useQuery({
        queryKey: ["projectChangeLogData", projectId],
        queryFn: () => changeLogQueryFn(projectId),
        enabled: !!projectId,
    })

    const data = (projectChangeLogData || [])
        .map((row) => ({
            entityDescription: row.entityDescription,
            entityName: row.entityName,
            propertyName: row.propertyName,
            fieldName: row.propertyName ? `${row.entityName}.${row.propertyName}` : row.entityName,
            oldValue: row.oldValue,
            newValue: row.newValue,
            username: row.username,
            timestampUtc: row.timestampUtc,
            entityState: row.entityState,
            category: ChangeLogCategory[row.category],
        }))

    const rowData = useMemo<IRow[]>(() => data, [data])

    const [colDefs] = useState<ColDef<IRow>[]>([
        {
            field: "category",
            headerName: "Area",
            maxWidth: 200,
        },
        {
            field: "entityDescription",
            headerName: "Entity name",
            maxWidth: 210,
        },
        {
            field: "fieldName",
            headerName: "Field name",
            maxWidth: 420,
        },
        {
            field: "oldValue",
            headerName: "Old value",
        },
        {
            field: "newValue",
            headerName: "New value",
        },
        {
            field: "username",
            headerName: "Changed by",
            maxWidth: 180,
        },
        {
            field: "timestampUtc",
            headerName: "Changed at",
            width: 100,
            valueFormatter: (params) => formatDateAndTime(params.value),
            maxWidth: 155,
        },
        {
            field: "entityState",
            headerName: "Change type",
            maxWidth: 155,
        },
    ])

    const defaultColDef: ColDef = {
        flex: 1,
        sortable: true,
        filter: true,
    }

    if (!projectChangeLogData || !revisionAndProjectData) {
        return <ProjectSkeleton />
    }

    return (
        <ChangeLogWrapper>
            <div>
                <Typography variant="h3">
                    Project changelog
                </Typography>
            </div>
            <div
                style={{ height: "100%", marginTop: "40px" }}
            >
                <AgGridReact
                    rowData={rowData}
                    columnDefs={colDefs}
                    defaultColDef={defaultColDef}
                    getContextMenuItems={getCustomContextMenuItems}
                />
            </div>
        </ChangeLogWrapper>
    )
}

export default ProjectChangeLog
