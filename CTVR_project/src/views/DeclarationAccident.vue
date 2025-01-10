<template>
  <html>

  <body class="bg-[#A3A3A3] p-20">
    <section>
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/conducteur"></Retour>
        </template>
        <template v-slot:title>
          Déclaration d'accident
        </template>
      </Header>
    </section>
    <form class="p-[20px] border rounded-[10px] bg-white w-full">
      <label class="text-black">Informations générales</label>
      <section class="mt-[10px] mb-[10px]">
        <div>
          <label class="text-black m-[20px] mr-[160px]">Date</label>
          <label class="text-black m-[20px]">Heure</label>
        </div>
        <div class="flex items-center">
          <input v-model="dateAccident" type="date"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] mr-[20px]">
          <input v-model="heureAccident" type="time"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] mr-[20px]">
        </div>
        <div>
          <label class="text-black m-[20px] mr-[170px]">Rue</label>
          <label class="text-black m-[20px] mr-[170px]">Ville</label>
          <label class="text-black m-[20px]">Code Postal</label>
        </div>
        <div class="flex items-center">
          <input v-model="rue" type="text"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] mr-[20px]">
          <input v-model="ville" type="text"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] mr-[20px]">
          <input v-model="codePostal" type="text"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] mr-[20px]">
        </div>
      </section>
      <label class="text-black mt-[20px]">Information de la CTVR</label>
      <section class="mt-[10px] mb-[10px]">
        <div>
          <label class="text-black m-[20px] mr-[90px]">Numéro de permis</label>
          <label class="text-black m-[20px]">Immatriculation bus</label>
        </div>
        <div>
          <input v-model="numPermis" type="text"
            class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] mr-[20px]">
          <input v-model="immat" type="text"
            class="bg-[lightgray] border-10 border-black rounded-[10px] text-black text-center w-[200px]"
            placeholder="AA-000-AA">
        </div>
        <div>
          <label class="text-black ml-[15px]">Code intempérie</label>
        </div>
        <div>
          <select v-model="codeIntemp"
            class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] mr-[20px] mb-[10px]">
            <option value="0">Soleil</option>
            <option value="1">Brouillard</option>
            <option value="2">Pluie</option>
            <option value="3">Orage</option>
            <option value="4">Autres</option>
          </select>
        </div>
      </section>
      <label class="text-black mb-[10px] mt-[10px]">Information des tiers impliqués</label>
      <section>
        <div v-for="(tiers, index) in ListeTiers" :key="index" class="border rounded-[20px] w-[700px]">
          <div>
            <label class="text-black mr-[150px] ml-[30px]">Nom tiers</label>
            <label class="text-black mr-[130px]">Prénom tiers</label>
            <label class="text-black">Date de naissance tiers</label>
          </div>
          <div>
            <input v-model="tiers.nomtiers" type="text"
              class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] ml-[20px] mr-[20px]">
            <input v-model="tiers.prenomtiers" type="text"
              class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] mr-[20px]">
            <input v-model="tiers.datenaiss" type="date"
              class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px]">
          </div>
          <div>
            <label class="text-black mr-[155px] ml-[30px]">Rue Tiers</label>
            <label class="text-black mr-[150px]">Ville Tiers</label>
            <label class="text-black">Code Postal Tiers</label>
          </div>
          <div class="flex items-center">
            <input v-model="tiers.rue" type="text"
              class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] ml-[20px] mr-[20px]">
            <input v-model="tiers.ville" type="text"
              class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] mr-[20px]">
            <input v-model="tiers.cp" type="text"
              class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] mr-[20px]">
          </div>
          <div>
            <label class="text-black ml-[30px]">Nature des blessures du tiers</label>
          </div>
          <div>
            <input v-model="tiers.blessuretiers" type="text"
              class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[400px] mr-[10px] ml-[20px] mb-[10px]">
          </div>
          <div>
            <label class="text-black ml-[30px]">Nature des dégâts matériels</label>
          </div>
          <div>
            <input v-model="tiers.degamat" type="text"
              class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[400px] mr-[10px] ml-[20px] mb-[10px]">
            <button @click.prevent="deltiers(index)"
              class="bg-[darkgray] border-3 border-black rounded-[8px] text-black text-center w-[150px] mr-[10px] ml-[80px] mb-[10px]">Supprimer
              tiers</button>
          </div>
        </div>
      </section>
      <button @click.prevent="newTiers"
        class="ml-[30px] mt-[10px] bg-[darkgray] border rounded-[30px] w-[200px] h-[30px] text-black">+ Ajouter un
        tiers</button>
      <section class="mt-[10px]">
        <label class="text-black">Dommages CTVR</label>
        <div>
          <label class="text-black ml-[30px]">Description des dégâts</label>
        </div>
        <div>
          <textarea v-model="descriptionDegatsBus"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black w-[500px] h-[100px] mr-[10px] ml-[20px]"></textarea>
        </div>
        <div>
          <label class="text-black ml-[30px]">Description des blessures du conducteur</label>
        </div>
        <div>
          <textarea v-model="descriptionDegatsConducteurs"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black w-[500px] h-[100px] mr-[10px] ml-[20px]"></textarea>
        </div>
        <label class="text-black">Circonstances de l'accident</label>
        <div class="ml-[30px]">
          <label class="text-black">Description des circonstances</label>
        </div>
        <div>
          <textarea v-model="circonstancesAccident"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black w-[500px] h-[100px] mr-[10px] ml-[20px]"></textarea>
        </div>
      </section>
      <div class="flex flex-tiers justify-center mt-[20px]">
        <div class="w-[420px] h-[100px] mr-[50px]">
          <input v-model="signatureConducteur" type="checkbox">
          <label class="text-black text-sm">Le conducteur certifie sur l'honneur avoir rempli avec honnêteté des
            informations véridiques dans la déclaration précédente en présence d'un contrôleur.</label>
        </div>
        <div class="w-[450px] h-[100px]">
          <input v-model="signatureControleur" type="checkbox">
          <label class="text-black text-sm">La personne en charge du contrôle présent certifie sur l'honneur avoir
            vérifié
            et assuré de la véracité des informations indiquées dans la déclaration précédente par le
            conducteur.</label>
        </div>
      </div>
      <div class="flex justify-center">
        <button @click.prevent="declarerAccident()" type="submit"
          class="bg-[darkgray] border rounded-[30px] w-[200px] h-[30px] text-black">Envoyer la
          déclaration</button>
      </div>
    </form>
    <p v-if="emptyFields" class="absolute text-red-700">Champ manquant !</p>
    <p v-if="!wrongNopermisOrImmat" class="absolute text-red-700">Le numéro de permis et/ou l'immatriculation n'existe
      pas !
    </p>
  </body>

  </html>
