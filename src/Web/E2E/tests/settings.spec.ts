import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/settings');
});

test.describe('Settings', () => {
  // Loop through each setting
  const settings = ['autodownload', 'deleteplayed', 'systemtheme', 'darktheme'];
  for (const setting of settings) {
    test(`should allow me to toggle setting ${setting}`, async ({ page }) => {
      // toggle setting
      await page.locator(`input[name="${setting}"]`).check();
      await page.locator(`input[name="${setting}"]`).uncheck();
    });
  }
});