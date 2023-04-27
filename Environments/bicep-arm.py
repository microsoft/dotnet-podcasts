# ------------------------------------
# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
# ------------------------------------

import os
import shutil
import subprocess

from pathlib import Path

environments_path = Path(__file__).resolve().parent

environments = []

print('Building ARM templates from bicep files...')
# walk the Environments directory and find all the child directories
for dirpath, dirnames, files in os.walk(environments_path):
    # os.walk includes the root directory (i.e. repo/Environments) so we need to skip it
    if not environments_path.samefile(dirpath) and Path(dirpath).parent.samefile(environments_path):
        environments.append(Path(dirpath))

# get the full path to the azure cli executable
az = shutil.which('az')

for environment in environments:
    print(f'  Compiling template: {environment}')
    # run the azure cli command to compile the template
    subprocess.run([az, 'bicep', 'build', '--file', environment / 'main.bicep', '--outfile', environment / 'azuredeploy.json'])

print('Done')
