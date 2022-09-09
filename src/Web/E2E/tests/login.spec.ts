/* eslint-disable notice/notice */

import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('');
});

test.describe('Login', () => {
  test('should allow me to login', async ({ page }) => {
    await page.locator('text=Sign In').click();
    await expect(page).toHaveURL('/discover');  
  });
});





