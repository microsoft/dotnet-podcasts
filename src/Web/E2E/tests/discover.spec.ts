import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  // Go to discover page
  await page.goto('/discover');
  // Log in
  await page.locator('[placeholder="Email\\, phone\\, or Skype"]').fill(process.env.AADUSERNAME);
  await page.locator('[placeholder="Email\\, phone\\, or Skype"]').press('Enter');
  await page.locator('[placeholder="Password"]').fill(process.env.AADPASSWORD);
  await page.locator('input:has-text("Sign in")').click();
  await page.locator('text=No').click();  
});

test.describe('Discover', () => {
  // Loop through each category
  const categories = ['Microsoft', 'Mobile', 'Community', 'M365'];
  for (const category of categories) {
    test(`should allow me to browse category ${category}`, async ({ page }) => {
        // click on the category
        await page.locator('.tags-item >> text=' + category).click();
        // assert category is selected
        await expect(page.locator('.titlePage')).toHaveText(category);
        // use visual comparison to check all images display
        await expect(page).toHaveScreenshot();        
    });
  }

  test('should allow me to search', async ({ page }) => {
    // use search bar
    await page.locator('[placeholder="Search here"]').click();
    // search for a podcast
    await page.locator('[placeholder="Search here"]').fill('.NET');
    await page.locator('[placeholder="Search here"]').press('Enter');
    // assert no results page isn't shown
    expect(page.locator('.main')).not.toContain('no results');
  });
});