<template>
  <html>

  <body class="bg-[#A3A3A3] p-20">
    <section>
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/servicesAdmin/consulterAccidents"></Retour>
        </template>
        <template v-slot:title>
          Consultation d'un accident
        </template>
      </Header>
    </section>
    <form v-for="accident in accidents" class="p-[20px] border rounded-[10px] bg-white w-full">
      <label class="text-black">Informations générales</label>
      <section class="mt-[10px] mb-[10px]">
        <div>
          <label class="text-black m-[20px] mr-[160px]">Date</label>
          <label class="text-black m-[20px] mr-[160px]">Heure</label>
          <label class="text-black m-[20px]">Coordonnées GPS / Adresse</label>
        </div>
        <div class="flex items-center">
          <input v-model="accident.dateaccident" type="date"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] mr-[20px]"
            disabled>
          <input v-model="accident.heureaccident" type="time"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px] mr-[20px]"
            disabled>
          <input v-model="accident.adresse" id='location' type="text"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black text-center w-[200px]"
            placeholder="... , ..." disabled>
        </div>
      </section>
      <label class="text-black mt-[20px]">Information de la CTVR</label>
      <section class="mt-[10px] mb-[10px]">
        <div>
          <label class="text-black m-[20px] mr-[90px]">Nom chauffeur</label>
          <label class="text-black m-[20px] mr-[90px]">Numéro de permis</label>
          <label class="text-black m-[20px]">Immatriculation bus</label>
        </div>
        <div>
          <input v-model="accident.nom" type="text"
            class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] mr-[20px]"
            disabled>
          <input v-model="accident.nopermis" type="text"
            class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] mr-[20px]"
            disabled>
          <input v-model="accident.immat" type="text"
            class="bg-[lightgray] border-10 border-black rounded-[10px] text-black text-center w-[200px]"
            placeholder="AA-000-AA" disabled>
        </div>
        <div>
          <label class="text-black mr-[170px] ml-[20px]">Ligne</label>
          <label class="text-black">Code intempérie</label>
        </div>
        <div>
          <input v-model="accident.depart" type="text"
            class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] mr-[20px] mb-[10px]"
            disabled>
          <input v-model="accident.intemperie" type="text"
            class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] mr-[20px]"
            disabled>
        </div>
        <input v-model="accident.terminus" type="text"
          class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px]" disabled>

      </section>
      <label class="text-black mb-[10px] mt-[10px]">Information des tiers impliqués</label>
      <section>
        <div v-for="impliquer in accident.impliquers" class="border rounded-[20px] w-[700px] my-2" disabled>
          <div v-for="tiers in impliquer.tiers">
            <div>
              <label class="text-black mr-[150px] ml-[30px]">Nom tiers</label>
              <label class="text-black mr-[150px]">Prénom tiers</label>
              <label class="text-black">Date de naissance tiers</label>
            </div>
            <div>
              <input v-model="tiers.nomtiers" type="text"
                class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] ml-[20px] mr-[20px]"
                disabled>
              <input v-model="tiers.prenomtiers" type="text"
                class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px] mr-[20px]"
                disabled>
              <input v-model="tiers.datenaissance" type="date"
                class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[200px]" disabled>
            </div>
            <div>
              <label class="text-black ml-[30px]">Adresse tiers</label>
            </div>
            <div>
              <input v-model="tiers.adressetiers" type="text"
                class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[400px] mr-[10px] ml-[20px]"
                placeholder="19 rue Jean Bertin , Valence" disabled>
            </div>
          </div>
          <div>
            <label class="text-black ml-[30px]">Nature des blessures du tiers</label>
          </div>
          <div>
            <input v-model="impliquer.natureblessures" type="text"
              class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[400px] mr-[10px] ml-[20px] mb-[10px]"
              disabled>
          </div>
          <div>
            <label class="text-black ml-[30px]">Nature des dégâts matériels</label>
          </div>
          <div>
            <input v-model="impliquer.degatsmateriels" type="text"
              class="bg-[lightgray] border-3 border-black rounded-[8px] text-black text-center w-[400px] mr-[10px] ml-[20px] mb-[10px]"
              disabled>
          </div>
        </div>
      </section>
      <section class="mt-[10px]">
        <label class="text-black">Dommages CTVR</label>
        <div>
          <label class="text-black ml-[30px]">Description des dégâts du bus</label>
        </div>
        <div>
          <textarea v-model="accident.dommagesbus"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black w-[500px] h-[100px] mr-[10px] ml-[20px]"
            disabled></textarea>
        </div>
        <div>
          <label class="text-black ml-[30px]">Description des blessures du conducteur</label>
        </div>
        <div>
          <textarea v-model="accident.dommagesconducteur"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black w-[500px] h-[100px] mr-[10px] ml-[20px]"
            disabled></textarea>
        </div>
        <label class="text-black">Circonstances de l'accident</label>
        <div class="ml-[30px]">
          <label class="text-black">Description des circonstances</label>
        </div>
        <div>
          <textarea v-model="accident.resumecirconstances"
            class="bg-[lightgray] border-3 border-black rounded-[5px] text-black w-[500px] h-[100px] mr-[10px] ml-[20px]"
            disabled></textarea>
        </div>
      </section>
      <div class="flex flex-accident justify-center mt-[20px]">
        <div class="w-[420px] h-[100px] mr-[50px]">
          <input type="checkbox" :checked="accident.signaturecontroleur" disabled>
          <label class="text-black text-sm">Le conducteur certifie sur l'honneur avoir rempli avec honnêteté et des
            informations véridiques la déclaration précédente en présence d'un contrôleur.</label>
        </div>
        <div class="w-[450px] h-[100px]">
          <input type="checkbox" :checked="accident.signaturecontroleur" disabled>
          <label class="text-black text-sm">La personne en charge du contrôle présent certifie sur l'honneur avoir
            vérifié et assuré de la véracité des informations indiquées dans la déclaration précédente par le
            conducteur</label>
        </div>
      </div>
    </form>
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
  setup() {
    const annee = localStorage.getItem("annee")
    const noseq = localStorage.getItem("noseq")
    const nopermis = localStorage.getItem("nopermis")
    const horaireservice = localStorage.getItem("horaireservice")
    const router = useRouter();

    if (localStorage.getItem("role") !== 'SAD') {
      if (localStorage.getItem("role") === 'CHF'){
        router.push('/conducteur')
      }
      else if (localStorage.getItem("role") === 'ADC'){
        router.push('/admin')
      }
      else if (localStorage.getItem("role") === 'ADU'){
        router.push('/gestionUtilisateurs')
      }
      else{
        router.push('/')
      }
    }

    const accidents = ref([]);

    onMounted(() => {
      fetch("http://localhost:5107/api/Accidents/GetOneByID?annee=" + annee + "&noseq=" + noseq + "&nopermis=" + nopermis + "&horaireservice=" + horaireservice, {
        method: "GET"
      }).then(data => data.json())
        .then(accident => {
          accidents.value = accident.map((item: any) => {
            return { // le map ne fonctionne que sur les listes
              dateaccident: item.dateaccident,
              heureaccident: item.heureaccident,
              adresse: item.rue + ", " + item.codepostal + " " + item.ville,
              resumecirconstances: item.resumecirconstances,
              intemperie: item.intemperie,
              nom: item.prenom + " " + item.nom,
              immat: item.immat,
              nopermis: item.nopermis,
              dommagesbus: item.dommagesbus,
              dommagesconducteur: item.dommagesconducteur,
              signatureconducteur: item.signatureconducteur,
              signaturecontroleur: item.signaturecontroleur,
              depart: item.depart,
              terminus: item.terminus,
              impliquers: item.impliquers.map((implique: any) => {
                return {
                  tiers: implique.tiers.map((tiers: any) => {
                    return {
                      nomtiers: tiers.nomtiers,
                      prenomtiers: tiers.prenomtiers,
                      datenaissance: tiers.datenaissance,
                      adressetiers: tiers.adressetiers + ", " + tiers.cptiers + " " + tiers.villetiers
                    }
                  }),
                  natureblessures: implique.natureblessures,
                  degatsmateriels: implique.degatsmateriels
                }
              })
            }
          })
        })
    })

    console.log(accidents)

    // Définition de la structure des tiers
    const rowsTiers = ref([{ nomtier: '', prenomtier: '', agetier: '', adresstier: '', blessuretier: '', degamat: '' }]);

    // Retourner les données et les méthodes
    return {
      rowsTiers,
      accidents
    };
  },
};
</script>