import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
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

  test('should display all podcast images', async ({ page }) => {
    // use visual comparison to check all images display
    await expect(page).toHaveScreenshot();
  });  
});