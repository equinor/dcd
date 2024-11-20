import { test, expect } from '@playwright/test'

let testCaseName = "e2e-case"
let page

const gomLocal = 'http://localhost:3000/apps/conceptapp/a58880a6-7ac7-471e-a4e6-139fa403f230'
const gomQA = 'https://fusion-s-portal-fqa.azurewebsites.net/apps/conceptapp/64abaac9-0627-439a-9d41-ac057984f05d'

const goToTestCase = async (page) => {
  const container = await page.locator('#casesList')
  const button = container.locator('button', { hasText: testCaseName })

  await expect(button).toBeVisible({ timeout: 30000 })
  await button.scrollIntoViewIfNeeded()
  await button.click()
}

test.describe('GOM Project case operation Tests', () => {
  test.beforeAll(async ({ browser }) => {
    const context = await browser.newContext()
    page = await context.newPage()
    await page.goto(gomLocal)

    try {
      await page.waitForSelector('text=What\'s New', { timeout: 8000 })

      const closeButton = page.getByRole('button', { name: 'Close' })
      await closeButton.click()
    } catch (error) {
      console.log("Modal not detected within 5 seconds, continuing test.")
    }
  })

  test.afterAll(async () => {
    await page.close()
  })


  test('Create new case', async () => {
    await expect(page.getByRole('button', { name: 'Add new Case' })).toBeVisible()
    await page.getByRole('button', { name: 'Add new Case' }).click()
    await expect(page.getByText('Add new case', { exact: true })).toBeVisible()
    await page.getByPlaceholder('Enter a name').fill(testCaseName)
    await page.waitForTimeout(3000)
    await page.getByRole('button', { name: 'Create case' }).click()
    await page.waitForTimeout(5000)

    const container = await page.locator('#casesList')
    console.log("testCaseName: " + testCaseName)
    const button = container.locator('button', { hasText: testCaseName })

    await expect(button).toBeVisible({ timeout: 30000 })
    await button.scrollIntoViewIfNeeded()
    await expect(button).toBeVisible()

  })

  test('Edit the case description tab fields', async () => {
    goToTestCase(page)
    await page.waitForTimeout(1000)
    const editButton = await page.locator('#toggleCaseEditButton')
    await editButton.click()

    await page.locator('#editor').getByRole('paragraph').click()
    await page.locator('#editor div').fill('e2e test')
    await page.waitForTimeout(1000)

    await page.getByRole('spinbutton').first().click()
    await page.getByRole('spinbutton').first().fill('1')
    await page.waitForTimeout(1000)

    await page.getByRole('spinbutton').nth(1).click()
    await page.getByRole('spinbutton').nth(1).fill('2')
    await page.waitForTimeout(1000)

    await page.getByRole('spinbutton').nth(2).click()
    await page.getByRole('spinbutton').nth(2).fill('3')
    await page.waitForTimeout(1000)

    await page.getByRole('spinbutton').nth(3).click()
    await page.getByRole('spinbutton').nth(3).fill('4')
    await page.waitForTimeout(1000)

    await page.getByRole('combobox').nth(1).selectOption('2')
    await page.waitForTimeout(1000)

    await page.getByRole('combobox').first().selectOption('2')
    await page.waitForTimeout(1000)

    await editButton.click()
    await page.waitForTimeout(2000)


    await expect(page.locator('#case-description-viewer')).toContainText('e2e test')
    await expect(page.locator('[id^="Production wells-1"]')).toContainText('1')
    await expect(page.locator('[id^="Water injector wells-2"]')).toContainText('2')
    await expect(page.locator('[id^="Gas injector wells-3"]')).toContainText('3')
    await expect(page.locator('[id="Production strategy overview-Gas injection"]')).toContainText('Gas injection')
    await expect(page.locator('[id="Artificial lift-Electrical submerged pumps"]')).toContainText('Electrical submerged pumps')
    await expect(page.locator('[id="Facilities availability-4 %"]:visible')).toContainText('4 %') // :visible is a workaround for multiple elements with the same id
  }
  )

  test('Delete the case', async () => {
    goToTestCase(page)
    const container = await page.locator('#casesList')
    const button = container.locator('button', { hasText: testCaseName })

    await expect(button).toBeVisible({ timeout: 30000 })
    await button.scrollIntoViewIfNeeded()
    await button.click()

    await page.locator('#caseDropMenuButton').click()
    await expect(page.getByRole('menuitem', { name: 'Delete' })).toBeVisible()
    await page.getByRole('menuitem', { name: 'Delete' }).click()
    await page.getByRole('button', { name: 'Delete' }).click()

    await expect(page).toHaveURL('http://localhost:3000/apps/conceptapp/a58880a6-7ac7-471e-a4e6-139fa403f230')
  })
})
