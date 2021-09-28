# Pokedex APIs
Project for technical screening at TrueLayer. REST APIs that return Pokemon information.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

What things you need to install the software and how to install them

```
.Net 5.0
Docker (to run container)
```

## Developed With

* ASP.Net Core 5.0 (Web API)

## Tests

* It has Unit Tests and Integration Tests to verify.

## Authors

* **Pankaj Kansodariya** - [Pokedex](https://github.com/pankaj-kansodariya/Pokedex)

## Acknowledgments

* Please make necessary changes as per your requirements.

## Content Type

* Content Type for all APIs is JSON

# End points
* GET /pokemon/\{pokemon name\}
* GET /pokemon/translated/\{pokemon name\} 

* Both APIs return following response type in case of Success
<pre>
{<code> 
    name: string (nullable)
    description: string (nullable)
    habitat: string (nullable)
    isLegendary: boolean</code>
}
</pre> 
    

* Both APIs return following response type in case of failure
  - Possible Status Codes
    - 404 - Not Found - When Pokemon name doesn't exist.
    - 404 - Bad Request - When Pokemon name not provided.
    - 500 - Internal Error - Unexpected error during whole operation.
<pre><code>string (reason of failure)</code></pre>.
    

## Downstream Dependencies
* Pokemon API - https://pokeapi.co/
* Translation API - https://api.funtranslations.com/


## PRODUCTION Deployment
* Logging - Currently it logs on Console but other loggings (Local File, Third Party, etc) can be added if required


## Instruction to Run
* Please follow following steps once code is downloaded
1. Visual Studio IDE
    - Right Click on Solution and select "Restore NuGet Packages"
    - Right Click on Solution and select "Rebuild Solution"
    - Make sure that Pokedex.API as Startup project.
      - If not then Right Click on Pokedex.API project and select "Set as Startup Project"
    - Select appropriate launch profile
      - IIS Express (Browser)
      - Pokedex.API (Kestrel - .Net Core's own web server - Console based)
      - Docker (You should have Docker for Desktop (Windows Containers) on your machine, not configured for Linux )
    - To run tests, 
      - Open Test Explorer and select and run the test you want to verify. 
2. Command Prompt
    - Navigate to root directory (Solution level) 
    - "dotnet restore"
    - "dotnet build"
    - "dotnet run --project Pokedex.API"
      - URL will be shown on console screen.
    - To run tests
      - "dotnet test"
    - 