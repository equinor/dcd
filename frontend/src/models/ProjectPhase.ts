export class ProjectPhase {
    private phase: number

    constructor(phase: number) {
        this.phase = phase
    }

    toString(): string | undefined {
        return {
            0: "DG1",
            1: "DG2",
            2: "DG3",
            3: "DG4",
        }[this.phase];
    }
}
