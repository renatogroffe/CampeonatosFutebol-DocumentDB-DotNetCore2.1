using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;

namespace APIDadosPremierLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremierLeagueController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(
            [FromServices]DocumentDBConfigurations configurations)
        {
            using (var client = new DocumentClient(
                new Uri(configurations.EndpointUri),
                        configurations.PrimaryKey))
            {
                FeedOptions queryOptions =
                    new FeedOptions { MaxItemCount = -1 };

                return new ObjectResult(
                    client.CreateDocumentQuery(
                        UriFactory.CreateDocumentCollectionUri(
                            configurations.Database,
                            configurations.Collection),
                        "SELECT c.id, c.NomeCampeonato, c.Esporte, c.Pais, " +
                        "c.Equipes[0].Nome AS Lider, c.Equipes[19].Nome AS Ultimo " +
                        "FROM c " +
                        "WHERE c.NomeCampeonato = 'Premier League'",
                        queryOptions).ToList());
            }
        }
    }
}