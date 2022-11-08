import { test, expect } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
});

test.describe('Discover', () => {
  // Loop through each category
  const categories = ['Microsoft', 'Mobile', 'Community', 'M365'];
  for (const category of categories) {
    test(`should allow me to browse category ${category}`, async ({ page }) => {
        // click on the category
        await page.locator('.tags-item').getByText(category).click();
        // assert category is selected
        await expect(page.locator('.titlePage')).toHaveText(category);
    });
  }

  test('should allow me to search', async ({ page }) => {
    // use search bar
    await page.getByPlaceholder('Search here').click();
    // search for a podcast
    await page.getByPlaceholder('Search here').fill('.NET');
    await page.getByPlaceholder('Search here').press('Enter');
    // assert no results page isn't shown
    expect(page.locator('.main')).not.toContain('no results');
  });
});