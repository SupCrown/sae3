
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Intervention
    {
        public Int16? norep { get; set; }
        public DateTime? dateentreebus { get; set; }
        public string description { get; set; }
        public string? pieces { get; set; }
        public DateTime? datefinreparation { get; set; }
        public DateTime? datedevis { get; set; }

        public DateTime? datedebutreparation { get; set; }
        public string immat { get; set; }


        public Intervention(Dictionary<string, object> fields)
        {
            norep = fields.ContainsKey("norep") ? (Int16?)fields["norep"] : null;


            datedevis = fields.ContainsKey("datedevis") && fields["datedevis"] != DBNull.Value
                ? (DateTime?)fields["datedevis"]
                : null;
            dateentreebus = fields.ContainsKey("dateentreebus") && fields["dateentreebus"] != DBNull.Value
                ? (DateTime?)fields["dateentreebus"]
                : null;
            description = fields.ContainsKey("description") && fields["description"] != DBNull.Value
                ? (string)fields["description"]
                : string.Empty;
            pieces = fields.ContainsKey("pieces") && fields["pieces"] != DBNull.Value
                ? (string?)fields["pieces"]
                : null;
            datefinreparation = fields.ContainsKey("datefinreparation") && fields["datefinreparation"] != DBNull.Value
                ? (DateTime?)fields["datefinreparation"]
                : null;
            datedebutreparation = fields.ContainsKey("datedebutreparation") && fields["datedebutreparation"] != DBNull.Value
                ? (DateTime?)fields["datedebutreparation"]
                : null;

            immat = fields.ContainsKey("immat") && fields["immat"] != DBNull.Value
                ? (string)fields["immat"]
                : string.Empty;
        }
    }
}