import { test, expect } from '@playwright/test';

test.describe('Listen Together', () => {
  test('should allow me to listen together', async ({ page }) => {
    await page.goto('/discover');
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click play
    await page.locator('.icon-play').first().click();
    // click go to listen together page
    await page.getByRole('link', { name: 'Listen Together' }).click();
    await expect(page).toHaveURL('/listen-together');
    // create new room
    await page.getByRole('button', { name: 'Create new room' }).click();
    await page.getByPlaceholder('Your name').fill('test');
    // open room
    await page.getByRole('button', { name: 'Open room' }).click();
    // leave the room
    await page.getByRole('button', { name: 'Leave the room' }).click();
  });
});