export class ProjectCategory {
    private category: number

    constructor(category: number) {
        this.category = category
    }

    toString(): string | undefined {
        return {
            0: "Unknown",
            1: "Brownfield",
            2: "Cessation",
            3: "Drilling upgrade",
            4: "Onshore",
            5: "Pipeline",
            6: "Platform FPSO",
            7: "Subsea",
            8: "Solar",
            9: "CO2 storage",
            10: "Efuel",
            11: "Nuclear",
            12: "CO2 Capture",
            13: "FPSO",
            14: "Hydrogen",
            15: "Hse",
            16: "Offshore wind",
            17: "Platform",
            18: "Power from shore",
            19: "Tie-in",
            20: "Renewable other",
            21: "CCS",
        }[this.category]
    }
}
