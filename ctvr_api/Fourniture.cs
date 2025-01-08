
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Fourniture
    {
        public string codeF { get; set; }
        public string libelleF { get; set; }
        public double prixUnitaire { get; set; }

        public Fourniture(Dictionary<string, object> fields)
        {
            codeF = (string)fields["codeF"];
            libelleF = (string)fields["libelleF"];
            prixUnitaire = (double)fields["prixUnitaire"];
        }

    }
}
