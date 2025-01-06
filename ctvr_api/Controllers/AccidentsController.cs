using ctvr_api;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CTVR_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccidentsController : ControllerBase
    {
        private IConfiguration _configuration;
        public AccidentsController(IConfiguration configuration)
        { 
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetAllAccidents()
        {
            string query = "SELECT * FROM accident";

            List<Dictionary<string, object?>> accidents = []; // [] = new List<Accident>()
            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                using (var reader = cmd.ExecuteReader()) // exécute la commande et renvoie un reader qui peut parcourir le resultat
                {
                    while (reader.Read()) // pour chacune des lignes renvoyées
                    {
                        Dictionary<string, object?> accidentFields = new Dictionary<string, object?>();
                        accidentFields.Add("annee", reader.GetInt32(0));
                        accidentFields.Add("noseq", reader.GetInt32(1));
                        accidentFields.Add("dateaccident", DateOnly.FromDateTime(reader.GetDateTime(2)));
                        accidentFields.Add("heureaccident", reader.GetTimeSpan(3));
                        accidentFields.Add("rue", reader.GetString(4));
                        accidentFields.Add("codepostal", reader.GetString(5));
                        accidentFields.Add("ville", reader.GetString(6));

                        if (!reader.IsDBNull(11))
                        {
                            accidentFields.Add("etatdossieraccident", EtatsAccident.Cloturer.ToString());
                        }
                        else if (!reader.IsDBNull(10))
                        {
                            accidentFields.Add("etatdossieraccident", EtatsAccident.Valide.ToString());
                        }
                        else if (!reader.IsDBNull(9))
                        {
                            accidentFields.Add("etatdossieraccident", EtatsAccident.Transmit.ToString());
                        }
                        else
                        {
                            accidentFields.Add("etatdossieraccident", EtatsAccident.Ouvert.ToString());
                        }
                        accidents.Add(accidentFields);
                    }
                }
            }
            return new JsonResult(accidents);
        }

        [HttpGet]
        public JsonResult GetOneByID(int? annee, int? noseq)
        {
            if (annee==null || noseq == null) return new JsonResult(2); // chanmp manquant

            string query = "SELECT * FROM accident WHERE annee=@annee AND noseq=@noseq";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            Dictionary<string, object?> accidentFields = [];

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return new JsonResult(3); // l'élément n'existe pas
                    } else
                    {
                        accidentFields.Add("annee", reader.GetInt32(0));
                        accidentFields.Add("noseq", reader.GetInt32(1));
                        accidentFields.Add("dateaccident", DateOnly.FromDateTime(reader.GetDateTime(2)));
                        accidentFields.Add("heureaccident", reader.GetTimeSpan(3));
                        accidentFields.Add("rue", reader.GetString(4));
                        accidentFields.Add("codepostal", reader.GetString(5));
                        accidentFields.Add("ville", reader.GetString(6));
                        accidentFields.Add("resumecirconstances", reader.GetString(7));
                        accidentFields.Add("dateouverturedossier", DateOnly.FromDateTime(reader.GetDateTime(8)));
                        accidentFields.Add("datetransmissionassur", reader.IsDBNull(9) ? null : DateOnly.FromDateTime(reader.GetDateTime(9)));
                        accidentFields.Add("datevalidationassur", reader.IsDBNull(10) ? null : DateOnly.FromDateTime(reader.GetDateTime(10)));
                        accidentFields.Add("datecloturedossier", reader.IsDBNull(11) ? null : DateOnly.FromDateTime(reader.GetDateTime(11)));
                        accidentFields.Add("sommerecue", reader.IsDBNull(12) ? null : reader.GetInt32(12));
                        accidentFields.Add("intemperie", reader.GetString(13));
                    }
                }
            }
            
            List<Impliquer> impliquers = [];

            query = "SELECT annee, noseq, notiers, natureblessures, degatsmateriels, sommeversee, nomtiers, prenomtiers, datenaissance, adressetiers, cptiers, villetiers FROM impliquer join tiers using(notiers) WHERE annee=@annee AND noseq=@noseq";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object?> impliquerFields = [];
                        Dictionary<string, object?> tiersFields = [];
                        impliquerFields.Add("annee", annee);
                        impliquerFields.Add("noseq", noseq);
                        impliquerFields.Add("natureblessures", reader.GetString(3));
                        impliquerFields.Add("degatsmateriels", reader.GetString(4));
                        impliquerFields.Add("sommeversee", reader.IsDBNull(5) ? null : reader.GetInt32(5));
                        
                        tiersFields.Add("notiers", reader.GetInt32(2));
                        tiersFields.Add("nomtiers", reader.GetString(6));
                        tiersFields.Add("prenomtiers", reader.GetString(7));
                        tiersFields.Add("datenaissance", DateOnly.FromDateTime(reader.GetDateTime(8)));
                        tiersFields.Add("adressetiers", reader.GetString(9));
                        tiersFields.Add("cptiers", reader.GetString(10));
                        tiersFields.Add("villetiers", reader.GetString(11));
                        
                        Tiers tiers = new Tiers(tiersFields);
                        
                        impliquerFields.Add("tiers", new List<Tiers> { tiers });

                        impliquers.Add(new Impliquer(impliquerFields));
                    }
                }
            }
            accidentFields.Add("impliquers", impliquers);
            return new JsonResult(new List<Object> { accidentFields });
        }

        [HttpPut]
        public JsonResult Insert(DateTime? dateAccident, string? rue, string? codepostal, string? ville, string? resumecirconstances, Intemperie? intemp)
        {
            if (dateAccident == null ||
                string.IsNullOrWhiteSpace(rue) || 
                string.IsNullOrWhiteSpace(codepostal) || 
                string.IsNullOrEmpty(resumecirconstances) || 
                intemp == null)
            {
                return new JsonResult(2);
            }
            
            int annee = DateTime.Now.Year;
            int noseq = 1;

            string query = "SELECT coalesce(max(noseq),0) FROM accident WHERE annee=@annee;";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString); // connexion

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) noseq = reader.GetInt32(0)+1; // on récupére le prochain numéro séquentiel de l'année en cours
                }
            }

            query = "INSERT INTO accident(annee, noseq, dateaccident, heureaccident, rue, codepostal, ville, resumecirconstances, dateouverturedossier, sommerecue, codeintemp) VALUES (@annee, @noseq, @dateaccident, @heureaccident, @rue, @codepostal, @ville, @resumecirconstances, @dateouverturedossier, @sommerecue, @intemp)";

            using (var cmd = dataSource.CreateCommand(query)) // preparation de la commande
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                cmd.Parameters.AddWithValue("@dateaccident", DateOnly.FromDateTime((DateTime)dateAccident));
                cmd.Parameters.AddWithValue("@heureaccident",TimeOnly.FromDateTime((DateTime)dateAccident));
                cmd.Parameters.AddWithValue("@rue", rue);
                cmd.Parameters.AddWithValue("@codepostal", codepostal);
                cmd.Parameters.AddWithValue("@ville", ville);
                cmd.Parameters.AddWithValue("@resumecirconstances", resumecirconstances);
                cmd.Parameters.AddWithValue("@dateouverturedossier", DateOnly.FromDateTime(DateTime.Now));
                cmd.Parameters.AddWithValue("@sommerecue", 0);
                cmd.Parameters.AddWithValue("@intemp", intemp.ToString().ToLower());

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie un reader qui peut parcourir le resultat

                if(result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }

            }
            Dictionary<string, int> cleAccident = [];
            cleAccident.Add("annee", annee);
            cleAccident.Add("noseq", noseq);
            return new JsonResult(cleAccident);
        }

        [HttpDelete]
        public JsonResult DeleteAccident(int? annee, int? noseq)
        {
            if (annee == null || noseq == null) return new JsonResult(2);

            string query = "DELETE FROM accident WHERE annee=@annee AND noseq=@noseq";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(3);
                }
            }

            return new JsonResult(1);

        }

        [HttpPut]
        public JsonResult PutDateTransmissionAssur(int? annee, int? noseq, DateOnly? date)
        {
            if (annee == null || noseq == null)
            {
                return new JsonResult(2); // champ.s manquant.s
            }

            /* exesist-il ? */
            string query = "SELECT annee FROM accident WHERE annee=@annee AND noseq=@noseq";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                using (var reader = cmd.ExecuteReader())
                {
                    /* Si Non : */
                    if (!reader.Read()) return new JsonResult(3); // l'élément n'existe pas
                }
            }

            /* Si Oui :*/
            query = "UPDATE accident SET datetransmissionassur=@datetransmissionassur";


            using (var cmd = dataSource.CreateCommand(query))
            {
                if (date == null)
                {
                    date = DateOnly.FromDateTime(DateTime.Now);
                }
                cmd.Parameters.AddWithValue("@datetransmissionassur", date);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }
            return new JsonResult(1);
        }

        [HttpPut]
        public JsonResult PutDateValidationAssur(int? annee, int? noseq, DateOnly? date)
        {
            if (annee == null || noseq == null)
            {
                return new JsonResult(2); // champ.s manquant.s
            }

            /* exesist-il ? */
            string query = "SELECT annee FROM accident WHERE annee=@annee AND noseq=@noseq";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                using (var reader = cmd.ExecuteReader())
                {
                    /* Si Non : */
                    if (!reader.Read()) return new JsonResult(3); // l'élément n'existe pas
                }
            }

            /* Si Oui :*/
            query = "UPDATE accident SET datevalidationassur=@datevalidationassur";

            using (var cmd = dataSource.CreateCommand(query))
            {
                if (date == null)
                {
                    date = DateOnly.FromDateTime(DateTime.Now);
                }
                cmd.Parameters.AddWithValue("@datevalidationassur", date);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }
            return new JsonResult(1);
        }

        [HttpPut]
        public JsonResult PutDateClotureDossier(int? annee, int? noseq, DateOnly? date)
        {
            if (annee == null || noseq == null)
            {
                return new JsonResult(2); // champ.s manquant.s
            }

            /* exesist-il ? */
            string query = "SELECT annee FROM accident WHERE annee=@annee AND noseq=@noseq";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                using (var reader = cmd.ExecuteReader())
                {
                    /* Si Non : */
                    if (!reader.Read()) return new JsonResult(3); // l'élément n'existe pas
                }
            }

            /* Si Oui :*/
            query = "UPDATE accident SET datecloturedossier=@datecloturedossier";

            using (var cmd = dataSource.CreateCommand(query))
            {
                if (date == null)
                {
                    date = DateOnly.FromDateTime(DateTime.Now);
                }
                cmd.Parameters.AddWithValue("@datecloturedossier", date);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }
            return new JsonResult(1);
        }

        [HttpPut]
        public JsonResult PutSommeRecue(int? annee, int? noseq, int? somme)
        {
            if (annee == null || noseq == null || somme == null)
            {
                return new JsonResult(2); // champ.s manquant.s
            }

            /* exesist-il ? */
            string query = "SELECT annee FROM accident WHERE annee=@annee AND noseq=@noseq";

            string connectionString = _configuration.GetConnectionString("ctvrtestsDBConn");
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@annee", annee);
                cmd.Parameters.AddWithValue("@noseq", noseq);
                using (var reader = cmd.ExecuteReader())
                {
                    /* Si Non : */
                    if (!reader.Read()) return new JsonResult(3); // l'élément n'existe pas
                }
            }

            /* Si Oui :*/
            query = "UPDATE accident SET sommerecue=@sommerecue";

            using (var cmd = dataSource.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@sommerecue", somme);

                int result = cmd.ExecuteNonQuery(); // exécute la commande et renvoie le nombre de lignes affectées -> 1 si tout se passe bien

                if (result == 0) // si ca n'a pas fonctionné
                {
                    return new JsonResult(5);
                }
            }
            return new JsonResult(1);
        }

        // action pour upadate un accident complet : Admin CTVR

    }
}
