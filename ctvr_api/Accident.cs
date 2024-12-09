namespace ctvr_api
{
    public class Accident
    {
        private int annee { get; set; }
        private int noseq { get; set; }
        private DateOnly dateAccident { get; set; }
        private TimeOnly heureAccident { get; set; }
        private string? rue { get; set; }
        private string? codePostal { get; set; }
        private string? ville { get; set; }
        private string? resumeCirconstances { get; set; }
        private DateOnly dateOuvertureDossier { get; set; }
        private DateOnly? dateTransmissionAssur { get; set; }
        private DateOnly? dateValidationAssur { get; set; }
        private DateOnly? dateClotureDossier { get; set; }
        private int? sommeRecue { get; set; }
        private Intemperie intemperie { get; set; }

        public Accident(Dictionary<string, object> fields)
        {
            annee = (int)fields["annee"];
            noseq = (int)fields["noSeq"];
            dateAccident = (DateOnly)fields["dateAccident"];
            heureAccident = (TimeOnly)fields["heureAccident"];
            rue = (string)fields["rue"];
            codePostal = (string)fields["codePostal"];
            ville = (string)fields["ville"];
            resumeCirconstances = (string)fields["resumeCirconstances"];
            dateOuvertureDossier = (DateOnly)fields["dateOuvertureDossier"];
            dateTransmissionAssur = (DateOnly)fields["dateTransmissionAssur"];
            dateValidationAssur = (DateOnly)fields["dateValidationAssur"];
            dateClotureDossier = (DateOnly)fields["dateClotureDossier"];
            sommeRecue = (int)fields["sommeRecue"];
            intemperie = (Intemperie)fields["intemperie"];
        }
    }
}
