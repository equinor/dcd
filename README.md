# Digital Concept Development (DCD)

## Frontend

The frontend is built using TypeScript and components from the Equinor Design System ([EDS](https://eds.equinor.com/components/component-status/)).

### Run frontend

```
cd frontend
npm install
npm start
```

### Testing

We use Cypress to create end-to-end tests for the application.

#### Test Strategy

We aim to create a test automation suite that will `describe and validate the systems behavior and functionality as seen by its users`.

The Cypress tests constitute a suite of automated, functional regression tests.

#### Run the tests

To run the Cypress tests locally, type

`npm run cyopen` to open the Cypress interactive runner. This requires the application to be run locally.
