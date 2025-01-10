using Npgsql;
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Tiers
    {
        public int notiers { get; set; }
        public string? nomtiers { get; set; }
        public string? prenomtiers { get; set; }
        public DateOnly datenaissance { get; set; }
        public string? adressetiers { get; set; }
        public string? cptiers { get; set; }
        public string? villetiers { get; set; }

        public Tiers(Dictionary<string, object> fields) {
            notiers = (int)fields["notiers"];
            nomtiers = (string)fields["nomtiers"];
            prenomtiers = (string)fields["prenomtiers"];
            datenaissance = (DateOnly)fields["datenaissance"];
            adressetiers = (string)fields["adressetiers"];
            cptiers = (string)fields["cptiers"];
            villetiers = (string)fields["villetiers"];
        }
    }
}
