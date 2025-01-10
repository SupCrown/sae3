<template>

  <body class="flex items-center justify-top flex-col w-full p-20 bg-[#A3A3A3]">
    <section class="w-full">
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/servicesAdmin/gestionEntretiens"></Retour>
        </template>
        <template v-slot:title>
          Consulter Devis
        </template>
      </Header>
    </section>
    <form v-for="devis in listeDevis"
      class="relative w-[900px] h-[525px] rounded-[10px] bg-[#D9F3E4] p-8 border-[12px] border-white">
      <p class="text-black text-center font-bold text-3xl">Intervention n°{{ devis.norep }}</p>
      <div class="flex flex-row mt-[60px]">
        <div class="mb-6 w-[406px]">
          <div class="flex items-center gap-2 mb-4">
            <label class="text-black text-base font-semibold">Pièces</label>
          </div>
          <div class="w-full overflow-y-auto">
            <div v-for="fourniture in devis.fourniture" class="flex items-center gap-4 mb-2">
              <input v-model="fourniture.code"
                class="w-[150px] h-[30px] text-base text-black text-center rounded-[10px] shadow border border-gray-300 bg-white">
              <input v-model="fourniture.qte" type="text" placeholder="Qte"
                class="w-[70px] h-[30px] text-base text-center text-black rounded-[10px] shadow border border-gray-300 bg-white" />
            </div>
          </div>
        </div>
        <div class="mb-6 w-[406px]">
          <div class="flex items-right gap-2 mb-4">
            <label class="text-black text-base font-semibold">Main-d'œuvre</label>
          </div>
          <div class="w-full overflow-y-auto">
            <div v-for="mainOeuvre in devis.mainoeuvre" class="flex items-center gap-4 mb-2">
              <input v-model="mainOeuvre.code"
                class="w-[150px] h-[30px] text-base text-black text-center rounded-[10px] shadow border border-gray-300 bg-white">
              <input v-model="mainOeuvre.qte" type="text" placeholder="Qte Heure"
                class="w-3/12 h-[30px] text-base text-center text-black rounded-[10px] shadow border border-gray-300 bg-white" />
            </div>
          </div>
        </div>
      </div>
      <div class="w-full shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white rounded-[20px]">
        <table class="w-full">
          <thead class="w-full h-[50px] text-base text-center text-black">
            <tr>
              <th class="w-2/7 p-[10px]  border-b-4 border-gray-300">Total main d'oeuvre</th>
              <th class="w-2/7 p-[10px]  border-b-4 border-gray-300">Total fourniture</th>
              <th class="w-2/7 p-[10px]  border-b-4 border-gray-300">Total TTC</th>
              <th class="w-2/7 p-[10px]  border-b-4 border-gray-300">Tva</th>
            </tr>
          </thead>
        </table>
        <table class="w-full">
          <tbody>
            <tr v-for="infodevis in listeDevis" class="w-full">
              <td class="w-2/7 p-[10px] border border-hidden">
                <div class="w-full flex justify-center">
                  <p>{{ infodevis.totalMoe }}</p>
                </div>
              </td>
              <td class="w-2/7 p-[10px] border border-hidden">
                <div class="w-full flex justify-center">
                  <p>{{ infodevis.totalFourn }}</p>
                </div>
              </td>
              <td class="w-2/7 p-[10px] border border-hidden">
                <div class="w-full flex justify-center">
                  <p>{{ infodevis.prix }}€</p>
                </div>
              </td>
              <td class="w-2/7 p-[10px] border border-hidden">
                <div class="w-full flex justify-center">
                  <p>{{ infodevis.tva }}€</p>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
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
  setup() {
    const listeDevis = ref([])
    const router = useRouter();


    if (localStorage.getItem("role") !== 'SAD' && localStorage.getItem("role") !== 'TCN') {
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
      fetch("http://localhost:5107/api/devis/GetDevisById?norep=" + localStorage.getItem("norep"), {
        method: "GET"
      }).then(data => data.json())
        .then(reponse => {
          listeDevis.value = reponse.map((item: any) => {
            return {
              dateCreation: item.dateCreation,
              fourniture: item.fourniture.map((item: any) => {
                return {
                  code: item.code,
                  qte: item.qte
                }
              }),
              mainoeuvre: item.mainOeuvre.map((item: any) => {
                return {
                  code: item.code,
                  qte: item.qte
                }
              }),
              totalMoe: item.totalMoe,
              totalFourn: item.totalFourn,
              prix: item.prix,
              tva: item.tva,
              norep: item.norep
            }
          })
        });
    };

    onMounted(() => {
      fechtData()
      console.log(listeDevis)
    })

    const row = ref({
      dateentreebus: '',
      nom: '',
    });

    return {
      listeDevis,
      row,
      fechtData
    };
  },
};
</script>
