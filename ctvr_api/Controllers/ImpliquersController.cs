using CTVR_API;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ctvr_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImpliquersController : ControllerBase
    {
        private IConfiguration _configuration;
        public ImpliquersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPut]
        public JsonResult Insert(int? annee, int? noseq, int? notiers, string? natureblessures, string? degatsmateriels)
        {
            if (annee == null || noseq == null ||
                notiers == null ||
                string.IsNullOrWhiteSpace(natureblessures) ||
                string.IsNullOrWhiteSpace(degatsmateriels))
            {
                return new JsonResult(2);
            }

            string query = "SELECT annee FROM impliquer WHERE annee=@annee AND noseq=@noseq AND notiers=@notiers";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                cmd.Parameters.AddWithValue("@notiers", notiers);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return new JsonResult(4);
                }
            }

            query = "INSERT INTO impliquer(annee, noseq, notiers, natureblessures, degatsmateriels) VALUES (@annee, @noseq, @notiers, @natureblessures, @degatsmateriels)";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                cmd.Parameters.AddWithValue("@notiers", notiers);
                cmd.Parameters.AddWithValue("@natureblessures", natureblessures);
                cmd.Parameters.AddWithValue("@degatsmateriels", degatsmateriels);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie un reader qui peut parcourir le resultat

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }

            }
            
            return new JsonResult(1);
        }
    }
}
