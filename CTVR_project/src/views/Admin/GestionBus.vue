<template>

  <body class="bg-[#A3A3A3] p-20">
    <section>
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/admin"></Retour>
        </template>
        <template v-slot:title>
          Gestion des bus
        </template>
      </Header>
    </section>
    <form class="p-[20px] h-[700px] border rounded-[10px] bg-white w-full flex flex-col items-center">
      <div class="w-[90%]">
        <table class="w-full">
          <thead
            class="w-full h-[50px] text-base text-center shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
            <tr>
              <th class="w-2/5 p-[10px] border border-hidden">Immatriculation</th>
              <th class="w-2/5 p-[10px] border border-hidden">Date de mise en service</th>
              <th></th>
            </tr>
          </thead>
        </table>
        <div class="w-full h-[500px] overflow-y-auto">
          <table class="w-full">
            <tbody>
              <tr v-for="row in Bus" class="w-full">
                <td class="w-2/5 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="row.Immatriculation" type="text" placeholder="AA-000-AA" :disabled="row.disabled"
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-2/5 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="row.DateMiseService" type="date" placeholder="..." :disabled="row.disabled"
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[5px] mr-[2px]">
                  </div>
                </td>
                <td>
                  <div class="w-full flex justify-left">
                    <button :class="{ 'hidden': row.added, 'bg-white': !row.added }" @click.prevent="addBus(row)"
                      class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black mr-[5px] ml-[5px]">
                      Ajouter
                    </button>
                    <button v-if="row.added" :class="{ 'hidden': row.modif, 'bg-white': !row.modif }"
                      @click.prevent="delBus(row)"
                      class="w-[20%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[10px]">Suppr</button>
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
                <button @click.prevent="newBusAction()"
                  class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white"
                  :disabled="addInProgress">+</button>
              </th>
            </tr>
          </tfoot>
        </table>
        <p v-if="answer === 2" class="absolute text-red-700">Champ manquant !</p>
        <p v-if="answer === 3" class="absolute text-red-700">Le Bus n'existe pas !</p>
        <p v-if="answer === 4" class="absolute text-red-700">Le Bus existe déjà !</p>
        <p v-if="answer === 5" class="absolute text-red-700">Erreur lors de l'ajout, réessayez.</p>
        <p v-if="answer === 6" class="absolute text-red-700">Un champ du est erronée !</p>
        <p v-if="answer === 7" class="absolute text-red-700">Erreur vous voulez supprimer un bus utiliser sur une autre
          page
          !</p>
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
      oldNumeroBus: 0,
      addInProgress: false
    }
  },
  setup() {
    // Définition de la structure des buss du tableau
    const Bus = ref([])

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

    const fechtData = () => {
      fetch("http://localhost:5107/api/Bus/GetAllbus", {
        method: "GET"
      }).then((data) => data.json())
        .then((response) => {
          Bus.value = response.map((item: any) => {
            return {
              Immatriculation: item.immat,
              DateMiseService: item.miseenservice,
              added: true,
              disabled: true
            }
          })
        })
    };

    onMounted(() => {
      fechtData()
    })

    const newBus = () => {
      Bus.value.push({ Immatriculation: '', datemiseservice: '', added: false, disabled: false });
    };
    return {
      Bus,
      newBus,
      fechtData
    };

  },
  methods: {
    async addBus(busCourrant: any) {
      let res = await fetch("http://localhost:5107/api/Bus/Addbus?immat=" + busCourrant.Immatriculation + "&miseenservice=" + busCourrant.DateMiseService, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        busCourrant.added = !busCourrant.added;
        busCourrant.disabled = !busCourrant.disabled;
        this.addInProgress = false
        this.fechtData()
      }
    },
    async delBus(busCourrant: any) {
      let res = await fetch("http://localhost:5107/api/Bus/Deletebus?immat=" + busCourrant.Immatriculation, {
        "method": "POST"
      })
      let data = await res.json()
      if (data === 1) {
        this.fechtData()
      }
    },
    newBusAction() {
      this.addInProgress = true
      this.newBus();
    }
  }
};
</script>