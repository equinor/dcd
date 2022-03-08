export class ProjectPhase {
    private phase: number

    constructor(phase: number) {
        this.phase = phase
    }

    static ParseJSON(value: ProjectPhase) {
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
