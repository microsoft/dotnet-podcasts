import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  // Go to discover page
  await page.goto('/discover');
});

test.describe('Listen Together', () => {
  test('should allow me to listen together', async ({ page }) => {
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click play
    await page.locator('.icon-play').first().click();
    // click go to listen together page
    await page.getByText('ListenTogether').click();
    await expect(page).toHaveURL('/listen-together');
    // assert Create new room button isn't disabled
    expect(page.locator('.buttonApp.primary >> text=Create new room')).toBeEnabled
    // create new room
    await page.locator('.buttonApp.primary >> text=Create new room').click();
    await page.getByPlaceholder("Your name").fill('test');
    // open room
    await page.locator('button:has-text("Open room")').click();
    // leave the room
    await page.getByText('Leave the room').click();
  });
});