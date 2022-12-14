import { test, expect } from '@playwright/test';

test.describe('Subscriptions', () => {
  test('should allow me to subscribe', async ({ page }) => {
    await page.goto('/discover');
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click subscribe
    await page.getByRole('button', { name: 'Subscribe' }).click();
    // view subscriptions
    await page.getByRole('link', { name: 'Subscriptions' }).click();
    await expect(page).toHaveURL('/subscriptions');
    // assert subscriptions are shown
    await expect(page.locator('.containerPage')).not.toHaveClass('no-results');
  });
});