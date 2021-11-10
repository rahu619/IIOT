# IIOT

A basic project for mimicking an IIOT environment. 

## Description

A basic project for mimicking an IIOT environment. 
   

## Getting Started

### Dependencies

* MQTTnet library
* SignalR
* EntityFrameworkCore
* SqLite


### Executing program

* The following command can be executed at the solution level, to spin up the Broker, Simulator, API and Consumer Services.
```
docker-compose up --build
```

* As UI service hasn't been included in the compose file yet, a simple hack might be to use the live server in VS Code to view the **index.html**
* The UI project will be relying on the **localhost:5000** API endpoint for retrieving real-time changes from the server.


## Help

The executables could also be run locally by starting the following services from their respective bin folders,

* **IIOT.MessageBroker.exe**
* **IIOT.Simulator.exe**
* **IIOT.API.exe**

And the message broker server will have to changed from **iiot.messagebroker** to **localhost** in the Simulator and API appsettings.json files.



