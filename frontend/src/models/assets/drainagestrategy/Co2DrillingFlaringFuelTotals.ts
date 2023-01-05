export class Co2DrillingFlaringFuelTotals implements Components.Schemas.Co2DrillingFlaringFuelTotalsDto {
    co2Drilling?: number | undefined
    co2Flaring?: number | undefined
    co2Fuel?: number | undefined

    constructor(data?: Components.Schemas.Co2DrillingFlaringFuelTotalsDto) {
        this.co2Drilling = data?.co2Drilling ?? 0
        this.co2Flaring = data?.co2Flaring ?? 0
        this.co2Fuel = data?.co2Fuel ?? 0
    }

    static fromJson(data?: Components.Schemas.Co2DrillingFlaringFuelTotalsDto):
        Co2DrillingFlaringFuelTotals | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new Co2DrillingFlaringFuelTotals(data)
    }
}
