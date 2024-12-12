
using System.Runtime.CompilerServices;

namespace CTVR_API
{

    public class Parcours
    {
        public string nopermis { get; set; }
        public TimeSpan? horaireservice { get; set; }
        public string immat { get; set; }
        public string nomchauffeur { get; set; }
        public string prenomchauffeur { get; set; }
        public Int16? noligne { get; set; }


        public Parcours(Dictionary<string, object> fields)
        {
            nopermis = fields.ContainsKey("nopermis") ? (string)fields["nopermis"] : null;

            horaireservice = fields.ContainsKey("horaireservice") ? (TimeSpan?)fields["horaireservice"] : null;

            immat = fields.ContainsKey("immat") ? (string)fields["immat"] : null;

            nomchauffeur = fields.ContainsKey("nom") ? (string)fields["nom"] : null;

            prenomchauffeur = fields.ContainsKey("prenom") ? (string)fields["prenom"] : null;

            noligne = fields.ContainsKey("noligne") ? (Int16?)fields["noligne"] : null;


        }
    }
}