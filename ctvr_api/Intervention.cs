
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Intervention
    {
        public Int16? norep { get; set; }
        public DateOnly? dateentreebus { get; set; }
        public string description { get; set; }
        public string? pieces { get; set; }
        public DateOnly? datefinreparation { get; set; }
        public DateOnly? datedevis { get; set; }

        public DateOnly? datedebutreparation { get; set; }
        public string immat { get; set; }

        public Int16? annee { get; set; }
       
        public Int16? noseq { get; set; }


        public Intervention(Dictionary<string, object> fields)
        {
            norep = fields.ContainsKey("norep") ? (Int16?)fields["norep"] : null;

            annee = fields.ContainsKey("annee") ? (Int16?)fields["annee"] : null;
            
            noseq = fields.ContainsKey("noseq") ? (Int16?)fields["noseq"] : null;


            datedevis = fields.ContainsKey("datedevis") && fields["datedevis"] != DBNull.Value
                ? (DateOnly?)fields["datedevis"]
                : null;
            dateentreebus = fields.ContainsKey("dateentreebus") && fields["dateentreebus"] != DBNull.Value
                ? (DateOnly?)fields["dateentreebus"]
                : null;
            description = fields.ContainsKey("description") && fields["description"] != DBNull.Value
                ? (string)fields["description"]
                : string.Empty;
            pieces = fields.ContainsKey("pieces") && fields["pieces"] != DBNull.Value
                ? (string?)fields["pieces"]
                : null;
            datefinreparation = fields.ContainsKey("datefinreparation") && fields["datefinreparation"] != DBNull.Value
                ? (DateOnly?)fields["datefinreparation"]
                : null;
            datedebutreparation = fields.ContainsKey("datedebutreparation") && fields["datedebutreparation"] != DBNull.Value
                ? (DateOnly?)fields["datedebutreparation"]
                : null;

            immat = fields.ContainsKey("immat") && fields["immat"] != DBNull.Value
                ? (string)fields["immat"]
                : string.Empty;
        }
    }
}