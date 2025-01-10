identifient , nom , prenom , mdp , role
<template>

    <body class="bg-[#A3A3A3] p-20">
        <section>
            <HeaderAcceuil>
                Gestion des Utilisateurs
            </HeaderAcceuil>
        </section>
        <form class="p-[20px] h-[700px] border rounded-[10px] bg-white w-full flex flex-col items-center">
            <div class="w-[90%]">
                <table class="w-full">
                    <thead
                        class="w-full h-[50px] text-base text-center shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
                        <tr>
                            <th class="w-1/9 py-[10px] border border-hidden">Identifiant</th>
                            <th class="w-1/9 py-[10px] border border-hidden">Prénom</th>
                            <th class="w-1/9 py-[10px] border border-hidden">Nom</th>
                            <th class="w-1/9 py-[10px] border border-hidden">N° Permis</th>
                            <th class="w-1/9 py-[10px] border border-hidden">Mot de passe</th>
                            <th class="w-1/9 py-[10px] border border-hidden">Rôle</th>
                            <th class="w-1/6 py-[10px] border border-hidden"></th>
                        </tr>
                    </thead>
                </table>
                <div class="w-full h-[500px] overflow-y-auto">
                    <table class="w-full border-collapse border border-hidden">
                        <tbody>
                            <tr v-for="user in utilisateurs" class="w-full">
                                <td class="w-1/6 p-[10px] border border-hidden">
                                    <input v-model="user.Identifiant" type="text" placeholder="Identifiant"
                                        :disabled="user.disabled"
                                        class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
                                </td>
                                <td class="w-1/6 p-[10px] border border-hidden">
                                    <input v-model="user.Prenom" type="text" placeholder="Prénom"
                                        :disabled="user.disabled"
                                        class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
                                </td>
                                <td class="w-1/6 p-[10px] border border-hidden">
                                    <input v-model="user.Nom" type="text" placeholder="Nom" :disabled="user.disabled"
                                        class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
                                </td>
                                <td class="w-1/6 p-[10px] border border-hidden">
                                    <input v-model="user.NoPermis" type="text" placeholder="N° Permis (ssi CHF)"
                                        :disabled="user.disabled || modificationInProgressOnADriver"
                                        class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
                                </td>
                                <td class="w-1/6 p-[10px] border border-hidden">
                                    <input v-model="user.Mdp" type="text" placeholder="Mot de passe"
                                        :disabled="user.disabled"
                                        class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
                                </td>
                                <td class="w-1/6 p-[10px] border border-hidden">
                                    <input v-model="user.Role" type="text" placeholder="Rôle" :disabled="user.disabled"
                                        class="w-full h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white">
                                </td>
                                <td class="p-[10px] border border-hidden">
                                    <div class="flex justify-left">
                                        <button :class="{ 'hidden': user.added, 'bg-white': !user.added }"
                                            @click.prevent="addUser(user)"
                                            class="h-[50px] px-[20px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black">Ajouter</button>
                                        <button v-if="user.added"
                                            :class="{ 'hidden': user.modif, 'bg-white': !user.modif }"
                                            @click.prevent="editUser(user)"
                                            class="h-[50px] px-[20px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-hidden text-black"
                                            :disabled="modificationInProgress">Modif</button>
                                        <button v-if="user.modif" @click.prevent="updateUser(user)"
                                            class="h-[50px] px-[20px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-hidden text-black">Confirmer</button>
                                        <button v-if="user.added"
                                            :class="{ 'hidden': user.modif, 'bg-white': !user.modif }"
                                            @click.prevent="delUser(user)"
                                            class="h-[50px] px-[20px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-hidden text-black bg-white">Suppr</button>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <table class="w-full border-collapse border border-hidden">
                    <tfoot>
                        <tr>
                            <th class="w-full p-[10px]">
                                <button @click.prevent="newUserAction()"
                                    class="w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-white"
                                    :disabled="addInProgess">+</button>
                            </th>
                        </tr>
                    </tfoot>
                </table>
                <p v-if="answer === 2" class="absolute text-red-700">Champ manquant !</p>
                <p v-if="answer === 3" class="absolute text-red-700">L'utilisateur n'existe pas !</p>
                <p v-if="answer === 4" class="absolute text-red-700">L'utilisateur existe déjà !</p>
                <p v-if="answer === 5" class="absolute text-red-700">Erreur lors de l'ajout, réessayez.</p>
                <p v-if="answer === 6" class="absolute text-red-700">Un champ de l'utilisateur est erroné !</p>
            </div>
        </form>
    </body>
