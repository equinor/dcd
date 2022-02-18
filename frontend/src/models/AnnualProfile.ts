type AnnualProfileConstructor = {
    startYear: number
    values: number[]
}

export class AnnualProfile {
    startsAt: number
    values: number[]

    constructor(data: AnnualProfileConstructor) {
        this.startsAt = data.startYear
        this.values = data.values
    }

    static fromJSON(data: AnnualProfileConstructor): AnnualProfile {
        return new AnnualProfile(data)
    }
}
