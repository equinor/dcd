export class PhysicalUnit {
    private phase: number

    constructor(phase: number) {
        this.phase = phase
    }

    // this seemingly silly method is used for custom JSON parsing for the
    // Project class and looks silly just because of type security in
    // typescript. value a priori has a type of object.
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
