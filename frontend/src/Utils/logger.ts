interface Logger {
    log: (message: string, ...args: any[]) => void
    error: (message: string, ...args: any[]) => void
    warn: (message: string, ...args: any[]) => void
    info: (message: string, ...args: any[]) => void
}

interface LoggerConfig {
    name: string
    enabled: boolean
    customLogger?: Logger
}

export const createLogger = ({ name, enabled = false, customLogger }: LoggerConfig): Logger => {
    if (!enabled) {
        return {
            log: () => { },
            error: () => { },
            warn: () => { },
            info: () => { },
        }
    }

    if (customLogger) {
        return customLogger
    }

    return {
        log: console.log.bind(console, `[${name}]`),
        error: console.error.bind(console, `[${name}]`),
        warn: console.warn.bind(console, `[${name}]`),
        info: console.info.bind(console, `[${name}]`),
    }
}
