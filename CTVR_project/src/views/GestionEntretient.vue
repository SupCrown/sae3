<template>
    <body class="bg-[#A3A3A3] p-20">
      <form class="p-[20px] h-[700px] border rounded-[10px] bg-white w-full flex justify-center">
        <table class="w-[90%] h-[300px]">
          <thead class="peer w-[100%] h-[50px] text-base text-center shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none text-black bg-[white]">
            <tr>
              <th>Nom</th>
              <th>Date</th>
              <th>Pièce/Quantité</th>
              <th>Etat</th> 
            </tr>
          </thead>
          <tbody>
            <!-- Boucle pour afficher dynamiquement les lignes -->
            <tr v-for="(row, index) in rows" :key="index">
              <td><input v-model="row.name" type="text" placeholder="..." class="peer w-[100%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none text-black bg-[white] m-[20px] ml-[0px] mr-[2px]"></td>
              <td><input v-model="row.date" type="date" class="peer w-[100%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none text-black bg-[white] ml-[3px] "></td>
              <td class="mr-[20px]">
                <div
                  contenteditable="true"
                  :text-content="row.piece"
                  @input="updatePiece($event, index)"
                  class="w-[100%] h-[50px] overflow-scroll whitespace-nowrap border font-sans text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 text-black bg-[white] ml-[5px]"
                ></div>
              </td>
              <td>
                <button 
                  :class="{'bg-green-500 border-green-500': row.started, 'bg-white': !row.started}"
                  @click.prevent="toggleStart(index)" 
                  class="peer w-[70%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none text-black mr-[5px] ml-[25px]"
                >
                  Commencer
                </button>
              </td>
              <td>
                <button 
                  :class="{'bg-red-500 border-red-500': row.finished, 'bg-white': !row.finished}"
                  @click.prevent="delRow(index)"
                  class="peer w-[100%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none text-black mr-[0px] ml-[5px]"
                >
                  Finir
                </button>
              </td>
              <td>
                <button class="peer w-[100%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none text-black mr-[0px] ml-[5px]">Modif</button>
              </td>
            </tr>  
          </tbody>
          <tfoot>
            <tr>
              <th>
                <button @click.prevent="addRow" class="peer w-[250%] h-[50px] text-base text-center rounded-[25px] shadow-[0_4px_10px_gray] border border-gray-300 focus:outline-none text-black bg-[white] p">+</button>
              </th>  
              <td></td>
            </tr>
          </tfoot>
        </table>
      </form>
    </body>
  </template>
  
  <script>
  import { ref } from 'vue';
  
  export default {
    setup() {
      // Définition de la structure des lignes du tableau
      const rows = ref([
        { name: '', date: '', piece: '', started: false, finished: false },
      ]);
  
      // Fonction pour ajouter une nouvelle ligne
      const addRow = () => {
        rows.value = [...rows.value, { name: '', date: '', piece: '', started: false, finished: false }];
      };
  
      // Fonction pour mettre à jour la valeur de "piece" quand l'utilisateur modifie le contenu du div
      const updatePiece = (event, index) => {
        rows.value[index].piece = event.target.innerText;
      };
  
      // Fonction pour toggler l'état "Commencer" (changer la couleur en vert)
      const toggleStart = (index) => {
        rows.value[index].started = !rows.value[index].started;
      };
  
      const delRow = (index) => {
        rows.value.splice(index, 1);
      };


      // Retourner les données et les méthodes
      return {
        rows,
        addRow,
        updatePiece,
        toggleStart,
        delRow
      };
    }
  };
  </script>