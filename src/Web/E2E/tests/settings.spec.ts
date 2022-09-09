/* eslint-disable notice/notice */

import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
});

test.describe('Settings', () => {
  test('should allow me to toggle settings', async ({ page }) => {
    // Click text=Settings
    await page.locator('text=Settings').click();
    await expect(page).toHaveURL('/settings');

    // Toggle autodownload
    await page.locator('input[name="autodownload"]').check();

    // Toggle deleteplayed
    await page.locator('input[name="deleteplayed"]').check();

    // Toggle systemtheme
    await page.locator('input[name="systemtheme"]').check();
    });
});





