import { test, expect, Page } from '@playwright/test';

test.describe.configure({ mode: 'parallel' });

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
});

test.describe('Discover', () => {
  test('should allow me to browse categories', async ({ page }) => {
    // Loop through each category
    for (const category of ['Microsoft', 'Mobile', 'Community', 'M365']) {
      // click on the category
      await page.locator('.tags-item >> text=' + category).click();
      // assert category is selected
      await expect(page.locator('.titlePage')).toHaveText(category);
      // navigate back to discover page
      await page.locator('button:has-text("Back")').click();
    }
  });

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