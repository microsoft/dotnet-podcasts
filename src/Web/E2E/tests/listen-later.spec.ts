/* eslint-disable notice/notice */

import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
});

test.describe('Listen Later', () => {
  test('should allow me to listen to podcast later', async ({ page }) => {
    // Click first podcast in list
    await page.locator('.item-primary-action').first().click();

    // Click first listen later button
    await page.locator('button.buttonIcon.episode-actions-later').first().click();

    // View listen later tab
    await page.locator('text=ListenLater').click();
    await expect(page).toHaveURL('/listen-later');    
  });
});





