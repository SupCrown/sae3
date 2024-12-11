
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Conducteur
    {
        public Int16 nopermis { get; set; }
        public DateTime datepermis { get; set; }
        public string? lieudelivrancepermis { get; set; }
        public DateOnly dateprisefonction { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }

        public DateTime datenaissance { get; set; }

        public string? adresse { get; set; }

        public string? codepostal { get; set; }

        public string? ville { get; set; }


        public Intervention(Dictionary<string, object> fields)
        {
            nopermis = fields.ContainsKey("nopermis") ? (Int16)fields["nopermis"] : null;

            nom = fields.ContainsKey("nom") ? (string)fields["nom"] : null;

            prenom = fields.ContainsKey("prenom") ? (string)fields["prenom"] : null;

            datenaissance = fields.ContainsKey("datenaissance") ? (DateOnly)fields["datenaissance"] : null;

            dateprisefonction = fields.ContainsKey("dateprisefonction") ? (DateOnly)fields["dateprisefonction"] : null;

            datepermis = fields.ContainsKey("datepermis") ? (DateOnly)fields["datepermis"] : null;

            codepostal = fields.ContainsKey("codepostal") && fields["codepostal"] != DBNull.Value
                ? (string?)fields["codepostal"]
                : string.Empty;

            ville = fields.ContainsKey("ville") && fields["ville"] != DBNull.Value
                ? (string?)fields["ville"]
                : string.Empty;

            lieudelivrancepermis = fields.ContainsKey("lieudelivrancepermis") && fields["lieudelivrancepermis"] != DBNull.Value
                ? (string?)fields["lieudelivrancepermis"]
                : string.Empty;

            adresse = fields.ContainsKey("adresse") && fields["adresse"] != DBNull.Value
                ? (string?)fields["adresse"]
                : null;

        }
    }
}