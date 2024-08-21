import React, { useEffect, useState } from "react"
import { useQueryClient } from "react-query"
import { useParams } from "react-router-dom"
import CaseTabTable from "../../../Components/CaseTabTable"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { ITimeSeriesData, ProfileNames } from "../../../../../Models/Interfaces"
import { ITimeSeries } from "../../../../../Models/ITimeSeries"

interface AggregatedTotalsTableProps {
    tableYears: [number, number];
    addEdit: any;
    apiData: Components.Schemas.CaseWithAssetsDto;
    studyGridRef: React.MutableRefObject<any>;
}

const AggregatedTotalsTable: React.FC<AggregatedTotalsTableProps> = ({
    tableYears,
    addEdit,
    apiData,
    studyGridRef,
}) => {
    const { project } = useProjectContext()
    const { caseId } = useParams()
    const queryClient = useQueryClient()
    const projectId = project?.id || null
    const [aggregatedTimeSeriesData, setAggregatedTimeSeriesData] = useState<ITimeSeriesData[]>([])

    // Helper function to aggregate profiles and create ITimeSeries
    const aggregateProfiles = (profiles: any[], initialTotals: { [key: number]: number }): ITimeSeries => {
        const totals = { ...initialTotals }
        const dg4Year = new Date(apiData.case.dG4Date).getFullYear()

        profiles.forEach((profile) => {
            if (profile && Array.isArray(profile.values)) {
                console.log("Processing Profile:", profile)
                const { values } = profile
                let firstYear = dg4Year + profile.startYear

                values.forEach((value: any) => {
                    if (totals[firstYear] !== undefined) {
                        totals[firstYear] += Number(value)
                    }
                    firstYear += 1
                })
            } else {
                console.log("Profile is missing values or is undefined.")
            }
        })

        const aggregatedProfile: ITimeSeries = {
            id: crypto.randomUUID(),
            startYear: Math.min(...Object.keys(totals).map(Number)),
            values: Object.values(totals),
        }

        return aggregatedProfile
    }

    useEffect(() => {
        if (apiData) {
            const yearRange = Array.from(
                { length: tableYears[1] - tableYears[0] + 1 },
                (_, i) => tableYears[0] + i,
            )

            const profiles = {
                studyProfiles: [
                    apiData.totalFeasibilityAndConceptStudies,
                    apiData.totalFeasibilityAndConceptStudiesOverride,
                    apiData.totalFEEDStudies,
                    apiData.totalFEEDStudiesOverride,
                    apiData.totalOtherStudiesCostProfile,
                ],
                opexProfiles: [
                    apiData.historicCostCostProfile,
                    apiData.wellInterventionCostProfile,
                    apiData.offshoreFacilitiesOperationsCostProfile,
                    apiData.offshoreFacilitiesOperationsCostProfileOverride,
                    apiData.onshoreRelatedOPEXCostProfile,
                    apiData.additionalOPEXCostProfile,
                ],
                cessationProfiles: [
                    apiData.cessationWellsCost,
                    apiData.cessationWellsCostOverride,
                    apiData.cessationOffshoreFacilitiesCost,
                    apiData.cessationOffshoreFacilitiesCostOverride,
                    apiData.cessationOnshoreFacilitiesCostProfile,
                ],
                offshoreFacilityProfiles: [
                    apiData.surf,
                    apiData.topside,
                    apiData.transport,
                    apiData.substructure,
                    apiData.surfCostProfile,
                    apiData.surfCostProfileOverride,
                    apiData.topsideCostProfile,
                    apiData.topsideCostProfileOverride,
                    apiData.substructureCostProfile,
                    apiData.substructureCostProfileOverride,
                    apiData.transportCostProfile,
                    apiData.transportCostProfileOverride,
                ],
            }

            const newTimeSeriesData: ITimeSeriesData[] = []
            const totals: { [key: number]: number } = {}
            yearRange.forEach((year) => {
                totals[year] = 0
            })

            Object.entries(profiles).forEach(([profileName, profileData]) => {
                const aggregatedProfile = aggregateProfiles(profileData, totals)

                const resourceName: ProfileNames = profileName as ProfileNames // Cast profileName to ProfileNames type

                newTimeSeriesData.push({
                    profileName: profileName.replace(/Profiles$/, "").replace(/([A-Z])/g, " $1").trim(),
                    unit: project?.currency === 1 ? "MNOK" : "MUSD",
                    profile: aggregatedProfile,
                    resourceName,
                    resourceId: caseId || "",
                    resourcePropertyKey: profileName,
                    overridable: false,
                    editable: false,
                })
            })

            console.log("Final TimeSeries Data:", newTimeSeriesData)
            setAggregatedTimeSeriesData(newTimeSeriesData)
        }
    }, [apiData, tableYears])

    if (!apiData) {
        return <p>Loading...</p>
    }

    return (
        <CaseTabTable
            timeSeriesData={aggregatedTimeSeriesData}
            tableYears={tableYears}
            tableName="Aggregated Totals"
            totalRowName="Aggregated Total"
            addEdit={addEdit}
            dg4Year={apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030}
            includeFooter={false}
            gridRef={studyGridRef}
        />
    )
}

export default AggregatedTotalsTable
