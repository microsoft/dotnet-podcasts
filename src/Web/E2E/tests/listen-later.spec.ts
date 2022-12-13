import { test, expect } from '@playwright/test';

test.describe('Listen Later', () => {
  test('should allow me to listen to podcast later', async ({ page }) => {
    await page.goto('/discover');
    // click first podcast in list
    await page.locator('.item-primary-action').first().click();
    // click first listen later button
    await page.locator('button.buttonIcon.episode-actions-later').first().click();
    // view listen later tab
    await page.getByRole('link', { name: 'Listen Later' }).click();
    await expect(page).toHaveURL('/listen-later');
    // assert no results page isn't shown
    await expect(page.locator('main')).not.toContainText('no results'); 
  });
});