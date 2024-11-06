# Chirp
Analysis, Design and Software Architecture opgave


## Migration

navigate to src

`dotnet ef migrations add [name] -p .\Chirp.Infrastructure\  -s .\Chirp.Web\`
`dotnet ef database update -p .\Chirp.Infrastructure\ -s .\Chirp.Web\    `