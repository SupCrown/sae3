using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class survenirController : ControllerBase
    {
        private IConfiguration _configuration;

        public survenirController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult addConducteur(string? nopermis, TimeOnly? heure, DateOnly? date, Int16? annee, Int16? noseq, string? dmgBus, string? dmgConducteur, bool signatureConduct, bool signatureControl)
        {
            if (string.IsNullOrWhiteSpace(nopermis)
                || heure == null
                || string.IsNullOrWhiteSpace(dmgBus)
                || date == null
                || string.IsNullOrWhiteSpace(dmgConducteur)
                || annee == null
                || noseq == null
                )
            {
                return new JsonResult(2); // champ manquant
            }

            DateTime timing = date.Value.ToDateTime(heure.Value);

            //obtention de l'horaireservice du parcour le plus proche
            string queryTiming = "select horaireservice from parcours where horaireservice<@timing and nopermis = @nopermis order by horaireservice desc limit 1";
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmdTiming = dataSource.CreateCommand(queryTiming))
            {
                cmdTiming.Parameters.AddWithValue("@nopermis", nopermis);
                cmdTiming.Parameters.AddWithValue("@timing", timing);

                using (var readerTiming = cmdTiming.ExecuteReader())
                {
                    if (readerTiming.Read())
                    {
                        timing = readerTiming.GetDateTime(0);
                    }
                    else
                    {
                        return new JsonResult(6); // aucun parcours ne correspond
                    }
                }
            }

            // Vérification de la présence de survenir à entrer dans la base
            string constraintQuery = "SELECT * FROM survenir where nopermis=@nopermis and horaireservice=@timing and annee=@annee and noseq=@noseq ";

            connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(constraintQuery))
            {
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                cmd.Parameters.AddWithValue("@timing", timing);
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);


                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new JsonResult(4); // l'élément existe déjà}
                    }
                }
            }

            string query = "INSERT INTO survenir VALUES (@annee, @noseq, @nopermis, @timing, @dmgBus, @dmgConducteur, @signatureConduct, @signatureControl)";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                cmd.Parameters.AddWithValue("@nopermis", nopermis);
                cmd.Parameters.AddWithValue("@timing", timing);
                cmd.Parameters.AddWithValue("@dmgBus", dmgBus);
                cmd.Parameters.AddWithValue("@dmgConducteur", dmgConducteur);
                cmd.Parameters.AddWithValue("@signatureConduct", signatureConduct);
                cmd.Parameters.AddWithValue("@signatureControl", signatureControl);



                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }

            return new JsonResult(1); // fonctionnement correcte

        }
        [HttpPost]
        public JsonResult signer(string? qui, string? nopermis, TimeOnly? heure, DateOnly? date, short? annee, short? noseq)
        {
            if (string.IsNullOrWhiteSpace(nopermis) || heure == null || string.IsNullOrWhiteSpace(qui) || date == null || annee == null || noseq == null)
            {
                return new JsonResult(2); // Champ manquant
            }

            // Combinaison de DateOnly et TimeOnly
            DateTime timing = date.Value.ToDateTime(heure.Value);

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            using (var dataSource = NpgsqlDataSource.Create(connectionString))
            {
                // Obtention de l'horaireservice du parcours le plus proche
                string queryTiming = "SELECT horaireservice FROM parcours WHERE horaireservice < @timing AND nopermis = @nopermis ORDER BY horaireservice DESC LIMIT 1";
                using (var cmdTiming = dataSource.CreateCommand(queryTiming))
                {
                    cmdTiming.Parameters.AddWithValue("@nopermis", nopermis);
                    cmdTiming.Parameters.AddWithValue("@timing", timing);

                    using (var readerTiming = cmdTiming.ExecuteReader())
                    {
                        if (readerTiming.Read())
                        {
                            timing = readerTiming.GetDateTime(0);
                        }
                        else
                        {
                            return new JsonResult(6); // Aucun parcours ne correspond
                        }
                    }
                }

                // Vérification de la présence de "survenir"
                string constraintQuery = "SELECT * FROM survenir WHERE nopermis = @nopermis AND horaireservice = @timing AND annee = @annee AND noseq = @noseq";
                using (var cmdConstraint = dataSource.CreateCommand(constraintQuery))
                {
                    cmdConstraint.Parameters.AddWithValue("@nopermis", nopermis);
                    cmdConstraint.Parameters.AddWithValue("@timing", timing);
                    cmdConstraint.Parameters.AddWithValue("@annee", annee);
                    cmdConstraint.Parameters.AddWithValue("@noseq", noseq);

                    using (var readerConstraint = cmdConstraint.ExecuteReader())
                    {
                        if (!readerConstraint.Read())
                        {
                            return new JsonResult(3); // L'élément n'existe pas
                        }
                    }
                }

                // Déclaration de la requête de mise à jour
                string query;
                bool signatureParam = true;

                switch (qui)
                {
                    case "controleur":
                        query = "UPDATE survenir SET signatureControleur = @signature WHERE nopermis = @nopermis AND horaireservice = @timing AND annee = @annee AND noseq = @noseq";
                        break;

                    case "conducteur":
                        query = "UPDATE survenir SET signatureConducteur = @signature WHERE nopermis = @nopermis AND horaireservice = @timing AND annee = @annee AND noseq = @noseq";
                        break;

                    default:
                        return new JsonResult(62); // Ni conducteur, ni contrôleur
                }

                // Exécution de la mise à jour
                using (var cmdUpdate = dataSource.CreateCommand(query))
                {
                    cmdUpdate.Parameters.AddWithValue("@nopermis", nopermis);
                    cmdUpdate.Parameters.AddWithValue("@timing", timing);
                    cmdUpdate.Parameters.AddWithValue("@annee", annee);
                    cmdUpdate.Parameters.AddWithValue("@noseq", noseq);
                    cmdUpdate.Parameters.AddWithValue("@signature", signatureParam);

                    int result = cmdUpdate.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return new JsonResult(5); // Erreur lors de la mise à jour
                    }
                }
            }

            return new JsonResult(1); // Fonctionnement correct
        }

    }
}