using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql;
using System.Data;
using System.Data.SqlClient;
using System.Formats.Tar;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public JsonResult GetAllUsers() 
        {
            string query = "SELECT * FROM Utilisateurs";
            
            List<Utilisateur> users = new List<Utilisateur>();
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des lignes renvoyées
                    {
                        Dictionary<string, string> userFields = new Dictionary<string, string>();
                        userFields.Add("identifiant", reader.GetString(0));
                        userFields.Add("nom", reader.GetString(1));
                        userFields.Add("prenom", reader.GetString(2));
                        userFields.Add("motDePasse", reader.GetString(3));
                        userFields.Add("role", reader.GetString(4));
                        users.Add(new Utilisateur(userFields));
                    }
                }
            }

            return new JsonResult(users);
        }

        [HttpPost]
        [Route("GetUserById")]
        public JsonResult GetUserById([FromForm] string identifiant)
        {
            string query = "SELECT * FROM Utilisateurs WHERE identifiant=@identifiant";

            Utilisateur? user = null;
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
                        userFields.Add("identifiant", reader.GetString(0));
                        userFields.Add("nom", reader.GetString(1));
                        userFields.Add("prenom", reader.GetString(2));
                        userFields.Add("motDePasse", reader.GetString(3));
                        userFields.Add("role", reader.GetString(4));
                        user = new Utilisateur(userFields);
                    }
                }
            }

            return new JsonResult(user);
        }

        [HttpPost]
        [Route("AddUser")]
        public JsonResult AddUser([FromForm] string identifiant, [FromForm] string nom, [FromForm] string prenom, [FromForm] string motDePasse, [FromForm] string role)
        {
            // Vérification de la présence de l'utilisateur à entrer dans la base
            string constraintQuery = "SELECT * FROM utilisateurs where identifiant=@identifiant";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
            
            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@identifiant", identifiant);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return new JsonResult(false); // si celui-ci existe déjà : on stop;
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
                
                if(result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(false); 
                }
            }

            return new JsonResult(true);
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public JsonResult DeleteUser([FromForm] string identifiant)
        {

            string query = "DELETE FROM utilisateurs WHERE identifiant=@identifiant";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@identifiant", identifiant);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(false);
                }
            }
            
            return new JsonResult(true);
        }

        [HttpPost]
        [Route("Check")]
        public JsonResult Check([FromForm] string identifiant, [FromForm] string motDePasse)
        {
            string query = "SELECT * FROM utilisateur WHERE identifiant=@identifiant AND motdepasse=@motdepasse";

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
                        return new JsonResult(true);
                    }
                }
            }

            return new JsonResult(false);
        }
    }
}
