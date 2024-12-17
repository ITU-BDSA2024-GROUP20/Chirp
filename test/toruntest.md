# Runing the test

### Unit tests
To run the unit test for this program you will need to open a terminal and<br> navigate to the ```Chirp.Razor.Test``` directory which can be done from the root of the repository with:
```
cd .\test\Chirp.Razor.Test\ 
```
Now run ```dotnet test```

### Integration and end-to-end tests
Before runing these tests, please delete the mychirp.db file in the ```\src\Chirp.Web``` directory and do the same when the tests are done running.

To run integration and end-to-end tests there are some pre-requisites.<br>
To install Playwright, navigate to the root of Chirp, then run:
```
cd .\test\Chirp.Web.Test\ 
```
Build the program with:
```
dotnet build
```
Then run
```
pwsh bin/Debug/net8.0/playwright.ps1 install
```
Followed by
``` 
npx playwright install --with-deps
```
When Playwright is installed, open a separate terminal and navigate to the Chirp repository root and run:
``` 
cd .\src\Chirp.Web
```
And then run:
``` 
dotnet run
```
When the program is running, go to the terminal that is in ```.\test\Chirp.Web.Test``` and run the test with
``` 
dotnet test
```
Now, as the test are running you should see a browser popup where things are happening. That is the tests running.<br>
At some point, a GitHub window may appear where it asks you to authorize, please press the button otherwise the test will fail.