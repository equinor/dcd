import { useQuery } from "@tanstack/react-query"
import { AgGridReact } from "@ag-grid-community/react"
import { useMemo, useState } from "react"
import { ColDef } from "@ag-grid-community/core"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { styled } from "styled-components"
import { Typography } from "@equinor/eds-core-react"
import { useProjectContext } from "@/Store/ProjectContext"
import { changeLogQueryFn } from "@/Services/QueryFunctions"
import ProjectSkeleton from "../LoadingSkeletons/ProjectSkeleton"
import { useDataFetch } from "@/Hooks/useDataFetch"

interface IRow {
    entityDescription: string | null;
    entityName: string;
    propertyName: string;
    oldValue: string | null;
    newValue: string | null;
    username: string;
    timestampUtc: string;
    entityState: string;
}

const ChangeLogWrapper = styled.div`
    height: calc(100vh - 230px);
    padding: 40px;
`

const ChangeLogView: React.FC = () => {
    useStyles()
    const { projectId } = useProjectContext()
    const revisionAndProjectData = useDataFetch()

    const { data: projectChangeLogData } = useQuery({
        queryKey: ["projectChangeLogData", projectId],
        queryFn: () => changeLogQueryFn(projectId),
        enabled: !!projectId,
    })

    const data = projectChangeLogData as IRow[]

    const rowData = useMemo<IRow[]>(() => data, [data])

    const [colDefs] = useState<ColDef<IRow>[]>(
        [
            { field: "entityDescription" },
            { field: "entityName" },
            { field: "propertyName" },
            { field: "oldValue" },
            { field: "newValue" },
            { field: "username" },
            { field: "timestampUtc" },
            { field: "entityState" },
        ],
    )
    const defaultColDef: ColDef = { flex: 1 }

    if (!projectChangeLogData || !revisionAndProjectData) {
        return <ProjectSkeleton />
    }

    return (
        <ChangeLogWrapper>
            <div>
                <Typography variant="h3">
                    Project change log
                </Typography>
            </div>
            <div
                style={{ height: "100%", marginTop: "20px" }}
            >
                <AgGridReact rowData={rowData} columnDefs={colDefs} defaultColDef={defaultColDef} />
            </div>
        </ChangeLogWrapper>
    )
}

export default ChangeLogView
