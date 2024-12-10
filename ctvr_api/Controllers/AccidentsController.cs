using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccidentsController : ControllerBase
    {
        private IConfiguration _configuration;
        public AccidentsController(IConfiguration configuration)
        { 
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllAccidents(int? filtreItemp)
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
                        accidentFields.Add("noseq", reader.GetInt32(1));
                        accidentFields.Add("dateaccident", reader.GetDateTime(2));
                        accidentFields.Add("heureaccident", reader.GetTimeSpan(3));
                        accidentFields.Add("rue", reader.GetString(4));
                        accidentFields.Add("codepostal", reader.GetString(5));
                        accidentFields.Add("ville", reader.GetString(6));
                        accidentFields.Add("resumecirconstances", reader.GetString(7));
                        accidentFields.Add("dateouverturedossier", reader.GetDateTime(8));
                        accidentFields.Add("datetransmissionassur", reader.IsDBNull(9) ? null : reader.GetDateTime(9));
                        accidentFields.Add("datevalidationassur", reader.IsDBNull(10) ? null : reader.GetDateTime(10));
                        accidentFields.Add("datedloturedossier", reader.IsDBNull(11) ? null : reader.GetDateTime(11));
                        accidentFields.Add("sommerecue", reader.IsDBNull(12) ? null : reader.GetInt32(12));
                        accidentFields.Add("intemperie", ItemperieMethodes.parseItemp(reader.GetString(13)));
                        accidents.Add(accidentFields);
                    }
                }
            }
            return new JsonResult(accidents);
        }

        [HttpGet]
        public JsonResult GetOneByID(int? annee, int? noseq)
        {
            if (annee==null || noseq == null) return new JsonResult(2); // chanmp manquant

            string query = "SELECT * FROM accident WHERE annee=@annee AND noseq=@noseq";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Dictionary<string, object?> accident = new Dictionary<string, object?>();
                        accident.Add("annee", reader.GetInt32(0));
                        accident.Add("noseq", reader.GetInt32(1));
                        accident.Add("dateaccident", reader.GetDateTime(2));
                        accident.Add("heureaccident", reader.GetTimeSpan(3));
                        accident.Add("rue", reader.GetString(4));
                        accident.Add("codepostal", reader.GetString(5));
                        accident.Add("ville", reader.GetString(6));
                        accident.Add("resumecirconstances", reader.GetString(7));
                        accident.Add("dateouverturedossier", reader.GetDateTime(8));
                        accident.Add("datetransmissionassur", reader.IsDBNull(9) ? null : reader.GetDateTime(9));
                        accident.Add("datevalidationassur", reader.IsDBNull(10) ? null : reader.GetDateTime(10));
                        accident.Add("datedloturedossier", reader.IsDBNull(11) ? null : reader.GetDateTime(11));
                        accident.Add("sommerecue", reader.IsDBNull(12) ? null : reader.GetInt32(12));
                        accident.Add("intemperie", ItemperieMethodes.parseItemp(reader.GetString(13)));
                        return new JsonResult(accident);
                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

        [HttpGet]
        public JsonResult insert(int? annee, int? noseq, DateTime? dateAccident, string? rue, string? code, string? ville, string? )
    }
}
