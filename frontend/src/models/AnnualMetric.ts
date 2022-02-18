type AnnualMetricConstructor = {
    id: any
    value: number
    year: number
}

export class AnnualMetric {
    id: string
    value: number
    year: number

    constructor(data: AnnualMetricConstructor) {
        this.id = data.id
        this.value = data.value
        this.year = data.year
    }

    static fromJSON(data: AnnualMetricConstructor): AnnualMetric {
        return new AnnualMetric(data)
    }
}
