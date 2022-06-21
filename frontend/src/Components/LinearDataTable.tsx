/* eslint-disable max-len */
/* eslint-disable no-plusplus */
/* eslint-disable import/no-extraneous-dependencies */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable no-empty-pattern */
/* eslint-disable @typescript-eslint/no-use-before-define */
/* eslint-disable react/no-unused-prop-types */
import { Chart as ReactChart } from "react-chartjs-2"
import {
    Chart as ChartJS, ChartData, ChartOptions, registerables,
} from "chart.js"
import { min, maxBy, range } from "lodash"

ChartJS.register(...registerables)

const COLORS: string[] = ["#005500", "#000000", "#FF00FF", "#0000FF", "#00FFFF", "#00FF00", "#FFFF00", "#FF0000", "#808080", "#FF0080", "#FF80FF", "#800080", "#8000FF",
    "#8080FF", "#000080", "#0080FF", "#80FFFF", "#008080", "#00FF80", "#80FF00", "#FFFF80", "#808000", "#FF8000", "#FF8080", "#800000", "#AA0055", "#AA5555",
    "#550000", "#555500"]

interface Props {
    capexYearX: number[]
    capexYearY: number[][]
    caseTitles: string[]
}

// Minimum start years can be ex. [2,3]. arrayOfValues can be ex. [ [ 100,200,50 ], [ 900, 500, 200] ]
// Returns a list of years from today, added with lowest Start Year, ranged until the array with the longest length.
// Example [ "2024", "2025, "2026" ]
const yearLabels = (minimumStartYears: number[], arrayOfValues: number[][]) :string[] => {
    const start: number = new Date().getFullYear()
    const start2: number = min(minimumStartYears.filter((startYear) => startYear !== null || startYear !== 0)) as number
    const totalstart: number = start + start2
    const add: number[] | undefined = maxBy(arrayOfValues, (xx) => xx.length)
    const end: number = totalstart + add!.length

    const yearArr: number[] = range(totalstart, end)
    return yearArr.map((yy) => yy.toString())
}

const generateChartDatas = (arrayOfValues: number[][], caseTitles: string[]) => {
    const allObjects: any[] = []

    for (let index = 0; index < arrayOfValues.length; index++) {
        const element = {
            label: caseTitles[index],
            fillColor: COLORS[index],
            strokeColor: COLORS[index],
            highlightFill: COLORS[index],
            highlightStroke: COLORS[index],
            backgroundColor: COLORS[index],
            borderColor: COLORS[index],
            color: COLORS[index],
            data: arrayOfValues[index],
        }
        allObjects.push(element)
    }
    return allObjects
}

const LinearDataTable = ({
    capexYearX, capexYearY, caseTitles,
}: Props) => {
    const chartData: ChartData = {
        datasets: [

        ],
    }
    chartData.labels = yearLabels(capexYearX, capexYearY)
    chartData.datasets = generateChartDatas(capexYearY, caseTitles)
    return (
        <ReactChart
            type="line"
            options={chartoptions()}
            data={chartData}
        />
    )
}

export default LinearDataTable

export const chartoptions = (): ChartOptions => ({
    maintainAspectRatio: true,
    responsive: true,
    plugins: {

        tooltip: {
            backgroundColor: "white",
            titleColor: "black",
            bodyColor: "black",
        },
        title: {
            text: "Compare cases",
            display: true,
            align: "start",
            color: "black",
            font: {
                size: 20,
                family: "Equinor",
                weight: "bolder",
            },
        },
        legend: {
            display: true,
            labels: {
                usePointStyle: true,
            },
        },
    },
    scales: {
        y: {
            beginAtZero: true,
            position: "left",
            type: "linear",
            title: {
                display: true,
                text: "CapEx",
                font: {
                    size: 18,
                    family: "Equinor",
                    weight: "bolder",
                },
            },
        },
        xAxis: {
            offset: true,
            title: {
                display: true,
                text: "Year",
                font: {
                    size: 18,
                    family: "Equinor",
                    weight: "bolder",
                },
            },
        },
    },
})
