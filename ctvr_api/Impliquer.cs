namespace CTVR_API
{
    public class Impliquer
    {
        public int annee { get; set; }
        public int noseq { get; set; }
        public Tiers tiers { get; set; }
        public string? natureBlessures { get; set; }
        public string? degatsMateriels { get; set; }
        public int? sommeVersee { get; set; }

        public Impliquer(Dictionary<string,object> fields)
        {
            annee = (int)fields["annee"];
            noseq = (int)fields["noseq"];
            tiers = (Tiers)fields["tiers"];
            natureBlessures = (string)fields["natureblessures"];
            degatsMateriels = (string)fields["degatsmateriels"];
            sommeVersee = (int?)fields["sommeversee"];
        }
    }
}
