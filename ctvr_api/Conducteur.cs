
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Conducteur
    {
        public Int16? nopermis { get; set; }
        public DateTime? datepermis { get; set; }
        public string lieudelivrancepermis { get; set; }
        public DateTime? dateprisefonction { get; set; }
        public string? nom { get; set; }
        public string prenom { get; set; }

        public DateTime? datenaissance { get; set; }

        public string? adresse { get; set; }

        public string? codepostal { get; set; }

        public string? ville { get; set; }


        public Intervention(Dictionary<string, object> fields)
        {
            nopermis = fields.ContainsKey("nopermis") ? (Int16?)fields["nopermis"] : null;

            nopermis = fields.ContainsKey("codepostal") ? (Int16?)fields["codepostal"] : null;

            nopermis = fields.ContainsKey("ville") ? (string?)fields["ville"] : null;


            datenaissance = fields.ContainsKey("datenaissance") && fields["datenaissance"] != DBNull.Value
                ? (DateTime?)fields["datenaissance"]
                : null;
            datepermis = fields.ContainsKey("datepermis") && fields["datepermis"] != DBNull.Value
                ? (DateTime?)fields["datepermis"]
                : null;
            lieudelivrancepermis = fields.ContainsKey("lieudelivrancepermis") && fields["lieudelivrancepermis"] != DBNull.Value
                ? (string)fields["lieudelivrancepermis"]
                : string.Empty;
            nom = fields.ContainsKey("nom") && fields["nom"] != DBNull.Value
                ? (string?)fields["nom"]
                : null;
            dateprisefonction = fields.ContainsKey("dateprisefonction") && fields["dateprisefonction"] != DBNull.Value
                ? (DateTime?)fields["dateprisefonction"]
                : null;
            adresse = fields.ContainsKey("adresse") && fields["adresse"] != DBNull.Value
                ? (DateTime?)fields["adresse"]
                : null;

            prenom = fields.ContainsKey("prenom") && fields["prenom"] != DBNull.Value
                ? (string)fields["prenom"]
                : string.Empty;
        }
    }
}