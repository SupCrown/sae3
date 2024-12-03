

<template>
    <div name="body" class="flex items-center justify-center min-h-[100vh] w-full p-[10px] bg-[#A3A3A3]">
      <div name="rectangle" class="w-[900px] h-[500px] rounded-[10px] border-white p-[40px] bg-[#D9F3E4] border-[12px] border-white flex justify-center">
        <div name="coteleft" class="flex justify-left w-[462px] pl-[50px] pt-[80px] flex-col">
          <img src="../assets/logo.jpg" class="w-[150px] h-[110px] pl-[10px]" />
          <h2 class="text-[250%] text-black">Connexion</h2>
        </div>
        <div name="coteright" class="pt-[80px] flex justify-center w-[462px]">
          <form @submit.prevent="onSubmit()">
            <!-- Champ Login -->
            <div class="relative m-[20px] flex justify-center">
              <input type="text" name="Login"
                v-model="login"
                placeholder=" "
                ref="login-ref"
                required
                class="peer w-[110%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none text-black"
                @focus="onFocus('login')"
                @blur="onBlur('login')"
              />
              <label
                :class="{
                  'absolute top-[-10px] left-[20px] text-sm bg-gray-400 text-white px-2 rounded-[30px] transition-all': true,
                  'top-[15px] text-lg text-gray-400': !isLoginFocused && !login,
                }"
                class="transition-all duration-300 ease-in-out"
              >
                Login
              </label>
            </div>
  
            <!-- Champ Mot de passe -->
            <div class="relative m-[20px] flex justify-center">
              <div>
                  <input
                  :type="passwordInputType"
                  name="Motdepasse"
                  v-model="password"
                  ref="mdp-ref"
                  placeholder=" "
                  required
                  class="peer w-[100%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none text-black"
                  @focus="onFocus('password')"
                  @blur="onBlur('password')"
                />
                <label
                  :class="{
                    'absolute top-[-10px] left-[20px] text-sm bg-gray-400 text-white px-2 rounded-[30px] transition-all': true,
                    'top-[15px] text-lg text-gray-400': !isPasswordFocused && !password,
                  }"
                  class="transition-all duration-300 ease-in-out"
                >
                  Mot de passe
                </label>
                
                </div>
                <img
                :src="passwordInputType === 'password' ? './src/assets/eye-slash-solid.png' : './src/assets/eye-solid.png'"
                alt="Toggle visibility"
                @click="togglePasswordVisibility"
                class="absolute top-1/2 transform -translate-y-1/2 h-5 cursor-pointer ml-[260px]"
                />
    
              </div>
              
            <!-- Bouton Connexion -->
            <div class="relative m-[50px] flex justify-center pt-[50px]">
              <button
                type="submit"
                class="bg-[#003E1C] font-150 w-[90%] h-[50px] text-center text-white cursor-pointer rounded-[25px] text-x3.5 border-2 border-[#003E1C] shadow-[0_4px_10px_gray]"
                @click="Check()"
              >
                Connexion
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </template>
  
<script lang="ts">

export default {
  data() {
    return {
      login: '', // Stocke la valeur du champ Login
      password: '', // Stocke la valeur du champ Mot de passe
      passwordInputType: 'password', // Contrôle la visibilité du mot de passe
      isLoginFocused: false, // Gère l'état de focus du champ Login
      isPasswordFocused: false, // Gère l'état de focus du champ Mot de passe
    };
  },
  methods: {
    async Check(){
      let res = await fetch("https://localhost:7109/api/Users/Check?identifiant=" + this.login + "&motDePasse=" + this.password, {
        "method": "POST",
      })
      let data = await res.json()
      if (data == false){
        // on afficeh un message indiquant un champ érroné
      } else if (data == 2) {
        // on affiche un message indiquant la nécessiter de remplir les deux champs
      } else {
        this.$router.push("/gestionEntretiens") // changer la page actuelle
      }
    },
    // Méthode de gestion du focus (au clic sur le champ)
    onFocus(field: string) {
      if (field === 'login') {
        this.isLoginFocused = true;
      } else if (field === 'password') {
        this.isPasswordFocused = true;
      }
    },
    // Méthode de gestion du blur (perte de focus)
    onBlur(field: string) {
      if (field === 'login') {
        this.isLoginFocused = false;
      } else if (field === 'password') {
        this.isPasswordFocused = false;
      }
    },
    // Méthode pour basculer la visibilité du mot de passe
    togglePasswordVisibility() {
      this.passwordInputType = this.passwordInputType === 'password' ? 'text' : 'password';
    },
    // Méthode de soumission du formulaire
    onSubmit() {
      console.log('Formulaire soumis avec:', this.login, this.password);
    },
  },
};
</script>