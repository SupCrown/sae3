<template>
  <div class="flex items-center justify-center min-h-[100px] w-[103px] p-[10px] bg-[#grayCustom]">
    <div class="w-[900px] h-[500px] rounded-[10px] border-white p-[40px] bg-[#greenCustom] border-[3px] flex justify-center">
      <div class="flex justify-start w-[462px] pl-[50px] pt-[80px]">
        <img src="@/assets/logo.jpg" class="w-[100px] h-[100px] pl-[40px]" />
        <h2 class="text-2xl font-bold pl-[20px]">Connexion</h2>
      </div>
      <div class="pt-[80px] flex justify-center w-[462px]">
        <form @submit.prevent="onSubmit">
          <div class="relative m-[20px] flex justify-center">
            <input type="text" name="Login" size="20" placeholder=" " require class="peer w-[110%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none"/>
            <label
              class="absolute top-[15px] left-[20px] text-lg text-gray-400 bg-transparent pointer-events-none transition-all duration-300 ease-in-out peer-placeholder-shown:top-[15px] peer-placeholder-shown:text-lg peer-placeholder-shown:text-gray-400 peer-focus:top-[-10px] peer-focus:left-[20px] peer-focus:text-sm peer-focus:bg-gray-400 peer-focus:text-white peer-focus:rounded-[30px] peer-focus:px-2"
            >
              Login
            </label>
          </div>
          <div class="relative m-[20px] flex justify-center">
            <input
              type="password"
              name="Motdepasse"
              size="12"
              placeholder=" "
              required
              class="peer w-[110%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none"
            />
            <label
              class="absolute top-[15px] left-[20px] text-lg text-gray-400 bg-transparent pointer-events-none transition-all duration-300 ease-in-out peer-placeholder-shown:top-[15px] peer-placeholder-shown:text-lg peer-placeholder-shown:text-gray-400 peer-focus:top-[-10px] peer-focus:left-[20px] peer-focus:text-sm peer-focus:bg-gray-400 peer-focus:text-white peer-focus:rounded-[30px] peer-focus:px-2"
            >
              Motdepasse
            </label>
          </div>
          <div class="relative m-[50px] flex justify-center pt-[50px]">
            <button
              type="submit"
              class="bg-[#greenCustom2] font-bold w-[90%] h-[50px] text-center text-white rounded-[25px] text-xl border-2 border-[#greenCustom2] shadow-[0_4px_10px_gray]"
            >
              Connexion
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import './assets/tailwind.css';
import { createApp } from 'vue';
import App from './App.vue';

createApp(App).mount('#app');
</script>

