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

test.describe('Subscriptions', () => {
  test('should allow me to subscribe', async ({ page }) => {
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click subscribe
    await page.locator('button:has-text("Subscribe")').click();
    // view subscriptions
    await page.locator('.navbarApp-item >> text=subscriptions').click();
    await expect(page).toHaveURL('/subscriptions');
    // assert subscriptions are shown
    expect(page.locator('.main')).not.toContain('You haven’t subscribed to any channel yet.');
  });
});