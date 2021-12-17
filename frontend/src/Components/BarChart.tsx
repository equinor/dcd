import React from 'react'
import Plot from 'react-plotly.js'
import { tokens } from '@equinor/eds-tokens'

interface Props {
    data: {
        x: (number | string)[]
        y: number[]
    }
    title: string
}

const BarChart = ({ data, title }: Props) => {
    const color = tokens.colors.infographic.primary__moss_green_34.rgba

    return (
        <Plot
            data={[
                {
                    x: data.x,
                    y: data.y,
                    type: 'bar',
                    marker: {
                        color: color,
                    },
                },
            ]}
            layout={{ width: 600, height: 400, title: title }}
        />
    )
}

export default BarChart
