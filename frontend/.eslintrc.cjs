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
        camelcase: 0,

        // React specific
        "react/jsx-indent": [2, 4],
        "react/jsx-indent-props": [2, 4],
        "react/jsx-filename-extension": [2, { extensions: [".jsx", ".tsx"] }],
        "react/jsx-props-no-spreading": "off",
        "react/function-component-definition": 0,
        "react/require-default-props": 0,
        "react/react-in-jsx-scope": 0,
        "react/prop-types": 0,

        // Import rules
        "import/extensions": 0,
        "import/no-unresolved": 0,
        "import/prefer-default-export": "off",

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
                    "AssignmentExpression[left.object.name='localStorage']"
                ].join(", "),
                message: "Direct localStorage manipulation is not allowed. Use the useLocalStorage hook instead.",
            },
        ],
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
    ],
}
