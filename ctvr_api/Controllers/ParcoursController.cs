
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


            switch (filtre)//date, debut rep, fin rep
            {
                case "chauffeur"://recherche par nom et prenom
                    if (nom != null & prenom != null)
                    {
                        query = "SELECT nopermis, horaireservice, nom, prenom, noligne immat FROM Parcours join Conducteur using(nopermis) where nom = @nom and prenom = @prenom"; break;
                    }
                    else
                    {
                        return new JsonResult(2); // champ manquant

                    }

                case "immat":
                    if (immat != null)
                    {
                        query = "SELECT nopermis, horaireservice, nom, prenom, noligne, immat FROM Parcours join Conducteur using(nopermis) where immat = @immat"; break;
                    }
                    else
                    {
                        return new JsonResult(2); // champ manquant

                    }
                case "asc": query = "SELECT nopermis, horaireservice, nom, prenom, noligne, immat FROM Parcours join Conducteur using(nopermis)order by nom asc;"; break;
                case "desc": query = "SELECT nopermis, horaireservice, nom, prenom, noligne, immat FROM Parcours join Conducteur using(nopermis) order by nom desc;"; break;


                default:
                    query = "SELECT nopermis, horaireservice, nom, prenom, noligne, immat FROM Parcours join Conducteur using(nopermis)";
                    break;
            }

            List<Parcours> Parcours = new List<Parcours>();
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
                            Dictionary<string, object> ParcoursFields = new Dictionary<string, object>();
                            ParcoursFields.Add("nopermis", reader.GetString(0)); // TEXT
                            ParcoursFields.Add("horaireservice",reader.GetTimeSpan(1));//TIMESTAMP
                            ParcoursFields.Add("nom", reader.GetString(2)); // TEXT
                            ParcoursFields.Add("prenom", reader.GetString(3)); // TEXT
                            ParcoursFields.Add("noligne", reader.GetInt16(4));//INTERGER
                            ParcoursFields.Add("immat", reader.GetString(5)); // TEXT
                            var Parcours = new Parcours(ParcoursFields);
                            Parcourss.Add(Parcours);
                        }
                    }
                }
                return new JsonResult(Parcourss);
            }
        }


        [HttpPost]
        public JsonResult GetParcoursById(Int16? identifiant)
        {
            if (identifiant == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "SELECT * FROM Parcours WHERE norep=@identifiant";

            Parcours? Parcours = null;
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

                        Dictionary<string, object> ParcoursFields = new Dictionary<string, object>();
                        ParcoursFields.Add("norep", reader.GetInt16(0)); // Si c'est un smallint
                        ParcoursFields.Add("dateentreebus", reader.GetDateTime(1));
                        ParcoursFields.Add("description", reader.GetString(2));
                        ParcoursFields.Add("datedevis", reader.GetDateTime(3));
                        ParcoursFields.Add("datefinreparation", reader.GetDateTime(4));
                        ParcoursFields.Add("datedebutreparation", reader.GetDateTime(5));
                        ParcoursFields.Add("immat", reader.GetString(6));
                        ParcoursFields.Add("annee", reader.GetInt16(0));
                        ParcoursFields.Add("noseq", reader.GetInt16(0));
                        Parcours = new Parcours(ParcoursFields);
                        return new JsonResult(Parcours);

                    }
                }
            }
            return new JsonResult(3); // l'élément n'existe pas
        }

        [HttpPost]
        public JsonResult AddParcours(string rawdatedevis, string description, string immat, Int16? annee, Int16? noseq)
        {

            DateTime datedevis = DateTime.Parse(rawdatedevis);
            if (description == null || datedevis == null || immat == null)
            {
                return new JsonResult(2); // champ manquant
            }



            string constraintQuery = "SELECT * FROM Parcours WHERE datedevis = @datedevis and description = @description and immat = @immat and annee=@annee and noseq=@noseq";
            string connectionString = _configuration.GetConnectionString("ctvrtestDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@datedevis", datedevis);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@immat", immat);
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return new JsonResult(4); // L'élément existe déjà
                }
            }


            string query = "INSERT INTO Parcours (description, datedevis, immat) VALUES ( @description, @datedevis, @immat)";
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
        public JsonResult DeleteParcours(Int16? norep)
        {
            if (norep == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "DELETE FROM Parcours WHERE norep=@norep";

            string connectionString = _configuration.GetConnectionString("ctvrtestDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@norep", norep);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(3);
                }
            }

            return new JsonResult(1);
        }


        //vestige d'une civilisation utopique dans laquel utiliser l'api dans l'api fonctionne
        /* [HttpPost]
         public string getOneAtribute(Int16 norep, string atribute)
         {
             {
                 string baseUrl = "http://localhost:5107/api/ParcoursController/GetParcoursById?";

                 // Créer un client HTTP
                 using (HttpClient client = new HttpClient())
                 {
                     try
                     {
                         // Construire l'URL de l'API
                         string url = "http://localhost:5107/api/ParcoursController/GetParcoursById?identifiant=3";

                         // Envoyer une requête GET à l'API et attendre le résultat
                         HttpResponseMessage response = client.GetAsync(url).Result;

                         if (response.IsSuccessStatusCode)
                         {
                             // Lire la réponse JSON et attendre le résultat
                             string jsonResponse = response.Content.ReadAsStringAsync().Result;

                             // Parser le JSON pour accéder aux valeurs
                             var json = JObject.Parse(jsonResponse);
                             string atrib = json[atribute]?.ToString();
                             return atrib ?? "Attribut introuvable";
                         }
                         else
                         {
                             return "Erreur : API non accessible";
                         }
                     }
                     catch (Exception ex)
                     {
                         return $"Une erreur est survenue : {ex.Message}";
                     }
                 }
             }
         }



         [HttpPost]
         public IActionResult test()
         {
             Console.WriteLine("test");
             string result = getOneAtribute(3, "datedevis");
             Console.WriteLine(result);

             return Ok(result); // Renvoie le résultat comme réponse HTTP 200
         }*/

        [HttpPost]
        public JsonResult updateParcours(Int16 norep, int? annee, int? noseq, string? datedebutreparation, string? datefinreparation, string? entreegarage)
        {




            if (norep == null || norep == 0)
            {
                return new JsonResult(2); // champ manquant
            }



            string constraintQuery = "SELECT * FROM Parcours WHERE norep=@norep";
            string connectionString = _configuration.GetConnectionString("ctvrtestDBConn");
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

                string query = "update Parcours set datedebutreparation = @debutrep WHERE norep=@norep ";
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
                string query = "update Parcours set datefinreparation = @finrep WHERE norep = @norep ";
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
                string query = "update Parcours set dateentreebus = @dateentreegarage WHERE norep=@norep ";
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
                string query = "update Parcours set noseq=@noseq, annee=@annee where norep=@norep ";
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

    }

}


