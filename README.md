# Chat System (Server side)
This project was generated with .NET 5.0
###### DB setup
* Download and install MS Sql server on your system by visiting here (Ignore if you have already installed).
* Download SQL server management studio also, if you don't have already installed SSMS.
###### Connect to database engine
After the installation open Microsoft sql server management studio and connect it to your database engine. For further instructions visit Microsoft SSMS doc here.
Importing database
* Click on New query on the toolbar of Sql server management studio. This will open a new script window.
* Copy the script of script.sql file (inside db directory) on the script window.
* Now click on Execute query. This will create the database for the project.
## Project setup
###### Dowload Tools
Download visual studio from here Visual studio 2019 (If you don't already have). Install it on your system.
###### Edit DB Connection String
* Go to src\Chat.Api\Chat.Api.Web directory, open appsettings.json file.
* Find this section- `"ConnectionStrings": { "ConnectionString": "Server=<Server_name>;Database=ChatSystem;Trusted_Connection=True;MultipleActiveResultSets=True;" }`,
* Go to src\Chat.DIP.IS4 directory, open appsettings.json file.
* Find this section- `"ConnectionStrings": { "ConnectionString": "Server=<Server_name>;Database=ChatSystem;Trusted_Connection=True;MultipleActiveResultSets=True;" }`,
* Enter database server name in the place of <server_name>. Ther server name can be found on sql server management studio. 
### How to run the project with visual studio
* To start the project double click and run the Chat.sln file with visual studio.
* In the solution explorer right click on the solution file then click on properties 
** select Startup Project
** Click on multiple projects
** Set action to Start for Chat.Api.Web and Chat.DIP.IS4 projects
** Click on apply and ok
* Run the project with IISExpress by clicking on the Start button on the toolbar of visual studio. 
* Goto https://localhost:44391/index.html for API documentation
# Chat System (Client side)
This project was generated with Angular CLI version 11.0.0.
## Installing angular
* We need npm package manager to setup everything. Visit Nodejs , download and install nodejs which will install npm package manager with it.
* Now install angular cli by running **npm install -g @angular/cli** command on terminal or command prompt on your machine. Visit Angular CLI for detail.
## Project setup
###### Installing packages
* Open windows command prompt 
* Go to the location src\chat-claint-app.
* Run command **npm install** on terminal to install the packages. (This may take some time).
* After the package has been installed run **ng serve --open** to run the project on a dev server. 
* Navigate to http://localhost:4200/ on your browser.
* It needs to run both Server and Client side projects at the same time to make the project functional.
