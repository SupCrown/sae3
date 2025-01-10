
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class MOE
    {
        public string codeM { get; set; }
        public string libelleM { get; set; }
        public double tauxHoraire { get; set; }

        public MOE(Dictionary<string, object> fields)
        {
            codeM = (string)fields["codeM"];
            libelleM = (string)fields["libelleM"];
            tauxHoraire = (double)fields["tauxHoraire"];
        }

    }
}
