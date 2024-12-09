
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Intervention
    {
        public int? id { get; set; }
        public DateTime? DateEntreeBus { get; set; }
        public string Description { get; set; }
        public string? Piece { get; set; }
        public DateTime? DateFinReparation { get; set; }
        public DateTime? DateDebutReparation { get; set; }

        public Intervention(Dictionary<string, object> fields)
        {
            DateEntreeBus = (DateTime)fields["DateEntreeBus"];
            Description = (string)fields["Description"];
            Piece = (string)fields["Piece"];
            DateFinReparation = (DateTime)fields["DateFinReparation"];
            DateDebutReparation = (DateTime)fields["DateDebutReparation"];
        }

    }
}
