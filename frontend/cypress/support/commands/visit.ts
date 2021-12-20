Cypress.Commands.add("visitApplication", () => {
  const frontendUrl = Cypress.env("FRONTEND_URL") || "http://localhost:3000";

  cy.visit(`${frontendUrl}`);
});

export {};
declare global {
  namespace Cypress {
    interface Chainable {
      /**
       * Visit application
       * @example cy.visitApplication(user)
       */
      visitApplication(): Chainable;
    }
  }
}
