<template>

  <body class="bg-[#A3A3A3] p-20">
    <section>
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/admin"></Retour>
        </template>
        <template v-slot:title>
          Gestion conducteurs
        </template>
      </Header>
    </section>
    <form class="p-[20px] h-[700px] border rounded-[10px] bg-white w-full flex flex-col items-center">
      <div class="w-[90%]">
        <table class="w-full">
          <thead
            class="w-[100px] h-[50px] text-base text-center shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
            <tr>
              <th class="w-1/11 p-[10px] border border-hidden">Nom</th>
              <th class="w-1/11 p-[10px] border border-hidden">Prénom</th>
              <th class="w-1/11 p-[10px] border border-hidden">Date de <br> naissance</th>
              <th class="w-1/11 p-[10px] border border-hidden">N°Permis</th>
              <th class="w-1/11 p-[10px] border border-hidden">Adresse</th>
              <th class="w-1/11 p-[10px] border border-hidden">Code Postal</th>
              <th class="w-1/11 p-[10px] border border-hidden">Ville</th>
              <th class="w-1/11 p-[10px] border border-hidden">Date <br> d'obtention</th>
              <th class="w-1/11 p-[10px] border border-hidden">Lieu de <br> délivrance</th>
              <th class="w-1/11 p-[10px] border border-hidden">Date de prise <br> de fonction</th>
              <th class="w-1/11 p-[10px] border border-hidden"></th>
            </tr>
          </thead>
        </table>
        <div class="w-full h-[500px] overflow-y-auto">
          <table class="w-full">
            <tbody>
              <tr v-for="conducteur in conducteurs" class="w-full">
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.nom" type="text" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.prenom" type="text" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.datenais" type="date" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.npermis" type="text" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.adresse" type="text" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.codepostal" type="text" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.ville" type="text" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.dateopt" type="date" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.lieudeli" type="text" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.dateprisefonct" type="date" :disabled="conducteur.disabled"
                      placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td>
                  <div class="w-full flex justify-left">
                    <button :class="{ 'hidden': conducteur.added, 'bg-white': !conducteur.added }"
                      @click.prevent="addConducteur(conducteur)"
                      class="w-14/15 h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black mr-[5px] ml-[10px]">
                      Ajouter
                    </button>
                    <div class="flex justify-center ml-[3px]">
                      <button v-if="conducteur.added"
                        :class="{ 'hidden': conducteur.modif, 'bg-white': !conducteur.modif }"
                        @click.prevent="editConducteur(conducteur)"
                        class="w-14/15 h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black mr-[3px] ml-[5px]">
                        Modif
                      </button>
                      <button v-if="conducteur.modif" @click.prevent="updateConducteur(conducteur)"
                        class="w-14/15 h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black mr-[3px] ml-[5px]">
                        Confirm
                      </button>
                      <button v-if="conducteur.added"
                        :class="{ 'hidden': conducteur.modif, 'bg-white': !conducteur.modif }"
                        @click.prevent="delConducteur(conducteur)"
                        class="w-[20%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[10px]">Suppr</button>
                    </div>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <table class="w-full">
          <tfoot>
            <tr>
              <th class="w-full p-[10px]">
                <button @click.prevent="newConducteurAction()"
                  class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white"
                  :disabled="addInProgess">+</button>
              </th>
            </tr>
          </tfoot>
        </table>
        <p v-if="answer === 2" class="absolute text-red-700">Champ manquant !</p>
        <p v-if="answer === 3" class="absolute text-red-700">Le Conducteur n'existe pas !</p>
        <p v-if="answer === 4" class="absolute text-red-700">Le Conducteur existe déjà !</p>
        <p v-if="answer === 5" class="absolute text-red-700">Erreur lors de l'ajout, réessayez.</p>
        <p v-if="answer === 6" class="absolute text-red-700">Un champ du Conducteur est erronée !</p>
        <p v-if="answer === 7" class="absolute text-red-700">Erreur vous voulez supprimer un Conducteur utiliser sur une
          autre page !</p>
      </div>
    </form>
  </body>
</template>