</template>

<script lang="ts">
import { onMounted, ref } from 'vue';
import HeaderAcceuil from '@/components/headerAcceuil.vue';
import { useRouter } from 'vue-router';

export default {
    components: {
        HeaderAcceuil
    },
    data() {
        return {
            answer: 0,
            oldId: '',
            modificationInProgress: false,
            addInProgess: false
        };
    },
    setup() {
        const utilisateurs = ref([]);
        const router = useRouter();

        if (localStorage.getItem("role") !== 'ADU') {
            if (localStorage.getItem("role") === 'CHF') {
                router.push('/conducteur')
            }
            else if (localStorage.getItem("role") === 'ADC') {
                router.push('/admin')
            }
            else if (localStorage.getItem("role") === 'SAD') {
                router.push('/servicesAdmin')
            }
            else {
                router.push('/')
            }
        }

        const fetchData = () => {
            fetch("http://localhost:5107/api/Users/GetAllUsers", {
                method: "GET"
            }).then((data) => data.json())
                .then((response) => {
                    utilisateurs.value = response.map((item: any) => {
                        return {
                            Identifiant: item.identifiant,
                            Nom: item.nom,
                            Prenom: item.prenom,
                            NoPermis: item.nopermis,
                            Mdp: item.motDePasse,
                            Role: item.role,
                            added: true,
                            modif: false,
                            disabled: true
                        };
                    });
                });
        };

        onMounted(() => {
            fetchData();
        });

        const newUser = () => {
            utilisateurs.value.push({ Identifiant: '', Nom: '', Prenom: '', NoPermis: '', Mdp: '', Role: '', added: false, modif: false, disabled: false });
        };

        return {
            utilisateurs,
            newUser,
            fetchData,
        };
    },
    methods: {
        async addUser(currentUtilisateurs: any) {
            this.addInProgess = false
            let res = await fetch("http://localhost:5107/api/Users/AddUser?identifiant=" + currentUtilisateurs.Identifiant + "&nom=" + currentUtilisateurs.Nom + "&prenom=" + currentUtilisateurs.Prenom + "&nopermis=" + currentUtilisateurs.NoPermis + "&motDePasse=" + currentUtilisateurs.Mdp + "&role=" + currentUtilisateurs.Role, {
                "method": "POST"
            })
            let data = await res.json()
            this.answer = data
            if (data === 1) {
                currentUtilisateurs.added = true
                currentUtilisateurs.disabled = true
                this.fetchData()
            }
        },
        async updateUser(currentUtilisateurs: any) {
            let API = "http://localhost:5107/api/Users/UpdateUser?oldid=" + this.oldId + "&identifiant=" + currentUtilisateurs.Identifiant + "&nom=" + currentUtilisateurs.Nom + "&prenom=" + currentUtilisateurs.Prenom + "&nopermis=" + currentUtilisateurs.NoPermis + "&motDePasse=" + currentUtilisateurs.Mdp + "&role=" + currentUtilisateurs.Role
            if (currentUtilisateurs.NoPermis == null) {
                API = "http://localhost:5107/api/Users/UpdateUser?oldid=" + this.oldId + "&identifiant=" + currentUtilisateurs.Identifiant + "&nom=" + currentUtilisateurs.Nom + "&prenom=" + currentUtilisateurs.Prenom + "&motDePasse=" + currentUtilisateurs.Mdp + "&role=" + currentUtilisateurs.Role
            }
            let res = await fetch(API, {
                "method": "POST"
            })
            let data = await res.json()
            this.answer = data
            if (data === 1) {
                currentUtilisateurs.modif = false
                currentUtilisateurs.disabled = true
                this.modificationInProgress = false
            }
        },
        editUser(currentUtilisateurs: any) {
            currentUtilisateurs.modif = true
            currentUtilisateurs.disabled = false
            this.modificationInProgress = true
            this.oldId = currentUtilisateurs.Identifiant
        },
        async delUser(currentUtilisateurs: any) {
            let res = await fetch("http://localhost:5107/api/Users/DeleteUser?identifiant=" + currentUtilisateurs.Identifiant, {
                "method": "POST"
            })
            let data = await res.json()
            if (data === 1) {
                this.fetchData()
            }
        },
        newUserAction() {
            this.addInProgess = true
            this.newUser();
        }
    }
};
</script>
