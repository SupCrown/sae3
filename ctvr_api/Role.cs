namespace ctvr_api
{
    public class Role
    {
        public string code { get;}
        public string libelle { get; }

        public Role(Dictionary<string, string> field)
        {
            code = field["code"];
            libelle = field["libelle"];
        }
    }
}
