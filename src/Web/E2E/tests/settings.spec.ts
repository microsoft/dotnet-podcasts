import { test, expect } from '@playwright/test';

test.describe('Settings', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/settings');
  });

  test.use({ colorScheme: 'dark' });
  
  test('should toggle auto download', async ({ page }) => {
    const autoDownload = await page.getByLabel('Autodownload using data');
    await autoDownload.check();
    await autoDownload.uncheck();
  })

  test('should toggle delete played episodes', async ({ page }) => {
    const deletePlayed = await page.getByLabel('Delete played episodes');
    await deletePlayed.check();
    await deletePlayed.uncheck();
  })

  test('should toggle system mode', async ({ page }) => {
    const systemMode = await page.getByLabel('Use System Theme (Dark)');
    await systemMode.check();
    await expect(page.getByLabel('Dark Theme')).toBeDisabled();
    await expect(page.locator('body')).toHaveAttribute('data-theme', 'Dark');
    await systemMode.uncheck();
    await expect(page.getByLabel('Dark Theme')).toBeEnabled();
  })

  test('should toggle dark and light mode', async ({ page }) => {
    const darkTheme = await page.getByLabel('Dark Theme');
    const body = await page.locator('body')
    await expect(darkTheme).toBeChecked();
    await expect(body).toHaveAttribute('data-theme', 'Dark');
    await darkTheme.uncheck();
    await expect(body).not.toHaveAttribute('data-theme', 'Dark')
  })
});
