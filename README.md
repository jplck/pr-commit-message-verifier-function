# Azure DevOps Pull Request verfification with Azure Function
You can find steps on how to setup the infrastructure and applications in the following description.

## Architecture
![PR Update Architecture](./docs/pr_update.png)

## Setup steps

### One click environment setup
You can use the button below to setup an environment in azure directly from here. 

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fjplck%2Fpr-commit-message-verifier-function%2Fmain%2Fscripts%2Fdeploy.json)

### Setup with Azure DevOps

Prerequisites:
1. Create a service connection named "prdemoappsc" or choose a custom name. If you have choosen a custom name, make sure to change the name of the service connection in your pipeline definition.
2. Define the name of your function app in your libary as parameter "functionAppName".
3. Define the name of your resource group in your libary as parameter "rgName". 
3. Define your subscription id in your libary as parameter "subscriptionId". 
3. Define your project prefix in your libary as parameter "projectPrefix". 
3. Define your PAT in your libary as parameter "PAT".

Pipeline Setup:
1. Start the pipeline creation by selecting this GitHub repository.
2. You will be asked to configure your pipeline. Select "Existing Azure Pipeline YAML file" for the list of options. In the popup window, select the main branch and "pipelines/deploy_func.yml" as path. Just save (not save & run) the pipeline.
3. Create another pipeline as in step 2 but select "pipelines/deploy_infrastrucuture.yml" this time. You can save & run after you have setup the pipeline.
