
using System.Runtime.CompilerServices;

namespace CTVR_API
{

    public class Ligne
    {
        public Int16? noligne { get; set; }

        public string? nomarretdepart { get; set; }


        public string? nomarretterminus { get; set; }



        public Intervention(Dictionary<string, object> fields)
        {
            noligne = fields.ContainsKey("noligne") && fields["noligne"] != DBNull.Value
                ? (Int16?)fields["noligne"]
                : string.Empty;

            nomarretdepart = fields.ContainsKey("nomarretdepart") && fields["nomarretdepart"] != DBNull.Value
                ? (string?)fields["nomarretdepart"]
                : string.Empty;

            nomarretterminus = fields.ContainsKey("nomarretterminus") && fields["nomarretterminus"] != DBNull.Value
                ? (string?)fields["nomarretterminus"]
                : string.Empty;

        }
    }
}