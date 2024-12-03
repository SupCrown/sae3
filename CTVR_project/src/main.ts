import './assets/main.css'

import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'

//import App from './App.vue'
import Login from './Login.vue'
import DéclarationAccident from './views/DéclarationAccident.vue'
import gestionEntretien from './views/GestionEntretient.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {path: '/', name: 'Login', component: Login},
        {path: '/declaration', name: 'Declaration', component: DéclarationAccident}

    ]
})

createApp(gestionEntretien)
.use(router)
.mount('#app')
