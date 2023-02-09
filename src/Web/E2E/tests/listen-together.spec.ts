import { test, expect } from '@playwright/test';

test.describe('Listen Together', () => {
  test('should allow me to listen together', async ({ page }) => {
    await page.goto('/discover');
    await page.locator('.item-primary-action').first().click();
    await page.locator('.icon-play').first().click();
    await page.getByRole('link', { name: 'Listen Together' }).click();
    await expect(page).toHaveURL('/listen-together');
    await page.getByRole('button', { name: 'Create new room' }).click();
    await page.getByPlaceholder('Your name').fill('test');
    await page.getByRole('button', { name: 'Open room' }).click();
    await page.getByRole('button', { name: 'Leave the room' }).click();
  });
});
