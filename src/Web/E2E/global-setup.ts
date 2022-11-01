// @ts-nocheck
import { chromium, FullConfig } from '@playwright/test';

async function globalSetup(config: FullConfig) {
  const browser = await chromium.launch();
  const page = await browser.newPage();
  await page.goto(process.env.BASEURL);
  // Sign in using creds from env variables
  await page.getByTitle('Sign In').click();
  await page.getByPlaceholder("Email\\, phone\\, or Skype").fill(process.env.AADUSERNAME);
  await page.getByPlaceholder("Email\\, phone\\, or Skype").press('Enter');
  await page.getByPlaceholder("Password").fill(process.env.AADPASSWORD);
  await page.locator('input:has-text("Sign in")').click();
  // Save signed-in state to 'storageState.json'.
  await page.context().storageState({ path: 'storageState.json' });
  await browser.close();
}

export default globalSetup;