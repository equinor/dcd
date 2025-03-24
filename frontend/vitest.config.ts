import { defineConfig } from 'vitest/config'
import { resolve } from 'path'

export default defineConfig({
  test: {
    environment: 'jsdom',
    globals: true,
  },
  resolve: {
    alias: {
      '@': resolve(__dirname, './src'),
    },
  },
  // Mock up the modules that aren't essential for tests
  define: {
    'import.meta.vitest': 'undefined',
  },
}) 