import { test, expect } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
});

test.describe('Discover Categories', () => {
  
  const categories = ['Microsoft', 'Mobile', 'Community', 'Web', 'Desktop'];
  for (const category of categories) {
    test(`should allow me to browse category ${category}`, async ({ page }) => {
      await page.getByRole('list').getByRole('link', { name: category }).click();
      await expect(page.getByRole('heading', { name: category, level: 1, exact: true })).toBeVisible();
      await expect.poll(() =>
      page.locator('.card').count()).toBeGreaterThan(0);
    });
  }

  test('should show all categories', async ({ page }) => {
    await page.getByRole('link', { name: 'See all categories' }).click();
    await expect(page).toHaveURL('/categories')
    await expect(page.getByRole('heading', { name: 'All Categories' })).toBeVisible();
  });

  test('back button goes back to previous page', async ({ page }) => {
    await page.getByRole('link', { name: 'See all categories' }).click();
    await page.getByRole('button', { name: 'Back' }).click();
    await expect(page).toHaveURL('/discover')
  });

});

test.describe('Discover search bar', () => {

  // these tests will not pass due to JS loading issues
  // Playwright is like a very fast user
  // to see whats going on in your code sometimes we need to slow down the code
  // You can do this by using the site with 3G network throttling

  test.fixme('should return results when category/podcast found', async ({ page }) => {
    const searchBar = page.getByPlaceholder('Search here');
    await searchBar.click();
    await searchBar.fill('dogma');
    await searchBar.press('Enter');
    await expect(page.getByRole('heading', { name: 'dogma' }).first()).toBeVisible();
  });

  test.fixme('should show no results when no category/podcast found', async ({ page }) => {
    const searchBar = page.getByPlaceholder('Search here');
    await searchBar.click();
    await searchBar.fill('test');
    await searchBar.press('Enter');
    await expect(page.getByRole('heading', { name: 'Oops, no results found.' })).toBeVisible();
    await expect(page.locator('.card')).not.toBeVisible();
  });
});
