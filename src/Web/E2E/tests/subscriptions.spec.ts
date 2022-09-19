import { test, expect, Page } from '@playwright/test';

test.describe('Subscriptions', () => {
  test('should allow me to subscribe', async ({ page }) => {
    await page.goto('/discover');
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click subscribe
    await page.locator('button:has-text("Subscribe")').click();
    // view subscriptions
    await page.locator('.navbarApp-item >> text=subscriptions').click();
    await expect(page).toHaveURL('/subscriptions');
    // assert subscriptions are shown
    expect(page.locator('.main')).not.toContain('You haven’t subscribed to any channel yet.');
  });
});