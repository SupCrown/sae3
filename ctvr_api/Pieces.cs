namespace CTVR_API
{
    public enum PieceAutomobile
    {
        // Moteur et système d'alimentation
        BougieAllumage,
        Injecteur,
        PompeCarburant,
        FiltreCarburant,
        CourroieDistribution,
        Culasse,
        Turbo,
        Radiateur,
        VentilateurMoteur,
        JointCulasse,

        // Système électrique et électronique
        Batterie,
        Alternateur,
        Relais,
        Fusible,
        FaisceauElectrique,
        Ampoule,
        CapteurTemperature,
        CapteurOxygene,
        ECU,
        RegulateurTension,

        // Freinage
        PlaquetteFrein,
        DisqueFrein,
        EtrierFrein,
        FlexibleFrein,
        MaitreCylindre,
        CapteurABS,
        Servofrein,

        // Transmission
        Embrayage,
        BoiteVitesse,
        ArbreTransmission,
        Cardan,
        PontArriere,
        Differentiel,

        // Suspension et direction
        Amortisseur,
        Ressort,
        RotuleSuspension,
        BarreStabilisatrice,
        BielletteDirection,
        CremaillereDirection,
        SilentBloc,

        // Système d'échappement
        CollecteurEchappement,
        Catalyseur,
        Silencieux,
        FAP,
        SondeLambda,

        // Carrosserie et accessoires extérieurs
        PareChoc,
        Capot,
        Aile,
        Portiere,
        Retroviseur,
        Phare,
        FeuArriere,
        GrilleCalandre,
        Antibrouillard,
        EssuieGlace,

        // Intérieur et accessoires
        Volant,
        Pedale,
        Siege,
        TableauBord,
        LeveVitresElectrique,
        CommandeClimatisation,
        Autoradio,
        Moquette,
        CeintureSecurite,
        Airbag,

        // Système de refroidissement
        Thermostat,
        Durite,
        PompeEau,
        VaseExpansion,
        VentilateurRefroidissement,

        // Pneumatiques et roues
        Pneu,
        Jante,
        EcrouRoue,
        RoulementRoue,
        CapteurPressionPneus,

        // Systèmes auxiliaires
        CompresseurClimatisation,
        CourroieAccessoire,
        Poulie,
        ReservoirLaveGlace,
        PompeDirectionAssistee
    }

public static class PieceAutomobileMethodes
{
    public static PieceAutomobile ParsePiece(string pieceNom)
    {
        switch (pieceNom.ToLower())
        {
            // Moteur et système d'alimentation
            case "bougieallumage": return PieceAutomobile.BougieAllumage;
            case "injecteur": return PieceAutomobile.Injecteur;
            case "pompecarburant": return PieceAutomobile.PompeCarburant;
            case "filtrecarburant": return PieceAutomobile.FiltreCarburant;
            case "courroiedistribution": return PieceAutomobile.CourroieDistribution;
            case "culasse": return PieceAutomobile.Culasse;
            case "turbo": return PieceAutomobile.Turbo;
            case "radiateur": return PieceAutomobile.Radiateur;
            case "ventilateurmoteur": return PieceAutomobile.VentilateurMoteur;
            case "jointculasse": return PieceAutomobile.JointCulasse;

            // Système électrique et électronique
            case "batterie": return PieceAutomobile.Batterie;
            case "alternateur": return PieceAutomobile.Alternateur;
            case "relais": return PieceAutomobile.Relais;
            case "fusible": return PieceAutomobile.Fusible;
            case "faisceaulelectrique": return PieceAutomobile.FaisceauElectrique;
            case "ampoule": return PieceAutomobile.Ampoule;
            case "capteurtemperature": return PieceAutomobile.CapteurTemperature;
            case "capteuroxygene": return PieceAutomobile.CapteurOxygene;
            case "ecu": return PieceAutomobile.ECU;
            case "regulateurtension": return PieceAutomobile.RegulateurTension;

            // Freinage
            case "plaquettefrein": return PieceAutomobile.PlaquetteFrein;
            case "disquefrein": return PieceAutomobile.DisqueFrein;
            case "etrierfrein": return PieceAutomobile.EtrierFrein;
            case "flexiblefrein": return PieceAutomobile.FlexibleFrein;
            case "maitrecylindre": return PieceAutomobile.MaitreCylindre;
            case "capteurabs": return PieceAutomobile.CapteurABS;
            case "servofrein": return PieceAutomobile.Servofrein;

            // Transmission
            case "embrayage": return PieceAutomobile.Embrayage;
            case "boitevitesse": return PieceAutomobile.BoiteVitesse;
            case "arbetransmission": return PieceAutomobile.ArbreTransmission;
            case "cardan": return PieceAutomobile.Cardan;
            case "pontarriere": return PieceAutomobile.PontArriere;
            case "differentiel": return PieceAutomobile.Differentiel;

            // Suspension et direction
            case "amortisseur": return PieceAutomobile.Amortisseur;
            case "ressort": return PieceAutomobile.Ressort;
            case "rotulesuspension": return PieceAutomobile.RotuleSuspension;
            case "barrestabilisatrice": return PieceAutomobile.BarreStabilisatrice;
            case "biellettedirection": return PieceAutomobile.BielletteDirection;
            case "cremailleredirection": return PieceAutomobile.CremaillereDirection;
            case "silentbloc": return PieceAutomobile.SilentBloc;

            // Système d'échappement
            case "collectorechappement": return PieceAutomobile.CollecteurEchappement;
            case "catalyseur": return PieceAutomobile.Catalyseur;
            case "silencieux": return PieceAutomobile.Silencieux;
            case "fap": return PieceAutomobile.FAP;
            case "sondelambda": return PieceAutomobile.SondeLambda;

            // Carrosserie et accessoires extérieurs
            case "parechoc": return PieceAutomobile.PareChoc;
            case "capot": return PieceAutomobile.Capot;
            case "aile": return PieceAutomobile.Aile;
            case "portiere": return PieceAutomobile.Portiere;
            case "retroviseur": return PieceAutomobile.Retroviseur;
            case "phare": return PieceAutomobile.Phare;
            case "feuarriere": return PieceAutomobile.FeuArriere;
            case "grillecalandre": return PieceAutomobile.GrilleCalandre;
            case "antibrouillard": return PieceAutomobile.Antibrouillard;
            case "essuieglace": return PieceAutomobile.EssuieGlace;

            // Intérieur et accessoires
            case "volant": return PieceAutomobile.Volant;
            case "pedale": return PieceAutomobile.Pedale;
            case "siege": return PieceAutomobile.Siege;
            case "tableaubord": return PieceAutomobile.TableauBord;
            case "levevitreselectrique": return PieceAutomobile.LeveVitresElectrique;
            case "commandeclimatisation": return PieceAutomobile.CommandeClimatisation;
            case "autoradio": return PieceAutomobile.Autoradio;
            case "moquette": return PieceAutomobile.Moquette;
            case "ceinturesecurite": return PieceAutomobile.CeintureSecurite;
            case "airbag": return PieceAutomobile.Airbag;

            // Système de refroidissement
            case "thermostat": return PieceAutomobile.Thermostat;
            case "durite": return PieceAutomobile.Durite;
            case "pompeau": return PieceAutomobile.PompeEau;
            case "vaseexpansion": return PieceAutomobile.VaseExpansion;
            case "ventilateurrefroidissement": return PieceAutomobile.VentilateurRefroidissement;

            // Pneumatiques et roues
            case "pneu": return PieceAutomobile.Pneu;
            case "jante": return PieceAutomobile.Jante;
            case "ecnouroue": return PieceAutomobile.EcrouRoue;
            case "roulementroue": return PieceAutomobile.RoulementRoue;
            case "capteurpressionpneus": return PieceAutomobile.CapteurPressionPneus;

            // Systèmes auxiliaires
            case "compresseurclimatisation": return PieceAutomobile.CompresseurClimatisation;
            case "courroieaccessoire": return PieceAutomobile.CourroieAccessoire;
            case "poulie": return PieceAutomobile.Poulie;
            case "reservoirlaveglace": return PieceAutomobile.ReservoirLaveGlace;
            case "pompedirectionassistee": return PieceAutomobile.PompeDirectionAssistee;

            default: throw new ArgumentException($"La pièce '{pieceNom}' est inconnue.");
        }
    }
}
}
