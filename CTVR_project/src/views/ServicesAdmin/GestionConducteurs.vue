<template>

  <body class="bg-[#A3A3A3] p-20">
    <section>
      <Header>
        <template v-slot:retourAcceuil>
          <Retour to="/servicesAdmin"></Retour>
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
              <th class="w-1/11 p-[10px] border border-hidden">Prénom</th>
              <th class="w-1/11 p-[10px] border border-hidden">Nom</th>
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
                    <input v-model="conducteur.Prenom" type="text" :disabled="conducteur.disabled" placeholder="Prénom"
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.Nom" type="text" :disabled="conducteur.disabled" placeholder="Nom"
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.DateNaissance" type="date" :disabled="conducteur.disabled"
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.NoPermis" type="text" :disabled="conducteur.disabled"
                      placeholder="N° permis (13AA0002)"
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.Adresse" type="text" :disabled="conducteur.disabled"
                      placeholder="Code postal"
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.CodePostal" type="text" :disabled="conducteur.disabled"
                      placeholder="Code postal"
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.Ville" type="text" :disabled="conducteur.disabled" placeholder="Ville"
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.DateOpt" type="date" :disabled="conducteur.disabled" placeholder="..."
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.LieuDeli" type="text" :disabled="conducteur.disabled"
                      placeholder="Code postal"
                      class="w-full h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white m-[20px] ml-[0px] mr-[2px]">
                  </div>
                </td>
                <td class="w-1/11 p-[10px] border border-hidden">
                  <div class="w-full flex justify-center">
                    <input v-model="conducteur.DatePriseFonct" type="date" :disabled="conducteur.disabled"
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
                        class="w-14/15 h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black mr-[3px] ml-[5px]"
                        :disabled="modificationInProgress">
                        Modif
                      </button>
                      <button v-if="conducteur.modif" @click.prevent="updateConducteur(conducteur)"
                        class="w-14/15 h-[30px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black mr-[3px] ml-[5px]">
                        Confirmer
                      </button>
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
                <button @click.prevent="newConducteurAction()"
                  class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white"
                  :disabled="addInProgress">+</button>
              </th>
            </tr>
          </tfoot>
        </table>
        <p v-if="answer === 2" class="absolute text-red-700">Champ manquant !</p>
        <p v-if="answer === 3" class="absolute text-red-700">Le conducteur n'existe pas !</p>
        <p v-if="answer === 4" class="absolute text-red-700">Le conducteur existe déjà !</p>
        <p v-if="answer === 5" class="absolute text-red-700">Erreur lors de l'ajout, réessayez.</p>
        <p v-if="answer === 6" class="absolute text-red-700">Un champ du conducteur est erronée !</p>
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
      oldNoPermis: 0,
      modificationInProgress: false,
      addInProgress: false
    }
  },
  setup() {
    // Définition de la structure des conducteurs du tableau
    const conducteurs = ref([]);
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

    // Initialiser les données dans conducteurs
    const fechtData = () => {
      fetch("http://localhost:5107/api/Conducteurs/getAllconducteur", {
        method: "GET"
      }).then(data => data.json())
        .then((response) => {
          conducteurs.value = response.map((item: any) => {
            return {
              Nom: item.nom,
              Prenom: item.prenom,
              DateNaissance: item.datenaissance,
              NoPermis: item.nopermis,
              Adresse: item.adresse,
              CodePostal: item.codepostal,
              Ville: item.ville,
              DateOpt: item.datepermis,
              LieuDeli: item.lieudelivrance,
              DatePriseFonct: item.dateprisefonction,
              added: true,
              modif: false,
              disabled: true
            }
          });
        });
    };

    onMounted(() => {
      fechtData()
      console.log(conducteurs)
    })

    // Fonction pour ajouter une nouvelle ligne
    const newConducteur = () => {
      conducteurs.value.push({ Nom: '', Prenom: '', DateNaissance: '', NoPermis: '', Adresse: '', CodePostal: '', Ville: '', DateOpt: '', LieuDeli: '', DatePriseFonct: '', added: false, modif: false, disabled: false });
    };

    return {
      conducteurs,
      fechtData,
      newConducteur
    };

  },
  methods: {
    async addConducteur(conducteurCourrant: any) {
      let res = await fetch("http://localhost:5107/api/Conducteurs/addConducteur?nopermis=" + conducteurCourrant.NoPermis + "&datepermis=" + conducteurCourrant.DateOpt + "&lieudelivrance=" + conducteurCourrant.LieuDeli + "&dateprisefonction=" + conducteurCourrant.DatePriseFonct + "&nom=" + conducteurCourrant.Nom + "&prenom=" + conducteurCourrant.Prenom + "&datenaissance=" + conducteurCourrant.DateNaissance + "&adresse=" + conducteurCourrant.Adresse + "&codepostal=" + conducteurCourrant.CodePostal + "&ville=" + conducteurCourrant.Ville, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        conducteurCourrant.added = !conducteurCourrant.added;
        conducteurCourrant.disabled = !conducteurCourrant.disabled;
        this.addInProgress = false
        this.fechtData()
      }
    },
    async updateConducteur(conducteurCourrant: any) {
      let res = await fetch("http://localhost:5107/api/Conducteurs/updateConducteur?oldnopermis=" + this.oldNoPermis + "&nopermis=" + conducteurCourrant.NoPermis + "&datepermis=" + conducteurCourrant.DateOpt + "&lieudelivrance=" + conducteurCourrant.LieuDeli + "&dateprisefonction=" + conducteurCourrant.DatePriseFonct + "&nom=" + conducteurCourrant.Nom + "&prenom=" + conducteurCourrant.Prenom + "&datenaissance=" + conducteurCourrant.DateNaissance + "&adresse=" + conducteurCourrant.Adresse + "&codepostal=" + conducteurCourrant.CodePostal + "&ville=" + conducteurCourrant.Ville, {
        "method": "POST"
      })
      let data = await res.json()
      this.answer = data
      if (data === 1) {
        conducteurCourrant.modif = !conducteurCourrant.modif;
        conducteurCourrant.disabled = !conducteurCourrant.disabled;
        this.modificationInProgress = false
        this.fechtData()
      }
    },
    editConducteur(conducteurCourrant: any) {
      conducteurCourrant.modif = !conducteurCourrant.modif;
      conducteurCourrant.disabled = !conducteurCourrant.disabled;
      this.oldNoPermis = conducteurCourrant.NoPermis;
      this.modificationInProgress = true;
    },
    newConducteurAction() {
      this.addInProgress = true
      this.newConducteur()
    },
  }
};
</script>