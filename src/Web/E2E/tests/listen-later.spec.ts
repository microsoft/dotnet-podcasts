import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
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





