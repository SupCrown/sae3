<template>

  <body class="flex items-center justify-top flex-col w-full p-20 bg-[#A3A3A3]">
    <section class="w-full">
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/servicesAdmin/gestionEntretiens"></Retour>
        </template>
        <template v-slot:title>
          Saisie Devis
        </template>
      </Header>
    </section>
    <form @submit.prevent="insertDevis()"
      class="relative w-[900px] h-[500px] rounded-[10px] bg-[#D9F3E4] p-8 border-[12px] border-white">
      <p class="text-black text-center font-bold text-3xl">Intervention n°{{ norep }}</p>
      <div class="flex flex-col items-center gap-4 mb-6">
      </div>
      <div class="flex flex-row mt-[60px]">
        <div class="mb-6 w-[406px]">
          <div class="flex items-center gap-2 mb-4">
            <label class="text-black text-base font-semibold">Pièces</label>
            <button type="button" @click="addPiece"
              class="w-[30px] h-[30px] flex items-center justify-center text-black text-base rounded-[10px] shadow border border-gray-300 bg-white">
              +
            </button>
            <button type="button" @click="delPiece"
              class="w-[30px] h-[30px] flex items-center justify-center text-black text-base rounded-[10px] shadow border border-gray-300 bg-white">
              -
            </button>
          </div>
          <div class="w-full h-[100px] overflow-y-auto">
            <div v-for="piece in pieces" class="flex items-center gap-4 mb-2">
              <select v-model="piece.nom"
                class="w-[150px] h-[30px] text-base text-[gray] text-center rounded-[10px] shadow border border-gray-300 bg-white">
                <option disabled value="">Code pièce</option>
                <option v-for="optionPiece in allPieces" :value="optionPiece.codeF">{{ optionPiece.libelleF }}</option>
              </select>
              <input v-model="piece.qte" type="number" placeholder="Qte"
                class="w-[70px] h-[30px] text-base text-center text-black rounded-[10px] shadow border border-gray-300 bg-white" />
            </div>
          </div>
        </div>

        <div class="mb-6 w-[406px]">
          <div class="flex items-right gap-2 mb-4">
            <label class="text-black text-base font-semibold">Main-d'œuvre</label>
            <button type="button" @click="addMO"
              class="w-[30px] h-[30px] flex items-center justify-center text-black text-base rounded-[10px] shadow border border-gray-300 bg-white">
              +
            </button>
            <button type="button" @click="delMO"
              class="w-[30px] h-[30px] flex items-center justify-center text-black text-base rounded-[10px] shadow border border-gray-300 bg-white">
              -
            </button>
          </div>
          <div class="w-full h-[100px] overflow-y-auto">
            <div v-for="mo in MOs" class="flex items-center gap-4 mb-2">
              <select v-model="mo.type"
                class="w-6/12 h-[30px] text-base text-[gray] text-center rounded-[10px] shadow border border-gray-300 bg-white">
                <option disabled value="">Type main-d'oeuvre</option>
                <option v-for="optionMOE in allMOEs" :value="optionMOE.codeM">{{ optionMOE.libelleM }}</option>
              </select>
              <input v-model="mo.qteHeure" type="number" placeholder="Qte Heure"
                class="w-3/12 h-[30px] text-base text-center text-black rounded-[10px] shadow border border-gray-300 bg-white" />
            </div>
          </div>
        </div>
      </div>
      <div class="flex justify-center">
        <button type="submit"
          class="bg-[#003E1C] w-[150px] mb-[10px] h-[50px] text-white text-center rounded-[25px] shadow border-2 border-[#003E1C]">
          Envoyer
        </button>
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
  data(vm) {
    return {
      norep: 0
    }
  },
  setup() {

    let norep = localStorage.getItem("norep")

    const allPieces = ref([])
    const router = useRouter();

    if (localStorage.getItem("role") !== 'SAD' && localStorage.getItem("role") !== 'TCN') {
      if (localStorage.getItem("role") === 'CHF') {
        router.push('/conducteur')
      }
      else if (localStorage.getItem("role") === 'ADC') {
        router.push('/admin')
      }
      else if (localStorage.getItem("role") === 'ADU') {
        router.push('/gestionUtilisateurs')
      }
      else {
        router.push('/')
      }
    }

    const fetchPieces = () => {
      fetch("http://localhost:5107/api/fournitures/GetAllFourniture", {
        method: "GET"
      }).then(data => data.json())
        .then(reponse => {
          allPieces.value = reponse.map((item: any) => {
            return {
              codeF: item.codeF,
              libelleF: item.libelleF,
              prixUnitaire: item.prixUnitaire
            }
          })
        })
    }

    const allMOEs = ref([])

    const fetchMOEs = () => {
      fetch("http://localhost:5107/api/MOE/GetAllmoe", {
        method: "GET"
      }).then(data => data.json())
        .then(reponse => {
          allMOEs.value = reponse.map((item: any) => {
            return {
              codeM: item.codeM,
              libelleM: item.libelleM,
              tauxHoraire: item.tauxHoraire
            }
          })
        })
    }

    onMounted(() => {
      fetchPieces()
      fetchMOEs()
    })

    const pieces = ref([
      { nom: '', qte: '' },
    ]);

    const MOs = ref([
      { type: '', qteHeure: '' },
    ]);

    const addPiece = () => {
      pieces.value.push({ nom: '', qte: '' });
    };

    const delPiece = (index: any) => {
      if (pieces.value.length > 1) {
        pieces.value.splice(index, 1);
      }
    };

    const addMO = () => {
      MOs.value.push({ type: '', qteHeure: '' });
    };

    const delMO = (mo: any) => {
      if (MOs.value.length > 1) {
        MOs.value.splice(mo, 1);
      }
    };

    return {
      norep,
      pieces,
      MOs,
      allPieces,
      allMOEs,
      addPiece,
      delPiece,
      addMO,
      delMO,
      fetchPieces,
      fetchMOEs
    };
  },
  methods: {
    async insertDevis() {
      for (let piece of this.pieces) {
        await this.insertCodeFournituresDevis(piece)
      }
      for (let mo of this.MOs) {
        await this.insertTypeMainDOeuvreDevis(mo)
      }
      let res = await fetch("http://localhost:5107/api/Maintenance/SetDateDevis?norep=" + localStorage.getItem("norep"), {
        method: "POST"
      })
      let data = await res.json()
      if (data === 1) {
        this.$router.push("/servicesAdmin/gestionEntretiens")
      }
    },
    async insertCodeFournituresDevis(piece: any) {
      let res = await fetch("http://localhost:5107/api/fournitures/useFourniturte?norep=" + localStorage.getItem("norep") + "&codef=" + piece.nom + "&qtedevis=" + piece.qte, {
        method: "POST"
      })
      let data = await res.json()
    },
    async insertTypeMainDOeuvreDevis(mo: any) {
      let res = await fetch("http://localhost:5107/api/MOE/useMOE?norep=" + localStorage.getItem("norep") + "&codem=" + mo.type + "&nbhdevis=" + mo.qteHeure, {
        method: "POST"
      })
      let data = await res.json()
    }
  }
}
</script>
