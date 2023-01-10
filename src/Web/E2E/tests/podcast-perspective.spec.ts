import { test, expect } from '@playwright/test';

// From the perspective of a podcast listener what can that listener do
// Listener can subscribe, unsubscribe, see subscriptions, listen now, listen later and listen together

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
  await page.locator('.card').first().click();
  await expect(page).toHaveURL(/.*show*/);
});

test.describe('Subscriptions', () => {

  test('should allow me to subscribe and unsubscribe', async ({ page }) => { 
    const subscribe = page.getByRole('button', { name: /subscribe$/i });
    const subscribed = page.getByRole('button', { name: /subscribed/i});
    await expect(page.getByRole('heading', { name: 'Episodes'})).toBeVisible();
    await subscribe.click();
    await expect(subscribed).toBeVisible();
    await subscribed.click();
    await expect(subscribe).toBeVisible();
  });

  test('should allow me to see subscriptions', async ({ page }) => {
    await expect(page.getByRole('heading', { name: 'Episodes'})).toBeVisible();
    await page.getByRole('button', { name: /subscribe$/i }).click();
    await page.getByRole('link', { name: 'Subscriptions' }).click();
    await expect(page).toHaveURL('/subscriptions');
    await expect.poll(() =>
      page.locator('.card').count()).toBeGreaterThan(0);
  });
});

test.describe('Listen now', () => {

  test('should allow me to play and pause a podcast', async ({ page }) => {
    await page.getByTitle('Play').first().click();
    await expect(page.locator('.player-bars')).toBeVisible();
    await expect(page.getByTitle('Pause').first()).toBeVisible();
    page.getByTitle('Pause').first().click();
  });

});

test.describe('Listen Later', () => {

  test('should allow me to listen to podcast later', async ({ page }) => {
    await page.getByTitle('Listen Later').first().click();
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await expect(page).toHaveURL(/.*listen-later*/);
    await expect(page.locator('.episode')).toBeVisible();
  });

  test('should not contain podcasts when listen later is deactivated', async ({ page }) => {
    await page.getByTitle('Listen Later').first().dblclick();
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await expect(page.getByRole('heading', { name: 'You haven\'t added any podcasts yet.' })).toBeVisible();
  });

});

test.describe('Listen Together', () => {
  test('should allow me to listen together', async ({ page }) => {
    await page.getByTitle('Play').first().click();
    await page.getByRole('link', { name: 'Listen Together' }).click();
    await expect(page).toHaveURL('/listen-together');
    await page.getByRole('button', { name: 'Create new room' }).click();
    await page.getByPlaceholder('Your name').fill('test');
    await page.getByRole('button', { name: 'Open room' }).click();
    await page.getByRole('button', { name: 'Leave the room' }).click();
  });
});
