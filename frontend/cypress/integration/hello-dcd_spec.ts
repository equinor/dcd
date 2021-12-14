describe("Sample test DCD", () => {
  it("Navigate to home page", () => {
    cy.visitApplication();
    cy.contains("Initial DCD commit");
  });
});
