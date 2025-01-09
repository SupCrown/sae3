using CTVR_API;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ctvr_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : Controller
    {
        private IConfiguration _configuration;
        public RolesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult GetRoles()
        {
            string query = "SELECT * FROM roles ORDER BY libelle ASC";

            List<Role> roles = [];
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des lignes renvoyées
                    {
                        Dictionary<string, string?> roleFields = [];
                        roleFields.Add("code", reader.GetString(0));
                        roleFields.Add("libelle", reader.GetString(1));
                        roles.Add(new Role(roleFields));
                    }
                }
            }

            return new JsonResult(roles);

        }
    }
}
