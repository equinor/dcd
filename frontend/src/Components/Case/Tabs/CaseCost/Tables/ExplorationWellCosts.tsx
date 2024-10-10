import React, { useEffect, useState } from "react"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"

import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { projectQueryFn } from "@/Services/QueryFunctions"
import CaseTabTable from "../../../Components/CaseTabTable"

interface ExplorationWellCostsProps {
    tableYears: [number, number];
    explorationWellsGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    apiData: Components.Schemas.CaseWithAssetsDto;
    addEdit: any;
}

const ExplorationWellCosts: React.FC<ExplorationWellCostsProps> = ({
    tableYears,
    explorationWellsGridRef,
    alignedGridsRef,
    apiData,
    addEdit,
}) => {
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const [explorationTimeSeriesData, setExplorationTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

    useEffect(() => {
        const exploration = apiData?.exploration
        const gAndGAdminCostData = apiData.gAndGAdminCost
        const explorationGAndGAdminCostOverrideData = apiData.gAndGAdminCostOverride

        const seismicAcquisitionAndProcessingData = apiData.seismicAcquisitionAndProcessing
        const countryOfficeCostData = apiData.countryOfficeCost
        const explorationWellCostProfileData = apiData.explorationWellCostProfile
        const appraisalWellCostProfileData = apiData.appraisalWellCostProfile
        const sidetrackCostProfileData = apiData.sidetrackCostProfile

        if (!exploration) {
            console.error("No exploration data")
            return
        }

        const newExplorationTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "G&G and admin",
                unit: `${projectData?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: gAndGAdminCostData,
                resourceName: "gAndGAdminCost",
                resourceId: exploration.id,
                resourceProfileId: explorationGAndGAdminCostOverrideData?.id,
                resourcePropertyKey: "gAndGAdminCost",
                editable: true,
                overridable: true,
                overrideProfile: explorationGAndGAdminCostOverrideData,
            },
            {
                profileName: "Seismic acquisition and processing",
                unit: `${projectData?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: seismicAcquisitionAndProcessingData,
                resourceName: "seismicAcquisitionAndProcessing",
                resourceId: exploration.id,
                resourceProfileId: seismicAcquisitionAndProcessingData?.id,
                resourcePropertyKey: "seismicAcquisitionAndProcessing",
                editable: true,
                overridable: false,
            },
            {
                profileName: "Country office",
                unit: `${projectData?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: countryOfficeCostData,
                resourceName: "countryOfficeCost",
                resourceId: exploration.id,
                resourceProfileId: countryOfficeCostData?.id,
                resourcePropertyKey: "countryOfficeCost",
                editable: true,
                overridable: false,
            },
            {
                profileName: "Exploration well",
                unit: `${projectData?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: explorationWellCostProfileData,
                resourceName: "explorationWellCostProfile",
                resourceId: exploration.id,
                resourceProfileId: explorationWellCostProfileData?.id,
                resourcePropertyKey: "explorationWellCostProfile",
                editable: false,
                overridable: false,
            },
            {
                profileName: "Appraisal well",
                unit: `${projectData?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: appraisalWellCostProfileData,
                resourceName: "appraisalWellCostProfile",
                resourceId: exploration.id,
                resourceProfileId: appraisalWellCostProfileData?.id,
                resourcePropertyKey: "appraisalWellCostProfile",
                editable: false,
                overridable: false,
            },
            {
                profileName: "Sidetrack well",
                unit: `${projectData?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: sidetrackCostProfileData,
                resourceName: "sidetrackCostProfile",
                resourceId: exploration.id,
                resourceProfileId: sidetrackCostProfileData?.id,
                resourcePropertyKey: "sidetrackCostProfile",
                editable: false,
                overridable: false,
            },
        ]

        setExplorationTimeSeriesData(newExplorationTimeSeriesData)
    }, [apiData, projectData])

    return (
        <CaseTabTable
            timeSeriesData={explorationTimeSeriesData}
            dg4Year={apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Exploration well cost"
            gridRef={explorationWellsGridRef}
            alignedGridsRef={alignedGridsRef}
            includeFooter
            totalRowName="Total"
            addEdit={addEdit}
        />
    )
}

export default ExplorationWellCosts
