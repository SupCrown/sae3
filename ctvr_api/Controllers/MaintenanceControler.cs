
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MaintenanceControler : ControllerBase
    {
        private IConfiguration _configuration;

        public MaintenanceControler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllInterventions()
        {
            string query = "SELECT * FROM Intervention";

            List<Intervention> interventions = new List<Intervention>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des lignes renvoyées
                    {
                        Dictionary<string, object> interventionFields = new Dictionary<string, object>();
                        interventionFields.Add("DateEntreeBus", DateTime.Parse(reader.GetString(0)));
                        interventionFields.Add("Nom", reader.GetString(1));
                        interventionFields.Add("Piece", reader.GetString(2));
                        interventionFields.Add("DateFinReparation", DateTime.Parse(reader.GetString(3)));
                        interventionFields.Add("DateDebutReparation", DateTime.Parse(reader.GetString(4)));
                        interventions.Add(new Intervention(interventionFields));
                    }
                }
            }

            return new JsonResult(interventions);
        }

        [HttpPost]
        public JsonResult GetInterventionById(string? identifiant)
        {
            if (string.IsNullOrWhiteSpace(identifiant))
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "SELECT * FROM Utilisateurs WHERE identifiant=@identifiant";

            Intervention? intervention = null;
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@identifiant", identifiant); // ajout du paramètre

                using (var reader = cmd.ExecuteReader())
                {
                    Dictionary<string, string> userFields = new Dictionary<string, string>();
                    if (reader.Read()) // ssi on a un retour
                    {

                        Dictionary<string, object> interventionFields = new Dictionary<string, object>();
                        interventionFields.Add("DateEntreeBus", DateTime.Parse(reader.GetString(0)));
                        interventionFields.Add("Nom", reader.GetString(1));
                        interventionFields.Add("Piece", reader.GetString(2));
                        interventionFields.Add("DateFinReparation", DateTime.Parse(reader.GetString(3)));
                        interventionFields.Add("DateDebutReparation", DateTime.Parse(reader.GetString(4)));
                        intervention = new Intervention(interventionFields);
                        return new JsonResult(intervention);

                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

        [HttpPost]
        public JsonResult AddUser(string? identifiant, string? nom, string? prenom, string? motDePasse, string? role)
        {
            if (string.IsNullOrWhiteSpace(identifiant)
                || string.IsNullOrWhiteSpace(nom)
                || string.IsNullOrWhiteSpace(prenom)
                || string.IsNullOrWhiteSpace(motDePasse)
                || string.IsNullOrWhiteSpace(role)
                )
            {
                return new JsonResult(2); // champ manquant
            }

            // Vérification de la présence de l'utilisateur à entrer dans la base
            string constraintQuery = "SELECT * FROM utilisateurs where identifiant=@identifiant";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@identifiant", identifiant);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return new JsonResult(4); // l'élément existe déjà
                }
            }

            string query = "INSERT INTO utilisateurs VALUES (@identifiant, @nom, @prenom, @motDePasse, @role)";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@identifiant", identifiant);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@motDePasse", motDePasse);
                cmd.Parameters.AddWithValue("@role", role);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }

            return new JsonResult(1); // fonctionnement correcte
        }

        [HttpDelete]
        public JsonResult DeleteUser(string? identifiant)
        {
            if (string.IsNullOrWhiteSpace(identifiant))
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "DELETE FROM utilisateurs WHERE identifiant=@identifiant";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@identifiant", identifiant);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(3);
                }
            }

            return new JsonResult(1);
        }

        [HttpPost]
        public JsonResult Check(string? identifiant, string? motDePasse)
        {
            if (string.IsNullOrWhiteSpace(identifiant) || string.IsNullOrWhiteSpace(motDePasse))
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "SELECT * FROM utilisateurs WHERE identifiant=@identifiant AND motdepasse=@motdepasse";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@identifiant", identifiant);
                cmd.Parameters.AddWithValue("@motdepasse", motDePasse);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new JsonResult(true); // identifiant et mot de passe correspondant
                    }
                }
            }
            return new JsonResult(false); // muvais identifiant ou mot de passe
        }
    }
}
