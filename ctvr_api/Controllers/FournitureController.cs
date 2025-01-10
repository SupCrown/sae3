using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class fournituresController : ControllerBase
    {
        private IConfiguration _configuration;

        public fournituresController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllFourniture()
        {
            string query = "SELECT * FROM fourniture";

            List<Fourniture> fournitures = new List<Fourniture>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des fourniture renvoyées
                    {
                        Dictionary<string, object> fournitureFields = new Dictionary<string, object>();
                        fournitureFields.Add("codeF", reader.GetString(0));
                        fournitureFields.Add("libelleF", reader.GetString(1));
                        fournitureFields.Add("prixUnitaire", reader.GetDouble(2));
                        fournitures.Add(new Fourniture(fournitureFields));
                    }
                }
            }

            return new JsonResult(fournitures);
        }

        [HttpPost]
        public JsonResult GetfournitureBylibelle(String? libelleF)
        {
            if (libelleF == null)
            {
                return new JsonResult(2); // champ manquant
            }
            Fourniture fourniture = null;
            string query = "SELECT * FROM fourniture WHERE libelleF=@libelleF";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@LibelleF", libelleF); // ajout du paramètre

                using (var reader = cmd.ExecuteReader())
                {
                    Dictionary<string, Object> fournitureFields = new Dictionary<string, Object>();
                    if (reader.Read()) // ssi on a un retour
                    {
                        fournitureFields.Add("codeF", reader.GetString(0));
                        fournitureFields.Add("libelleF", reader.GetString(1));
                        fournitureFields.Add("prixUnitaire", reader.GetDouble(2));
                        fourniture = new Fourniture(fournitureFields);
                        return new JsonResult(fourniture);
                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

    }

}
