CREATE TABLE BUS(
   Immat varCHAR(7),
   MiseEnService DATE NOT NULL,
   PRIMARY KEY(Immat)
);

CREATE TABLE CONDUCTEUR(
   NoPermis VARCHAR(40),
   DatePermis DATE NOT NULL,
   LieuDelivrancePermis VARCHAR(50),
   DatePriseFonction DATE NOT NULL,
   Nom VARCHAR(50) NOT NULL,
   Prenom VARCHAR(50) NOT NULL,
   DateNaissance DATE NOT NULL,
   Adresse VARCHAR(50),
   CodePostal CHAR(5),
   Ville VARCHAR(50),
   PRIMARY KEY(NoPermis)
);

CREATE TABLE LIGNE(
   NoLigne SMALLINT,
   NomArretDepart VARCHAR(50),
   NomArretTerminus VARCHAR(50),
   PRIMARY KEY(NoLigne)
);

CREATE TABLE TIERS(
   NoTiers INTEGER,
   NomTiers VARCHAR(50) NOT NULL,
   PrenomTiers VARCHAR(50) NOT NULL,
   DateNaissance DATE NOT NULL,
   AdresseTiers VARCHAR(50),
   CPTiers CHAR(5),
   VilleTiers VARCHAR(50),
   PRIMARY KEY(NoTiers)
);

CREATE TABLE FOURNITURE(
   CodeF varCHAR(10),
   LibelleF VARCHAR(50) NOT NULL,
   PrixUnitaireCourant NUMERIC(6,2) NOT NULL,
   PRIMARY KEY(CodeF)
);

CREATE TABLE MOE(
   CodeM varCHAR(10),
   LibelleM VARCHAR(50) NOT NULL,
   TauxHoraireCourant NUMERIC(5,2) NOT NULL,
   PRIMARY KEY(CodeM)
);

CREATE TABLE INTEMPERIE(
   CodeIntemp varCHAR(10),
   PRIMARY KEY(CodeIntemp),
   CONSTRAINT verif_CodeIntemp CHECK (CodeIntemp IN ('pluie','brouillard','neige','verglas','autres'))
);

CREATE TABLE ACCIDENT(
   Annee INTEGER,
   NoSeq SERIAL,
   DateAccident DATE,
   HeureAccident TIME,
   Rue VARCHAR(50) NOT NULL,
   CodePostal CHAR(5),
   Ville VARCHAR(50),
   ResumeCirconstances TEXT,
   DateOuvertureDossier DATE,
   DateTransmissionAssur DATE,
   DateValidationAssur DATE,
   DateClotureDossier DATE,
   SommeRecue NUMERIC(15,2) NOT NULL,
   CodeIntemp CHAR(10),
   PRIMARY KEY(Annee, NoSeq),
   FOREIGN KEY(CodeIntemp) REFERENCES INTEMPERIE(CodeIntemp) on update cascade  ,
   CONSTRAINT verif_annee_accident CHECK (Annee <= EXTRACT(YEAR FROM CURRENT_DATE))
);

CREATE TABLE PARCOURS(
   NoPermis VARCHAR(40),
   HoraireService TIMESTAMP,
   Immat CHAR(7) NOT NULL,
   NoLigne SMALLINT NOT NULL,
   PRIMARY KEY(NoPermis, HoraireService),
   FOREIGN KEY(NoPermis) REFERENCES CONDUCTEUR(NoPermis) on update cascade  ,
   FOREIGN KEY(Immat) REFERENCES BUS(Immat) on update cascade  ,
   FOREIGN KEY(NoLigne) REFERENCES LIGNE(NoLigne) on update cascade  
);

CREATE TABLE INTERVENTION(
   NoRep SERIAL,
   DateEntreeBus DATE,
   Description TEXT,
   DateDevis DATE ,
   DateDebutReparation DATE,
   DateFinReparation DATE,
   Immat CHAR(7) NOT NULL,
   Annee INTEGER,
   NoSeq INTEGER,
   PRIMARY KEY(NoRep),
   FOREIGN KEY(Immat) REFERENCES BUS(Immat) on update cascade  ,
   FOREIGN KEY(Annee, NoSeq) REFERENCES ACCIDENT(Annee, NoSeq) on update cascade  
);

