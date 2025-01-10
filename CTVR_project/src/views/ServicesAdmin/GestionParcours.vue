<template>

  <body class="bg-[#A3A3A3] p-20">
    <section>
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/servicesAdmin"></Retour>
        </template>
        <template v-slot:title>
          Gestion des parcours
        </template>
      </Header>
    </section>
    <form class="p-[20px] h-[700px] border rounded-[10px] bg-white w-full flex flex-col items-center">
      <div class="w-[90%]">
        <table class="w-full">
          <thead
            class="w-full h-[50px] text-base text-center shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
            <tr>
              <th class="w-1/5 p-[10px] border border-hidden">Conducteur</th>
              <th class="w-1/5 p-[10px] border border-hidden">Bus</th>
              <th class="w-1/5 p-[10px] border border-hidden">Horaire</th>
              <th class="w-1/5 p-[10px] border border-hidden">Numéro de Ligne</th>
              <th class="w-1/5 p-[10px] border border-hidden"></th>
            </tr>
          </thead>
        </table>
        <div class="w-full h-[500px] overflow-y-auto">
          <table class="w-full">
            <tbody>
              <tr v-for="parcour in parcours" class="w-full">
                <td class="w-1/5 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-if="!parcour.modif && parcour.added" v-model="parcour.Conducteur" type="text"
                      placeholder="..." :disabled="parcour.disabled"
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[3px]">
                    <input v-if="parcour.modif || !parcour.added" v-model="parcour.NoPermis" type="text"
                      placeholder="N° permis (13AA00002)" :disabled="parcour.disabled"
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[3px]">
                  </div>
                </td>
                <td class="w-1/5 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="parcour.Bus" type="text" placeholder="AA000AA" :disabled="parcour.disabled"
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[3px]">
                  </div>

                </td>

                <td class="w-1/5 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="parcour.Horaire" type="datetime-local" placeholder="AA-000-AA"
                      :disabled="parcour.disabled"
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[3px]">
                  </div>
                </td>
                <td class="w-1/5 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="parcour.NoLigne" type="text" placeholder="Numéro de ligne"
                      :disabled="parcour.disabled"
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[3px]">
                  </div>
                </td>
                <td>
                  <div class="w-full flex justify-left">
                    <button :class="{ 'hidden': parcour.added, 'bg-white': !parcour.added }"
                      @click.prevent="addParcour(parcour)"
                      class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black m-[20px] mr-[5px] ml-[5px]">
                      Ajouter
                    </button>
                    <button v-if="parcour.added" :class="{ 'hidden': parcour.modif, 'bg-white': !parcour.modif }"
                      @click.prevent="editParcour(parcour)"
                      class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black m-[20px] mr-[3px] ml-[5px]"
                      :disabled="modificationInProgress">
                      Modif
                    </button>
                    <button v-if="parcour.modif" @click.prevent="updateParcour(parcour)"
                      class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black m-[20px] mr-[3px] ml-[5px]">
                      Confirmer
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <table class="w-full">
          <tfoot>
            <tr>
              <th class="w-full">
                <button @click.prevent="newParcourAction()"
                  class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white"
                  :disabled="addInProgress">+</button>
              </th>
            </tr>
          </tfoot>
        </table>
        <p v-if="answer === 2" class="absolute text-red-700">Champ manquant !</p>
        <p v-if="answer === 3" class="absolute text-red-700">Le parcours n'existe pas !</p>
        <p v-if="answer === 4" class="absolute text-red-700">Le parcours existe déjà !</p>
        <p v-if="answer === 5" class="absolute text-red-700">Erreur lors de l'ajout, réessayez.</p>
        <p v-if="answer === 6" class="absolute text-red-700">Un champ du parcours est erroné !</p>
      </div>
    </form>
  </body>
</template>



<script lang="ts">
import { onMounted, ref } from 'vue';
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
      oldNoPermisParcours: '',
      oldHoraireServiceParcours: Date,
      modificationInProgress: false,
      addInProgress: false
    }
  },
  setup() {
    // Définition de la structure des lignes du tableau
    const parcours = ref([])
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

    const fechtData = () => {
      fetch("http://localhost:5107/api/Parcours/GetAllParcours?filtre=asc", {
        method: "GET"
      }).then((data) => data.json())
        .then((response) => {
          parcours.value = response.map((item: any) => {
            return {
              Conducteur: item.prenomchauffeur + " " + item.nomchauffeur,
              NoPermis: item.nopermis,
              Bus: item.immat,
              Horaire: item.horaireservice,
              NoLigne: item.noligne,
              added: true,
              modif: false,
              disabled: true
            }
          })
        })
    };

    onMounted(() => {
      fechtData()
    })

    // Fonction pour ajouter une nouvelle ligne
    const newParcour = () => {
      parcours.value.push({ Conducteur: '', Bus: '', Horaire: '', NoLigne: '', added: false, modif: false, disabled: false });
    };

    // Retourner les données et les méthodes
    return {
      parcours,
      newParcour,
      fechtData
    };
  },
  methods: {
    async addParcour(currentParcours: any) {
      let res = await fetch("http://localhost:5107/api/Parcours/AddParcours?nopermis=" + currentParcours.NoPermis + "&horaireservice=" + currentParcours.Horaire + "&noligne=" + currentParcours.NoLigne + "&immat=" + currentParcours.Bus, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        currentParcours.added = !currentParcours.added;
        currentParcours.disabled = !currentParcours.disabled;
        this.fechtData()
      }
    },
    async updateParcour(currentParcours: any) {
      let res = await fetch("http://localhost:5107/api/Parcours/updateParcours?oldnopermis=" + this.oldNoPermisParcours + "&oldhoraireservice=" + this.oldHoraireServiceParcours + "&noligne=" + currentParcours.NoLigne + "&immat=" + currentParcours.Bus + "&nopermis=" + currentParcours.NoPermis + "&horaireservice=" + currentParcours.Horaire, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        currentParcours.modif = !currentParcours.modif;
        currentParcours.disabled = !currentParcours.disabled;
        this.modificationInProgress = false
        this.addInProgress = false
        this.fechtData()
      }
    },
    editParcour(currentParcours: any) {
      currentParcours.modif = !currentParcours.modif;
      currentParcours.disabled = !currentParcours.disabled;
      this.oldNoPermisParcours = currentParcours.NoPermis;
      this.oldHoraireServiceParcours = currentParcours.Horaire;
      this.modificationInProgress = true;
    },
    newParcourAction() {
      this.addInProgress = true
      this.newParcour()
    }
  },
}
</script>