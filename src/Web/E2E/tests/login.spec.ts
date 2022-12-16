import { test, expect } from '@playwright/test';

test.describe('Login', () => {
  test('should allow me to login', async ({ page }) => {
    await page.goto('');
    await page.getByRole('link', { name: 'Sign In' }).click();
    await expect(page).toHaveURL('/discover');
    await expect(page).toHaveTitle('.NET Podcasts - Discover')
  });
});
