

namespace CTVR_API
{
    public class Conducteur
    {
        public string nopermis { get; set; }
        public DateOnly datepermis { get; set; }
        public DateOnly dateprisefonction { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }

        public string lieudelivrance { get; set; }
        public DateOnly datenaissance { get; set; }
        public string adresse { get; set; }
        public string codepostal { get; set; }

        public string ville { get; set; }


        public Conducteur(Dictionary<string, object> fields)
        {
            nopermis = (string)fields["nopermis"];

            datepermis = (DateOnly)fields["datepermis"];

            dateprisefonction = (DateOnly)fields["dateprisefonction"]; 
                
            nom = (string)fields["nom"];

            prenom = (string)fields["prenom"];

            lieudelivrance = (string)fields["lieudelivrance"];
            
            datenaissance = (DateOnly)fields["datenaissance"]; 
            
            adresse = (string)fields["adresse"];

            codepostal = (string)fields["codepostal"];

            ville = (string)fields["ville"];
        }

    }
}
