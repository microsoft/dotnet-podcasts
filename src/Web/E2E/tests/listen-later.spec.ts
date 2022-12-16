import { test, expect } from '@playwright/test';

test.describe('Listen Later', () => {
  
  test.beforeEach(async ({ page }) => {
    await page.goto('/discover');
    await page.locator('.item-primary-action').first().click();
    await page.getByTitle('Listen Later').first().click();
  });
  
  test('should allow me to listen to podcast later', async ({ page }) => {
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await expect(page.locator('main')).not.toContainText('no results');
  });

  test('should not contain podcasts when listen later is clicked twice', async ({ page }) => {
    await page.getByTitle('Listen Later').first().click();
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await expect(page.getByRole('heading', { name: 'You haven\'t added any podcasts yet.' })).toBeVisible();
  });

  test('should be able to discover podcasts when none in listen later', async ({ page }) => {
    await page.getByTitle('Listen Later').first().click();
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await page.getByRole('link', { name: 'Discover podcasts' }).click();
    expect(page).toHaveURL('/discover');
  });
});

