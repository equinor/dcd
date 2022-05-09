export class Currency {
    private phase: number

    constructor(phase: number) {
        this.phase = phase
    }

    static parseJSON(value: Currency) {
        return new Currency(value.phase)
    }

    valueOf(): number {
        return this.phase
    }

    toString(): string | undefined {
        return {
            0: "USD",
            1: "NOK",
        }[this.phase]
    }
}
