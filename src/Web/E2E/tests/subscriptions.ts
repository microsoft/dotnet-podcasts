/* eslint-disable notice/notice */

import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
});

test.describe('Subscriptions', () => {
  test('should allow me to subscribe', async ({ page }) => {
    // Click first podcast in list
    await page.locator('.item-primary-action').first().click();

    // Click subscribe
    await page.locator('button:has-text("Subscribe")').click();

    // View subscriptions
    await page.locator('text=Subscriptions').click();
    await expect(page).toHaveURL('/subscriptions');
    expect(page.locator('.item-primary-action')).toHaveLength(1);
  });
});





