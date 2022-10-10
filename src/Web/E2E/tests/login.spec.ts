import { test, expect, Page } from '@playwright/test';

test.describe('Login', () => {
  test('should allow me to login', async ({ page }) => {
    await page.goto('/discover');
    // Fill username via env variable
    await page.locator('[placeholder="Email\\, phone\\, or Skype"]').fill('marcus.felling@gmail.com');
    // Press Enter
    await page.locator('[placeholder="Email\\, phone\\, or Skype"]').press('Enter');
    // // Fill password via env variable
    await page.locator('[placeholder="Password"]').fill('MSFTagrosk8!');
    // Click Sign In
    await page.locator('input:has-text("Sign in")').click();
    // Click text=Yes
    await page.locator('text=Yes').click();
    // If new AAD permissions are presented, click Yes
    const terms = await page.$('text=Yes');
    if (terms) {
    await terms.click();
    }
    // assert discover page is shown
    await expect(page).toHaveURL('/discover');
    expect(page).toHaveTitle('.NET Podcasts')
  });
});