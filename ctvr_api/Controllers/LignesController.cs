using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LignesController : ControllerBase
    {
        private IConfiguration _configuration;

        public LignesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllLigne()
        {
            string query = "SELECT * FROM ligne ORDER BY noligne ASC";

            List<Ligne> Lignes = new List<Ligne>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des Ligne renvoyées
                    {
                        Dictionary<string, object> LigneFields = new Dictionary<string, object>();
                        LigneFields.Add("noligne", reader.GetInt16(0));
                        LigneFields.Add("nomarretdepart", reader.GetString(1));
                        LigneFields.Add("nomarretterminus", reader.GetString(2));
                        Lignes.Add(new Ligne(LigneFields));
                    }
                }
            }

            return new JsonResult(Lignes);
        }

        [HttpPost]
        public JsonResult GetLigneById(Int16? noligne)
        {
            if (noligne == null)
            {
                return new JsonResult(2); // champ manquant
            }
            Ligne ligne = null;
            string query = "SELECT * FROM ligne WHERE noligne=@noligne";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@noligne", noligne); // ajout du paramètre

                using (var reader = cmd.ExecuteReader())
                {
                    Dictionary<string, Object> LigneFields = new Dictionary<string, Object>();
                    if (reader.Read()) // ssi on a un retour
                    {
                        LigneFields.Add("noligne", reader.GetInt16(0));
                        LigneFields.Add("nomarretdepart", reader.GetString(1));
                        LigneFields.Add("nomarretterminus", reader.GetString(2));
                        ligne = new Ligne(LigneFields);
                        return new JsonResult(ligne);
                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

        [HttpPost]
        public JsonResult AddLigne(Int16? noligne, string? nomarretdepart, string? nomarretterminus)
        {
            if (noligne == null
                || string.IsNullOrWhiteSpace(nomarretdepart)
                || string.IsNullOrWhiteSpace(nomarretterminus)
                )
            {
                return new JsonResult(2); // champ manquant
            }

            // Vérification de la présence de l'Ligne à entrer dans la base
            string constraintQuery = "SELECT * FROM ligne where noligne=@noligne";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@noligne", noligne);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return new JsonResult(4); // l'élément existe déjà
                }
            }

            string query = "INSERT INTO ligne VALUES (@noligne, @nomarretdepart, @nomarretterminus)";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@noligne", noligne);
                cmd.Parameters.AddWithValue("@nomarretdepart", nomarretdepart);
                cmd.Parameters.AddWithValue("@nomarretterminus", nomarretterminus);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de Ligne affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }

            return new JsonResult(1); // fonctionnement correcte
        }

        [HttpPost]
        public JsonResult DeleteLigne(Int16? noligne)
        {
            if (noligne == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "DELETE FROM ligne WHERE noligne=@noligne";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@noligne", noligne);
                try
                {
                    int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de Ligne affectées -> 1 si tout se passe bien
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

        [HttpPost]
        public JsonResult updateLigne(Int16? oldnoligne, Int16? noligne, string? nomarretdepart, string? nomarretterminus)
        {

            if (oldnoligne == null || noligne == null || noligne == 0)
            {
                return new JsonResult(2); // champ manquant
            }


            string constraintQuery = "SELECT * FROM ligne WHERE noligne=@oldnoligne";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@oldnoligne", oldnoligne);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return new JsonResult(3); // L'élément n'existe pas mais devrait
                }
            }

            string query = "update ligne set noligne = @noligne, nomarretdepart = @nomarretdepart, nomarretterminus = @nomarretterminus WHERE noligne = @oldnoligne";
            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@oldnoligne", oldnoligne);
                cmd.Parameters.AddWithValue("@noligne", noligne);
                cmd.Parameters.AddWithValue("@nomarretdepart", nomarretdepart);
                cmd.Parameters.AddWithValue("@nomarretterminus", nomarretterminus);

                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    return new JsonResult(5); // Erreur d'insertion
                }
            }

            return new JsonResult(1); // Ajout réussi
        }

    }


}