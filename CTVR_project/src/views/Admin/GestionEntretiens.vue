<template>

  <body class="bg-[#A3A3A3] p-20">
    <section>
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/admin"></Retour>
        </template>
        <template v-slot:title>
          Gestion des Entretiens
        </template>
      </Header>
    </section>
    <form class="p-[20px] h-[700px] border rounded-[10px] bg-white w-full flex flex-col items-center">
      <div class="w-[90%]">
        <table class="w-full">
          <thead
            class="w-full h-[50px] text-base text-center shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
            <tr>
              <th class="w-2/7 p-[10px] border border-hidden">Description</th>
              <th class="w-2/7 p-[10px] border border-hidden">Immat</th>
              <th class="w-2/7 p-[10px] border border-hidden">Devis</th>
              <th class="w-2/7 p-[10px] border border-hidden"></th>
            </tr>
          </thead>
        </table>
        <div class="w-full h-[500px] overflow-y-auto">
          <table class="w-full">
            <tbody>
              <tr v-for="entretien in entretiens" class="w-full">
                <td class="w-2/7 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="entretien.description" type="text" :disabled="entretien.started || entretien.added"
                      placeholder="..."
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-2/7 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="entretien.immat" type="text" :disabled="entretien.started || entretien.added"
                      placeholder="..."
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-2/7 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <button @click.prevent="consulterDevis(entretien)" :disabled="entretien.started || entretien.added"
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black mr-[0px] ml-[5px]">
                      Devis associé
                    </button>
                  </div>

                </td>
                <td>
                  <div class="w-full flex justify-left">
                    <button :class="{ 'hidden': entretien.added, 'bg-white': !entretien.added }"
                      @click.prevent="addEntretien(entretien)"
                      class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black my-[20px] mr-[5px] ml-[25px]">
                      Ajouter
                    </button>
                    <button v-if="entretien.added"
                      :class="{ 'hidden': entretien.started, 'bg-white': !entretien.started }"
                      @click.prevent="toggleStart(entretien)"
                      class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black my-[20px] mr-[5px] ml-[25px]">
                      Commencer
                    </button>
                    <div class="flex justify-center ml-[3px]">
                      <button v-if="entretien.started"
                        :class="{ 'hidden': entretien.finished, 'bg-white': !entretien.finished }"
                        @click.prevent="finishEntretien(entretien)"
                        class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black my-[20px] mr-[0px] ml-[5px]">
                        Finir
                      </button>
                      <button v-if="entretien.started" :class="{ 'hidden': entretien.finished }"
                        @click.prevent="editEntretien(entretien)"
                        class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black my-[20px] mr-[3px] ml-[5px]">
                        Modif
                      </button>
                      <button v-if="entretien.started"
                        :class="{ 'hidden': entretien.finished, 'bg-white': !entretien.finished }"
                        @click.prevent="delEntretien(entretien)"
                        class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white my-[20px] ml-[10px]">Suppr</button>
                      <label v-if="entretien.finished"
                        class="w-full bg-[green] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-green-500 text-white my-[20px] mr-[3px] ml-[5px] flex items-center justify-center">Entretient
                        Terminé !</label>
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
              <th class="w-full">
                <button @click.prevent="addEntretien"
                  class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">+</button>
              </th>
            </tr>
          </tfoot>
        </table>
        <p v-if="answer === 2" class="absolute text-red-700">Champ manquant !</p>
        <p v-if="answer === 3" class="absolute text-red-700">L'entretien n'existe pas !</p>
        <p v-if="answer === 4" class="absolute text-red-700">L'entretien existe déjà !</p>
        <p v-if="answer === 5" class="absolute text-red-700">Erreur lors de l'ajout, réessayez.</p>
        <p v-if="answer === 6" class="absolute text-red-700">Un champ de l'entretien est erronée !</p>
        <p v-if="answer === 7" class="absolute text-red-700">Erreur vous voulez supprimer un entretien utiliser sur une
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
      modificationInProgress: false,
      addInProgress: false
    }
  },
  setup() {
    // Définition de la structure des lignes du tableau
    const entretiens = ref([{ description: '', immat: '', added: true, started: true, finished: false }]);

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

    // Initialiser les données dans entretiens
    const fechtData = () => {
      fetch("http://localhost:5107/api/Maintenance/GetAllInterventions", {
        method: "GET"
      }).then(data => data.json())
        .then(reponse => {
          entretiens.value = reponse.map((item: any) => {
            return {
              norep: item.norep,
              description: item.description,
              immat: item.immat,
              added: true,
              started: !!item.datedebutreparation,
              finished: false,
              datedevis: item.datedevis,
              datefinreparation: item.datefinreparation,
              datedebutreparation: item.datedebutreparation
            }
          })
        });
    };

    onMounted(() => {
      fechtData()
    })

    // Fonction pour ajouter une nouvelle ligne
    const newEntretien = () => {
      entretiens.value.push({ description: '', immat: '', added: false, started: false, finished: false });
    };

    // Watch pour surveiller les changements dans entretiens
    watch(
      entretiens,
      (newEntretiens) => {
        newEntretiens.forEach((entretien) => {
          if (entretien.datefinreparation && !entretien.finished) {
            entretien.started = true;
            entretien.finished = true;
          } else if (entretien.datedebutreparation && entretien.added && !entretien.started) {
            entretien.started = true;
          } else if (!entretien.datedebutreparation && entretien.added && !entretien.started) {
            entretien.started = false;
          } else if (!entretien.datedebutreparation && !entretien.added && !entretien.started) {
            entretien.added = false;
            entretien.started = false;
          }
        });
      },
    );

    return {
      entretiens,
      fechtData,
      newEntretien
    };

  },

  methods: {
    async addEntretien(entretienCourant: any) {
      let res = await fetch("http://localhost:5107/api/Maintenance/AddIntervention?description=" + entretienCourant.description + "&immat=" + entretienCourant.immat, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        entretienCourant.added = !entretienCourant.added;
        entretienCourant.disabled = !entretienCourant.disabled;
        this.addInProgress = false
        this.fechtData()
      }
    },
    async updateEntretien(entretienCourant: any) {
      let res = await fetch("http://localhost:5107/api/Maintenance/updateIntervention?norep=" + entretienCourant.norep + "&datedebutreparation=" + currentDate.getDate() + "-" + (currentDate.getMonth() + 1) + "-" + currentDate.getFullYear(), {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        entretienCourant.modif = !entretienCourant.modif;
        entretienCourant.disabled = !entretienCourant.disabled;
        this.modificationInProgress = false
        this.fechtData()
      }
    },
    editEntretien(entretienCourant: any) {
      entretienCourant.modif = !entretienCourant.modif;
      entretienCourant.disabled = !entretienCourant.disabled;
      this.modificationInProgress = true;
    },
    async toggleStart(entretienCourant: any) {
      let currentDate = new Date();
      let res = await fetch("http://localhost:5107/api/Maintenance/updateIntervention?norep=" + entretienCourant.norep + "&datedebutreparation=" + currentDate.getDate() + "-" + (currentDate.getMonth() + 1) + "-" + currentDate.getFullYear(), {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        this.fechtData()
      }
    },
    async finishEntretien(entretienCourant: any) {
      let currentDate = new Date();
      let res = await fetch("http://localhost:5107/api/Maintenance/updateIntervention?norep=" + entretienCourant.norep + "&datefinreparation=" + currentDate.getDate() + "-" + (currentDate.getMonth() + 1) + "-" + currentDate.getFullYear(), {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        this.fechtData()
      }
    },
    async delEntretien(entretienCourant: any) {
      let res = await fetch("http://localhost:5107/api/Maintenance/DeleteIntervention?norep=" + entretienCourant.norep, {
        "method": "POST"
      })
      let data = await res.json()
      if (data === 1) {
        this.fechtData()
      }
    },
    newEntretienAction() {
      this.addInProgress = true
      this.newEntretien()
    },
    consulterDevis(entretienCourant: any) {
      localStorage.setItem("norep", entretienCourant.norep)
      this.$router.push("/servicesAdmin/consulterDevis")
    },
    creerDevis(entretienCourant: any) {
      localStorage.setItem("norep", entretienCourant.norep)
      this.$router.push("/servicesAdmin/saisieDevis")
    }
  }
};
</script>