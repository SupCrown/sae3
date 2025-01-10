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
            string query = "SELECT * FROM moe ORDER BY libelleM";

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

        [HttpGet]
        public JsonResult CheckMOE(string? code)
        {
            if (code == null)
            {
                return new JsonResult(2);
            }
            string query = "SELECT * FROM moe WHERE codem=@code";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@code", code);
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    if (reader.Read())
                    {
                        return new JsonResult(true);
                    }
                    return new JsonResult(false);
                }
            }
        }

        [HttpPost]
        public JsonResult useMOE(Int16? norep, string? codem, Int32? nbhdevis)
        {
            if (norep == null || string.IsNullOrWhiteSpace(codem) || nbhdevis == null)
            {
                return new JsonResult(2);
            }

            if (nbhdevis <= 0)
                return new JsonResult(1);

            string query = "INSERT INTO necessiter(norep, codem, nbhdevis, tauxhoraire) VALUES(@norep,@codem,@nbhdevis,(SELECT tauxhorairecourant FROM moe WHERE codem=@codem2))";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@norep", norep);
                cmd.Parameters.AddWithValue("@codem", codem);
                cmd.Parameters.AddWithValue("@codem2", codem);
                cmd.Parameters.AddWithValue("@nbhdevis", nbhdevis);

                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    return new JsonResult(5); // Erreur d'insertion
                }
                return new JsonResult(1);
            }
        }

    }

}