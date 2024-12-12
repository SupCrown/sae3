using Npgsql;
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Tiers
    {
        public int noTiers { get; set; }
        public string? nomTiers { get; set; }
        public string? prenomTiers { get; set; }
        public DateOnly dateNaissance { get; set; }
        public string? adresseTiers { get; set; }
        public string? cpTiers { get; set; }
        public string? villeTiers { get; set; }

        public Tiers(Dictionary<string, object> fields) {
            noTiers = (int)fields["notiers"];
            nomTiers = (string)fields["nomtiers"];
            prenomTiers = (string)fields["prenomtiers"];
            dateNaissance = (DateOnly)fields["datenaissance"];
            adresseTiers = (string)fields["adressetiers"];
            cpTiers = (string)fields["cptiers"];
            villeTiers = (string)fields["villetiers"];
        }
    }
}
