# FoosballAPI

Very simple REST API with CQRS - not yet fully ready

# Structure of the code

* FoosballAPI - Web API + business logic
* FoosballAPI.Infrastructure - the infrastructure code
* APITester -  the console applications that calls API for testing purposes

# Requirements

The FoosballAPI needs a local database. Alternatively you can modify a connection string in appsettings.json

# TODO

* To consider if EF should be hidden behind repositories.
* Move validations from Execute methods to separated classes (validators).
* Paging in query handlers.
* Optimistic concurrency i.e. Version.
* Introduce dedicated classes for exceptions instead of using generic Exception class.
* Implements events and events dispatching mechanism.
* Currently the read model and the write model use the same database and the same DbContext. It should be separated. 
* Write unit and integration tests.
* Use some automapper instead of manually mapping properties e.g. from a request to a command, from an entity to a result.
* Move things like max number of sets or max number of goals to settings.



