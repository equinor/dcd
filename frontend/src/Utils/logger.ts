interface LoggerConfig {
    name: string
    enabled: boolean
}

export const createLogger = ({ name, enabled = false }: LoggerConfig) => {
    if (!enabled) {
        return {
            log: () => { },
            error: () => { },
            warn: () => { },
            info: () => { },
        }
    }
    return {
        log: console.log.bind(console, `[${name}]`),
        error: console.error.bind(console, `[${name}]`),
        warn: console.warn.bind(console, `[${name}]`),
        info: console.info.bind(console, `[${name}]`),
    }
}
