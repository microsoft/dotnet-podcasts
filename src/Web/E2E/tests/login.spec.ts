import { test, expect, Page } from '@playwright/test';

test.describe('Login', () => {
  test('should allow me to login', async ({ page }) => {
    await page.goto('/discover');
    // Fill username via env variable
    await page.locator('[placeholder="Email\\, phone\\, or Skype"]').fill(process.env.AADUSERNAME);
    // Press Enter
    await page.locator('[placeholder="Email\\, phone\\, or Skype"]').press('Enter');
    // // Fill password via env variable
    await page.locator('[placeholder="Password"]').fill(process.env.AADPASSWORD);
    // Click Sign In
    await page.locator('input:has-text("Sign in")').click();
    // Click No to not remember login
    await page.locator('text=No').click();
    // assert discover page is shown
    await expect(page).toHaveURL('/discover');
    await expect(page).toHaveTitle('.NET Podcasts');
  });
});