CREATE TABLE Survenir(
   Annee INTEGER,
   NoSeq INTEGER,
   NoPermis VARCHAR(40),
   HoraireService TIMESTAMP,
   DommagesBus TEXT,
   DommagesConducteur TEXT,
   SignatureConducteur BOOLEAN,
   SignatureControleur BOOLEAN,
   PRIMARY KEY(Annee, NoSeq, NoPermis, HoraireService),
   FOREIGN KEY(Annee, NoSeq) REFERENCES ACCIDENT(Annee, NoSeq) on update cascade  ,
   FOREIGN KEY(NoPermis, HoraireService) REFERENCES PARCOURS(NoPermis, HoraireService) on update cascade  
);

CREATE TABLE Impliquer(
   Annee INTEGER,
   NoSeq INTEGER,
   NoTiers INTEGER,
   NatureBlessures TEXT,
   DegatsMateriels TEXT,
   SommeVersee NUMERIC(15,2),
   PRIMARY KEY(Annee, NoSeq, NoTiers),
   FOREIGN KEY(Annee, NoSeq) REFERENCES ACCIDENT(Annee, NoSeq) on update cascade  ,
   FOREIGN KEY(NoTiers) REFERENCES TIERS(NoTiers) on update cascade  
);

CREATE TABLE Utiliser(
   NoRep INTEGER,
   CodeF CHAR(10),
   QteDevis NUMERIC(5,2) NOT NULL,
   QteValidee NUMERIC(5,2),
   PrixUnitaire NUMERIC(6,2) NOT NULL,
   PRIMARY KEY(NoRep, CodeF),
   FOREIGN KEY(NoRep) REFERENCES INTERVENTION(NoRep) on update cascade  ,
   FOREIGN KEY(CodeF) REFERENCES FOURNITURE(CodeF) on update cascade  
);

CREATE TABLE Necessiter(
   NoRep INTEGER,
   CodeM varCHAR(10),
   NbHDevis NUMERIC(5,2) NOT NULL,
   NbHValidees NUMERIC(5,2),
   TauxHoraire NUMERIC(5,2) NOT NULL,
   PRIMARY KEY(NoRep, CodeM),
   FOREIGN KEY(NoRep) REFERENCES INTERVENTION(NoRep),
   FOREIGN KEY(CodeM) REFERENCES MOE(CodeM)
);

create table Roles (
	IdRole varchar(5) primary key,
	libelle varchar(100) not null
	constraint verif_role check(
		libelle in ('Service Administratifs', 'Chauffeur', 'Technicien', 'Administrateur CTVR', 'Administrateur Utilisateurs')
		and
		IdRole in ('SAD', 'CHF', 'TCN', 'ADC', 'ADU'))
);

create table Utilisateurs (
	Identifiant varchar(100) primary key,
	nopermis varchar(40),
	nom varchar(100) not null,
	prenom varchar(100) not null,
	MotDePasse varchar(100) not null,
	role varchar(100) not null,
	foreign key (role) references Roles(IdRole),
	foreign key (nopermis) references Conducteur(nopermis),
	constraint verif_role check(role in ('SAD', 'CHF', 'TCN', 'ADC', 'ADU'))
);

-- TABLE BUS
INSERT INTO BUS VALUES
('RH001A', '2010-01-15'),
('RH002B', '2011-03-20'),
('RH003C', '2015-07-25'),
('RH004D', '2018-12-01'),
('RH005E', '2020-06-15'),
('RH006F', '2021-05-10'),
('RH007G', '2022-08-05'),
('RH008H', '2023-01-25');

