/* eslint-disable notice/notice */

import { test, expect, Page } from '@playwright/test';

test.beforeEach(async ({ page }) => {
  await page.goto('/discover');
});

test.describe('Discover', () => {
  test('should allow me to browse categories', async ({ page }) => {
    // Microsoft
    await page.locator('ul >> text=Microsoft').click();
    await expect(page).toHaveURL('/category/5f923017-86da-4793-9332-7b74197acc51');
    await page.locator('button:has-text("Back")').click();

    // Mobile
    await page.locator('ul >> text=Mobile').click();
    await expect(page).toHaveURL('/category/2f07481d-5f3f-4bbf-923f-60e62fcfe4e7');
    await page.locator('button:has-text("Back")').click();

    // Community
    await page.locator('text=Community').click();
    await expect(page).toHaveURL('/category/a5ae013c-14a1-4c2d-a731-47fbbd0ba527');
    await page.locator('button:has-text("Back")').click();

    // M365
    await page.locator('text=M365').click();
    await expect(page).toHaveURL('/category/bee871ad-750b-400b-91b0-c34056c92297');
    await page.locator('button:has-text("Back")').click();

    // See all categories
    await page.locator('text=See all categories').click();
    await expect(page).toHaveURL('/categories');
    await page.locator('button:has-text("Back")').click();
  });

  test('should allow me to search', async ({ page }) => {
    await page.locator('[placeholder="Search here"]').click();
    await page.locator('[placeholder="Search here"]').fill('.NET');
    await page.locator('[placeholder="Search here"]').press('Enter');

  });  
});





