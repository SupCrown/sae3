using CTVR_API;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ctvr_api.Controllers
{
    public class AccidentsController : ControllerBase
    {
        private IConfiguration _configuration;
        public AccidentsController(IConfiguration configuration) { 
            _configuration = configuration;
        }

        public JsonResult getAllAccidents()
        {
            string query = "SELECT * FROM accidents";

            List<Utilisateur> users = new List<Utilisateur>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {

            }
        }
    }
}
