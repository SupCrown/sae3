<template>

    <body class="flex text-black p-5 bg-[rgb(163,163,163)]">
        <section class="basis-3/4 flex flex-col justify-center pr-4 gap-4">
            <button @click="declareAccident()"
                class="h-1/5 bg-[#ffffff] rounded-md flex items-center justify-center flex-col p-2 hover:bg-[#cccccc]">
                <img src="@/assets/panneauAttention.png" alt="Logo HubSpot" class="w-[100px] h-[100px]" />
                <div> Declarer un accident </div>
            </button>
            <div class="h-4/5 rounded-md bg-white border border-white flex flex-col p-2">
                <h3 class="w-full flex justify-center mb-2">Parcours de la journée</h3>
                <div class="w-full rounded-md bg-[#D9F3E4] border border-white p-4 h-full flex flex-col">
                    <table class="w-full border-collapse border-spacing-0">
                        <thead class="w-full border-b border-gray-400">
                            <tr class="text-left">
                                <th class="w-1/3 px-2 py-1 border-r border-gray-400 text-center">Bus</th>
                                <th class="w-1/3 px-2 py-1 border-r border-gray-400 text-center">Horaires de départ</th>
                                <th class="w-1/3 px-2 py-1 text-center">Lignes</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="ligne in ligneBus" class="border-b border-gray-200">
                                <td class="w-1/3 px-2 py-2">{{ ligne.immat }}</td>
                                <td class="w-1/3 px-2 py-2">
                                    {{ ligne.horaireservice }}
                                </td>
                                <td class="w-1/3 px-2 py-2">Ligne {{ ligne.noligne }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </section>
        <section class="flex flex-col basis-1/4  rounded-md bg-[white] border-white p-4">

            <div class="basis-1/2 rounded-md bg-[white] border-[#D9F3E4] p-5">
                <div class="mb-8 ">
                    <div id="LaBarre" class="bg-white rounded-lg p-5 basis-full flex flex-nowrap place-content-between">
                        <div class="grow-0">
                            <img src="../assets/logo.png" class="w-[55px] h-[55px] mt-2" />
                        </div>
                        <div class="grow-0 basis-4/12 flex justify-end">
                            <button @click="deconnexion()">
                                <img src="@/assets/deconnexion.png" class="w-[50px] h-[50px] mt-1">
                            </button>
                        </div>
                    </div>
                    <div class="flex justify-center ">
                        <img src="../assets/photoProfil-removebg-preview.png"
                            class="w-[275px] h-[275px] ml-[15px] mt-2" />
                    </div>
                    <div v-for="conducteur in conducteurs" :key="conducteur.nopermis"
                        class="flex justify-center place-content-end">
                        <div v-if="conducteur">
                            <p class="text-bold text-2xl">{{ conducteur.conducteur }}</p>
                        </div>
                    </div>
                </div>
            </div>

            <div v-for="conducteur in conducteurs" :key="conducteur.nopermis"
                class="basis-1/2 max-w-sm mx-auto bg-[#D9F3E4] rounded-lg shadow-md p-6">
                <p class="text-center font-semibold text-gray-800 text-lg">Chauffeur</p>
                <hr class="my-3 border-gray-300" />
                <div v-if="conducteur" class="space-y-3">
                    <p class="text-gray-700">
                        <strong>Date de naissance:</strong> {{ conducteur.datenaissance }}
                    </p>
                    <p class="text-gray-700">
                        <strong>Adresse:</strong> {{ conducteur.adresse }}
                    </p>
                    <p class="text-gray-700">
                        <strong>Date de prise de fonction:</strong> {{ conducteur.dateprisefonction }}
                    </p>
                </div>
            </div>
        </section>




    </body>
</template>

<script lang="ts">
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

export default {
    setup() {
        const conducteurs = ref([])
        const router = useRouter();

        if (localStorage.getItem("role") !== 'CHF') {
            if (localStorage.getItem("role") === 'SAD') {
                router.push('/servicesAdmin')
            }
            else if (localStorage.getItem("role") === 'ADC') {
                router.push('/admin')
            }
            else if (localStorage.getItem("role") === 'ADU') {
                router.push('/gestionUtilisateurs')
            }
            else {
                router.push('/')
            }
        }

        const ligneBus = ref([])
        const fechtDataLigne = () => {
            fetch("http://localhost:5107/api/Parcours/getParcoursByNoPermis?nopermis=" + localStorage.getItem("noPermisConnectedUser"), {
                method: "POST"
            }).then((data) => data.json())
                .then((response) => {
                    ligneBus.value = response.map((item: any) => {
                        let rawhoraireservice = item.horaireservice.split('T')
                        return {
                            horaireservice: rawhoraireservice[0] + " " + rawhoraireservice[1],
                            immat: item.immat,
                            noligne: item.noligne
                        }
                    })
                })
        };

        const fechtDataConducteur = () => {
            fetch("http://localhost:5107/api/Conducteurs/getConducteurByNoPermis?nopermis=" + localStorage.getItem("noPermisConnectedUser"), {
                method: "POST"
            }).then((data) => data.json())
                .then((response) => {
                    conducteurs.value = response.map((item: any) => {
                        return {
                            nopermis: item.nopermis,
                            datepermis: item.datepermis,
                            dateprisefonction: item.dateprisefonction,
                            conducteur: item.nom + " " + item.prenom,
                            lieudelivrance: item.lieudelivrance,
                            datenaissance: item.datenaissance,
                            adresse: item.adresse,
                            codepostal: item.codepostal,
                            ville: item.ville
                        }
                    })
                })
        };


        onMounted(() => {
            fechtDataConducteur()
            fechtDataLigne()
        })

        return {
            conducteurs,
            ligneBus,
            fechtDataConducteur,
            fechtDataLigne
        };
    },
    methods: {

        deconnexion() {
            this.$router.push("/")
        },

        declareAccident() {
            this.$router.push("/declarationAccident")
        }

    }
};
</script>
