import { test, expect } from '@playwright/test';

test.describe('Login', () => {
  test('should allow me to login', async ({ page }) => {
    await page.goto('');
    // click sign in
    await page.locator('text=Sign In').click();
    // assert discover page is shown
    await expect(page).toHaveURL('/discover');
    await expect(page).toHaveTitle('.NET Podcasts')
  });
});