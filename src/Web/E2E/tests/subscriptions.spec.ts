import { test, expect } from '@playwright/test';

test.describe('Subscriptions', () => {

  test('should start with no subscriptions', async ({ page }) => {
    await page.goto('/subscriptions');
    await expect(page.getByRole('heading', { name: 'You haven’t subscribed to any channel yet.' })).toBeVisible();
    await page.getByRole('link', { name: 'Discover podcasts' }).click();
    await expect(page).toHaveURL('/discover');
  });

  test.fixme('should allow me to subscribe and unsubscribe', async ({ page }) => { 
    await page.goto('/discover');
    await page.getByTitle('Subscribe').first().click();
    await page.getByRole('link', { name: 'Subscriptions' }).click();
    await expect.poll(() =>
      page.locator('.card').count()).toBeGreaterThan(0);
    await page.getByTitle('Unsubscribe').click();
    await expect(page.getByRole('heading', { name: 'You haven\'t subscribed to any podcasts yet.' })).toBeVisible();
  });

});
