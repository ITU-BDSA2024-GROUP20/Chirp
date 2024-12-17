# Chirp - Analysis, Design and Software Architecture 2024
This is a repository for the Chirp Application


# How to run
## Web-app
There is a runing version at this [link](https://bdsagroup20chirprazor-hdb4bch7ejb3abbd.northeurope-01.azurewebsites.net)

## Locally
In order to run Chirp there are 2 options
1. Clone the repository
2. Run the release

### pre-requisites
Make sure you have dotnet 8.0 installed see [download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)


### Cloned Repository
To run this locally from a cloned repository, please do the following:

Clone the repository with this git command:
```
git clone https://github.com/ITU-BDSA2024-GROUP20/Chirp.git
```
Then go to the Chirp.Web directory with:
```
cd .\src\Chirp.Web\
```
Now run these commands inside the directory
- ```dotnet user-secrets set "authentication_github_clientId" "Ov23liGbRbgORjmb9wUp"```
- ```dotnet user-secrets set "authentication_github_clientSecret" "0293ae8fdb1f1b046f42ab98234b11469648708e"```

You should now be able to run Chirp with:```dotnet run``` and access it at http://localhost:5273 when it is running

## Release
To run the release first go to the main page of the Repository and click on the release section.<br>
Find the latest version and download one of the following files depending on your operations system:
- Chirp-Win.zip, for Windows users
- Chirp-Mac.zip, for Mac users
- Chirp-Linux.zip , for Linux users<br>

When the file has been downloaded please unzip it.<br>
Then open a terminal and navigate to one of the following directories depending on your operations system:
- Chirp-Win\artifact\win, for windows
- Chirp-Mac\artifact\mac, for mac
- Chirp-Linux\artifact\linux, for linux<br>

Now run the following commands in the terminal.

- ```dotnet dev-certs https -t```
- ```./Chirp.Web  --urls="http://localhost:5273" --"authentication_github_clientId" "Ov23liGbRbgORjmb9wUp" --"authentication_github_clientSecret" "0293ae8fdb1f1b046f42ab98234b11469648708e" --development```

When running the application, and it is done starting up, a popup will appear in your terminal indicating which port it is running on "http://localhost:5273"