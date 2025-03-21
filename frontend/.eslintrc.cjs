module.exports = {
    env: {
        browser: true,
        es2021: true,
    },

    extends: [
        "plugin:react/recommended",
        "airbnb",
    ],

    parser: "@typescript-eslint/parser",
    plugins: ["react", "@typescript-eslint"],

    parserOptions: {
        ecmaFeatures: { jsx: true },
        ecmaVersion: "latest",
        sourceType: "module",
        project: require.resolve("./tsconfig.json"),
    },

    settings: {
        react: { version: "detect" },
    },

    // Global rules
    rules: {
        // Code style
        semi: ["error", "never"],
        quotes: ["error", "double"],
        indent: ["warn", 4],
        "max-len": ["warn", 180],
        "linebreak-style": 0,
        curly: ["error", "all"],

        // React specific
        "react/jsx-indent": [2, 4],
        "react/jsx-indent-props": [2, 4],
        "react/jsx-filename-extension": [2, { extensions: [".jsx", ".tsx"] }],
        "react/jsx-props-no-spreading": "off",
        "react/function-component-definition": [2, {
            namedComponents: "arrow-function",
            unnamedComponents: "arrow-function",
        }],
        "react/require-default-props": 0,
        "react/react-in-jsx-scope": 0,
        "react/prop-types": 0,

        // Import rules
        "import/extensions": 0,
        "import/no-unresolved": 0,
        "import/prefer-default-export": "off",
        "import/order": ["error", {
            groups: ["builtin", "external", "internal", "parent", "sibling", "index"],
            "newlines-between": "always",
            alphabetize: { order: "asc" },
        }],

        // Warning rules
        "no-unused-vars": "warn",
        "no-unused-expressions": "warn",
        "no-console": process.env.NODE_ENV === "production" ? "warn" : "off",

        // Class related
        "lines-between-class-members": ["error", "always", { exceptAfterSingleLine: true }],

        // Restricted syntax
        "no-restricted-syntax": [
            "error",
            {
                selector: "NewExpression[callee.name=\"Date\"][arguments.length > 0]",
                message: "Do not use new Date(xyz). Use a date utility function instead.",
            },
            {
                selector: [
                    "CallExpression[callee.object.name='localStorage'][callee.property.name=/^(setItem|getItem|removeItem|clear)$/]",
                    "MemberExpression[object.name='localStorage'][property.name=/^(setItem|getItem|removeItem|clear)$/]",
                    "AssignmentExpression[left.object.name='localStorage']",
                ].join(", "),
                message: "Direct localStorage manipulation is not allowed. Use the useLocalStorage hook instead.",
            },
        ],

        // Enforce single responsibility principle
        "max-lines-per-function": ["warn", { max: 400, skipBlankLines: true, skipComments: true }],
        "max-statements": ["warn", 50],
        "max-depth": ["warn", 4],
        "max-nested-callbacks": ["warn", 3],

        // Enforce consistent spacing
        "padding-line-between-statements": [
            "error",
            { blankLine: "always", prev: "*", next: "return" },
            { blankLine: "always", prev: ["const", "let", "var"], next: "*" },
            { blankLine: "any", prev: ["const", "let", "var"], next: ["const", "let", "var"] },
        ],

        // Enforce consistent naming
        camelcase: ["warn", { properties: "never", ignoreDestructuring: true, ignoreImports: true }],
        "id-length": ["warn", { min: 2, max: 50, exceptions: ["e", "p", "c", "i", "j", "x", "y", "v", "t", "_"] }],

        // Prevent prop drilling
        "react/jsx-max-props-per-line": ["error", { maximum: 3, when: "multiline" }],
    },

    // TypeScript specific rules
    overrides: [
        {
            files: ["*.ts", "*.tsx"],
            rules: {
                // TypeScript specific rules
                "no-unused-vars": "off",
                "@typescript-eslint/no-unused-vars": "warn",
                "no-use-before-define": "off",
                "@typescript-eslint/no-use-before-define": "error",
                "no-shadow": "off",
                "@typescript-eslint/no-shadow": "error",
                "no-undef": "off",
                semi: ["error", "never"],

                // Enforce TypeScript best practices
                "@typescript-eslint/explicit-function-return-type": "warn",
                "@typescript-eslint/no-explicit-any": "warn",
                "@typescript-eslint/no-unnecessary-type-assertion": "warn",
                "@typescript-eslint/prefer-as-const": "error",
                "@typescript-eslint/no-non-null-assertion": "warn",

                // Accessibility rules
                "jsx-a11y/label-has-associated-control": ["error", {
                    required: { some: ["nesting", "id"] },
                }],
                "jsx-a11y/label-has-for": ["error", {
                    required: { some: ["nesting", "id"] },
                }],
            },
        },
        {
            // Test and utility files
            files: ["**/__tests__/**", "**/setupTests.ts"],
            rules: {
                "import/no-extraneous-dependencies": "off",
            },
        },
        {
            // Date utility files
            files: ["**/DateUtils.ts", "**/__tests__/DateUtils.test.ts"],
            rules: {
                "no-restricted-syntax": "off",
            },
        },
        {
            // LocalStorage hook
            files: ["**/useLocalStorage.tsx"],
            rules: {
                "no-restricted-syntax": "off",
            },
        },
        {
            // Config files
            files: [".eslintrc.cjs", "*.config.js", "*.config.ts"],
            parserOptions: {
                project: null,
            },
        },
    ],
}
