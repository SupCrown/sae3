
using System.Runtime.CompilerServices;

namespace CTVR_API
{
    /*NoPermis VARCHAR(40),
   HoraireService TIMESTAMP,
   Immat CHAR(7) NOT NULL,
   NoLigne SMALLINT NOT NULL,
   PRIMARY KEY(NoPermis, HoraireService),
   FOREIGN KEY(NoPermis) REFERENCES CONDUCTEUR(NoPermis),
   FOREIGN KEY(Immat) REFERENCES BUS(Immat),
   FOREIGN KEY(NoLigne) REFERENCES LIGNE(NoLigne)*/

    public class Parcours
    {
        public string nopermis { get; set; }
        public TimeSpan horaireservice { get; set; }
        public string immat { get; set; }
        public string nomchauffeur { get; set; }
        public string prenomchauffeur { get; set; }
        public Int16 noligne { get; set; }


        public Parcours(Dictionary<string, object> fields)
        {
            nopermis = fields.ContainsKey("nopermis") ? (string)fields["nopermis"] : null;

            horaireservice = fields.ContainsKey("horaireservice") ? (TimeSpan)fields["horaireservice"] : null;

            immat = fields.ContainsKey("immat") ? (string)fields["immat"] : null;

            nomchauffeur = fields.ContainsKey("nomchauffeur") ? (string)fields["nomchauffeur"] : null;

            prenomchauffeur = fields.ContainsKey("prenomchauffeur") ? (string)fields["prenomchauffeur"] : null;

            noligne = fields.ContainsKey("noligne") ? (Int16)fields["noligne"] : null;


        }
    }
}