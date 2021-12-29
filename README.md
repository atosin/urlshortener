# urlshortener
Url Shortener prototype

Application Stack

MongoDb -> .NET Core -> React

Before you run:

Please ensure that you are using linux containers. Right click on the docker desktop icon on the system tray and click on "switch to linux containers" if the option appears. Also, if you have MongoDb running as a service on the computer, stop the service before running to avoid conflicts.

To run:

Navigate to the server folder and run "docker-compose up". The container consists of a database image, the server image, and a separate client image. 

Navigate to "http://localhost:3000/" to launch the app.

Unit Tests:
Server Unit Tests are run as part of the containerization build. This was done since it is more likely that the poeple will not have access to the C# environment. The client tests are separate and can be run by running "npm test" in the client directory.
