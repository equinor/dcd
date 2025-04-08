import Grid from "@mui/material/Grid2"
import { AgCharts } from "ag-charts-react"
import React, { useEffect, useMemo, useState } from "react"

import { setValueToCorrespondingYear } from "@/Components/Charts/TimeSeriesChart"
import { useDataFetch } from "@/Hooks"
import { ITimeSeries, ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"
import {
    formatCurrencyUnit,
    formatProfileName,
    formatNumberForView,
    roundToDecimals,
} from "@/Utils/FormatingUtils"
import { mergeTimeseries, mergeTimeseriesList } from "@/Utils/TableUtils"

interface AggregatedTotalsProps {
    tableYears: [number, number];
    apiData: Components.Schemas.CaseWithAssetsDto;
    barColors: string[];
    enableLegend?: boolean;
}

const AggregatedTotals: React.FC<AggregatedTotalsProps> = ({
    apiData,
    barColors,
    enableLegend,
    tableYears,
}) => {
    const revisionAndProjectData = useDataFetch()
    const [aggregatedTimeSeriesData, setAggregatedTimeSeriesData] = useState<ITimeSeriesTableData[]>([])
    const [mergedCostProfiles, setMergedCostProfiles] = useState<any>({})

    useEffect(() => {
        if (apiData) {
            const costProfiles = {
                studyProfiles: mergeTimeseriesList([
                    (apiData.totalFeasibilityAndConceptStudiesOverride?.override ? apiData.totalFeasibilityAndConceptStudiesOverride : apiData.totalFeasibilityAndConceptStudies),
                    (apiData.totalFeedStudiesOverride?.override ? apiData.totalFeedStudiesOverride : apiData.totalFeedStudies),
                    apiData.totalOtherStudiesCostProfile,
                ]),
                opexProfiles: mergeTimeseriesList([
                    apiData.historicCostCostProfile,
                    (apiData.wellInterventionCostProfileOverride?.override ? apiData.wellInterventionCostProfileOverride : apiData.wellInterventionCostProfile),
                    (apiData.offshoreFacilitiesOperationsCostProfileOverride?.override ? apiData.offshoreFacilitiesOperationsCostProfileOverride : apiData.offshoreFacilitiesOperationsCostProfile),
                    apiData.onshoreRelatedOpexCostProfile,
                    apiData.additionalOpexCostProfile,
                ]),
                cessationProfiles: mergeTimeseriesList([
                    (apiData.cessationWellsCostOverride?.override ? apiData.cessationWellsCostOverride : apiData.cessationWellsCost),
                    (apiData.cessationOffshoreFacilitiesCostOverride?.override ? apiData.cessationOffshoreFacilitiesCostOverride : apiData.cessationOffshoreFacilitiesCost),
                    apiData.cessationOnshoreFacilitiesCostProfile,
                ]),
                offshoreFacilityProfiles: mergeTimeseriesList([
                    (apiData.surfCostProfileOverride?.override ? apiData.surfCostProfileOverride : apiData.surfCostProfile),
                    (apiData.topsideCostProfileOverride?.override ? apiData.topsideCostProfileOverride : apiData.topsideCostProfile),
                    (apiData.substructureCostProfileOverride?.override ? apiData.substructureCostProfileOverride : apiData.substructureCostProfile),
                    (apiData.transportCostProfileOverride?.override ? apiData.transportCostProfileOverride : apiData.transportCostProfile),
                    (apiData.onshorePowerSupplyCostProfileOverride?.override ? apiData.onshorePowerSupplyCostProfileOverride : apiData.onshorePowerSupplyCostProfile),
                ]),
                developmentWellCostProfiles: mergeTimeseriesList([
                    (apiData.oilProducerCostProfileOverride?.override ? apiData.oilProducerCostProfileOverride : apiData.oilProducerCostProfile),
                    (apiData.gasProducerCostProfileOverride?.override ? apiData.gasProducerCostProfileOverride : apiData.gasProducerCostProfile),
                    (apiData.waterInjectorCostProfileOverride?.override ? apiData.waterInjectorCostProfileOverride : apiData.waterInjectorCostProfile),
                    (apiData.gasInjectorCostProfileOverride?.override ? apiData.gasInjectorCostProfileOverride : apiData.gasInjectorCostProfile),
                ]),
                explorationWellCostProfiles: mergeTimeseriesList([
                    apiData.seismicAcquisitionAndProcessing,
                    apiData.countryOfficeCost,
                    (apiData.gAndGAdminCostOverride?.override ? apiData.gAndGAdminCostOverride : apiData.gAndGAdminCost),
                    apiData.projectSpecificDrillingCostProfile,
                    apiData.explorationWellCostProfile,
                    apiData.appraisalWellCostProfile,
                    apiData.sidetrackCostProfile,
                ]),
            }

            setMergedCostProfiles(costProfiles)
            const incomeProfiles = {
                liquidRevenue: [
                    {
                        startYear: apiData.calculatedTotalOilIncomeCostProfile?.startYear ?? 0,
                        values: apiData.calculatedTotalOilIncomeCostProfile?.values ?? [],
                    },

                ],
                gasRevenue: [
                    {
                        startYear: apiData.calculatedTotalGasIncomeCostProfile?.startYear ?? 0,
                        values: apiData.calculatedTotalGasIncomeCostProfile?.values ?? [],
                    },
                ],
            }

            const newTimeSeriesData: ITimeSeriesTableData[] = []

            Object.entries(costProfiles).forEach(([profileName, profileData]) => {
                const updatedProfile = {
                    ...profileData,
                    values: profileData.values?.map((value: number) => -Math.abs(value)) || [],
                }
                const resourceName: ProfileTypes = profileName as ProfileTypes

                newTimeSeriesData.push({
                    profileName: formatProfileName(profileName),
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: updatedProfile,
                    resourceName,
                    resourceId: apiData.case.caseId,
                    resourcePropertyKey: profileName,
                    overridable: false,
                    editable: false,
                })
            })

            Object.entries(incomeProfiles).forEach(([profileName, profileData]) => {
                const resourceName: ProfileTypes = profileName as ProfileTypes

                newTimeSeriesData.push({
                    profileName: formatProfileName(profileName),
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: {
                        startYear: profileData[0]?.startYear ?? 0,
                        values: profileData.flatMap((p) => p.values ?? []),
                    },
                    resourceName,
                    resourceId: apiData.case.caseId,
                    resourcePropertyKey: profileName,
                    overridable: false,
                    editable: false,
                })
            })

            setAggregatedTimeSeriesData(newTimeSeriesData)
        }
    }, [apiData, tableYears, revisionAndProjectData])

    const dg4Year = getYearFromDateString(apiData.case.dg4Date)
    const cashflowProfile = apiData.calculatedTotalCashflow
    const discountedCashflowData = apiData.calculatedDiscountedCashflowService

    const cashFlow: ITimeSeries = {
        startYear: cashflowProfile?.startYear ?? 0,
        values: cashflowProfile?.values ?? [],
    }

    const discountedCashflow = {
        startYear: discountedCashflowData?.startYear ?? 0,
        values: discountedCashflowData?.values ?? [],
    }
    const chartData = useMemo(() => {
        const data: any[] = []
        const years = Array.from({ length: tableYears[1] - tableYears[0] + 1 }, (_, i) => tableYears[0] + i)

        years.forEach((year) => {
            const yearData: any = { year }

            aggregatedTimeSeriesData.forEach((series) => {
                const value = setValueToCorrespondingYear(series.profile, year, dg4Year)

                yearData[series.profileName] = value
            })
            yearData.cashflow = setValueToCorrespondingYear(cashFlow, year, dg4Year)
            yearData.discountedCashflow = setValueToCorrespondingYear(discountedCashflow, year, dg4Year)

            data.push(yearData)
        })

        return data
    }, [aggregatedTimeSeriesData, tableYears, apiData, dg4Year])

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
            bar: { axes: { category: { label: { rotation: -20 } } } },
        },
    }

    const axesConfig = {
        type: "category",
        position: "bottom",
        title: { text: "Year" },
        gridLine: {
            style: [
                {
                    stroke: "rgba(0, 0, 0, 0.2)",
                    lineDash: [3, 2],
                },
                {
                    stroke: "rgba(0, 0, 0, 0.2)",
                    lineDash: [3, 2],
                },
            ],
        },
        label: {
            formatter: (label: any) => Math.floor(Number(label.value)),
        },
    }

    const barChartOptions: object = {
        data: chartData,
        title: {
            text: "Cashflow",
            fontSize: 24,
        },

        padding: {
            top: 10,
            right: 10,
            bottom: 10,
            left: 10,
        },
        theme: figmaTheme,
        series: [
            ...Object.keys(chartData[0] || {}).filter((key) => key !== "year" && key !== "cashflow" && key !== "discountedCashflow").map((key, index) => ({
                type: "bar",
                xKey: "year",
                yKey: key,
                yName: key,
                fill: barColors[index],
                stacked: true,
            })),
            {
                type: "line",
                xKey: "year",
                yKey: "cashflow",
                yName: "Cashflow",
                stroke: "red",
                strokeWidth: 2,
                marker: {
                    enabled: false,
                },
            },
            {
                type: "line",
                xKey: "year",
                yKey: "discountedCashflow",
                yName: "Discounted cashflow",
                stroke: "red",
                strokeWidth: 2,
                lineDash: [4, 4],
                marker: {
                    enabled: false,
                },
            },
        ],
        tooltip: {
            format: "{label}: {value}",
            formatter: (params: any) => {
                if (params.key === "value") {
                    return formatNumberForView(roundToDecimals(params.value, 4))
                }

                return params.value
            },
        },
        axes: [
            axesConfig,
            {
                type: "number",
                position: "left",
                title: { text: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData?.currency) },
                label: {
                    formatter: (params: any) => formatNumberForView(roundToDecimals(params.value, 4)),
                },
            },
        ],
        legend: { enabled: enableLegend, position: "bottom", spacing: 40 },
    }

    const pieChartData = [
        {
            label: "Study Costs",
            value: Math.abs(mergedCostProfiles.studyProfiles?.values?.reduce((sum: any, value: any) => sum + (typeof value === "number" ? value : 0), 0) ?? 0),
        },
        {
            label: "OPEX",
            value: Math.abs(mergedCostProfiles.opexProfiles?.values?.reduce((sum: any, value: any) => sum + (typeof value === "number" ? value : 0), 0) ?? 0),
        },
        {
            label: "Cessation",
            value: Math.abs(mergedCostProfiles.cessationProfiles?.values?.reduce((sum: any, value: any) => sum + (typeof value === "number" ? value : 0), 0) ?? 0),
        },
        {
            label: "Offshore Facilities",
            value: Math.abs(mergedCostProfiles.offshoreFacilityProfiles?.values?.reduce((sum: any, value: any) => sum + (typeof value === "number" ? value : 0), 0) ?? 0),
        },
        {
            label: "Development Wells",
            value: Math.abs(mergedCostProfiles.developmentWellCostProfiles?.values?.reduce((sum: any, value: any) => sum + (typeof value === "number" ? value : 0), 0) ?? 0),
        },
        {
            label: "Exploration Wells",
            value: Math.abs(mergedCostProfiles.explorationWellCostProfiles?.values?.reduce((sum: any, value: any) => sum + (typeof value === "number" ? value : 0), 0) ?? 0),
        },
    ]
    const totalValue = pieChartData.reduce((acc, curr) => acc + (curr.value || 0), 0)

    // Filter out zero values from the pie chart data
    const filteredPieChartData = pieChartData.filter((item) => item.value > 0)

    // Use default data if all values are zero
    const finalPieChartData = filteredPieChartData.length > 0
        ? filteredPieChartData
        : [
            { label: "Study Costs", value: 1 },
            { label: "OPEX", value: 1 },
            { label: "Cessation", value: 1 },
        ]

    const pieChartOptions: object = {
        data: finalPieChartData,
        title: {
            text: "Cost Distribution",
            fontSize: 22,
        },
        subtitle: { text: `(${formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData?.currency)})` },
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
                title: {
                    text: formatNumberForView(roundToDecimals(Math.abs(totalValue), 4)),
                    showInLegend: false,
                    fontSize: 20,
                },
                subtitle: {
                    text: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData?.currency),
                    showInLegend: false,
                    fontSize: 14,
                },
                innerRadiusRatio: 0.7,
                centerTitle: true,
                calloutLabelKey: "label",
                angleKey: "value",
                calloutLabel: {
                    enabled: true,
                    formatter: (params: any) => {
                        const dataPoint = params.datum

                        if (!dataPoint) {
                            return "0"
                        }

                        const value = typeof dataPoint.value === "number"
                            ? Math.abs(dataPoint.value)
                            : Math.abs(parseFloat(dataPoint.value || "0"))

                        return formatNumberForView(roundToDecimals(value, 4))
                    },
                },
                fills: barColors,
                strokes: ["white"],
                strokeWidth: 1,
                legendItemKey: "label",
            },
        ],
        tooltip: {
            format: "{label}: {value}",
        },
        legend: { enabled: enableLegend, position: "bottom", spacing: 40 },
    }

    if (!apiData) {
        return <p> see you cell</p>
    }

    return (
        <Grid container spacing={2} style={{ width: "100%" }}>
            <Grid size={{ xs: 12, xl: 6 }}>
                <AgCharts options={barChartOptions} style={{ height: "450px" }} />
            </Grid>
            <Grid size={{ xs: 12, xl: 6 }}>
                <AgCharts options={pieChartOptions} style={{ height: "380px" }} />
            </Grid>
        </Grid>
    )
}

export default AggregatedTotals
