import { test, expect } from '@playwright/test';

test.beforeEach(async ({ page }) => {
    await page.goto('/discover');
    await page.locator('.card').first().click();
  });

test.describe('Subscriptions', () => {

  test.beforeEach(async ({ page }) => {
    await page.getByRole('button', { name: 'Subscribe' }).click();
  });

  test('should allow me to subscribe', async ({ page }) => { 
    await expect(page.getByRole('button', { name: 'Subscribed' })).toBeVisible({ timeout: 10000 });
  });

  test('should allow me to unsubscribe', async ({ page }) => {
    await page.getByRole('button', { name: 'Subscribe' }).click();
    await expect(page.getByRole('button', { name: 'Subscribe' })).toBeVisible();
  });

  test('should allow me to see subscriptions', async ({ page }) => {
    await page.getByRole('link', { name: 'Subscriptions' }).click();
    await expect(page).toHaveURL('/subscriptions');
    await expect(page.locator('.containerPage')).not.toHaveClass('no-results');
  });
});

test.describe('Listen Later', () => {

  test('should have the correct URL', async ({ page }) => {
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await expect(page).toHaveURL('/listen-later');
  });

  test('should allow me to listen to podcast later', async ({ page }) => {
    await page.getByTitle('Listen Later').first().click();
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await expect(page.locator('.episode')).toBeVisible();
  });

  test('should not contain podcasts when listen later is clicked twice', async ({ page }) => {
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await expect(page.getByRole('heading', { name: 'You haven\'t added any podcasts yet.' })).toBeVisible();
  });

  test('should be able to discover podcasts when none in listen later', async ({ page }) => {
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await page.getByRole('link', { name: 'Discover podcasts' }).click();
    expect(page).toHaveURL('/discover');
  });
});
