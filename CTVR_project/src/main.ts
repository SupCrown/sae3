import './assets/main.css'

import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'

import App from '@/App.vue'
import Login from '@/views/Login.vue'
import DeclarationAccident from '@/views/DeclarationAccident.vue'
import AcceuilServicesAdmin from '@/views/AcceuilServicesAdmin.vue'
import GestionEntretiens from  '@/views/ServicesAdmin/GestionEntretient.vue'
import GestionParcours from '@/views/ServicesAdmin/GestionParcours.vue'
import GestionLignes from '@/views/ServicesAdmin/GestionLignes.vue'
import GestionBus from '@/views/ServicesAdmin/GestionBus.vue'
import ConsulterAccidents from '@/views/ServicesAdmin/ConsulterAccidents.vue'
import ConsulterAccidentUnique from '@/views/ServicesAdmin/ConsulterAccidentUnique.vue'
import GestionConducteurs from '@/views/ServicesAdmin/GestionConducteurs.vue'
import ConsulterDevis from '@/views/ServicesAdmin/ConsulterDevis.vue'
import AcceuilAdmin from '@/views/AcceuilAdmin.vue'
import GestionConducteursAd from '@/views/Admin/GestionConducteurs.vue'
import GestionEntretiensAd from '@/views/Admin/GestionEntretiens.vue'
import GestionParcoursAd from '@/views/Admin/GestionParcours.vue'
import GestionLignesAd from '@/views/Admin/GestionLignes.vue'
import GestionBusAd from '@/views/Admin/GestionBus.vue'
import ConsulterAccidentsAd from '@/views/Admin/ConsulterAccidents.vue'
import ConsulterAccidentUniqueAd from '@/views/Admin/ConsulterAccidentUnique.vue'
import SaisieDevis from '@/views/ServicesAdmin/SaisieDevis.vue'
import AcceuilConducteur from '@/views/AcceuilConducteur.vue'
import GestionUtilisateurs from './views/Admin/GestionUtilisateurs.vue'


const router = createRouter({
    history: createWebHistory(),
    routes: [
        {path: '/', name: 'Login', component: Login},
        {path: '/declarationAccident', name: 'DeclarationAccident', component: DeclarationAccident},
        {path: '/servicesAdmin', name:'AcceuilServicesAdmin', component: AcceuilServicesAdmin},
        {path: '/servicesAdmin/gestionEntretiens', name: 'GestionEntretiens', component: GestionEntretiens},
        {path: '/servicesAdmin/gestionParcours', name: 'GestionParcours', component: GestionParcours},
        {path: '/servicesAdmin/gestionLignes', name: 'GestionLignes', component: GestionLignes},
        {path: '/servicesAdmin/gestionBus', name: 'GestionBus', component: GestionBus},
        {path: '/servicesAdmin/consulterAccidents', name: 'ConsulterAccidents', component: ConsulterAccidents},
        {path: '/servicesAdmin/consulterAccidentUnique', name: 'ConsulterAccidentUnique', component: ConsulterAccidentUnique},
        {path: '/servicesAdmin/gestionConducteurs', name: 'GestionConducteur', component: GestionConducteurs},
        {path: '/servicesAdmin/consulterDevis', name: 'ConsulterDevis', component: ConsulterDevis},
        {path: '/servicesAdmin//saisieDevis', name:'SaisieDevis', component: SaisieDevis},
        {path: '/admin', name: 'AcceuilAdmin', component: AcceuilAdmin},
        {path: '/admin/gestionConducteurs', name: 'GestionConducteurAd', component: GestionConducteursAd},
        {path: '/admin/gestionEntretiens', name: 'GestionEntretiensAd', component: GestionEntretiensAd},
        {path: '/admin/gestionParcours', name: 'GestionParcoursAd', component: GestionParcoursAd},
        {path: '/admin/gestionLignes', name: 'GestionLignesAd', component: GestionLignesAd},
        {path: '/admin/gestionBus', name: 'GestionBusAd', component: GestionBusAd},
        {path: '/admin/consulterAccidents', name:'ConsulterAccidentsAd', component: ConsulterAccidentsAd},
        {path: '/admin/consulterAccidentUnique', name:'ConsulterAccidentUniqueAd', component: ConsulterAccidentUniqueAd},
        {path: '/admin/gestionUtilisateurs', name: 'GestionUtilisateurs', component: GestionUtilisateurs},
        {path: '/conducteur', name: 'AcceuilConducteur', component: AcceuilConducteur},
        
    ]
})

createApp(App)
.use(router)
.mount('#app')
