
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Devis
    {

        public string? nom { get; set; }
        public DateOnly? dateCreation { get; set; }
        public List<string> codeF { get; set; }
        public double? prix { get; set; }

        public double? TVA { get; set; }
        public double? prixTTC { get; set; }

        public Int16? norep { get; set; }


        public Devis(Dictionary<string, object> fields)
        {
            norep = fields.ContainsKey("norep") && fields["norep"] != DBNull.Value
                ? (Int16?)fields["norep"]
                : null;

            dateCreation = fields.ContainsKey("dateCreation") && fields["dateCreation"] != DBNull.Value
                ? (DateOnly?)fields["dateCreation"]
                : null;

            nom = fields.ContainsKey("nom") && fields["nom"] != DBNull.Value
                ? (string?)fields["nom"]
                : null;

            prix = fields.ContainsKey("prix") && fields["prix"] != DBNull.Value
                ? (double?)fields["prix"]
                : null;

            TVA = fields.ContainsKey("TVA") && fields["TVA"] != DBNull.Value
                ? (double?)fields["TVA"]
                : null;

            prixTTC = fields.ContainsKey("prixTTC") && fields["prixTTC"] != DBNull.Value
                ? (double?)fields["prixTTC"]
                : null;

            codeF = new List<string>();

        }
    }
}