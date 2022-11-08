import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  // Go to discover page
  await page.goto('/discover');
});

test.describe('Subscriptions', () => {
  test('should allow me to subscribe', async ({ page }) => {
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click subscribe
    await page.locator('button:has-text("Subscribe")').click();
    // view subscriptions
    await page.locator('.navbarApp-item >> text=subscriptions').click();
    await expect(page).toHaveURL('/subscriptions');
    // assert subscriptions are shown
    await expect(page.locator('main')).not.toContainText('You haven’t subscribed to any channel yet.');
  });
});