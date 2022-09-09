/* eslint-disable notice/notice */

import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
});

test.describe('Listen Together', () => {
  test('should allow me to listen together', async ({ page }) => {
    // Click first podcast in list
    await page.locator('.item-primary-action').first().click();
    
    // Click play
    await page.locator('.icon-play').first().click();

    // Click go to listen together page
    await page.locator('text=ListenTogether').click();
    await expect(page).toHaveURL('/listen-together');

    // Create new room
    await page.locator('button:has-text("Create new room")').click();
    await page.locator('[placeholder="Your name"]').fill('test');

    // Open room
    await page.locator('button:has-text("Open room")').click();

    // Leave the room
    await page.locator('text=Leave the room').click();
  });
});





