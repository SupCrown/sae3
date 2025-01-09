
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Text.Json;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ParcoursController : ControllerBase
    {
        private IConfiguration _configuration;

        public ParcoursController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllParcours(string? filtre, string? nom, string? prenom, string? immat)
        {
            string query;

            switch (filtre)
            {
                case "chauffeur":
                    if (nom != null && prenom != null)
                    {
                        query = "SELECT nopermis, horaireservice, nom, prenom, noligne, immat FROM Parcours join Conducteur using(nopermis) where nom = @nom and prenom = @prenom";
                        break;
                    }
                    else
                    {
                        return new JsonResult(2); // Champ manquant
                    }

                case "immat":
                    if (immat != null)
                    {
                        query = "SELECT nopermis, horaireservice, nom, prenom, noligne, immat FROM Parcours join Conducteur using(nopermis) where immat = @immat";
                        break;
                    }
                    else
                    {
                        return new JsonResult(2); // Champ manquant
                    }

                case "asc":
                    query = "SELECT nopermis, horaireservice, nom, prenom, noligne, immat FROM Parcours join Conducteur using(nopermis) order by nom asc";
                    break;

                case "desc":
                    query = "SELECT nopermis, horaireservice, nom, prenom, noligne, immat FROM Parcours join Conducteur using(nopermis) order by nom desc";
                    break;

                default:
                    query = "SELECT nopermis, horaireservice, nom, prenom, noligne, immat FROM Parcours join Conducteur using(nopermis)";
                    break;
            }

            List<Parcours> Parco = new List<Parcours>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var connection = dataSource.OpenConnection())
            {
                using (var cmd = dataSource.CreateCommand(query))
                {
                    if (filtre == "chauffeur")
                    {
                        cmd.Parameters.AddWithValue("@nom", nom);
                        cmd.Parameters.AddWithValue("@prenom", prenom);
                    }
                    else if (filtre == "immat")
                    {
                        cmd.Parameters.AddWithValue("@immat", immat);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> ParcoursFields = new Dictionary<string, object>
                    {
                        { "nopermis", reader.GetString(0) },      // TEXT
                        { "horaireservice", reader.GetDateTime(1) }, // TIMESTAMP
                        { "nom", reader.GetString(2) },          // TEXT
                        { "prenom", reader.GetString(3) },       // TEXT
                        { "noligne", reader.GetInt16(4) },       // INTEGER
                        { "immat", reader.GetString(5) }         // TEXT
                    };
                            Parco.Add(new Parcours(ParcoursFields));
                        }
                    }
                }
            }

            return new JsonResult(Parco);
        }

        [HttpPost]
        public JsonResult getParcoursByNoPermis(string? nopermis)
        {
            if (string.IsNullOrWhiteSpace(nopermis))
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "SELECT horaireservice, immat, noligne FROM parcours WHERE nopermis=@nopermis ORDER BY horaireservice ASC";

            List<Dictionary<string, object>> parcours = [];
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis); // ajout du paramètre

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // ssi on a un retour
                    {
                        Dictionary<string, object> parcoursFields = new Dictionary<string, object>();
                        parcoursFields.Add("horaireservice",reader.GetDateTime(0));
                        parcoursFields.Add("immat", reader.GetString(1));
                        parcoursFields.Add("noligne", reader.GetInt16(2));
                        parcours.Add(parcoursFields);
                    }
                    return new JsonResult(parcours);
                }
            }
        }

        [HttpPost]
        public JsonResult AddParcours(string? nopermis, DateTime? horaireservice, Int16? noligne, string? immat)
        {

            if (nopermis == null || horaireservice == null || immat == null || noligne == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string constraintQuery = "SELECT * FROM Parcours WHERE nopermis = @nopermis and horaireservice = @horaireservice";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                cmd.Parameters.AddWithValue("@horaireservice", horaireservice);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return new JsonResult(4); // L'élément existe déjà
                }
            }


            string query = "INSERT INTO Parcours VALUES ( @nopermis, @horaireservice, @immat, @noligne)";
            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                cmd.Parameters.AddWithValue("@horaireservice", horaireservice);
                cmd.Parameters.AddWithValue("@noligne", noligne);
                cmd.Parameters.AddWithValue("@immat", immat);

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return new JsonResult(5); // Erreur d'insertion
                    }
                } catch {
                    return new JsonResult(6); // une clé étrangère ne correxpond pas
                }   
            }

            return new JsonResult(1); // Ajout réussi
        }

        [HttpPost]
        public JsonResult DeleteParcours(string nopermis, DateTime horaireservice)
        {
            if (nopermis == null || horaireservice == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "DELETE FROM Parcours WHERE nopermis=@nopermis and horaireservice=@horaireservice";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                cmd.Parameters.AddWithValue("@horaireservice", horaireservice);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(3);
                }
            }

            return new JsonResult(1);// element supprimé
        }


        [HttpPost]
        public JsonResult updateParcours(string? oldnopermis, DateTime? oldhoraireservice, int? noligne, string? immat, string? nopermis, DateTime? horaireservice)
        {


            if (oldnopermis == null || oldhoraireservice == null || noligne == null || immat == null || nopermis == null || horaireservice == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string constraintQuery = "SELECT * FROM Parcours WHERE nopermis = @nopermis and horaireservice = @horaireservice";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@nopermis", oldnopermis);
                cmd.Parameters.AddWithValue("@horaireservice", oldhoraireservice);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return new JsonResult(3); // L'élément n'existe pas
                }
            }

            string query = "update Parcours set nopermis=@nopermis, immat = @immat, horaireservice = @horaireservice, noligne=@noligne  WHERE nopermis=@oldnopermis and horaireservice=@oldhoraireservice";
            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                cmd.Parameters.AddWithValue("@immat", immat);
                cmd.Parameters.AddWithValue("@horaireservice", horaireservice);
                cmd.Parameters.AddWithValue("@noligne", noligne);
                cmd.Parameters.AddWithValue("@oldnopermis", oldnopermis);
                cmd.Parameters.AddWithValue("@oldhoraireservice", oldhoraireservice);

                try
                {
                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return new JsonResult(5); // Erreur d'insertion
                    }
                }
                catch
                {
                    return new JsonResult(6); // une clé étrangère ne correxpond pas
                }
            }

            return new JsonResult(1); // Ajout réussi
        }

    }

}


