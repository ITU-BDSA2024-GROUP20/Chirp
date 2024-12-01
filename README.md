# Chirp
Analysis, Design and Software Architecture opgave


## how to create a new Migration

Navigate to `\src\` folder

Use the following commands:

- `dotnet ef migrations add [name] -p .\Chirp.Infrastructure\  -s .\Chirp.Web\`
- `dotnet ef database update -p .\Chirp.Infrastructure\ -s .\Chirp.Web\    `

If an old migration is already in the Migrations folder, consider deleting it.

to run this localhost you need to run these comands
- ```dotnet user-secrets set "authentication_github_clientId" "Ov23liGbRbgORjmb9wUp"```
- ```dotnet user-secrets set "authentication_github_clientSecret" "0293ae8fdb1f1b046f42ab98234b11469648708e"```
