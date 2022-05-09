export class PhysicalUnit {
    private phase: number

    constructor(phase: number) {
        this.phase = phase
    }

    static parseJSON(value: PhysicalUnit) {
        return new PhysicalUnit(value.phase)
    }

    valueOf(): number {
        return this.phase
    }

    toString(): string | undefined {
        return {
            0: "SI",
            1: "oilfields",
        }[this.phase]
    }
}
