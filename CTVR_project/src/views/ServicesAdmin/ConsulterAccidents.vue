<template>

  <body class="bg-[#A3A3A3] p-20">
    <section>
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/servicesAdmin"></Retour>
        </template>
        <template v-slot:title>
          Consultations des accidents
        </template>
      </Header>
    </section>
    <form class="p-[20px] h-[700px] border rounded-[10px] bg-white w-full flex flex-col items-center">
      <div class="w-[90%]">
        <table class="w-full">
          <thead
            class="w-full h-[50px] text-base text-center shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
            <tr>
              <th class="w-1/5">Date</th>
              <th class="w-1/5">Lieu</th>
              <th class="w-1/5">Conducteur</th>
              <th class="w-1/5">Etat</th>
              <th class="w-1/5">Action</th>
            </tr>
          </thead>
        </table>
        <div class="w-full h-[500px] overflow-y-auto flex justify-start flex-col items-center px-2">
          <table class="w-full">
            <tbody>
              <tr v-for="(row, index) in rows" :key="(row.annee)" class="w-full">
                <td class="w-1/5"><input v-model="row.dateaccident" type="text" placeholder="..."
                    class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                </td>
                <td class="w-1/5"><input v-model="row.addresse" type="text" placeholder="..."
                    class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                </td>
                <td class="w-1/5"><input v-model="row.conducteur" type="text" placeholder="..."
                    class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                </td>
                <td class="w-1/5"><input v-model="row.etatdossieraccident" type="text" placeholder="..."
                    class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                </td>
                <td>
                  <button @click.prevent="consulterAccident(index)"
                    class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black m-[20px] ml-[0px] mr-[2px]">
                    Consulter
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </form>
  </body>
</template>

<script lang="ts">
import { ref, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
import Header from '@/components/header.vue';
import Retour from '@/components/Retour.vue';

export default {
  components: {
    Header,
    Retour
  },
  setup() {
    // Définition de la structure des lignes du tableau
    const rows = ref([]);
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


    // Initialiser les données dans rows
    onMounted(() => {
      fetch("http://localhost:5107/api/Accidents/GetAllAccidents", {
        method: "GET"
      }).then(data => data.json())
        .then(entretiens => {
          rows.value = entretiens.map(item => ({
            annee: item.annee,
            noseq: item.noseq,
            dateaccident: item.dateaccident,
            heureaccident: item.heureaccident,
            addresse: item.rue + ", " + item.codepostal + " " + item.ville,
            conducteur: item.prenom + " " + item.nom,
            nopermis: item.nopermis,
            horaireservice: item.horaireservice,
            etatdossieraccident: item.etatdossieraccident
          }));
        });
    });

    const consulterAccident = (index) => {
      localStorage.setItem("annee", rows.value[index].annee)
      localStorage.setItem("noseq", rows.value[index].noseq)
      localStorage.setItem("nopermis", rows.value[index].nopermis)
      localStorage.setItem("horaireservice", rows.value[index].horaireservice)
      router.push('consulterAccidentUnique')
    }

    // Retourner les données et les méthodes
    return {
      rows,
      consulterAccident
    }
  },
  methods: {
    visualizeAccident() {
      this.$router.push("/servicesAdmin/consulterAccidentUnique")
    }
  }
}
</script>