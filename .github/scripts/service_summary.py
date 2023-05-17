import os
import subprocess
import json
from pathlib import Path

repo_dir = Path(__file__).parent.parent.parent

# export GITHUB_OUTPUT=~/GitHub/colbylwilliams/dotnet-podcasts/.local/step_output.txt
# export GITHUB_ENV=~/GitHub/colbylwilliams/dotnet-podcasts/.local/step_env.txt
# export GITHUB_STEP_SUMMARY=~/GitHub/colbylwilliams/dotnet-podcasts/.local/step_summary.md


def write_output(key, value, env_key=None):
    if output := os.environ.get('GITHUB_OUTPUT', None):
        with open(output, 'a+') as f:
            f.write(f'{key}={value}\n')
    else:
        print(f'OUTPUT: {key}={value}')

    if env_key:
        if env_vars := os.environ.get('GITHUB_ENV', None):
            with open(env_vars, 'a+') as f:
                f.write(f'{env_key}={value}\n')
        else:
            print(f'ENVIRONMENT: {env_key}={value}')


def write_summary(messages):
    if isinstance(messages, str):
        messages = [messages]

    if step_summary := os.environ.get('GITHUB_STEP_SUMMARY', None):
        with open(step_summary, 'a+') as f:
            for message in messages:
                f.write(f'{message}\n')
    else:
        for message in messages:
            print(message)


def get_row(envs, service):
    repo_name = repo_dir.name
    acr_name = envs['AZURE_CONTAINER_REGISTRY_NAME']
    img_prefix = f'{acr_name}.azurecr.io/{repo_name}/'

    row = [service]
    row.append(envs.get(f'SERVICE_{service.upper()}_NAME', ''))

    url = envs.get(f'REACT_APP_{service.upper()}_BASE_URL', 'n/a')

    if service == 'web':
        write_summary(f'- View the website: {url}')

    if service == 'api':
        url = url + '/swagger'
        write_summary(f'- Explore the API: {url}')

    if service == 'hub':
        url = url + '/listentogether'

    if service in ['web', 'api']:
        write_output(service, url, f'ADE_{service.upper()}_URL')

    if service in ['web', 'api', 'hub']:
        url = f'[visit {service}]({url})'

    row.append(url)

    # portal = os.environ.get('ADE_PORTAL_URL', None)

    img = envs.get(f'SERVICE_{service.upper()}_IMAGE_NAME', '')
    img_name = img.removeprefix(img_prefix).split(':')[0]
    img_tag = img.removeprefix(img_prefix).split(':')[1].removeprefix('azd-deploy-')

    row.append(f'[{img_name}]({img})')
    row.append(f'[{img_tag}]({img})')

    return row


def get_table():
    env = subprocess.run([
        'azd', 'env', 'get-values', '--output', 'json'
    ], cwd=repo_dir, capture_output=True, check=True, text=True)

    envs = json.loads(env.stdout)

    services = ['web', 'api', 'hub', 'ingestion', 'updater']
    table = [['Service', 'Name', 'Url', 'Image', 'Tag'], ['-' * 10, '-' * 28, '-' * 105, '-' * 113, '-' * 101]]

    for service in services:
        table.append(get_row(envs, service))

    return table


write_summary('### Deployment Successful :rocket:\n')

if portal_url := os.environ.get('ADE_PORTAL_URL', None):
    write_summary(f'The app has been deployed to Azure for review and can be accessed [here]({portal_url})\n')

table = get_table()

write_summary('\n#### Services Deployed:\n')

write_summary(['| {: <10} | {: <28} | {: <105} | {: <113} | {: <101} |'.format(*row) for row in table])
