/* eslint-disable notice/notice */

import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/settings');
});

test.describe('Settings', () => {
  test('should allow me to toggle settings', async ({ page }) => {
    // loop through each setting
    for (const setting of ['autodownload', 'deleteplayed', 'systemtheme', 'darktheme']) {   
      // toggle setting
      await page.locator('input[name="' + setting + '"]').check();
      await page.locator('input[name="' + setting + '"]').uncheck();
    }
  });
});




