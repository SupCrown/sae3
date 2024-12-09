
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Utiliser
    {
        public int? id { get; set; }
        public DateTime? DateEntreeBus { get; set; }
        public string Nom { get; set; }
        public string? Piece { get; set; }

        public Intervention(Dictionary<string, object> fields)
        {
            DateEntreeBus = (DateTime)fields["DateEntreeBus"];
            Nom = (string)fields["Nom"];
            Piece = (string)fields["Piece"];

        }

    }
}
