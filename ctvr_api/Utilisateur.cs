
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    public class Utilisateur
    {
        public string identifiant { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string motDePasse { get; set; }
        public string role { get; set; }

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
