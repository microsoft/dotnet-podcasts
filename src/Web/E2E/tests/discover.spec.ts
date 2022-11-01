import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  // Go to discover page
  await page.goto('/discover');
});

test.describe('Discover', () => {
  // Loop through each category
  const categories = ['Microsoft', 'Mobile', 'Community', 'M365'];
  for (const category of categories) {
    test(`should allow me to browse category ${category}`, async ({ page, channel }) => {
      // only run vrt for MS Edge
      test.skip(channel !== 'msedge', 'Screenshots only generated using MS Edge');      
      // click on the category
      await page.getByRole('link', { name: category }).click();  // using aria-label
      // assert category is selected
      await expect(page.getByRole('heading', { name: category, level: 1 })).toHaveText(category);
      // use visual comparison to check all images display
      await expect(page).toHaveScreenshot();
    });
  }

  test('should allow me to search', async ({ page }) => {
    // use search bar
    await page.getByPlaceholder("Search here").click();
    // search for a podcast
    await page.getByPlaceholder("Search here").fill('.NET');
    await page.getByPlaceholder("Search here").press('Enter');
    // assert no results page isn't shown
    expect(page.locator('.main')).not.toContain('no results');
  });
});