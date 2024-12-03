import './assets/main.css'

import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'

import App from './App.vue'
import Login from './Login.vue'
import DeclarationAccident from './views/DéclarationAccident.vue'
import GestionEntretiens from  './views/GestionEntretient.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {path: '/', name: 'Login', component: Login},
        {path: '/declarationAccident', name: 'DeclarationAccident', component: DeclarationAccident},
        {path: '/gestionEntretiens', name: 'GestionEntretiens', component: GestionEntretiens}
    ]
})

createApp(App)
.use(router)
.mount('#app')
