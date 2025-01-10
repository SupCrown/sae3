<template>

  <body class="bg-[#d8d8d8] p-5 ">
    <section>
      <HeaderAcceuil>Acceuil Admin</HeaderAcceuil>
    </section>
    <div class="flex flex-col">
      <section id="LesBoutons" class="bg-[#f3f3f3] rounded-[25px] ">
        <div>
          <p class="flex justify-center text-center text-black text-xl pt-6 pb-4">Que voulez-vous faire ?</p>
        </div>
        <div class=" gap-6 bg-[#f3f3f3] text-center rounded-[10px] p-10 items-stretch grid grid-cols-3">
          <button name="gererBus" id="gererBus"
            class="bg-[#a0ada6] hover:bg-[#4b5550] font-150 text-center text-black cursor-pointer rounded-[10px] text-xl py-10"
            @click="redirection('/admin/gestionBus')">
            Gérer Bus
          </button>
          <button id="gererConducteur"
            class="bg-[#a0ada6] hover:bg-[#4b5550] font-150 text-center text-black cursor-pointer rounded-[10px] text-xl py-10"
            @click="redirection('/admin/gestionConducteurs')">
            Gérer Conducteur
          </button>
          <button id="gererLignes"
            class="bg-[#a0ada6] hover:bg-[#4b5550] font-150 text-center text-black cursor-pointer rounded-[10px] text-xl py-10"
            @click="redirection('/admin/gestionLignes')">
            Gérer Lignes
          </button>
          <button id="gererParcours"
            class="bg-[#a0ada6] hover:bg-[#4b5550] font-150 text-center text-black cursor-pointer rounded-[10px] text-xl py-10"
            @click="redirection('/admin/gestionParcours')">
            Gérer Parcours
          </button>
          <button id="gererEntretiens"
            class="bg-[#a0ada6] hover:bg-[#4b5550] font-150 text-center text-black cursor-pointer rounded-[10px] text-xl py-10"
            @click="redirection('/admin/gestionEntretiens')">
            Gérer Entretiens
          </button>
          <button id="consulterAccidents"
            class="bg-[#a0ada6] hover:bg-[#4b5550] font-150 text-center text-black cursor-pointer rounded-[10px] text-xl py-10"
            @click="redirection('/admin/consulterAccidents')">
            Consulter Accidents
          </button>

        </div>
      </section>
    </div>
  </body>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import HeaderAcceuil from '@/components/headerAcceuil.vue';
import { useRouter } from 'vue-router';



export default {
  components: {
    HeaderAcceuil,
  },
  setup() {
    const router = useRouter();

    if (localStorage.getItem("role") !== 'ADC') {
      if (localStorage.getItem("role") === 'SAD'){
        router.push('/servicesAdmin')
      }
      else if (localStorage.getItem("role") === 'CHF'){
        router.push('/conducteur')
      }
      else if (localStorage.getItem("role") === 'ADU'){
        router.push('/gestionUtilisateurs')
      }
      else{
        router.push('/')
      }
    }
  },
  methods: {

    redirection(path: string) {
      this.$router.push(path);
    },

  }
};
</script>