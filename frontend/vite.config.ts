import { defineConfig } from "vite"
import cssInjectedByJsPlugin from "vite-plugin-css-injected-by-js"
import checker from "vite-plugin-checker"

export default defineConfig(({ command }) => {
    if (command === "build") {
    // https://github.com/equinor/fusion-framework/issues/997
        const sourcemap = process.env.GENERATE_SOURCEMAP === "true" ? "inline" : false
        return {
            build: {
                sourcemap,
                rollupOptions: {
                    output: {
                        manualChunks: undefined,
                    },
                },
            },
            plugins: [cssInjectedByJsPlugin()],
        }
    }
    return {
        plugins: [
            checker({
                typescript: true,
            }),
        ],
        define: { global: "{}" },
    }
})
