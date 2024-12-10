namespace CTVR_API
{
    public class Accident
    {
        public int annee { get; set; }
        public int noseq { get; set; }
        public DateOnly? dateAccident { get; set; }
        public TimeSpan heureAccident { get; set; }
        public string? rue { get; set; }
        public string? codePostal { get; set; }
        public string? ville { get; set; }
        public string? resumeCirconstances { get; set; }
        public DateOnly dateOuvertureDossier { get; set; }
        public DateOnly? dateTransmissionAssur { get; set; }
        public DateOnly? dateValidationAssur { get; set; }
        public DateOnly? dateClotureDossier { get; set; }
        public int? sommeRecue { get; set; }
        public Intemperie intemperie { get; set; }

        public Accident(Dictionary<string, object?> fields)
        {
            annee = (int)fields["annee"];
            noseq = (int)fields["noseq"];
            dateAccident = (DateOnly)fields["dateaccident"];
            heureAccident = (TimeSpan)fields["heureaccident"];
            rue = (string)fields["rue"];
            codePostal = (string)fields["codepostal"];
            ville = (string)fields["ville"];
            resumeCirconstances = (string)fields["resumecirconstances"];
            dateOuvertureDossier = (DateOnly)fields["dateouverturedossier"];
            dateTransmissionAssur = fields["datetransmissionassur"] == null ? null : (DateOnly)fields["datetransmissionassur"];
            dateValidationAssur = fields["datevalidationassur"] == null ? null : (DateOnly)fields["datevalidationassur"];
            dateClotureDossier = fields["datecloturedossier"] == null ? null : (DateOnly)fields["datecloturedossier"];
            sommeRecue = (int)fields["sommerecue"];
            intemperie = (Intemperie)fields["intemperie"];
        }

    }
}
