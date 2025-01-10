<template>
  <button class="rounded-lg border border-gray-300 bg-gray-300 text-black flex flex-row items-center text-xl pr-5 hover:bg-gray-600" @click="navigate">
    <img src="@/assets/flechGauche-removebg-preview.png" class="w-[40px] h-[40px] mt-1"> 
    <div class="ml-2"> Retour </div>
  </button>
</template>

<script lang="ts">
import { defineComponent, type PropType } from 'vue';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: 'Button',

  methods : {

    retourAcceuilAdmin (){
      this.$router.push("/admin")
    },
  },

  props: {
    to: {
      type: String,
      required: true, // Destination de la navigation
    },
  },

  setup(props) {
    const router = useRouter();

    const navigate = () => {
      router.push(props.to); // Navigue vers la route spécifiée
    };

    return {
      navigate,
    };
  },
});
</script>