import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  // Go to discover page
  await page.goto('/discover');
  // Log in
  await page.getByPlaceholder("Email\\, phone\\, or Skype").fill(process.env.AADUSERNAME);
  await page.getByPlaceholder("Email\\, phone\\, or Skype").press('Enter');
  await page.getByPlaceholder("Password").fill(process.env.AADPASSWORD);
  await page.locator('input:has-text("Sign in")').click();
  await page.getByText('No').click();  
  await expect(page).toHaveURL('/discover');
  await expect(page).toHaveTitle('.NET Podcasts');
});

test.describe('Listen Together', () => {
  test('should allow me to listen together', async ({ page }) => {s
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