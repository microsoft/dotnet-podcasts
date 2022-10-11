import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  // Go to discover page
  await page.goto('/discover');
  // Log in
  await page.locator('[placeholder="Email\\, phone\\, or Skype"]').fill(process.env.AADUSERNAME);
  await page.locator('[placeholder="Email\\, phone\\, or Skype"]').press('Enter');
  await page.locator('[placeholder="Password"]').fill(process.env.AADPASSWORD);
  await page.locator('input:has-text("Sign in")').click();
  await page.locator('text=No').click();  
});

test.describe('Listen Together', () => {
  test('should allow me to listen together', async ({ page }) => {s
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click play
    await page.locator('.icon-play').first().click();
    // click go to listen together page
    await page.locator('text=ListenTogether').click();
    await expect(page).toHaveURL('/listen-together');
    // assert Create new room button isn't disabled
    expect(page.locator('.buttonApp.primary >> text=Create new room')).toBeEnabled
    // create new room
    await page.locator('.buttonApp.primary >> text=Create new room').click();
    await page.locator('[placeholder="Your name"]').fill('test');
    // open room
    await page.locator('button:has-text("Open room")').click();
    // leave the room
    await page.locator('text=Leave the room').click();
  });
});