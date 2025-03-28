import Grid from "@mui/material/Grid2"
import { AgCharts } from "ag-charts-react"
import React, { useEffect, useMemo, useState } from "react"

import { setValueToCorrespondingYear } from "@/Components/Charts/TimeSeriesChart"
import { useDataFetch } from "@/Hooks"
import { ITimeSeries, ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { formatCurrencyUnit, formatNumberWithDecimals, formatProfileName } from "@/Utils/FormatingUtils"
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
        axes: [
            axesConfig,
            {
                type: "number",
                position: "left",
                title: { text: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData?.currency) },
            },
        ],
        legend: { enabled: enableLegend, position: "bottom", spacing: 40 },
    }

    const pieChartData = [
        { label: "Study Costs", value: mergedCostProfiles.studyProfiles?.values?.reduce((sum: any, value: any) => sum + value, 0) ?? 0 },
        { label: "OPEX", value: mergedCostProfiles.opexProfiles?.values?.reduce((sum: any, value: any) => sum + value, 0) ?? 0 },
        { label: "Cessation", value: mergedCostProfiles.cessationProfiles?.values?.reduce((sum: any, value: any) => sum + value, 0) ?? 0 },
        { label: "Offshore Facilities", value: mergedCostProfiles.offshoreFacilityProfiles?.values?.reduce((sum: any, value: any) => sum + value, 0) ?? 0 },
        { label: "Development Wells", value: mergedCostProfiles.developmentWellCostProfiles?.values?.reduce((sum: any, value: any) => sum + value, 0) ?? 0 },
        { label: "Exploration Wells", value: mergedCostProfiles.explorationWellCostProfiles?.values?.reduce((sum: any, value: any) => sum + value, 0) ?? 0 },
    ]
    const totalValue = pieChartData.reduce((acc, curr) => acc + curr.value, 0)

    const pieChartOptions: object = {
        data: pieChartData,
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
                calloutLabel: { enabled: false },
                innerRadiusOffset: -25,
                strokes: ["white"],
                innerLabels: [
                    {
                        text: formatNumberWithDecimals(totalValue),
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
