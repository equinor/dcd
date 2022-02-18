enum ProjectPhases {
    DG1,
    DG2,
    DG3,
    DG4,
}

export class ProjectPhase {
    private phase: number

    constructor(phase: number) {
        this.phase = phase
    }

    toString(): string {
        return ProjectPhases[this.phase];
    }
}
