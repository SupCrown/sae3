using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;

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

            string queryUtil = "SELECT * FROM utiliser where norep not in (select norep from necessiter where nbhdevis = nbhvalidees);";

            List<Devis> allDevis = new List<Devis>();
            Dictionary<string, object> listCodeF = [];
            Dictionary<string, object> listCodeM = [];
            List<Dictionary<string, object>> listQuantite = [];
            List<Dictionary<string, object>> listMoe = [];

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
                            string queryCodeM = "SELECT codeM, nbhdevis FROM necessiter where norep=@norep";
                            using (var cmdCodeM = dataSource.CreateCommand(queryCodeM)) // preparation de la commande
                            {
                                cmdCodeM.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerCodeM = cmdCodeM.ExecuteReader())
                                {
                                    while (readerCodeM.Read()) // tant qu'on a un retour
                                    {
                                        listCodeM = new Dictionary<string, object>();
                                        listCodeM.Add("code", readerCodeM.GetString(0));
                                        listCodeM.Add("qte", readerCodeM.GetInt32(1));
                                        listMoe.Add(listCodeM);

                                    }

                                }
                            }//fin de la recup des fournitures. Elle n'ont pas été ajouté à devisField mais seront ajouté directement au futur object

                            //recuperation des fournitures
                            string queryCodeF = "SELECT codeF, qtedevis FROM utiliser where norep=@norep";
                            using (var cmdCodeF = dataSource.CreateCommand(queryCodeF)) // preparation de la commande
                            {
                           
                                cmdCodeF.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser


                                using (var readerCodeF = cmdCodeF.ExecuteReader())
                                {
                                    while (readerCodeF.Read()) // tant qu'on a un retour
                                    {
                                        listCodeF = new Dictionary<string, object>();
                                        listCodeF.Add("code", readerCodeF.GetString(0));
                                        listCodeF.Add("qte", readerCodeF.GetInt32(1));
                                        listQuantite.Add(listCodeF);


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
                            theDevis.fourniture = listQuantite;
                            theDevis.mainOeuvre = listMoe;
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
            Dictionary<string, object> listCodeF = [];
            Dictionary<string, object> listCodeM = [];
            List<Dictionary<string, object>> listQuantite = [];
            List<Dictionary<string, object>> listMoe = [];


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
                            string queryCodeM = "SELECT codeM, nbhdevis FROM necessiter where norep=@norep";
                            using (var cmdCodeM = dataSource.CreateCommand(queryCodeM)) // preparation de la commande
                            {
                               
                                cmdCodeM.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerCodeM = cmdCodeM.ExecuteReader())
                                {
                                    while (readerCodeM.Read()) // tant qu'on a un retour
                                    {
                                        listCodeM = new Dictionary<string, object>();
                                        listCodeM.Add("code", readerCodeM.GetString(0));
                                        listCodeM.Add("qte", readerCodeM.GetInt32(1));
                                        listMoe.Add(listCodeM);

                                    }

                                }
                            }//fin de la recup des fournitures. Elle n'ont pas été ajouté à devisField mais seront ajouté directement au futur object

                            //recuperation des fournitures
                            string queryCodeF = "SELECT codeF, qtedevis FROM utiliser where norep=@norep";
                            using (var cmdCodeF = dataSource.CreateCommand(queryCodeF)) // preparation de la commande
                            {
                                cmdCodeF.Parameters.AddWithValue("@norep", readerUtil.GetInt16(0)); // ajout du paramètre norep recuperé dans la table utiliser

                                using (var readerCodeF = cmdCodeF.ExecuteReader())
                                {
                                    while (readerCodeF.Read()) // tant qu'on a un retour
                                    {
                                        listCodeF = new Dictionary<string, object>();
                                        listCodeF.Add("code", readerCodeF.GetString(0));
                                        listCodeF.Add("qte", readerCodeF.GetInt32(1));
                                        listQuantite.Add(listCodeF);

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
                            theDevis.fourniture = listQuantite;
                            theDevis.mainOeuvre = listMoe;
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
        public JsonResult valider(short norep)
        {
            string queryQte = "SELECT norep, qtedevis FROM Utiliser WHERE norep = @norep ";
            string queryH = "SELECT norep, nbhdevis FROM Necessiter WHERE norep = @norep";
            string updateQuery = "UPDATE Utiliser SET qtevalidee = @qtevalidee WHERE norep = @norep and qtedevis=@qte";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");

            try
            {
                using (var dataSource = NpgsqlDataSource.Create(connectionString))
                {
                    // Traitement de la table "Utiliser"
                    using (var cmdQte = dataSource.CreateCommand(queryQte))
                    {
                        cmdQte.Parameters.AddWithValue("@norep", norep);

                        using (var readerQte = cmdQte.ExecuteReader())
                        {
                            while (readerQte.Read())
                            {
                                // Récupération des valeurs spécifiques à cette ligne
                                short norepFromReader = readerQte.GetInt16(0); // norep
                                int qte = readerQte.GetInt32(1);
                                Console.WriteLine(qte);             // qtevalidee

                                // Mise à jour de la ligne
                                using (var cmdUpdate = dataSource.CreateCommand(updateQuery))
                                {
                                    Console.WriteLine(qte);
                                    cmdUpdate.Parameters.AddWithValue("@norep", norepFromReader);
                                    cmdUpdate.Parameters.AddWithValue("@qtevalidee", qte);
                                    cmdUpdate.Parameters.AddWithValue("@qte", qte);

                                    int result = cmdUpdate.ExecuteNonQuery();

                                    if (result == 0)
                                    {
                                        return new JsonResult(5); // Erreur d'insertion
                                    }
                                }
                            }
                        }
                    }

                    // Traitement de la table "Necessiter"
                    using (var cmdH = dataSource.CreateCommand(queryH))
                    {
                        cmdH.Parameters.AddWithValue("@norep", norep);

                        using (var readerH = cmdH.ExecuteReader())
                        {
                            while (readerH.Read())
                            {
                                // Récupération des valeurs spécifiques à cette ligne
                                short norepFromReader = readerH.GetInt16(0); // norep
                                int H = readerH.GetInt32(1);               // qtevalidee

                                // Mise à jour de la ligne
                                updateQuery = "UPDATE necessiter SET nbhvalidees = @hvalide WHERE norep = @norep and nbhdevis = @nbh";
                                using (var cmdUpdate = dataSource.CreateCommand(updateQuery))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@norep", norepFromReader);
                                    cmdUpdate.Parameters.AddWithValue("@hvalide", H);
                                    cmdUpdate.Parameters.AddWithValue("@nbh", H);

                                    int result = cmdUpdate.ExecuteNonQuery();

                                    if (result == 0)
                                    {
                                        return new JsonResult(5); // Erreur d'insertion
                                    }
                                }
                            }
                        }
                    }
                }

                // Tout s'est bien passé
                return new JsonResult(1);
            }
            catch (Exception ex)
            {
                // Gestion des exceptions
                return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddDevis(Int16 norep, Dictionary<string, Dictionary<string, int>> dicoDevis)
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

            string queryNec = "INSERT INTO necessiter(norep, codeM, NbHDevis, tauxHoraire) VALUES (@norep, @codeM, @NbHDevis, @TauxHoraire)";
            string queryUti = "INSERT INTO utiliser(norep, codeF, QteDevis, PrixUnitaire) VALUES (@norep, @codeF, @QteDevis, @PrixUnitaire)";
            string queryInter = "UPDATE Intervention SET DateDevis = @dateDevis WHERE norep = @norep";

            using (var cmd = dataSource.CreateCommand(queryNec))//insertion des ligne dans la table necessiter. iteration autant de fois qu'il y a d'element dans le dico
            {
                foreach (var item in dicoDevis["nec"])//iteration dans le dico
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@norep", norep);
                    cmd.Parameters.AddWithValue("@codeM", item.Key);
                    cmd.Parameters.AddWithValue("@NbHDevis", item.Value);

                    string query = "select tauxHoraireCourant from moe where codeM = @codeM";//recuperation du taux horaire actuel
                    using (var cmdTaux = dataSource.CreateCommand(query))
                    {
                        cmdTaux.Parameters.AddWithValue("@codeM", item.Key);
                        using (var readerTaux = cmdTaux.ExecuteReader())
                        {
                            if (readerTaux.Read())
                            {
                                cmd.Parameters.AddWithValue("@TauxHoraire", readerTaux.GetInt32(0));
                            }
                        }
                    }

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
                        return new JsonResult(61); // une clé étrangère ne correxpond pas pour la table necessiter
                    }

                }

            }//fin de necessiter

            using (var cmd = dataSource.CreateCommand(queryUti))//insertion des ligne dans la table utiliser. iteration autant de fois qu'il y a d'element dans le dico
            {
                foreach (var item in dicoDevis["uti"])//iteration dans le dico
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@norep", norep);
                    cmd.Parameters.AddWithValue("@codeF", item.Key);
                    cmd.Parameters.AddWithValue("@QteDevis", item.Value);

                    string query = "select prixunitairecourant from fourniture where codef = @codef";//recuperation du prix unitaire actuel
                    using (var cmdUnit = dataSource.CreateCommand(query))
                    {
                        cmdUnit.Parameters.AddWithValue("@codef", item.Key);
                        using (var readerUnit = cmdUnit.ExecuteReader())
                        {
                            if (readerUnit.Read())
                            {
                                cmd.Parameters.AddWithValue("@prixunitaire", readerUnit.GetInt32(0));
                            }
                        }
                    }

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
                        return new JsonResult(62); // une clé étrangère ne correxpond pas pour la table Utiliser
                    }

                }

            }//fin utiliser

            using (var cmd = dataSource.CreateCommand(queryInter))//insertion de la date courante dans la table intervention pour oficialiser lajout d'un devis pour cette intervention
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@norep", norep);
                cmd.Parameters.AddWithValue("@datedevis", DateOnly.FromDateTime(DateTime.Now));

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
                    return new JsonResult(63); // une clé étrangère ne correxpond pas pour la table Intervention
                }



            }//fin datedevis


            return new JsonResult(1); // fonctionnement correcte
        }

    }



}

/*tests*************************************************************
{
  "nec": {
    "MOE07": 25,
    "M100": 30
  },
  "uti": {
    "TURBO": 2,
    "FAISC": 7
  }
}
*/
