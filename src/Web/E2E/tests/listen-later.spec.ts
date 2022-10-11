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

test.describe('Listen Later', () => {
  test('should allow me to listen to podcast later', async ({ page }) => {
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click first listen later button
    await page.locator('button.buttonIcon.episode-actions-later').first().click();
    // view listen later tab
    await page.locator('.navbarApp-item >> text=ListenLater').click();
    await expect(page).toHaveURL('/listen-later');
    // assert no results page isn't shown
    expect(page.locator('.main')).not.toContain('no results');
  });
});