using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ctvr_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccidentsController : ControllerBase
    {
        private IConfiguration _configuration;
        public AccidentsController(IConfiguration configuration) { 
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult getAllAccidents()
        {
            string query = "SELECT * FROM accident";

            List<Dictionary<string, object?>> accidents = new List<Dictionary<string, object?>>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des lignes renvoyées
                    {
                        Dictionary<string, object?> accidentFields = new Dictionary<string, object?>();
                        accidentFields.Add("annee", reader.GetInt32(0));
                        accidentFields.Add("noSeq", reader.GetInt32(1));
                        accidentFields.Add("dateAccident", reader.GetDateTime(2));
                        accidentFields.Add("heureAccident", reader.GetTimeSpan(3));
                        accidentFields.Add("rue", reader.GetString(4));
                        accidentFields.Add("codePostal", reader.GetString(5));
                        accidentFields.Add("ville", reader.GetString(6));
                        accidentFields.Add("resumeCirconstances", reader.GetString(7));
                        accidentFields.Add("dateOuvertureDossier", reader.GetDateTime(8));
                        accidentFields.Add("dateTransmissionAssur", reader.IsDBNull(9) ? null : reader.GetDateTime(9));
                        accidentFields.Add("dateValidationAssur", reader.IsDBNull(10) ? null : reader.GetDateTime(10));
                        accidentFields.Add("dateClotureDossier", reader.IsDBNull(11) ? null : reader.GetDateTime(11));
                        accidentFields.Add("sommeRecue", reader.IsDBNull(12) ? null : reader.GetInt32(12));
                        accidentFields.Add("intemperie", ItemperieMethodes.parseItemp(reader.GetString(13)));
                        accidents.Add(accidentFields);
                    }
                }
            }
            return new JsonResult(accidents);
        }
    }
}
