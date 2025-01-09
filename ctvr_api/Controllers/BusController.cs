using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusController : ControllerBase
    {
        private IConfiguration _configuration;

        public BusController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllbus()
        {
            string query = "SELECT * FROM bus ORDER BY immat ASC";

            List<Bus> buses = new List<Bus>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des lignes renvoyées
                    {
                        Dictionary<string, object> busFields = new Dictionary<string, object>();
                        busFields.Add("immat", reader.GetString(0));
                        busFields.Add("miseenservice", DateOnly.FromDateTime(reader.GetDateTime(1)));
                        buses.Add(new Bus(busFields));
                    }
                }
            }

            return new JsonResult(buses);
        }

        [HttpPost]
        public JsonResult GetBusByImmat(string? immat)
        {
            if (string.IsNullOrWhiteSpace(immat))
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "SELECT * FROM bus WHERE immat=@immat";

            Bus? bus = null;
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@immat", immat); // ajout du paramètre

                using (var reader = cmd.ExecuteReader())
                {
                    Dictionary<string, object> busFields = new Dictionary<string, object>();
                    if (reader.Read()) // ssi on a un retour
                    {
                        busFields.Add("immat", reader.GetString(0));
                        busFields.Add("miseenservice", DateOnly.FromDateTime(reader.GetDateTime(1)));
                        bus = new Bus(busFields);
                        return new JsonResult(bus);
                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

        [HttpPost]
        public JsonResult Addbus(string? immat, DateOnly? miseenservice)
        {
            if (string.IsNullOrWhiteSpace(immat) || miseenservice == null)
            {
                return new JsonResult(2); // champ manquant
            }

            // Vérification de la présence du Bus à entrer dans la base
            string constraintQuery = "SELECT * FROM bus where immat=@immat";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@immat", immat);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return new JsonResult(4); // l'élément existe déjà
                }
            }

            string query = "INSERT INTO bus VALUES (@immat, @miseenservice)";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@immat", immat);
                cmd.Parameters.AddWithValue("@miseenservice", miseenservice);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }

            return new JsonResult(1); // fonctionnement correcte
        }

        [HttpPost]
        public JsonResult Deletebus(string? immat)
        {
            if (string.IsNullOrWhiteSpace(immat))
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "DELETE FROM bus WHERE immat=@immat";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@immat", immat);

                try
                {
                    int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien
                    if (result == 0) // si ca n'a pas fonctionné
                    {
                        return new JsonResult(3);
                    }
                }
                catch
                {
                    return new JsonResult(7);
                }
            }

            return new JsonResult(1);
        }

    }
}
