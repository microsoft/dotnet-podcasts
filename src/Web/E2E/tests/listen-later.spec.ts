import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  // Go to discover page
  await page.goto('/discover');
});

test.describe('Listen Later', () => {
  test('should allow me to listen to podcast later', async ({ page }) => {
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click first listen later button
    await page.locator('button:nth-child(2)').first().click();
    // view listen later tab
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await expect(page).toHaveURL('/listen-later');
    // assert no results page isn't shown
    expect(page.locator('.main')).not.toContain('no results');
  });
});