// setup.js
const { chromium } = require('@playwright/test');

(async () => {
  // Launch browser in non-headless mode
  const browser = await chromium.launch({ headless: false });
  const page = await browser.newPage();

  // Navigate to your application
  await page.goto('http://localhost:3000/apps/conceptapp/');

  // Pause the script to allow manual login
  console.log('Please complete the login process manually.');
  await page.pause();

  // Save the authentication state to a file
  await page.context().storageState({ path: 'auth.json' });

  await browser.close();
})();
