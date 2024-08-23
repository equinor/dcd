import React, { useEffect, useMemo, useState } from "react"
import { useQueryClient } from "react-query"
import { useParams } from "react-router-dom"
import { AgChartsReact } from "ag-charts-react"
import { useProjectContext } from "../../../../../Context/ProjectContext"
import { ITimeSeriesData, ProfileNames } from "../../../../../Models/Interfaces"
import { ITimeSeries } from "../../../../../Models/ITimeSeries"
import { mergeTimeseries } from "../../../../../Utils/common"

interface AggregatedTotalsProps {
    tableYears: [number, number];
    apiData: Components.Schemas.CaseWithAssetsDto;
    aggregatedGridRef: React.MutableRefObject<any>;
    alignedGridsRef: any[];
    barColors: string[];
    unit?: string;
    enableLegend?: boolean;
}

const setValueToCorrespondingYear = (profile: any, year: number, dg4Year: number) => {
    if (profile && profile.values) {
        const profileStartYear: number = Number(profile.startYear) + dg4Year
        const valueYearIndex = year - profileStartYear
        return profile.values[valueYearIndex] ?? 0
    }
    return 0
}

const AggregatedTotals: React.FC<AggregatedTotalsProps> = ({
    apiData,
    barColors,
    unit,
    enableLegend,
    tableYears,
    aggregatedGridRef,
    alignedGridsRef,
}) => {
    const { project } = useProjectContext()
    const { caseId } = useParams()
    const queryClient = useQueryClient()
    const projectId = project?.id || null
    const [aggregatedTimeSeriesData, setAggregatedTimeSeriesData] = useState<ITimeSeriesData[]>([])

    const aggregateProfiles = (profiles: any[], dg4Year: number): ITimeSeries => {
        const totals: { [key: number]: number } = {}
        profiles.forEach((profile) => {
            if (profile && Array.isArray(profile.values)) {
                const profileStartYear = dg4Year + profile.startYear
                for (let i = 0; i < profile.values.length; i += 1) {
                    const year = profileStartYear + i
                    if (!totals[year]) {
                        totals[year] = 0
                    }
                    totals[year] += Number(profile.values[i])
                }
            }
        })

        return {
            id: crypto.randomUUID(),
            startYear: Math.min(...Object.keys(totals).map(Number)) - dg4Year,
            values: Object.values(totals),
        }
    }

    useEffect(() => {
        if (apiData) {
            const dg4Year = new Date(apiData.case.dG4Date).getFullYear()

            const profiles = {
                studyProfiles: [
                    apiData.totalFeasibilityAndConceptStudiesOverride || apiData.totalFeasibilityAndConceptStudies,
                    apiData.totalFEEDStudiesOverride || apiData.totalFEEDStudies,
                    apiData.totalOtherStudiesCostProfile,
                ],
                opexProfiles: [
                    apiData.wellInterventionCostProfileOverride || apiData.wellInterventionCostProfile,
                    apiData.offshoreFacilitiesOperationsCostProfileOverride || apiData.offshoreFacilitiesOperationsCostProfile,
                    apiData.onshoreRelatedOPEXCostProfile,
                    apiData.additionalOPEXCostProfile,
                    apiData.historicCostCostProfile,
                ],
                cessationProfiles: [
                    apiData.cessationWellsCostOverride || apiData.cessationWellsCost,
                    apiData.cessationOffshoreFacilitiesCostOverride || apiData.cessationOffshoreFacilitiesCost,
                    apiData.cessationOnshoreFacilitiesCostProfile,
                ],
                offshoreFacilityProfiles: [
                    apiData.surfCostProfileOverride || apiData.surfCostProfile,
                    apiData.topsideCostProfileOverride || apiData.topsideCostProfile,
                    apiData.substructureCostProfileOverride || apiData.substructureCostProfile,
                    apiData.transportCostProfileOverride || apiData.transportCostProfile,
                ],
                developmentWellCostProfiles: [
                    apiData.oilProducerCostProfileOverride || apiData.oilProducerCostProfile,
                    apiData.gasProducerCostProfileOverride || apiData.gasProducerCostProfile,
                    apiData.waterInjectorCostProfileOverride || apiData.waterInjectorCostProfile,
                    apiData.gasInjectorCostProfileOverride || apiData.gasInjectorCostProfile,
                ],
                explorationWellCostProfiles: [
                    apiData.gAndGAdminCostOverride || apiData.gAndGAdminCost,
                    apiData.seismicAcquisitionAndProcessing,
                    apiData.countryOfficeCost,
                    apiData.explorationWellCostProfile,
                    apiData.appraisalWellCostProfile,
                    apiData.sidetrackCostProfile,
                ],
            }

            const newTimeSeriesData: ITimeSeriesData[] = []

            Object.entries(profiles).forEach(([profileName, profileData]) => {
                const aggregatedProfile = aggregateProfiles(profileData, dg4Year)
                const resourceName: ProfileNames = profileName as ProfileNames

                newTimeSeriesData.push({
                    profileName: profileName.replace(/Profiles$/, "").replace(/([A-Z])/g, " $1").trim(),
                    unit: project?.currency === 1 ? "MNOK" : "MUSD",
                    profile: aggregatedProfile,
                    resourceName,
                    resourceId: apiData.case.id,
                    resourceProfileId: aggregatedProfile.id,
                    resourcePropertyKey: profileName,
                    overridable: false,
                    editable: false,
                })
            })

            setAggregatedTimeSeriesData(newTimeSeriesData)
        }
    }, [apiData, tableYears, project])

    const calculateIncome = () => {
        const oilPrice = 75.0 // USD
        const gasPrice = 0.3531 // USD
        const cubicMetersToBarrelsFactor = 6.29
        const million = 1e6

        // Merge oil production profiles
        const totalOilProduction = mergeTimeseries(apiData.productionProfileOil, apiData.additionalProductionProfileOil)

        // Ensure values are defined, fallback to an empty array if undefined
        const oilProductionInMillionsOfBarrels = (totalOilProduction.values || []).map((v) => (v * cubicMetersToBarrelsFactor))

        // Calculate oil income
        const oilIncome = {
            id: crypto.randomUUID(),
            startYear: totalOilProduction.startYear + new Date(apiData.case.dG4Date).getFullYear(),
            values: oilProductionInMillionsOfBarrels.map((v) => v * oilPrice),
        }

        // Merge gas production profiles
        const totalGasProduction = mergeTimeseries(apiData.productionProfileGas, apiData.additionalProductionProfileGas)

        // Ensure values are defined, fallback to an empty array if undefined
        const gasIncome = {
            id: crypto.randomUUID(),
            startYear: totalGasProduction.startYear + new Date(apiData.case.dG4Date).getFullYear(),
            values: (totalGasProduction.values || []).map((v) => (v * gasPrice)),
        }

        // Merge oil and gas income to get total income
        const totalIncome = mergeTimeseries(oilIncome, gasIncome)
        console.log("totalIncome", totalIncome)
        return totalIncome
    }

    const chartData = useMemo(() => {
        const data: number[] = []
        const dg4Year = new Date(apiData.case.dG4Date).getFullYear()
        const years = Array.from({ length: tableYears[1] - tableYears[0] + 1 }, (_, i) => tableYears[0] + i)

        // Calculate total income data
        const totalIncomeData = calculateIncome()

        let cumulativeSum = 0
        years.forEach((year) => {
            const yearData: any = { year }
            aggregatedTimeSeriesData.forEach((series) => {
                const value = setValueToCorrespondingYear(series.profile, year, dg4Year)
                yearData[series.profileName] = value
                cumulativeSum += value
            })
            // Instead of cumulative sum of aggregated data, set cumulative sum to total income
            yearData.cumulativeSum = (totalIncomeData.values || []).reduce((acc, value, index) => {
                if (totalIncomeData.startYear + index === year) {
                    return acc + value
                }
                return acc
            }, 0)
            data.push(yearData)
        })

        return data
    }, [aggregatedTimeSeriesData, tableYears, apiData])

    const figmaTheme = {
        palette: {
            fills: barColors,
            strokes: ["black"],
        },
        overrides: {
            cartesian: {
                title: {
                    fontSize: 24,
                },
            },
            column: { axes: { category: { label: { rotation: -20 } } } },
        },
    }

    const barChartOptions: object = {
        data: chartData,
        title: {
            text: "Annual Cost Profile", // Updated bar chart title
            fontSize: 24, // Title font size, if required
        },
        subtitle: { text: unit },
        padding: {
            top: 10,
            right: 10,
            bottom: 10,
            left: 10,
        },
        theme: figmaTheme,
        series: [
            ...Object.keys(chartData[0] || {}).filter((key) => key !== "year" && key !== "cumulativeSum").map((key, index) => ({
                type: "column",
                xKey: "year",
                yKey: key,
                name: key,
                fill: barColors[index],
                stacked: true,
            })),
            {
                type: "line",
                xKey: "year",
                yKey: "cumulativeSum",
                name: "Total Income", // Updated line name
                yName: "Total Income", // Updated y-axis label
                stroke: "red",
                strokeWidth: 2,
                marker: {
                    enabled: true,
                    shape: "circle",
                    size: 5,
                    fill: "red",
                },
            },
        ],
        axes: [
            {
                type: "category",
                position: "bottom",
                title: { text: "Year" },
            },
            {
                type: "number",
                position: "left",
                title: { text: `Cost (${unit})` },
            },
            {
                type: "number",
                position: "right",
                title: { text: "Total Income" }, // Updated y-axis title
                keys: ["cumulativeSum"],
                visibleRange: [0, 1],
            },
        ],
        legend: { enabled: enableLegend, position: "bottom", spacing: 40 },
    }

    // Prepare pie chart data
    const pieChartData = useMemo(() => {
        const pieData: any[] = aggregatedTimeSeriesData.map((series) => ({
            profile: series.profileName,
            value: series.profile?.values?.reduce((sum, value) => sum + value, 0) ?? 0, // Sum values for the pie chart
        }))

        return pieData
    }, [aggregatedTimeSeriesData])

    const totalValue = pieChartData.reduce((acc, curr) => acc + curr.value, 0) // Calculate the total sum of all values

    const pieChartOptions: object = {
        data: pieChartData,
        title: {
            text: "Cost Distribution", // Updated bar chart title
            fontSize: 24, // Title font size, if required
        },
        subtitle: { text: unit ?? "" },
        padding: {
            top: 10,
            right: 10,
            bottom: 10,
            left: 10,
        },
        theme: figmaTheme,
        series: [
            {
                type: "pie",
                calloutLabelKey: "profile",
                angleKey: "value",
                calloutLabel: { enabled: false },
                innerRadiusOffset: -25,
                strokes: ["white"],
                innerLabels: [
                    {
                        text: `${totalValue.toFixed(2)}`, // Display the total sum in the middle of the pie chart
                        fontSize: 18,
                        color: "#000000",
                        margin: 0,
                    },
                    {
                        text: unit ?? "",
                        fontSize: 14,
                        color: "#B4B4B4",
                        margin: 4,
                    },
                ],
                highlightStyle: {
                    item: {
                        fill: undefined,
                        stroke: undefined,
                        strokeWidth: 1,
                    },
                    series: {
                        enabled: true,
                        dimOpacity: 0.2,
                        strokeWidth: 2,
                    },
                },
            },
        ],
        legend: { enabled: enableLegend, position: "bottom", spacing: 40 },
    }

    if (!apiData) {
        return <p>Loading...</p>
    }

    return (
        <div style={{ display: "flex", justifyContent: "space-between", gap: "20px" }}>
            <div style={{ flex: "1 1 67%" }}>
                <AgChartsReact options={barChartOptions} />
            </div>
            <div style={{ flex: "1 1 33%" }}>
                <AgChartsReact options={pieChartOptions} />
            </div>
        </div>
    )
}

export default AggregatedTotals
