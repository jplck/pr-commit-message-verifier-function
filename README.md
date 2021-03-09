# Azure DevOps Pull Request verfification with Azure Function

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fjplck%2Fpr-commit-message-verifier-function%2Fmain%2Fscripts%2Fdeploy.json)

![PR Update Architecture](./docs/pr_update.png)

## Steps
The following steps guide you through the setup of this example project.

### Settings up a service hook in Azure DevOps
To wire up the Pull Request Event (create/upgrade) to the Azure function, you have to setup a service hook first. The setup is easily done via the Azure DevOps UI. 

In your project settings, open the service hooks entry. From there you can choose the type of hook you want to use. In this example I have used an Azure Service Bus. To setup your function with a different hook type, change the trigger binding in PRTriggerFunc.cs.

Take all the neccessary information for the hook setup from a previously created service bus namespace and queue.

### Setting up Azure Function

### Testing
