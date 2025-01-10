<template>
  <section class="mb-8">
    <div id="LaBarre"class="bg-white rounded-lg p-5 basis-full flex flex-nowrap">
      <div class="grow-0">
        <img src="../assets/logo.png" class="w-[50px] h-[50px] ml-[15px] mt-2"/>
      </div>
      <div class="grow flex justify-center content-center mt-4">
        <span class="text-black text-xl"><slot></slot></span>
      </div>
      <div class="grow-0 flex justify-end">
        <button @click="deconnexion()">
          <img src="@/assets/deconnexion.png" class="w-[50px] h-[50px] mt-1">
        </button>
      </div>
    </div>
</section>
</template>



<script lang="ts">
import { computed, defineComponent } from 'vue';

export default defineComponent({
  name: 'template',
  

methods : {

  deconnexion (){
    localStorage.removeItem("role")
    this.$router.push("/")
  },

}
});
</script>