import { Chart as ReactChart } from "react-chartjs-2"
import {
    Chart as ChartJS, ChartData, ChartOptions, registerables,
} from "chart.js"

ChartJS.register(...registerables)

interface Props {
    x: string[]
    y: number[]
}

const chartData: ChartData = {
    labels:
            ["2022", "2023"],

    datasets: [{
        data: [10000, 20000],
    }],
}
const ManniDataTable = ({
    x,
    y,
}: Props) => (

    <ReactChart
        type="line"
        options={chartoptions("Compare cases")}
        data={chartData}
    />
)

export default ManniDataTable

export const chartoptions = (title?: string): ChartOptions => ({
    maintainAspectRatio: true,
    responsive: true,
    plugins: {
        // @ts-ignore

        tooltip: {
            backgroundColor: "yellow",
            titleColor: "yellow",
            bodyColor: "yellow",
        },
        title: {
            text: title,
            display: true,
            align: "start",
            color: "black",
            font: {
                size: 16,
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
                text: "# of SWCRs",
            },
        },

        xAxis: {
            offset: true,
        },
    },
})