-- TABLE CONDUCTEUR
INSERT INTO CONDUCTEUR VALUES
('RH001', '2005-06-12', 'Lyon', '2006-01-01', 'Dupont', 'Jean', '1980-05-10', '1 Rue des Cèdres', '69003', 'Lyon'),
('RH002', '2010-11-05', 'Avignon', '2011-02-20', 'Martin', 'Louise', '1985-09-15', '20 Rue des Lilas', '84000', 'Avignon'),
('RH003', '2015-08-18', 'Valence', '2016-03-01', 'Durand', 'Claude', '1990-11-30', '15 Avenue des Platanes', '26000', 'Valence'),
('RH004', '2017-04-10', 'Vienne', '2017-07-10', 'Petit', 'Marie', '1992-02-20', '5 Impasse des Vignes', '38200', 'Vienne'),
('RH005', '2018-09-25', 'Montélimar', '2019-01-15', 'Bernard', 'Paul', '1988-04-18', '50 Chemin des Roches', '26200', 'Montélimar'),
('RH006', '2020-01-15', 'Lyon', '2020-05-10', 'Morel', 'Elise', '1995-03-05', '10 Allée des Roses', '69004', 'Lyon'),
('14BB11113', '2018-11-11', 'Lyon', '2020-11-11', 'Guichard', 'Geoffroy', '2000-04-04', '10 Allée des Tulipes', '69000', 'Lyon'),
('15CC22224', '2000-12-12', 'Valence', '2021-12-12', 'Richard', 'Madeleine', '1988-05-05', '10 Allée des Paquerette', '26000', 'Valence');

-- TABLE LIGNE
INSERT INTO LIGNE VALUES
(10, 'Lyon Gare Part-Dieu', 'Avignon Centre'),
(20, 'Valence Gare', 'Vienne Gare'),
(30, 'Avignon Centre', 'Montélimar Gare'),
(40, 'Lyon Perrache', 'Valence Sud'),
(50, 'Vienne Centre', 'Montélimar Nord'),
(60, 'Montélimar Gare', 'Lyon Bellecour'),
(70, 'Marseille Gare', 'Valence Gare');

-- TABLE TIERS
INSERT INTO TIERS VALUES
(1,'Rossi', 'François', '1980-05-12', '10 Rue des Pins', '84000', 'Avignon'),
(2,'Garcia', 'Sophie', '1995-08-30', '25 Avenue du Rhône', '69004', 'Lyon'),
(3,'Nguyen', 'Thierry', '1975-01-20', '12 Rue des Érables', '26000', 'Valence'),
(4,'Leclerc', 'Anne', '1988-09-14', '5 Boulevard des Acacias', '38200', 'Vienne'),
(5,'Dupuis', 'Michel', '1983-12-01', '50 Allée des Marronniers', '26200', 'Montélimar'),
(6,'Roccossi', 'Freddi', '1991-05-07', '7 Chemin de Travers', '84000', 'Avignon');

-- TABLE FOURNITURE
INSERT INTO FOURNITURE VALUES
('F100', 'Rétroviseur', 60.00),
('F101', 'Pare-brise', 250.00),
('F102', 'Pneu', 130.00),
('F103', 'Ampoule', 15.00),
('F104', 'Batterie', 300.00);

-- TABLE MOE
INSERT INTO MOE VALUES
('M100', 'Mécanicien', 45.00),
('M101', 'Carrossier', 50.00),
('M102', 'Électricien', 40.00);

-- TABLE INTEMPERIE
INSERT INTO INTEMPERIE VALUES
('pluie'), ('neige'), ('verglas'), ('brouillard'), ('autres');

-- TABLE ACCIDENT
INSERT INTO ACCIDENT VALUES
(2023, 1, '2023-03-12', '08:00:00', 'Boulevard des Alpes', '69003', 'Lyon', 'Collision avec un camion.', '2023-03-13', '2023-03-15', '2023-03-20', NULL, 5000.00, 'pluie'),
(2023, 2, '2023-07-05', '14:30:00', 'Avenue des Roses', '26000', 'Valence', 'Choc avec un vélo.', '2023-07-06', '2023-07-10', NULL, '2023-08-01', 2000.00, 'brouillard'),
(2024, 1, '2024-01-10', '06:45:00', 'Rue des Lilas', '84000', 'Avignon', 'Glissade sur verglas.', '2024-01-11', '2024-01-15', '2024-02-01', '2024-02-10', 6000.00, 'verglas');

