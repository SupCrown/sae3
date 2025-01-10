using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConducteursController : ControllerBase
    {
        private IConfiguration _configuration;

        public ConducteursController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult getAllconducteur()
        {
            string query = "SELECT * FROM conducteur order by nom";

            List<Conducteur> conducteur = new List<Conducteur>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des lignes renvoyées
                    {
                        Dictionary<string, object> conductFields = new Dictionary<string, object>();
                        conductFields.Add("nopermis", reader.GetString(0));
                        conductFields.Add("datepermis", DateOnly.FromDateTime(reader.GetDateTime(1)));
                        conductFields.Add("lieudelivrance", reader.GetString(2));
                        conductFields.Add("dateprisefonction", DateOnly.FromDateTime(reader.GetDateTime(3)));
                        conductFields.Add("nom", reader.GetString(4));
                        conductFields.Add("prenom", reader.GetString(5));
                        conductFields.Add("datenaissance", DateOnly.FromDateTime(reader.GetDateTime(6)));
                        conductFields.Add("adresse", reader.GetString(7));
                        conductFields.Add("codepostal", reader.GetString(8));
                        conductFields.Add("ville", reader.GetString(9));
                        conducteur.Add(new Conducteur(conductFields));
                    }
                }
            }

            return new JsonResult(conducteur);
        }

        [HttpPost]
        public JsonResult getConducteurByNoPermis(string? nopermis)
        {
            if (string.IsNullOrWhiteSpace(nopermis))
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "SELECT * FROM Conducteur WHERE nopermis=@nopermis";

            Conducteur? conducteur = null;
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis); // ajout du paramètre

                using (var reader = cmd.ExecuteReader())
                {
                    Dictionary<string, object> conductFields = new Dictionary<string, object>();
                    if (reader.Read()) // ssi on a un retour
                    {
                        conductFields.Add("nopermis", reader.GetString(0));
                        conductFields.Add("datepermis", DateOnly.FromDateTime(reader.GetDateTime(1)));
                        conductFields.Add("lieudelivrance", reader.GetString(2));
                        conductFields.Add("dateprisefonction", DateOnly.FromDateTime(reader.GetDateTime(3)));
                        conductFields.Add("nom", reader.GetString(4));
                        conductFields.Add("prenom", reader.GetString(5));
                        conductFields.Add("datenaissance", DateOnly.FromDateTime(reader.GetDateTime(6)));
                        conductFields.Add("adresse", reader.GetString(7));
                        conductFields.Add("codepostal", reader.GetString(8));
                        conductFields.Add("ville", reader.GetString(9));
                        conducteur = new Conducteur(conductFields);
                        List<Conducteur> conducteurResult = [];
                        conducteurResult.Add(conducteur);
                        return new JsonResult(conducteurResult);
                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

        [HttpPost]
        public JsonResult addConducteur(string? nopermis, DateOnly? datepermis, string? lieudelivrance, DateOnly? dateprisefonction, string? nom, string? prenom, DateOnly? datenaissance, string? adresse, string? codepostal, string? ville)
        {
            if (string.IsNullOrWhiteSpace(nopermis)
                || datepermis == null
                || string.IsNullOrWhiteSpace(lieudelivrance)
                || dateprisefonction == null
                || string.IsNullOrWhiteSpace(nom)
                || string.IsNullOrWhiteSpace(prenom)
                || datenaissance == null
                || string.IsNullOrWhiteSpace(adresse)
                || string.IsNullOrWhiteSpace(codepostal)
                || string.IsNullOrWhiteSpace(ville)
                )
            {
                return new JsonResult(2); // champ manquant
            }

            // Vérification de la présence de l'Conducteur à entrer dans la base
            string constraintQuery = "SELECT * FROM Conducteur where nopermis=@nopermis";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new JsonResult(4); // l'élément existe déjà}
                    }
                }
            }

            string query = "INSERT INTO Conducteur VALUES (@nopermis, @datepermis, @lieudelivrance, @dateprisefonction, @nom, @prenom, @datenaissance, @adresse, @codepostal, @ville)";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                cmd.Parameters.AddWithValue("@datepermis", datepermis);
                cmd.Parameters.AddWithValue("@lieudelivrance", lieudelivrance);
                cmd.Parameters.AddWithValue("@dateprisefonction", dateprisefonction);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@datenaissance", datenaissance);
                cmd.Parameters.AddWithValue("@adresse", adresse);
                cmd.Parameters.AddWithValue("@codepostal", codepostal);
                cmd.Parameters.AddWithValue("@ville", ville);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }

            return new JsonResult(1); // fonctionnement correcte

        }
        [HttpPost]
        public JsonResult deleteConducteur(string? nopermis)
        {
            if (string.IsNullOrWhiteSpace(nopermis))
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "DELETE FROM Conducteur WHERE nopermis=@nopermis";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
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
        [HttpPost]
        public JsonResult updateConducteur(string? oldnopermis, string? nopermis, DateOnly? datepermis, string? lieudelivrance, DateOnly? dateprisefonction, string? nom, string? prenom, DateOnly? datenaissance, string? adresse, string? codepostal, string? ville)
        {
            if (string.IsNullOrWhiteSpace(oldnopermis)
                || string.IsNullOrWhiteSpace(nopermis)
                || datepermis == null
                || string.IsNullOrWhiteSpace(lieudelivrance)
                || dateprisefonction == null
                || string.IsNullOrWhiteSpace(nom)
                || string.IsNullOrWhiteSpace(prenom)
                || datenaissance == null
                || string.IsNullOrWhiteSpace(adresse)
                || string.IsNullOrWhiteSpace(codepostal)
                || string.IsNullOrWhiteSpace(ville)
                )
            {
                return new JsonResult(2); // champ manquant
            }

            // Vérification de la présence du Conducteur à modifier dans la base
            string constraintQuery = "SELECT * FROM Conducteur where nopermis=@oldnopermis";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@oldnopermis", oldnopermis);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return new JsonResult(3); // L'élément n'existe pas
                    }
                }
            }

            string query = "update conducteur set nopermis=@nopermis, datepermis=@datepermis, lieudelivrancepermis=@lieudelivrance, dateprisefonction=@dateprisefonction, nom=@nom, prenom=@prenom, datenaissance=@datenaissance, adresse=@adresse, codepostal=@codepostal, ville=@ville where nopermis = @oldnopermis";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                cmd.Parameters.AddWithValue("@datepermis", datepermis);
                cmd.Parameters.AddWithValue("@lieudelivrance", lieudelivrance);
                cmd.Parameters.AddWithValue("@dateprisefonction", dateprisefonction);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@datenaissance", datenaissance);
                cmd.Parameters.AddWithValue("@adresse", adresse);
                cmd.Parameters.AddWithValue("@codepostal", codepostal);
                cmd.Parameters.AddWithValue("@ville", ville);
                cmd.Parameters.AddWithValue("@oldnopermis", oldnopermis);

                try
                {
                    int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien
                    if (result == 0) // si ca n'a pas fonctionné
                    {
                        return new JsonResult(5);
                    }
                }
                catch
                {
                    return new JsonResult(6);
                }
            }

            return new JsonResult(1); // fonctionnement correcte
        }

        [HttpGet]
        public JsonResult checkConducteur(string? nopermis)
        {
            if (string.IsNullOrWhiteSpace(nopermis))
                return new JsonResult(2);

            // Vérification de la présence du Conducteur à modifier dans la base
            string constraintQuery = "SELECT * FROM Conducteur where nopermis=@nopermis";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new JsonResult(true); // L'élément n'existe pas
                    }
                    return new JsonResult(false);
                }
            }
        }
    }
}
