
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class MOE
    {
        public int? codeM { get; set; }
        public string libelleM { get; set; }
        public float tauxHoraire { get; set; }

        public MOE(Dictionary<string, object> fields)
        {
            codeM = (int)fields["codeM"];
            libelleM = (string)fields["libelleM"];
            tauxHoraire = (int)fields["tauxHoraire"];
        }

    }
}
