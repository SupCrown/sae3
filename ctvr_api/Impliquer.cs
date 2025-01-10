namespace CTVR_API
{
    public class Impliquer
    {
        public int annee { get; set; }
        public int noseq { get; set; }
        public List<Tiers> tiers { get; set; }
        public string? natureblessures { get; set; }
        public string? degatsmateriels { get; set; }
        public int? sommeversee { get; set; }

        public Impliquer(Dictionary<string,object> fields)
        {
            annee = (int)fields["annee"];
            noseq = (int)fields["noseq"];
            tiers = (List<Tiers>)fields["tiers"];
            natureblessures = (string)fields["natureblessures"];
            degatsmateriels = (string)fields["degatsmateriels"];
            sommeversee = (int?)fields["sommeversee"];
        }
    }
}
