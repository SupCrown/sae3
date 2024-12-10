namespace CTVR_API
{
    public class Impliquer
    {
        private Accident accident { get; set; }
        private Tiers tiers { get; set; }
        private string? natureBlessures { get; set; }
        private string? degatsMateriels { get; set; }
        private int sommeVersee { get; set; }

        public Impliquer(Dictionary<string,object> fields)
        {
            accident = (Accident)fields["accident"];
            tiers = (Tiers)fields["tiers"];
            natureBlessures = (string)fields["natureBlessures"];
            degatsMateriels = (string)fields["degatsMateriels"];
            sommeVersee = (int)fields["sommeVersee"];
        }
    }
}
