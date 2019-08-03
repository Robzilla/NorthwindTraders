# API Layer

This layer exposes all business services such as CreateCustomer via Azure Functions
The .gitignore allows local.settings.json to be stored in git as this project uses localdb
Don't store any secrets in the local.settings.json file.
If you do need to store secrets look at strategies like using secret.settings.json
see https://www.tomfaltesek.com/azure-functions-local-settings-json-and-source-control/
Don't forget to add secret.settings.json to the .gitignore file