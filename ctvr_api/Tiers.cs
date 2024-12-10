using Npgsql;
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Tiers
    {
        private int noTiers { get; set; }
        private string? nomTiers { get; set; }
        private string? prenomTiers { get; set; }
        private DateOnly dateNaissance { get; set; }
        private string? adresseTiers { get; set; }
        private string? cpTiers { get; set; }
        private string? villeTiers { get; set; }

        public Tiers(Dictionary<string, object> fields) {
            noTiers = (int)fields["noTiers"];
            nomTiers = (string)fields["nomTiers"];
            prenomTiers = (string)fields["prenomTiers"];
            dateNaissance = (DateOnly)fields["dateNaissance"];
            adresseTiers = (string)fields["adresseTiers"];
            cpTiers = (string)fields["cpTiers"];
            villeTiers = (string)fields["villeTiers"];
        }
    }
}
