
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ctvr_API.Controllers
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
            string connectionString = _configuration.GetConnectionString("ctvrtestDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion
            using (var connection = dataSource.OpenConnection())
            {

                using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
                {
                    using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                    {
                        while (reader.Read()) // pour chacune des lignes renvoyées
                        {
                            Dictionary<string, object> interventionFields = new Dictionary<string, object>();
                            interventionFields.Add("norep", reader.GetInt16(0)); // SMALLINT
                            interventionFields.Add("dateentreebus", 
                                reader.IsDBNull(1) ? null : reader.GetDateTime(1));
                            interventionFields.Add("description", reader.GetString(2)); // TEXT
                            interventionFields.Add("datedevis", reader.GetDateTime(3)); // DATE (NOT NULL)
                            interventionFields.Add("immat", reader.GetString(6)); // TEXT

                            
                            // Gestion des champs DATE pouvant être NULL
                            interventionFields.Add("datedebutreparation", 
                                reader.IsDBNull(4) ? null : reader.GetDateTime(4));
                            interventionFields.Add("datefinreparation", 
                                reader.IsDBNull(5) ? null : reader.GetDateTime(5));
                            var intervention = new Intervention(interventionFields);
                            interventions.Add(intervention);
                        }
                    }
                }
                return new JsonResult(interventions);
            }
        }


        [HttpPost]
        public JsonResult GetInterventionById(Int16? identifiant)
        {
            if (identifiant == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "SELECT * FROM Intervention WHERE norep=@identifiant";

            Intervention? intervention = null;
            string connectionString = _configuration.GetConnectionString("ctvrtestDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.Add("@identifiant", NpgsqlTypes.NpgsqlDbType.Integer).Value = identifiant;
                using (var reader = cmd.ExecuteReader())
                {
                    Dictionary<string, object> userFields = new Dictionary<string, object>();
                    if (reader.Read()) // ssi on a un retour
                    {

                        Dictionary<string, object> interventionFields = new Dictionary<string, object>();
                        interventionFields.Add("norep", reader.GetInt16(0)); // Si c'est un smallint
                        interventionFields.Add("dateentreebus", reader.GetDateTime(1));
                        interventionFields.Add("description", reader.GetString(2));
                        interventionFields.Add("datedevis", reader.GetDateTime(3));
                        interventionFields.Add("datefinreparation", reader.GetDateTime(4));
                        interventionFields.Add("datedebutreparation", reader.GetDateTime(5));
                        interventionFields.Add("immat", reader.GetString(6));
                        intervention = new Intervention(interventionFields);
                        return new JsonResult(intervention);

                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

        [HttpPost]
public JsonResult AddIntervention(string rawdatedevis, string description, string immat)
{
    //pense a ajouter les atribut necessaires!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!              

    DateTime datedevis = DateTime.Parse(rawdatedevis);
    if(description == null || datedevis == null || immat == null)
    {
        return new JsonResult(2); // champ manquant
    }



    string constraintQuery = "SELECT * FROM Intervention WHERE datedevis = @datedevis and description = @description and immat = @immat";
    string connectionString = _configuration.GetConnectionString("ctvrtestDBConn"); 
    NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

    using (var cmd = dataSource.CreateCommand(constraintQuery))
    {
        cmd.Parameters.AddWithValue("@datedevis", datedevis);
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters.AddWithValue("@immat", immat);

        using (var reader = cmd.ExecuteReader())
        {
            if (reader.Read())
                return new JsonResult(4); // L'élément existe déjà
        }
    }
    Console.WriteLine($"Ligne lue: {datedevis}"); // Ajout d'un log pour debug


    string query = "INSERT INTO Intervention (description, datedevis, immat) VALUES ( @description, @datedevis, @immat)";
    using (var cmd = dataSource.CreateCommand(query))
    {
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters.AddWithValue("@datedevis", datedevis);
        cmd.Parameters.AddWithValue("@immat", immat);


        int result = cmd.ExecuteNonQuery();
        if (result == 0)
        {
            return new JsonResult(5); // Erreur d'insertion
        }
    }

    return new JsonResult(1); // Ajout réussi
}

        [HttpDelete]
        public JsonResult DeleteUser(string? identifiant)
        {
            if (string.IsNullOrWhiteSpace(identifiant))
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "DELETE FROM utilisateurs WHERE identifiant=@identifiant";

            string connectionString = _configuration.GetConnectionString("ctvrtestDBConn");
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

            string connectionString = _configuration.GetConnectionString("ctvrtestDBConntestsDBConn");
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
