
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private IConfiguration _configuration;

        public MaintenanceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllInterventions(string? filtre)
        {
            string query;
            switch (filtre)//date, debut rep, fin rep
            {
                case "debutrep": query = "SELECT * FROM Intervention where datedebutreparation is not null and datefinreparation is null"; break;
                case "finrep": query = "SELECT * FROM Intervention where datefinreparation is not null"; break;


                default:
                    query = "SELECT * FROM Intervention ";
                    break;
            }

            List<Intervention> interventions = new List<Intervention>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion
            using (var connection = dataSource.OpenConnection())
            {

                using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
                {
                    using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                    {
                        while (reader.Read()) // pour chacune des lignes renvoyées
                        {
                            Dictionary<string, object?> interventionFields = new Dictionary<string, object>();
                            interventionFields.Add("norep", reader.GetInt16(0)); // SMALLINT
                            interventionFields.Add("dateentreebus",
                                reader.IsDBNull(1) ? null : DateOnly.FromDateTime(reader.GetDateTime(1)));
                            interventionFields.Add("description", reader.GetString(2)); // TEXT
                            interventionFields.Add("datedevis", reader.IsDBNull(3) ? null : DateOnly.FromDateTime(reader.GetDateTime(3))); // DATE (NOT NULL)
                            interventionFields.Add("immat", reader.GetString(6)); // TEXT
                            interventionFields.Add("annee",
                                reader.IsDBNull(7) ? null : reader.GetInt16(7));
                            interventionFields.Add("noseq",
                                reader.IsDBNull(8) ? null : reader.GetInt16(8)); // SMALLINT

                            // Gestion des champs DATE pouvant être NULL
                            interventionFields.Add("datedebutreparation",
                                reader.IsDBNull(4) ? null : DateOnly.FromDateTime(reader.GetDateTime(4)));
                            interventionFields.Add("datefinreparation",
                                reader.IsDBNull(5) ? null : DateOnly.FromDateTime(reader.GetDateTime(5)));
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
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
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
                        interventionFields.Add("dateentreebus", DateOnly.FromDateTime(reader.GetDateTime(1)));
                        interventionFields.Add("description", reader.GetString(2));
                        interventionFields.Add("datedevis", DateOnly.FromDateTime(reader.GetDateTime(3)));
                        interventionFields.Add("datefinreparation", DateOnly.FromDateTime(reader.GetDateTime(4)));
                        interventionFields.Add("datedebutreparation", DateOnly.FromDateTime(reader.GetDateTime(5)));
                        interventionFields.Add("immat", reader.GetString(6));
                        interventionFields.Add("annee", reader.GetInt16(7));
                        interventionFields.Add("noseq", reader.GetInt16(8));
                        intervention = new Intervention(interventionFields);
                        return new JsonResult(intervention);

                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

        [HttpPost]
        public JsonResult AddIntervention(string? rawdatedevis, string description, string immat, Int16? annee, Int16? noseq)
        {
            string constraintQuery = "SELECT * FROM Intervention WHERE description = @description and immat = @immat";
            string query = "INSERT INTO Intervention (description, immat) VALUES ( @description, @immat)";

            if (rawdatedevis != null)
            {
                DateOnly datedevis = DateOnly.Parse(rawdatedevis);
                constraintQuery = "SELECT * FROM Intervention WHERE description = @description and datedevis =@datedevis and immat = @immat";
                query = "INSERT INTO Intervention (description, immat, datedevis) VALUES ( @description, @immat, @datedevis)";
                if (annee != null & noseq != null)
                {
                    constraintQuery = "SELECT * FROM Intervention WHERE description = @description and immat = @immat";
                    query = "INSERT INTO Intervention (description, immat,datedevis, annee, noseq) VALUES ( @description, @immat, @datedevis, @annee, @noseq)";
                }
                else
                {
                    constraintQuery = "SELECT * FROM Intervention WHERE description = @description and immat = @immat";
                    query = "INSERT INTO Intervention (description, immat,datedevis) VALUES ( @description, @immat, @datedevis)";
                }

            }

            if (description == null || immat == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                if (rawdatedevis != null)
                {
                    cmd.Parameters.AddWithValue("@datedevis", DateOnly.Parse(rawdatedevis));
                }
                cmd.Parameters.AddWithValue("@description", description);

                cmd.Parameters.AddWithValue("@immat", immat);

                if (annee != null & noseq != null)
                {
                    cmd.Parameters.AddWithValue("@annee", annee);
                    cmd.Parameters.AddWithValue("@noseq", noseq);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return new JsonResult(4); // L'élément existe déjà
                }
            }

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@description", description);
                if (rawdatedevis != null)
                {
                    cmd.Parameters.AddWithValue("@datedevis", DateOnly.Parse(rawdatedevis));
                }

                cmd.Parameters.AddWithValue("@immat", immat);

                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    return new JsonResult(5); // Erreur d'insertion
                }
            }

            return new JsonResult(1); // Ajout réussi
        }

        [HttpPost]
        public JsonResult DeleteIntervention(Int16? norep)
        {
            if (norep == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "DELETE FROM Intervention WHERE norep=@norep";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@norep", norep);
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
        public JsonResult updateIntervention(Int16 norep, int? annee, int? noseq, string? datedebutreparation, string? datefinreparation, string? entreegarage)
        {
            if (norep == null || norep == 0)
            {
                return new JsonResult(2); // champ manquant
            }

            string constraintQuery = "SELECT * FROM Intervention WHERE norep=@norep";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@norep", norep);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return new JsonResult(3); // L'élément n'existe pas mais devrait
                }
            }

            if (datedebutreparation != null)
            {
                DateTime debutrep = DateTime.Parse(datedebutreparation);

                string query = "update Intervention set datedebutreparation = @debutrep WHERE norep=@norep ";
                using (var cmd = dataSource.CreateCommand(query))
                {
                    cmd.Parameters.AddWithValue("@debutrep", debutrep);
                    cmd.Parameters.AddWithValue("@norep", norep);

                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return new JsonResult(5); // Erreur d'insertion
                    }
                }
            }

            if (datefinreparation != null)
            {
                DateTime finrep = DateTime.Parse(datefinreparation);
                string query = "update Intervention set datefinreparation = @finrep WHERE norep = @norep ";
                using (var cmd = dataSource.CreateCommand(query))
                {
                    cmd.Parameters.AddWithValue("@finrep", finrep);
                    cmd.Parameters.AddWithValue("@norep", norep);

                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return new JsonResult(5); // Erreur d'insertion
                    }
                }
            }

            if (entreegarage != null)
            {
                DateTime dateentreegarage = DateTime.Parse(entreegarage);
                string query = "update Intervention set dateentreebus = @dateentreegarage WHERE norep=@norep ";
                using (var cmd = dataSource.CreateCommand(query))
                {
                    cmd.Parameters.AddWithValue("@dateentreegarage", dateentreegarage);
                    cmd.Parameters.AddWithValue("@norep", norep);

                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return new JsonResult(5); // Erreur d'insertion
                    }
                }
            }

            if (annee != null & noseq != null) // si un accident est lié
            {
                string query = "update Intervention set noseq=@noseq, annee=@annee where norep=@norep ";
                using (var cmd = dataSource.CreateCommand(query))
                {
                    cmd.Parameters.Add("@noseq", NpgsqlTypes.NpgsqlDbType.Integer).Value = noseq;
                    cmd.Parameters.Add("@annee", NpgsqlTypes.NpgsqlDbType.Integer).Value = annee;
                    cmd.Parameters.AddWithValue("@norep", norep);

                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return new JsonResult(5); // Erreur d'insertion
                    }
                }
            }

            return new JsonResult(1); // Ajout réussi
        }

        [HttpPost]
        public JsonResult SetDateDevis(Int16? norep)
        {
            if (norep == null)
            {
                return new JsonResult(2);
            }

            string query = "UPDATE intervention SET datedevis=@date WHERE norep=@norep";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@norep", norep);
                cmd.Parameters.AddWithValue("@date", DateOnly.FromDateTime(DateTime.Now));

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