-- TABLE PARCOURS
INSERT INTO PARCOURS VALUES
('RH001', '2023-03-12 07:30:00', 'RH001A', 10),
('RH003', '2023-07-05 14:00:00', 'RH003C', 20),
('RH004', '2024-01-10 06:30:00', 'RH004D', 30),
('RH005', '2024-12-25 06:30:00', 'RH005E', 50),
('15CC22224', '2024-12-25 06:30:00', 'RH007G', 60),
('15CC22224', '2024-12-25 08:00:00', 'RH007G', 60),
('15CC22224', '2024-12-25 09:30:00', 'RH007G', 60),
('15CC22224', '2024-12-25 11:00:00', 'RH007G', 60),
('15CC22224', '2024-12-25 12:30:00', 'RH007G', 60),
('15CC22224', '2024-12-25 14:00:00', 'RH007G', 60),
('14BB11113', '2024-12-25 05:45:00', 'RH008H', 70),
('14BB11113', '2024-12-25 06:30:00', 'RH008H', 70),
('14BB11113', '2024-12-25 07:15:00', 'RH008H', 70),
('14BB11113', '2024-12-25 08:00:00', 'RH008H', 70),
('14BB11113', '2024-12-25 08:45:00', 'RH008H', 70),
('14BB11113', '2024-12-25 09:30:00', 'RH008H', 70),
('14BB11113', '2024-12-25 10:15:00', 'RH008H', 70),
('14BB11113', '2024-12-25 11:00:00', 'RH008H', 70),
('14BB11113', '2024-12-25 11:45:00', 'RH008H', 70),
('14BB11113', '2024-12-25 12:30:00', 'RH008H', 70);

-- TABLE INTERVENTION
INSERT INTO INTERVENTION(dateentreebus, description, datedevis, datedebutreparation, datefinreparation, immat, annee, noseq) VALUES
('2023-03-13', 'Réparation pare-brise', '2023-03-14', '2023-03-16', '2023-03-19', 'RH001A', 2023, 1),
('2023-07-06', 'Réparation carrosserie', '2023-07-07', '2023-07-09', NULL, 'RH003C', 2023, 2),
('2024-01-11', 'Réparation système électrique', '2024-01-12', '2024-01-13', '2024-01-15', 'RH004D', 2024, 1);

-- TABLE Survenir
INSERT INTO Survenir VALUES
(2023, 1, 'RH001', '2023-03-12 07:30:00', 'Pare-brise cassé.', 'Aucune blessure.', TRUE, TRUE),
(2023, 2, 'RH003', '2023-07-05 14:00:00', 'Carrosserie rayée.', 'Contusion mineure.', TRUE, TRUE),
(2024, 1, 'RH004', '2024-01-10 06:30:00', 'Roue abîmée.', 'Choc au bras.', FALSE, TRUE);

-- TABLE Impliquer
INSERT INTO Impliquer VALUES
(2023, 1, 1, 'Contusions.', 'Aucun.', 1500.00),
(2023, 2, 2, 'Fracture.', 'Vélo endommagé.', 500.00),
(2024, 1, 3, 'Commotion.', 'Véhicule détruit.', 4000.00);

-- TABLE Utiliser
INSERT INTO Utiliser VALUES
(1, 'F100', 1, 1, 250.00),
(1, 'F101', 1, 1, 250.00),
(1, 'F102', 1, 1, 250.00),
(1, 'F103', 1, 1, 250.00),
(1, 'F104', 1, 1, 250.00),
(2, 'F103', 5, 5, 15.00),
(3, 'F104', 1, 1, 300.00);

-- TABLE Necessiter
INSERT INTO Necessiter VALUES
(1, 'M100', 2.00, 2.00, 45.00),
(2, 'M101', 4.00, 3.50, 50.00),
(3, 'M102', 3.00, 3.00, 40.00);

insert into roles values ('SAD','Service Administratifs'),
('CHF','Chauffeur'),
('TCN','Technicien'),
('ADC','Administrateur CTVR'),
('ADU','Administrateur Utilisateurs');

insert into utilisateurs values ('adminCTVR',null,'Admin', 'CTVR', 'adminCTVR', 'ADC'),
('adminUti',null,'Admin', 'Utilisateurs', 'adminUti', 'ADU'),
('geoguich','14BB11113','Geoffroy', 'Guichard', 'geoguich', 'CHF'),
('madelric','15CC22224','Madeleine', 'Richard', 'madelric', 'CHF'),
('fredglass',null,'Frédéric ', 'De Carglass', 'fredglass', 'TCN'),
('juliault',null,'Julie', 'Renault', 'juliault', 'SAD'),
('jereducr',null,'Jérémy', 'Ducros', 'jereducr', 'SAD');












