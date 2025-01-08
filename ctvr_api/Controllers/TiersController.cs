using CTVR_API;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ctvr_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TiersController
    {

        private IConfiguration _configuration;
        public TiersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Insert(string? nomtiers, string? prenomtiers, DateOnly? datenaissance, string? adressetiers, string? cptiers, string? villetiers)
        {
            if (string.IsNullOrWhiteSpace(nomtiers) ||
                string.IsNullOrWhiteSpace(prenomtiers) ||
                string.IsNullOrWhiteSpace(adressetiers) ||
                datenaissance == null ||
                string.IsNullOrWhiteSpace(cptiers) ||
                string.IsNullOrWhiteSpace(villetiers))
            {
                return new JsonResult(2);
            }

            string query = "SELECT notiers FROM tiers WHERE nomtiers=@nomtiers AND prenomtiers=@prenomtiers";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@nomtiers", nomtiers);
                cmd.Parameters.AddWithValue("@prenomtiers", prenomtiers);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return new JsonResult(4);
                }
            }

            query = "INSERT INTO tiers(nomtiers, prenomtiers, datenaissance, adressetiers, cptiers, villetiers) VALUES (@nomtiers, @prenomtiers, @datenaissance, @adressetiers, @cptiers, @villetiers)";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@nomtiers", nomtiers);
                cmd.Parameters.AddWithValue("@prenomtiers", prenomtiers);
                cmd.Parameters.AddWithValue("@datenaissance", datenaissance);
                cmd.Parameters.AddWithValue("@adressetiers", adressetiers);
                cmd.Parameters.AddWithValue("@cptiers", cptiers);
                cmd.Parameters.AddWithValue("@villetiers", villetiers);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie un reader qui peut parcourir le resultat

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }

            }

            query = "SELECT notiers FROM tiers WHERE nomtiers=@nomtiers AND prenomtiers=@prenomtiers";
            int notiers;

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@nomtiers", nomtiers);
                cmd.Parameters.AddWithValue("@prenomtiers", prenomtiers);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return new JsonResult(reader.GetInt32(0));
                    else return new JsonResult(5);
                }
            }
        }
    }
}
