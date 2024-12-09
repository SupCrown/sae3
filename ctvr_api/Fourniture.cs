
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Fourniture
    {
        public int? codeF { get; set; }
        public string libelleF { get; set; }
        public float PrixUnitaire { get; set; }

        public Fourniture(Dictionary<string, object> fields)
        {
            codeF = (int)fields["codeF"];
            libelleF = (string)fields["libelleF"];
            PrixUnitaire = (int)fields["PrixUnitaire"];
        }

    }
}
