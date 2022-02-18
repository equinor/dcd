export class ProjectCategory {
    private category: number

    constructor(category: number) {
        this.category = category
    }

    toString(): string {
        return {
            0: 'Offshore wind',
            1: 'Hydrogen',
            2: 'Carbon capture and storage',
            3: 'Solar',
            4: 'FPSO',
            5: 'Platform',
            6: 'Tie in',
            7: 'Electrification',
            8: 'Brownfield',
            9: 'Onshore',
            10: 'Pipeline',
            11: 'Subsea',
            12: 'Drilling upgrade',
            13: 'Cessation',
        }[this.category]!
    }
}
