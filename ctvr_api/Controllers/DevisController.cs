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
            List<string> listCodeM = new List<string>();

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
                            string queryCodeM = "SELECT codeM FROM necessiter where norep=@norep";
                            using (var cmdCodeM = dataSource.CreateCommand(queryCodeM)) // preparation de la commande
                            {
                                listCodeM = new List<string>();
                                cmdCodeM.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerCodeM = cmdCodeM.ExecuteReader())
                                {
                                    while (readerCodeM.Read()) // tant qu'on a un retour
                                    {
                                        listCodeM.Add(readerCodeM.GetString(0));

                                    }

                                }
                            }//fin de la recup des fournitures. Elle n'ont pas été ajouté à devisField mais seront ajouté directement au futur object

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

                            //recuperation du prix des fournitures
                            string queryPrixF = "SELECT sum(prixUnitaire) FROM utiliser where norep=@norep";
                            using (var cmdPrixF = dataSource.CreateCommand(queryPrixF)) // preparation de la commande
                            {
                                cmdPrixF.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerPrixF = cmdPrixF.ExecuteReader())
                                {
                                    if (readerPrixF.Read()) // ssi on a un retour
                                    {
                                        devisFields.Add("totalFourn", readerPrixF.GetDouble(0));

                                    }
                                    else
                                    {
                                        devisFields.Add("totalFourn", 0);
                                    }
                                }
                            }//fin de la recup du prix des fournitures

                            //recuperation du prix de la main d'oeuvre
                            string queryPrixM = "SELECT sum(tauxhoraire) FROM necessiter where norep=@norep";
                            using (var cmdPrixM = dataSource.CreateCommand(queryPrixM)) // preparation de la commande
                            {
                                cmdPrixM.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerPrixM = cmdPrixM.ExecuteReader())
                                {
                                    if (readerPrixM.Read()) // ssi on a un retour
                                    {
                                        devisFields.Add("totalMoe", readerPrixM.GetDouble(0));

                                    }
                                    else
                                    {
                                        devisFields.Add("totalMoe", 0);
                                    }
                                }
                            }//fin de la recup du prix de la main d'oeuvre

                            devisFields.Add("prix", ((double)devisFields["totalMoe"] + (double)devisFields["totalFourn"]));
                            devisFields.Add("TVA", (double)devisFields["prix"] * 0.2);
                            devisFields.Add("prixTTC", (double)devisFields["TVA"] + (double)devisFields["prix"]);
                            devisFields.Add("norep", readerUtil.GetInt16(0));


                            Devis theDevis = new Devis(devisFields);
                            theDevis.codeF = listCodeF;
                            theDevis.codeM = listCodeM;
                            allDevis.Add(theDevis);
                        }

                    }
                }

                return new JsonResult(allDevis);
            }
        }
        [HttpGet]
        public JsonResult GetDevisById(Int16 norep)
        {

            string queryUtil = "SELECT * FROM utiliser where norep = @norep";

            List<Devis> allDevis = new List<Devis>();
            List<string> listCodeF = new List<string>();
            List<string> listCodeM = new List<string>();

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmdUtil = dataSource.CreateCommand(queryUtil)) // preparation de la commande
            {
                cmdUtil.Parameters.AddWithValue("@norep", norep);
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
                            string queryCodeM = "SELECT codeM FROM necessiter where norep=@norep";
                            using (var cmdCodeM = dataSource.CreateCommand(queryCodeM)) // preparation de la commande
                            {
                                listCodeM = new List<string>();
                                cmdCodeM.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerCodeM = cmdCodeM.ExecuteReader())
                                {
                                    while (readerCodeM.Read()) // tant qu'on a un retour
                                    {
                                        listCodeM.Add(readerCodeM.GetString(0));

                                    }

                                }
                            }//fin de la recup des fournitures. Elle n'ont pas été ajouté à devisField mais seront ajouté directement au futur object

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

                            //recuperation du prix des fournitures
                            string queryPrixF = "SELECT sum(prixUnitaire) FROM utiliser where norep=@norep";
                            using (var cmdPrixF = dataSource.CreateCommand(queryPrixF)) // preparation de la commande
                            {
                                cmdPrixF.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerPrixF = cmdPrixF.ExecuteReader())
                                {
                                    if (readerPrixF.Read()) // ssi on a un retour
                                    {
                                        devisFields.Add("totalFourn", readerPrixF.GetDouble(0));

                                    }
                                    else
                                    {
                                        devisFields.Add("totalFourn", 0);
                                    }
                                }
                            }//fin de la recup du prix des fournitures

                            //recuperation du prix de la main d'oeuvre
                            string queryPrixM = "SELECT sum(tauxhoraire) FROM necessiter where norep=@norep";
                            using (var cmdPrixM = dataSource.CreateCommand(queryPrixM)) // preparation de la commande
                            {
                                cmdPrixM.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerPrixM = cmdPrixM.ExecuteReader())
                                {
                                    if (readerPrixM.Read()) // ssi on a un retour
                                    {
                                        devisFields.Add("totalMoe", readerPrixM.GetDouble(0));

                                    }
                                    else
                                    {
                                        devisFields.Add("totalMoe", 0);
                                    }
                                }
                            }//fin de la recup du prix de la main d'oeuvre

                            devisFields.Add("prix", ((double)devisFields["totalMoe"] + (double)devisFields["totalFourn"]));
                            devisFields.Add("TVA", (double)devisFields["prix"] * 0.2);
                            devisFields.Add("prixTTC", (double)devisFields["TVA"] + (double)devisFields["prix"]);
                            devisFields.Add("norep", readerUtil.GetInt16(0));


                            Devis theDevis = new Devis(devisFields);
                            theDevis.codeF = listCodeF;
                            theDevis.codeM = listCodeM;
                            allDevis.Add(theDevis);
                        }

                    }
                }

                return new JsonResult(allDevis);
            }
        }


        [HttpDelete]
        public JsonResult Deletedevis(Int16? norep)
        {
            if (norep == null)
            {
                return new JsonResult(2); // champ manquant
            }

            string query = "DELETE FROM utiliser WHERE norep=@norep";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@norep", norep);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de devis affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(3);
                }
            }

            query = "DELETE FROM necessiter WHERE norep=@norep";

            connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@norep", norep);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de devis affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(3);
                }
            }

            query = "UPDATE Intervention SET DateDevis = NULL WHERE norep = @norep";

            connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@norep", norep);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de devis affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(3);
                }
            }

            return new JsonResult(1);
        }



        [HttpPost]
        public JsonResult AddDevis(Int16 norep, Dictionary<string, int> dicPiece, Dictionary<string, int> dicMoe, double taux, double pUnit)
        {

            // Vérification de la présence de l'devis à entrer dans la base
            string constraintQuery = "SELECT * FROM necessiter where norep=@norep";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@norep", norep);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return new JsonResult(4); // l'élément existe déjà
                }
            }

            string queryNec = "INSERT INTO necessiter(norep, codeM, NbHDevis, @auxHoraire) VALUES (@norep, @codeM, @NbHDevis, @TauxHoraire)";
            string queryUti = "INSERT INTO devis(norep, codeF, QteDevis, PrixUnitaire) VALUES (@norep, @codeF, @QteDevis, @PrixUnitaire)";
            string queryInter = "UPDATE Intervention SET DateDevis = @dateDevis WHERE norep = @norep";
            string query = "";

            using (var cmd = dataSource.CreateCommand(queryNec))
            {
                foreach (var item in dicMoe)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@norep", norep);
                    cmd.Parameters.AddWithValue("@codeM", item.Key);
                    cmd.Parameters.AddWithValue("@NbHDevis", item.Value);

                    query = "select tauxHoraireCourant from moe where codeM = @codeM";
                    using (var cmdTaux = dataSource.CreateCommand(Query))
                    {
                        cmdTaux.Parameters.AddWithValue("@codeM", item.Key);
                        using (var readerTaux = cmdTaux.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cmd.Parameters.AddWithValue("@TauxHoraire", item.Key);
                            }
                        }
                    }
                    try
                    {
                        
                    }
                    catch (System.Exception)
                    {
                        
                        throw;
                    }

                    int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de devis affectées -> 1 si tout se passe bien
                }


                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }

            using (var cmd = dataSource.CreateCommand(queryUti))
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

            using (var cmd = dataSource.CreateCommand(queryInter))
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

    }



}