</template>

<script lang="ts">
import { ref, onMounted } from 'vue';
import Header from '@/components/header.vue';
import Retour from '@/components/Retour.vue';
import { useRouter } from 'vue-router';


export default {
  components: {
    Header,
    Retour
  },
  name: 'DeclarationAccident',
  data() {
    return {
      annee: 0,
      noSeq: 0,
      dateAccident: '',
      heureAccident: '',
      rue: '',
      codePostal: '',
      ville: '',
      numPermis: '',
      immat: '',
      codeIntemp: '',
      descriptionDegatsBus: '',
      descriptionDegatsConducteurs: '',
      circonstancesAccident: '',
      signatureConducteur: false,
      signatureControleur: false,
      answer: 0,
      emptyFields: false,
      wrongNopermisOrImmat: true
    }
  },
  setup() {
    const Intemperies = {
      Pluie: 0,
      Brouillard: 1,
      Neige: 2,
      Verglas: 3,
      Autres: 4
    }
    // Définition de la structure des tiers
    const ListeTiers = ref([{ nomtiers: '', prenomtiers: '', datenaiss: '', rue: '', ville: '', cp: '', blessuretiers: '', degamat: '' }]);

    const router = useRouter();
    if (localStorage.getItem("role") !== 'CHF') {
      if (localStorage.getItem("role") === 'SAD'){
        router.push('/servicesAdmin')
      }
      else if (localStorage.getItem("role") === 'ADC'){
        router.push('/admin')
      }
      else if (localStorage.getItem("role") === 'ADU'){
        router.push('/gestionUtilisateurs')
      }
      else {
        router.push('/')
      }
    }
    // Fonction pour ajouter une nouvelle ligne
    const newTiers = () => {
      ListeTiers.value.push({ nomtiers: '', prenomtiers: '', datenaiss: '', rue: '', ville: '', cp: '', blessuretiers: '', degamat: '' });
    };

    // Fonction pour supprimer une ligne
    const deltiers = (index: any) => {
      ListeTiers.value.splice(index, 1);
    };

    // Retourner les données et les méthodes
    return {
      Intemperies,
      ListeTiers,
      newTiers,
      deltiers
    };
  },
  methods: {
    async declarerAccident() {
      this.wrongNopermisOrImmat = true
      this.emptyFields = this.isAnyChampsEmpty()
      if (!this.emptyFields) {
        this.wrongNopermisOrImmat = await this.verifChamps()
        if (this.wrongNopermisOrImmat) {
          await this.insererAccident()
          await this.insererSurvenir()
          await this.insererTiers()
        }
      }
    },
    async insererAccident() {
      let res = await fetch("http://localhost:5107/api/Accidents/Insert?dateAccident=" + this.dateAccident + "&rue=" + this.rue + "&codepostal=" + this.codePostal + "&ville=" + this.ville + "fg&resumecirconstances=" + this.circonstancesAccident + "&intemp=" + this.codeIntemp, {
        "method": "POST"
      })
      let data = await res.json()
      console.log(data)
      this.annee = data['annee']
      this.noSeq = data['noseq']
    },
    async insererSurvenir() {
      let res = await fetch("http://localhost:5107/api/Survenir/addSurvenir ?nopermis=" + this.numPermis + "&heure=" + this.heureAccident + "&date=" + this.dateAccident + "&annee=" + this.annee + "&noseq=" + this.noSeq + "&dmgBus=" + this.descriptionDegatsBus + "&dmgConducteur=" + this.descriptionDegatsConducteurs + "&signatureConduct=" + this.signatureConducteur + "&signatureControl=" + this.signatureControleur, {
        "method": "POST"
      })
    },
    async insererTiers() {
      for (let tiers of this.ListeTiers) {
        let noTiers = await this.insererTiersUnique(tiers)
        await this.insererImpliquerUnique(tiers, noTiers)
      }
    },
    async insererTiersUnique(tiers: any) {
      let res = await fetch("http://localhost:5107/api/Tiers/Insert?nomtiers=" + tiers.nomtiers + "&prenomtiers=" + tiers.prenomtiers + "&datenaissance=" + tiers.datenaiss + "&adressetiers=" + tiers.rue + "&cptiers=" + tiers.cp + "&villetiers=" + tiers.ville, {
        "method": "POST"
      })
      return await res.json()
    },
    async insererImpliquerUnique(tiers: any, noTiers: Number) {
      let res = await fetch("http://localhost:5107/api/Impliquers/Insert?annee=" + this.annee + "&noseq=" + this.noSeq + "&notiers=" + noTiers + "&natureblessures=" + tiers.blessuretiers + "&degatsmateriels=" + tiers.degamat, {
        "method": "POST"
      })
      return await res.json()
    },
    async verifChamps(): Promise<boolean> {
      let res = await fetch("http://localhost:5107/api/Conducteurs/checkConducteur?nopermis=" + this.numPermis, {
        "method": "GET"
      })
      let data = await res.json()
      if (data) {
        res = await fetch("http://localhost:5107/api/Bus/checkBus?immat=" + this.immat, {
          "method": "GET"
        })
        return await res.json()
      }
      return false
    },
    isAnyChampsEmpty(): boolean {
      let champsAccident = this.dateAccident.trim().length === 0
        || this.heureAccident.trim().length === 0
        || this.rue.trim().length === 0
        || this.codePostal.trim().length === 0
        || this.ville.trim().length === 0
        || this.numPermis.trim().length === 0
        || this.immat.trim().length === 0
        || this.codeIntemp.trim().length === 0
        || this.descriptionDegatsBus.trim().length === 0
        || this.descriptionDegatsConducteurs.trim().length === 0
        || this.circonstancesAccident.trim().length === 0;
      let champsTiers = false
      for (let tiers of this.ListeTiers) {
        champsTiers = champsTiers
          || tiers.nomtiers.trim().length === 0
          || tiers.prenomtiers.trim().length === 0
          || tiers.datenaiss.trim().length === 0
          || tiers.rue.trim().length === 0
          || tiers.ville.trim().length === 0
          || tiers.cp.trim().length === 0
          || tiers.degamat.trim().length === 0
          || tiers.blessuretiers.trim().length === 0
      }
      return champsAccident || champsTiers
    }
  }
};
</script>