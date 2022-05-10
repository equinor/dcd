export class ArtificialLift {
    private type: number

    constructor(type: number) {
        this.type = type
    }

    toString(): string | undefined {
        return {
            0: "None",
            1: "Gas lift",
            2: "Electrical submerged pumps",
            3: "Subsea booster pumps",
        }[this.type]
    }
}
