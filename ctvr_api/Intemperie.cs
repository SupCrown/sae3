namespace CTVR_API
{
    public enum Intemperie
    {
        Pluie,
        Brouillard,
        Neige,
        Verglas,
        Autres
    }

    public static class ItemperieMethodes
    {
        public static Intemperie parseItemp(string stringIntemp)
        {
            switch (stringIntemp)
            {
                case "pluie": return Intemperie.Pluie;
                case "neige": return Intemperie.Neige;
                case "verglas": return Intemperie.Verglas;
                case "brouillard": return Intemperie.Brouillard;
                default: return Intemperie.Autres;
            }
        }

    }
}
