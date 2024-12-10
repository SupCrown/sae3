namespace CTVR_API
{
    public class Accident
    {
        private int annee { get; set; }
        private int noseq { get; set; }
        private DateOnly? dateAccident { get; set; }
        private TimeSpan heureAccident { get; set; }
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

        public Accident(Dictionary<string, object?> fields)
        {
            annee = (int)fields["annee"];
            noseq = (int)fields["noSeq"];
            dateAccident = DateOnly.FromDateTime((DateTime)fields["dateAccident"]);
            heureAccident = (TimeSpan)fields["heureAccident"];
            rue = (string)fields["rue"];
            codePostal = (string)fields["codePostal"];
            ville = (string)fields["ville"];
            resumeCirconstances = (string)fields["resumeCirconstances"];
            dateOuvertureDossier = DateOnly.FromDateTime((DateTime)fields["dateOuvertureDossier"]);
            dateTransmissionAssur = fields["dateTransmissionAssur"] == null ? null : DateOnly.FromDateTime((DateTime)fields["dateTransmissionAssur"]);
            dateValidationAssur = fields["dateValidationAssur"] == null ? null : DateOnly.FromDateTime((DateTime)fields["dateValidationAssur"]);
            dateClotureDossier = fields["dateClotureDossier"] == null ? null : DateOnly.FromDateTime((DateTime)fields["dateClotureDossier"]);
            sommeRecue = (int)fields["sommeRecue"];
            intemperie = (Intemperie)fields["intemperie"];
        }

        public string ToString()
        {
            return annee.ToString() + ' ' + noseq.ToString() + ' ' + dateAccident.ToString() + ' ' + heureAccident.ToString() + ' ' + rue + ' ' + ville + ' ' + resumeCirconstances
                 + ' ' + dateOuvertureDossier.ToString() + ' ' + dateTransmissionAssur.ToString() + ' ' + dateValidationAssur.ToString() + ' ' + dateClotureDossier.ToString()
                 + ' ' + sommeRecue.ToString() + ' ' + intemperie.ToString();
        }
    }
}
