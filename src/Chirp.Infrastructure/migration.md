## how to create a new Migration

Navigate to `\src\` folder

Use the following commands:

- `dotnet ef migrations add [name] -p .\Chirp.Infrastructure\  -s .\Chirp.Web\`
- `dotnet ef database update -p .\Chirp.Infrastructure\ -s .\Chirp.Web\    `

If an old migration is already in the Migrations folder, consider deleting it.