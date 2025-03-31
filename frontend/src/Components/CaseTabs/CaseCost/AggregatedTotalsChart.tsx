import Grid from "@mui/material/Grid2"
import { AgCharts } from "ag-charts-react"
import React, { useEffect, useMemo, useState } from "react"

import { setValueToCorrespondingYear } from "@/Components/Charts/TimeSeriesChart"
import { useDataFetch } from "@/Hooks"
import { ITimeSeries, ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"
import {
    formatCurrencyUnit, formatNumberWithDecimals, formatProfileName, formatChartNumber,
} from "@/Utils/FormatingUtils"
import { mergeTimeseries, mergeTimeseriesList } from "@/Utils/TableUtils"

interface AggregatedTotalsProps {
    tableYears: [number, number];
    apiData: Components.Schemas.CaseWithAssetsDto;
    barColors: string[];
    unit?: string;
    enableLegend?: boolean;
}

const AggregatedTotals: React.FC<AggregatedTotalsProps> = ({
    apiData,
    barColors,
    unit,
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

            console.log("DEBUG - Setting merged cost profiles:", costProfiles)
            setMergedCostProfiles(costProfiles)
            const incomeProfiles = {
                liquidRevenue: [
                    {
                        startYear: apiData.productionProfileOil?.startYear,
                        values: (() => {
                            const mergedValues = mergeTimeseries(
                                apiData.productionProfileOil,
                                apiData.additionalProductionProfileOil,
                            ).values

                            if (!mergedValues) { return null }
                            const oilPriceUsd = revisionAndProjectData?.commonProjectAndRevisionData?.oilPriceUsd ?? 0
                            const exchangeRateUsdToNok = revisionAndProjectData?.commonProjectAndRevisionData?.exchangeRateUsdToNok ?? 1

                            return mergedValues.map((v: number) => (v * 6.29 * oilPriceUsd) * exchangeRateUsdToNok)
                        })(),
                    },

                ],
                gasRevenue: [
                    {
                        startYear: apiData.productionProfileGas?.startYear,
                        values: mergeTimeseries(
                            apiData.productionProfileGas,
                            apiData.additionalProductionProfileGas,
                        ).values?.map((v: number) => v * 1000 * ((revisionAndProjectData?.commonProjectAndRevisionData?.gasPriceNok ?? 0)
                        )) ?? null,
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
    const incomeProfile = apiData.calculatedTotalIncomeCostProfileUsd
    const costProfile = apiData.calculatedTotalCostCostProfileUsd
    const discountedCashflowData = apiData.calculatedDiscountedCashflowService
    let minYear = dg4Year
    let maxYear = dg4Year

    if (incomeProfile && costProfile) {
        minYear = Math.min(incomeProfile.startYear, costProfile.startYear)
        maxYear = Math.max(
            incomeProfile.startYear + incomeProfile.values.length,
            costProfile.startYear + costProfile.values.length,
        )
    }

    const yearRange = Array.from({ length: maxYear - minYear }, (_, i) => minYear + i)

    const cashFlowData: ITimeSeries = {
        startYear: minYear,
        values: yearRange.map((year) => {
            const incomeIndex = year - incomeProfile.startYear
            const costIndex = year - costProfile.startYear
            const income = incomeIndex >= 0 && incomeIndex < incomeProfile.values.length
                ? incomeProfile.values[incomeIndex]
                : 0
            const cost = costIndex >= 0 && costIndex < costProfile.values.length
                ? costProfile.values[costIndex]
                : 0

            const exchangeRateUsdToNok = revisionAndProjectData?.commonProjectAndRevisionData?.exchangeRateUsdToNok ?? 1

            return income * exchangeRateUsdToNok - cost
        }),
    }

    const cashFlow = {
        startYear: minYear,
        values: (cashFlowData?.values || []).map((v) => v) ?? [],
    }
    const totalIncomeData = apiData.calculatedTotalIncomeCostProfileUsd

    const cashflow = {
        startYear: Math.min(totalIncomeData?.startYear, costProfile?.startYear) + dg4Year,
        values: (cashFlowData?.values || []).map((v) => v) ?? [],
    }

    const discountedCashflow = {
        startYear: (discountedCashflowData?.startYear ?? 0) + dg4Year,
        values: (discountedCashflowData?.values || []).map((v) => v) ?? [],
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
            yearData.cashflow = (cashflow.values || []).reduce((acc, value, index) => {
                if (cashflow.startYear + index === year) {
                    return acc + value
                }

                return acc
            }, 0)
            yearData.discountedCashflow = (discountedCashflow?.values || []).reduce((acc, value, index) => {
                if (discountedCashflow.startYear + index === year) {
                    return acc + value
                }

                return acc
            }, 0)

            data.push(yearData)
        })

        return data
    }, [aggregatedTimeSeriesData, tableYears, apiData, cashFlow])

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
            // Use format instead of renderer for AG-Charts compatibility
            format: "{label}: {value}",
            // Add a custom formatter to properly display values
            formatter: (params: any) => {
                // Only format the value part
                if (params.key === "value") {
                    return formatChartNumber(params.value)
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
                    formatter: (params: any) => formatChartNumber(params.value),
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

    // Debug logs for pie chart data
    console.log("DEBUG - Pie chart data:", pieChartData)
    console.log("DEBUG - Total value:", totalValue)
    console.log("DEBUG - Raw cost profiles:", {
        studyProfiles: mergedCostProfiles.studyProfiles?.values,
        opexProfiles: mergedCostProfiles.opexProfiles?.values,
        cessationProfiles: mergedCostProfiles.cessationProfiles?.values,
        offshoreFacilityProfiles: mergedCostProfiles.offshoreFacilityProfiles?.values,
        developmentWellCostProfiles: mergedCostProfiles.developmentWellCostProfiles?.values,
        explorationWellCostProfiles: mergedCostProfiles.explorationWellCostProfiles?.values,
    })

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

    // Debug logs for final pie chart data
    console.log("DEBUG - Filtered/Final pie chart data:", finalPieChartData)

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
                type: "donut",
                calloutLabelKey: "label",
                angleKey: "value",
                sectorLabelKey: "value",
                calloutLabel: {
                    enabled: true,
                    formatter: (params: any) => {
                        console.log("DEBUG - Callout label params:", params)

                        // Important: Look at params.datum for the actual data point
                        const dataPoint = params.datum

                        if (!dataPoint) {
                            console.log("DEBUG - No datum in params")

                            return "0"
                        }

                        const value = typeof dataPoint.value === "number"
                            ? Math.abs(dataPoint.value)
                            : Math.abs(parseFloat(dataPoint.value || "0"))

                        console.log("DEBUG - Corrected value from datum:", value, dataPoint)

                        return formatChartNumber(value)
                    },
                },
                innerRadiusOffset: -25,
                strokes: ["white"],
                innerLabels: [
                    {
                        text: formatChartNumber(Math.abs(totalValue)),
                        fontSize: 18,
                        color: "#000000",
                    },
                    {
                        text: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData?.currency),
                        fontSize: 14,
                        color: "#B4B4B4",
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
        tooltip: {
            // Use format property instead of renderer
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
