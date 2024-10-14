import React, { useEffect, useMemo, useState } from "react"
import { AgChartsReact } from "ag-charts-react"
import { Grid } from "@mui/material"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"

import { ProfileNames } from "@/Models/Interfaces"
import { ITimeSeries, ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { mergeTimeseries } from "@/Utils/common"
import { projectQueryFn } from "@/Services/QueryFunctions"

interface AggregatedTotalsProps {
    tableYears: [number, number];
    apiData: Components.Schemas.CaseWithAssetsDto;
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
}) => {
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const [aggregatedTimeSeriesData, setAggregatedTimeSeriesData] = useState<ITimeSeriesTableData[]>([])

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
            id: "",
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
                    (apiData.surfCostProfileOverride?.override ? apiData.surfCostProfileOverride : apiData.surfCostProfile),
                    (apiData.topsideCostProfileOverride?.override ? apiData.topsideCostProfileOverride : apiData.topsideCostProfile),
                    (apiData.substructureCostProfileOverride?.override ? apiData.substructureCostProfileOverride : apiData.substructureCostProfile),
                    (apiData.transportCostProfileOverride?.override ? apiData.transportCostProfileOverride : apiData.transportCostProfile),
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

            const newTimeSeriesData: ITimeSeriesTableData[] = []

            Object.entries(profiles).forEach(([profileName, profileData]) => {
                const aggregatedProfile = aggregateProfiles(profileData, dg4Year)
                const resourceName: ProfileNames = profileName as ProfileNames

                newTimeSeriesData.push({
                    profileName: profileName.replace(/Profiles$/, "").replace(/([A-Z])/g, " $1").trim(),
                    unit: projectData?.currency === 1 ? "MNOK" : "MUSD",
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
    }, [apiData, tableYears, projectData])

    const calculateIncome = () => {
        const oilPrice = projectData?.oilPriceUSD ?? 75.0
        const gasPrice = projectData?.gasPriceNOK ?? 3
        const cubicMetersToBarrelsFactor = 6.29
        const exchangeRateUSDToNOK = projectData?.exchangeRateUSDToNOK ?? 10.0
        const exchangeRateNOKToUSD = 1 / exchangeRateUSDToNOK

        const totalOilProductionInMegaCubics = mergeTimeseries(apiData.productionProfileOil, apiData.additionalProductionProfileOil)

        const oilProductionInBarrels = (totalOilProductionInMegaCubics.values || []).map((v) => v * cubicMetersToBarrelsFactor)
        // finally one should have multiplied oilProduction by 1 million because the oilProduction used in the calculation is in mega cubics.
        // But because the OilIncome in the diagram is in MNOK/MUSD, we would divide by 1 million to get the correct oilincome
        // We can therefore ignore these multiplications and divisions

        const oilIncome = {
            id: "",
            startYear: totalOilProductionInMegaCubics.startYear + new Date(apiData.case.dG4Date).getFullYear(),
            values: oilProductionInBarrels.map((v) => v * oilPrice * exchangeRateUSDToNOK),
        }

        const totalGasProductionInGigaCubics = mergeTimeseries(apiData.productionProfileGas, apiData.additionalProductionProfileGas)
        const gasIncome = {
            id: "",
            startYear: totalGasProductionInGigaCubics.startYear + new Date(apiData.case.dG4Date).getFullYear(),
            values: (totalGasProductionInGigaCubics.values || []).map((v) => v * gasPrice * 1000),
        }
        // similar to previous comment, we should have multiplied the totalGasProduction value by 1 billion since its set in giga cubics, but since the diagram is in MNOK/MUSD
        // We only need to multiply by 1000 to get the correct gas Income

        const totalIncome = mergeTimeseries(oilIncome, gasIncome)

        // Uncomment this to adjust income based on currency
        // if (project?.currency === 2 && totalIncome.values) {
        //     totalIncome = {
        //         ...totalIncome,
        //         values: totalIncome.values.map((v) => v * exchangeRateNOKToUSD),
        //     }
        // }

        return totalIncome
    }

    const chartData = useMemo(() => {
        const data: number[] = []
        const dg4Year = new Date(apiData.case.dG4Date).getFullYear()
        const years = Array.from({ length: tableYears[1] - tableYears[0] + 1 }, (_, i) => tableYears[0] + i)

        const totalIncomeData = calculateIncome()

        let cumulativeSum = 0
        years.forEach((year) => {
            const yearData: any = { year }
            aggregatedTimeSeriesData.forEach((series) => {
                const value = setValueToCorrespondingYear(series.profile, year, dg4Year)
                yearData[series.profileName] = value
                cumulativeSum += value
            })
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
            text: "Annual Cost Profile", // (${project?.currency === 1 ? "MNOK" : "MUSD"})`, add this to dynamically show what MNOK or MUSD on graph based on project.currency
            fontSize: 24,
        },
        subtitle: { text: "(MNOK)" ?? "" },

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
                yName: key,
                fill: barColors[index],
                stacked: true,
            })),
            {
                type: "line",
                xKey: "year",
                yKey: "cumulativeSum",
                yName: "Total Income",
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
                title: { text: "Cost" },
            },
            {
                type: "number",
                position: "right",
                title: { text: "Total Income" },
                keys: ["cumulativeSum"],
                visibleRange: [0, 1],
            },
        ],
        legend: { enabled: enableLegend, position: "bottom", spacing: 40 },
    }

    const pieChartData = useMemo(() => {
        const pieData: any[] = aggregatedTimeSeriesData.map((series) => ({
            profile: series.profileName,
            value: series.profile?.values?.reduce((sum, value) => sum + value, 0) ?? 0, // Sum values for the pie chart
        }))

        return pieData
    }, [aggregatedTimeSeriesData])

    const totalValue = pieChartData.reduce((acc, curr) => acc + curr.value, 0)

    const pieChartOptions: object = {
        data: pieChartData,
        title: {
            text: "Cost Distribution",
            fontSize: 22,
        },
        subtitle: { text: "(MNOK)" ?? "" },
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
                        text: `${totalValue.toFixed(2)}`,
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
        <Grid container spacing={2} style={{ width: "100%" }}>
            <Grid item lg={12} xl={7}>
                <AgChartsReact options={barChartOptions} style={{ height: "100%" }} />
            </Grid>
            <Grid item lg={8} xl={5}>
                <AgChartsReact options={pieChartOptions} style={{ height: "100%" }} />
            </Grid>
        </Grid>
    )
}

export default AggregatedTotals
