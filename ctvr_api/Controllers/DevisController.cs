using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class devisController : ControllerBase
    {
        private IConfiguration _configuration;

        public devisController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllDevis()
        {

            string queryUtil = "SELECT * FROM utiliser";

            List<Devis> allDevis = new List<Devis>();
            List<string> listCodeF = new List<string>();

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmdUtil = dataSource.CreateCommand(queryUtil)) // preparation de la commande
            {
                using (var readerUtil = cmdUtil.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir les ligne de utiliser
                {
                    while (readerUtil.Read()) // tant qu'il y a des lignes dans utiliser
                    {
                        Dictionary<string, object> devisFields = new Dictionary<string, object>();

                        if (!allDevis.Any(devis => devis.norep == readerUtil.GetInt16(0)))//je verifie si un devis existe deja avec ce no d'intervention pour eviter les doublon à cause de double clé primaire dans la base
                        {//je rentre ssi aucun devis n'a le norep courant sinon ce devis à deja été traiter donc je skip

                            //recuperation de la date
                            string queryDate = "SELECT DateDevis FROM Intervention where norep=@norep";
                            using (var cmdDate = dataSource.CreateCommand(queryDate)) // preparation de la commande
                            {
                                cmdDate.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerDate = cmdDate.ExecuteReader())
                                {
                                    if (readerDate.Read()) // ssi on a un retour
                                    {
                                        devisFields.Add("dateCreation", DateOnly.FromDateTime(readerDate.GetDateTime(0)));

                                    }
                                }
                            }//fin de la recup de la date

                            //recuperation des fournitures
                            string queryCodeF = "SELECT codeF FROM utiliser where norep=@norep";
                            using (var cmdCodeF = dataSource.CreateCommand(queryCodeF)) // preparation de la commande
                            {
                                listCodeF = new List<string>();
                                cmdCodeF.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerCodeF = cmdCodeF.ExecuteReader())
                                {
                                    while (readerCodeF.Read()) // tant qu'on a un retour
                                    {
                                        listCodeF.Add(readerCodeF.GetString(0));

                                    }

                                }
                            }//fin de la recup des fournitures. Elle n'ont pas été ajouté à devisField mais seront ajouté directement au futur object

                            //recuperation du prix, de la TVA et du total ttc
                            string queryPrix = "SELECT sum(prixUnitaire) FROM utiliser where norep=@norep";
                            using (var cmdPrix = dataSource.CreateCommand(queryPrix)) // preparation de la commande
                            {
                                cmdPrix.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerPrix = cmdPrix.ExecuteReader())
                                {
                                    if (readerPrix.Read()) // ssi on a un retour
                                    {
                                        devisFields.Add("prix", readerPrix.GetDouble(0));
                                        devisFields.Add("TVA", (readerPrix.GetDouble(0) * 0.2));
                                        devisFields.Add("prixTTC", (double)devisFields["TVA"] + (double)devisFields["prix"]);


                                    }
                                }
                            }//fin de la recup du prix, de la TVA et du total ttc
                            devisFields.Add("norep", readerUtil.GetInt16(0));


                            Devis theDevis = new Devis(devisFields);
                            theDevis.codeF = listCodeF;
                            allDevis.Add(theDevis);
                        }

                    }
                }

                return new JsonResult(allDevis);
            }
            /*
                        [HttpPost]
                        public JsonResult GetdevisById(Int16? nodevis)
                        {
                            if (nodevis == null)
                            {
                                return new JsonResult(2); // champ manquant
                            }
                            devis devis = null;
                            string query = "SELECT * FROM devis WHERE nodevis=@nodevis";
                            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
                            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

                            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
                            {
                                cmd.Parameters.AddWithValue("@nodevis", nodevis); // ajout du paramètre

                                using (var reader = cmd.ExecuteReader())
                                {
                                    Dictionary<string, Object> devisFields = new Dictionary<string, Object>();
                                    if (reader.Read()) // ssi on a un retour
                                    {
                                        devisFields.Add("nodevis", reader.GetInt16(0));
                                        devisFields.Add("nomarretdepart", reader.GetString(1));
                                        devisFields.Add("nomarretterminus", reader.GetString(2));
                                        devis = new devis(devisFields);
                                        return new JsonResult(devis);
                                    }
                                }
                            }
                            return new JsonResult(3); // l'élément n'existe pas
                        }

                        [HttpPost]
                        public JsonResult Adddevis(Int16? nodevis, string? nomarretdepart, string? nomarretterminus)
                        {
                            if (nodevis == null
                                || string.IsNullOrWhiteSpace(nomarretdepart)
                                || string.IsNullOrWhiteSpace(nomarretterminus)
                                )
                            {
                                return new JsonResult(2); // champ manquant
                            }

                            // Vérification de la présence de l'devis à entrer dans la base
                            string constraintQuery = "SELECT * FROM devis where nodevis=@nodevis";

                            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
                            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

                            using (var cmd = dataSource.CreateCommand(constraintQuery))
                            {
                                cmd.Parameters.AddWithValue("@nodevis", nodevis);
                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read()) return new JsonResult(4); // l'élément existe déjà
                                }
                            }

                            string query = "INSERT INTO devis VALUES (@nodevis, @nomarretdepart, @nomarretterminus)";

                            using (var cmd = dataSource.CreateCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@nodevis", nodevis);
                                cmd.Parameters.AddWithValue("@nomarretdepart", nomarretdepart);
                                cmd.Parameters.AddWithValue("@nomarretterminus", nomarretterminus);

                                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de devis affectées -> 1 si tout se passe bien

                                if (result == 0) // si ca n'a pas fonctionné
                                {
                                    return new JsonResult(5);
                                }
                            }

                            return new JsonResult(1); // fonctionnement correcte
                        }

                        [HttpDelete]
                        public JsonResult Deletedevis(Int16? nodevis)
                        {
                            if (nodevis == null)
                            {
                                return new JsonResult(2); // champ manquant
                            }

                            string query = "DELETE FROM devis WHERE nodevis=@nodevis";

                            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
                            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

                            using (var cmd = dataSource.CreateCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@nodevis", nodevis);

                                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de devis affectées -> 1 si tout se passe bien

                                if (result == 0) // si ca n'a pas fonctionné
                                {
                                    return new JsonResult(3);
                                }
                            }

                            return new JsonResult(1);
                        }

                        [HttpPost]
                        public JsonResult updateIntervention(Int16? nodevis, string? nomarretdepart, string? nomarretterminus)
                        {

                            if (nodevis == null || nodevis == 0)
                            {
                                return new JsonResult(2); // champ manquant
                            }


                            string constraintQuery = "SELECT * FROM devis WHERE nodevis=@nodevis";
                            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
                            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

                            using (var cmd = dataSource.CreateCommand(constraintQuery))
                            {
                                cmd.Parameters.AddWithValue("@nodevis", nodevis);


                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (!reader.Read())
                                        return new JsonResult(3); // L'élément n'existe pas mais devrait
                                }
                            }

                            if (nomarretdepart != null)
                            {
                                string query = "update devis set nomarretdepart = @nomarretdepart WHERE nodevis = @nodevis ";
                                using (var cmd = dataSource.CreateCommand(query))
                                {
                                    cmd.Parameters.AddWithValue("@nodevis", nodevis);
                                    cmd.Parameters.AddWithValue("@nomarretdepart", nomarretdepart);

                                    int result = cmd.ExecuteNonQuery();
                                    if (result == 0)
                                    {
                                        return new JsonResult(5); // Erreur d'insertion
                                    }
                                }
                            }

                            if (nomarretterminus != null)
                            {
                                string query = "update devis set nomarretterminus = @nomarretterminus WHERE nodevis=@nodevis ";
                                using (var cmd = dataSource.CreateCommand(query))
                                {
                                    cmd.Parameters.AddWithValue("@nodevis", nodevis);
                                    cmd.Parameters.AddWithValue("@nomarretterminus", nomarretterminus);

                                    int result = cmd.ExecuteNonQuery();
                                    if (result == 0)
                                    {
                                        return new JsonResult(5); // Erreur d'insertion
                                    }
                                }
                            }


                            return new JsonResult(1); // Ajout réussi
                        }*/

        }


    }
}
