
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Bus
    {

        public string immat { get; set; }
        public DateOnly? miseenservice { get; set; }



        public Intervention(Dictionary<string, object> fields)
        {
            immat = fields.ContainsKey("immat") ? (string)fields["immat"] : null;

            miseenservice = fields.ContainsKey("miseenservice") && fields["miseenservice"] != DBNull.Value
                ? (DateOnly?)fields["miseenservice"]
                : string.Empty;

        }
    }
}