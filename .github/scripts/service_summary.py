import os
import subprocess
import json
from pathlib import Path

repo_dir = Path(__file__).parent.parent.parent


def get_row(envs, service):
    repo_name = repo_dir.name
    acr_name = envs['AZURE_CONTAINER_REGISTRY_NAME']  # ex: crvaktkqxgylcrs
    img_prefix = f'{acr_name}.azurecr.io/{repo_name}/'

    base_url = envs.get(f'REACT_APP_{service.upper()}_BASE_URL', 'n/a')

    row = [service]
    row.append(envs.get(f'SERVICE_{service.upper()}_NAME', ''))
    row.append(base_url + '/swagger' if service == 'api' else base_url + '/listentogether' if service == 'hub' else base_url)
    row.append(envs.get(f'SERVICE_{service.upper()}_IMAGE_NAME', '').removeprefix(img_prefix))

    return row


def get_table():
    env = subprocess.run([
        'azd', 'env', 'get-values', '--output', 'json'
    ], cwd=repo_dir, capture_output=True, check=True, text=True)

    envs = json.loads(env.stdout)

    services = ['web', 'api', 'hub', 'ingestion', 'updater']
    table = [['Service', 'Name', 'Url', 'Image'], ['-' * 10, '-' * 28, '-' * 96, '-' * 46]]

    for service in services:
        table.append(get_row(envs, service))

    return table


# step_summary = str(repo_dir / '.local' / 'step_summary.md')
if step_summary := os.environ.get('GITHUB_STEP_SUMMARY', None):
    with open(step_summary, 'a+') as f:
        f.write('# Services Deployed\n\n')
        for row in get_table():
            f.write('| {: <10} | {: <28} | {: <96} | {: <46} |\n'.format(*row))
else:
    for row in get_table():
        print('| {: <10} | {: <28} | {: <96} | {: <46} |'.format(*row))
