import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  // Go to settings page
  await page.goto('/settings');
  // Log in
  await page.locator('[placeholder="Email\\, phone\\, or Skype"]').fill(process.env.AADUSERNAME);
  await page.locator('[placeholder="Email\\, phone\\, or Skype"]').press('Enter');
  await page.locator('[placeholder="Password"]').fill(process.env.AADPASSWORD);
  await page.locator('input:has-text("Sign in")').click();
  await page.locator('text=No').click();  
});

test.describe('Settings', () => {
  // Loop through each setting
  const settings = ['autodownload', 'deleteplayed', 'systemtheme', 'darktheme'];
  for (const setting of settings) {  
    test(`should allow me to toggle setting ${setting}`, async ({ page }) => {
        // toggle setting
        await page.locator('input[name="' + setting + '"]').check();
        await page.locator('input[name="' + setting + '"]').uncheck();
      });
  }
});