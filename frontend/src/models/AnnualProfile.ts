interface AnnualProfileConstructor {
    startYear?: number
    values?: number[] | null
}

export class AnnualProfile {
    startsAt: number | null
    values: number[]

    constructor(data: AnnualProfileConstructor) {
        this.startsAt = data.startYear ?? null
        this.values = data.values ?? []
    }

    static fromJSON(data: AnnualProfileConstructor): AnnualProfile {
        return new AnnualProfile(data)
    }
}
