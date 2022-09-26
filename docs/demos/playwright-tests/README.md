# Testing with Playwright

The .NET Podcast app is set up to automatically run e2e tests using Playwright in [GitHub Actions](../../../.github/workflows/podcast-web.yml). The URL of the site to test is set via an environment variable `BASEURL`. In GitHub Actions, this variable is set using the value of `WEBAPP_NAME` e.g. <https://dotnetpodcasts.azurewebsites.net>

To run tests locally, you can use the cross-platform CLI, or our VS code extension.

## Prerequisites

- [Node](https://nodejs.org/en/download)
- [npm](https://docs.npmjs.com/downloading-and-installing-node-js-and-npm)

## CLI

By default tests will be run on Microsoft Edge. This can be configured in the [playwright.config](../../../src/Web/E2E/playwright.config.ts) file. Tests are run in headless mode meaning no browser will open up when running the tests. Results of the tests will be shown in the terminal and the HTML report. The HTML report automatically opens if there are are failures.

1. Open your favorite terminal.
1. Set environment variable `BASEURL` to the url of your site:

    Bash:

    ```bash
    export BASEURL='paste-the-key-value'
    ```

    PowerShell:

    ```powershell
    $env:BASEURL = 'paste-the-key-value'
    ```

1. Navigate to the tests:

    ```bash
    cd src/Web/E2E/
    ```

1. Install the package dependencies in your local directory:

    ```bash
    npm install
    ```

1. Run this command to run all of the Playwright tests:

    ```bash
    npx playwright test 
    ```

1. Open the HTML report to see more info about the results:

    ```bash
    npx playwright show-report
    ```

## VS Code

1. Install the [Playwright Test for VS Code extension](https://marketplace.visualstudio.com/items?itemName=ms-playwright.playwright) from the marketplace.
1. Set the value for BASEURL in [playwright.config](../../../src/Web/E2E/playwright.config.ts)

    e.g.

    ```typescript
      use: {
        baseURL: 'https://dotnetpodcasts.azurewebsites.net',
      },
    ```

1. Open any of the test spec files in the [tests folder](../../../src/Web/E2E/tests/).
1. Click the play button on any of the test cases.

View the docs to learn more: <https://playwright.dev/docs/next/getting-started-vscode>
