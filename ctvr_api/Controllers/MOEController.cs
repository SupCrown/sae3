using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MOEController : ControllerBase
    {
        private IConfiguration _configuration;

        public MOEController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllmoe()
        {
            string query = "SELECT * FROM moe";

            List<MOE> moes = new List<MOE>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des moe renvoyées
                    {
                        Dictionary<string, object> moeFields = new Dictionary<string, object>();
                        moeFields.Add("codeM", reader.GetString(0));
                        moeFields.Add("libelleM", reader.GetString(1));
                        moeFields.Add("tauxHoraire", reader.GetDouble(2));
                        moes.Add(new MOE(moeFields));
                    }
                }
            }

            return new JsonResult(moes);
        }

        [HttpPost]
        public JsonResult GetmoeBylibelle(String? libelleM)
        {
            if (libelleM == null)
            {
                return new JsonResult(2); // champ manquant
            }
            MOE moe = null;
            string query = "SELECT * FROM moe WHERE libelleM=@libelleM";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@LibelleM", libelleM); // ajout du paramètre

                using (var reader = cmd.ExecuteReader())
                {
                    Dictionary<string, Object> moeFields = new Dictionary<string, Object>();
                    if (reader.Read()) // ssi on a un retour
                    {
                        moeFields.Add("codeM", reader.GetString(0));
                        moeFields.Add("libelleM", reader.GetString(1));
                        moeFields.Add("tauxHoraire", reader.GetDouble(2));
                        moe = new MOE(moeFields);
                        return new JsonResult(moe);
                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

    }

}