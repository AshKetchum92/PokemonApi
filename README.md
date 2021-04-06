<pre>
██████╗  ██████╗ ██╗  ██╗███████╗███╗   ███╗ ██████╗ ███╗   ██╗     █████╗ ██████╗ ██╗
██╔══██╗██╔═══██╗██║ ██╔╝██╔════╝████╗ ████║██╔═══██╗████╗  ██║    ██╔══██╗██╔══██╗██║
██████╔╝██║   ██║█████╔╝ █████╗  ██╔████╔██║██║   ██║██╔██╗ ██║    ███████║██████╔╝██║
██╔═══╝ ██║   ██║██╔═██╗ ██╔══╝  ██║╚██╔╝██║██║   ██║██║╚██╗██║    ██╔══██║██╔═══╝ ██║
██║     ╚██████╔╝██║  ██╗███████╗██║ ╚═╝ ██║╚██████╔╝██║ ╚████║    ██║  ██║██║     ██║
╚═╝      ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═══╝    ╚═╝  ╚═╝╚═╝     ╚═╝ 
</pre>

## Requirements

* Docker

## Run

* Clone the repository and go to the solution folder

* Build the docker image
    
      docker build -f PokemonApi\Dockerfile -t pokemonapi .
      
* Run the docker image in a container

      docker run -d -p 5000:80 --rm pokemonapi

## Improvements for a production environment

* Unit tests for the clients using a mock server library, like WireMock.Net
* E2E tests that test the entire stack
* More logs
* Evaluation of a layer of cache that can be useful to reduce the invocation of the third party APIs