

Welcome to our Concept app front end! This README outlines our coding and structural conventions to ensure consistency.

# Git Workflow
## Commit Message Conventions

feat: Introduces a new feature.
Example: feat: add user login functionality

fix: Addresses a bug fix.
Example: fix: correct the user authentication error

# Front-End Development Guidelines

## Component Structure

All components should follow this common structure:

1. **All imports**: Group all import statements at the beginning of the file.
2. **Styled-components**: Declare styled components immediately after imports.
3. **Prop interface**: Define interfaces for props for TypeScript projects.
4. **Component function**: Use arrow functions for component declarations.
5. **Constants, Contexts, and States**: Declare constants, contexts, and states after the component function.
6. **Hooks**: Utilize React hooks after the state declarations.
7. **Methods**: Define any methods needed by the component.
8. **Return statement**: Place the return statement, which contains the JSX for the component.
9. **Default export**: Export the component as the default export of the file.

## Syntax Conventions

- Use **arrow functions** for components and functions.
- Adopt **camelCase** naming for variables and functions.
- Avoid the `any` type to ensure type safety.
- **Destructure props** for clarity and simplicity.
- Use **ternary expressions** for conditional rendering (`condition ? true : false`), but avoid nested ternary expressions.
- Remove any **unused components, variables, functions, or imports**.
- Refrain from using **inline styling** to keep styling separate and maintainable.

## Prop Drilling Rules

Prop drilling can be acceptable under these conditions:

- Data is passed down a maximum of **2 levels**.
- No more than **3 components** handle the passed-down data.

For scenarios not meeting these criteria, opt for a **context** approach to manage and pass down data.

## Avoid Repetition

- Establish a **single source of truth** for all data and configurations.
- Store all constant variables in the dedicated **common.ts** file.
- Centralize utility/helper functions in a common **helpers.ts** file.
- Ensure every new component adheres to the **single responsibility principle**.
- Favor **modular and adaptable components** over rigid, similar ones to encourage reusability and flexibility in the codebase.