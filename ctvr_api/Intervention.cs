
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Intervention
    {
        public DateTime? DateEntreeBus { get; set; }
        public string Nom { get; set; }
        public string? Piece { get; set; }
        public DateTime? DateFinReparation { get; set; }
        public DateTime? DateDebutReparation { get; set; }

        public Intervention(Dictionary<string, object> fields)
        {
            DateEntreeBus = (DateTime)fields["DateEntreeBus"];
            Nom = (string)fields["Nom"];
            Piece = (string)fields["Piece"];
            DateFinReparation = (DateTime)fields["DateFinReparation"];
            DateDebutReparation = (DateTime)fields["DateDebutReparation"];
        }

    }
}
