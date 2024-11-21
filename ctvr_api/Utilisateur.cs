
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Utilisateur
    {
        public string identifiant { get; }
        public string nom { get; }
        public string prenom { get; }
        public string motDePasse { get; }
        public string role { get; }

        public Utilisateur(Dictionary<string, string> fields)
        {
            identifiant = fields["identifiant"];
            nom = fields["nom"];
            prenom = fields["prenom"];
            motDePasse = fields["motDePasse"];
            role = fields["role"];
        }
    }
}