<script lang="ts">
import { ref, onMounted, watch } from 'vue';
import Header from '@/components/header.vue';
import Retour from '@/components/Retour.vue';
import { useRouter } from 'vue-router';

export default {
  components: {
    Header,
    Retour
  },
  data() {
    return {
      answer: 0,
      oldNumeroConducteur: 0,
      modificationInProgress: false,
      addInProgess: false,
    }
  },
  setup() {
    // Définition de la structure des Conducteurs du tableau
    const conducteurs = ref([]);


    const router = useRouter();

    if (localStorage.getItem("role") !== 'ADC') {
      if (localStorage.getItem("role") === 'CHF') {
        router.push('/conducteur')
      }
      else if (localStorage.getItem("role") === 'SAD') {
        router.push('/servicesAdmin')
      }
      else if (localStorage.getItem("role") === 'ADU') {
        router.push('/gestionUtilisateurs')
      }
      else {
        router.push('/')
      }
    }

    // Initialiser les données dans conducteurs
    const fechtData = () => {
      fetch("http://localhost:5107/api/Maintenance/GetAllInterventions", {
        method: "GET"
      }).then(data => data.json())
        .then(reponse => {
          conducteurs.value = reponse.map((item: any) => {
            return {
              nom: item.nom,
              prenom: item.prenom,
              datenais: item.datenais,
              npermis: item.npermis,
              adresse: item.adresse,
              codepostal: item.codepostal,
              ville: item.ville,
              dateopt: item.dateopt,
              lieudeli: item.lieudeli,
              dateprisefonct: item.dateprisefonct,
              added: true,
              modif: false,
              disabled: true,
            }
          });
        });
    };

    onMounted(() => {
      fechtData()
    })

    // Fonction pour ajouter une nouvelle Conducteur
    const newConducteur = () => {
      conducteurs.value.push({ nom: '', prenom: '', datenais: '', npermis: '', adresse: '', codepostal: '', ville: '', dateopt: '', lieudeli: '', dateprisefonct: '', added: false, modif: false, disabled: false });
    };

    return {
      conducteurs,
      fechtData,
      newConducteur
    };

  },
  methods: {
    async addConducteur(ConducteurCourrant: any) {
      let res = await fetch("http://localhost:5107/api/Conducteurs/AddConducteur?noConducteur=" + ConducteurCourrant.NumeroConducteur + "&nomarretdepart=" + ConducteurCourrant.Depart + "&nomarretterminus=" + ConducteurCourrant.Terminus, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        ConducteurCourrant.added = !ConducteurCourrant.added;
        ConducteurCourrant.disabled = !ConducteurCourrant.disabled;
        this.fechtData()
      }
    },
    async updateConducteur(ConducteurCourrant: any) {
      let res = await fetch("http://localhost:5107/api/Conducteurs/updateConducteur?oldnoConducteur=" + this.oldNumeroConducteur + "&noConducteur=" + ConducteurCourrant.NumeroConducteur + "&nomarretdepart=" + ConducteurCourrant.Depart + "&nomarretterminus=" + ConducteurCourrant.Terminus, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        ConducteurCourrant.modif = !ConducteurCourrant.modif;
        ConducteurCourrant.disabled = !ConducteurCourrant.disabled;
        this.modificationInProgress = false
        this.fechtData()
      }
    },
    editConducteur(ConducteurCourrant: any) {
      ConducteurCourrant.modif = !ConducteurCourrant.modif;
      ConducteurCourrant.disabled = !ConducteurCourrant.disabled;
      this.oldNumeroConducteur = ConducteurCourrant.NumeroConducteur;
      this.modificationInProgress = true;
    },
    async delConducteur(ConducteurCourrant: any) {
      let res = await fetch("http://localhost:5107/api/Conducteurs/deleteConducteur?nopermis=" + ConducteurCourrant, {
        "method": "POST"
      })
      let data = await res.json()
      if (data === 1) {
        this.fechtData()
      }
    },
    newConducteurAction() {
      this.addInProgess = true
      this.newConducteur();
    }
  }
};
</script>