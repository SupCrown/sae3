<template>

  <body class="bg-[#A3A3A3] p-20">
    <section>
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/servicesAdmin"></Retour>
        </template>
        <template v-slot:title>
          Gestion lignes de bus
        </template>
      </Header>
    </section>
    <form class="p-[20px] h-[700px] border rounded-[10px] bg-white w-full flex flex-col items-center">
      <div class="w-[90%]">
        <table class="w-full">
          <thead
            class="w-full h-[50px] text-base text-center shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
            <tr>
              <th class="w-2/7 p-[10px] border border-hidden">Numéro de Ligne</th>
              <th class="w-2/7 p-[10px] border border-hidden">Depart</th>
              <th class="w-2/7 p-[10px] border border-hidden">Terminus</th>
              <th></th>
            </tr>
          </thead>
        </table>
        <div class="w-full h-[500px] overflow-y-auto">
          <table class="w-full">
            <tbody>
              <tr v-for="ligne in lignes" class="w-full">
                <td class="w-2/7 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center"><input v-model="ligne.NumeroLigne" type="text"
                      :disabled="ligne.disabled" placeholder="N° de Ligne"
                      class="w-19/20 h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white py">
                  </div>
                </td>
                <td class="w-2/7 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center"><input v-model="ligne.Depart" type="text"
                      :disabled="ligne.disabled" placeholder="Depart"
                      class="w-19/20 h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
                  </div>
                </td>
                <td class="w-2/7 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center"><input v-model="ligne.Terminus" type="text"
                      :disabled="ligne.disabled" placeholder="Terminus"
                      class="w-19/20 h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
                  </div>
                </td>
                <td>
                  <div class="w-full flex justify-left"><button
                      :class="{ 'hidden': ligne.added, 'bg-white': !ligne.added }" @click.prevent="addLigne(ligne)"
                      class="w-14/15 h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black m-[20px] mr-[5px] ml-[5px]">
                      Ajouter
                    </button>
                    <button v-if="ligne.added" :class="{ 'hidden': ligne.modif, 'bg-white': !ligne.modif }"
                      @click.prevent="editLigne(ligne)"
                      class="w-14/15 h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black m-[20px] mr-[3px] ml-[5px]"
                      :disabled="modificationInProgress">
                      Modif
                    </button>
                    <button v-if="ligne.modif" @click.prevent="updateLigne(ligne)"
                      class="w-14/15 h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black m-[20px] mr-[3px] ml-[5px]">
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
                <button @click.prevent="newLigneAction()"
                  class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white"
                  :disabled="addInProgress">+</button>
              </th>
            </tr>
          </tfoot>
        </table>
        <p v-if="answer === 2" class="absolute text-red-700">Champ manquant !</p>
        <p v-if="answer === 3" class="absolute text-red-700">La ligne n'existe pas !</p>
        <p v-if="answer === 4" class="absolute text-red-700">La ligne existe déjà !</p>
        <p v-if="answer === 5" class="absolute text-red-700">Erreur lors de l'ajout, réessayez.</p>
        <p v-if="answer === 6" class="absolute text-red-700">Un champ de la ligne est erronée !</p>
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
      oldNumeroLigne: 0,
      modificationInProgress: false,
      addInProgress: false
    }
  },
  setup() {
    // Définition de la structure des lignes du tableau
    const lignes = ref([])
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
      fetch("http://localhost:5107/api/Lignes/GetAllLigne", {
        method: "GET"
      }).then((data) => data.json())
        .then((response) => {
          lignes.value = response.map((item: any) => {
            return {
              NumeroLigne: item.noligne,
              Depart: item.nomarretdepart,
              Terminus: item.nomarretterminus,
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
    const newLigne = () => {
      lignes.value.push({ NumeroLigne: '', Depart: '', Terminus: '', added: false, modif: false, disabled: false });
    };

    // Retourner les données et les méthodes
    return {
      lignes,
      fechtData,
      newLigne
    };
  },
  methods: {
    async addLigne(ligneCourrante: any) {
      let res = await fetch("http://localhost:5107/api/Lignes/AddLigne?noligne=" + ligneCourrante.NumeroLigne + "&nomarretdepart=" + ligneCourrante.Depart + "&nomarretterminus=" + ligneCourrante.Terminus, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        ligneCourrante.added = !ligneCourrante.added;
        ligneCourrante.disabled = !ligneCourrante.disabled;
        this.fechtData()
      }
    },
    async updateLigne(ligneCourrante: any) {
      let res = await fetch("http://localhost:5107/api/Lignes/updateLigne?oldnoligne=" + this.oldNumeroLigne + "&noligne=" + ligneCourrante.NumeroLigne + "&nomarretdepart=" + ligneCourrante.Depart + "&nomarretterminus=" + ligneCourrante.Terminus, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        ligneCourrante.modif = !ligneCourrante.modif;
        ligneCourrante.disabled = !ligneCourrante.disabled;
        this.modificationInProgress = false
        this.addInProgress = false
        this.fechtData()
      }
    },
    editLigne(ligneCourrante: any) {
      ligneCourrante.modif = !ligneCourrante.modif;
      ligneCourrante.disabled = !ligneCourrante.disabled;
      this.oldNumeroLigne = ligneCourrante.NumeroLigne;
      this.modificationInProgress = true;
    },
    newLigneAction() {
      this.addInProgress = true
      this.newLigne()
    }
  }
};
</script>