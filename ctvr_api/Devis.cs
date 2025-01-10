
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Devis
    {

        public DateOnly? dateCreation { get; set; }
        public List<Dictionary<string, object>> fourniture { get; set; }
        public List<Dictionary<string, object>> mainOeuvre { get; set; }

        public double? totalMoe { get; set; }

        public double? totalFourn { get; set; }


        public double? prix { get; set; }

        public double? TVA { get; set; }
        public double? prixTTC { get; set; }

        public Int16? norep { get; set; }


        public Devis(Dictionary<string, object> fields)
        {
            norep = fields.ContainsKey("norep") && fields["norep"] != DBNull.Value
                ? (Int16?)fields["norep"]
                : null;

            totalMoe = fields.ContainsKey("totalMoe") && fields["totalMoe"] != DBNull.Value
                ? (double?)fields["totalMoe"]
                : null;

            totalFourn = fields.ContainsKey("totalFourn") && fields["totalFourn"] != DBNull.Value
                ? (double?)fields["totalFourn"]
                : null;

            dateCreation = fields.ContainsKey("dateCreation") && fields["dateCreation"] != DBNull.Value
                ? (DateOnly?)fields["dateCreation"]
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

            fourniture = new List<Dictionary<string, object>>();
            mainOeuvre = new List<Dictionary<string, object>>();


        }
    }
}