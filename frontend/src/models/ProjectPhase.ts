export class ProjectPhase {
    private phase: number

    constructor(phase: number) {
        this.phase = phase
    }

    // this seemingly silly method is used for custom JSON parsing for the
    // Project class and looks silly just because of type security in
    // typescript. value a priori has a type of object.
    static parseJSON(value: ProjectPhase) {
        return new ProjectPhase(value.phase)
    }

    valueOf(): number {
        return this.phase
    }

    toString(): string | undefined {
        return {
            0: "Unknown",
            1: "Bid preparations",
            2: "Business identification",
            3: "Business planning",
            4: "Concept planning",
            5: "Concessions / Negotiations",
            6: "Defintion",
            7: "Execution",
            8: "Operation",
            9: "Screening business opportunities",
        }[this.phase]
    }
}
