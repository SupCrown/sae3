<template>
    <body class="bg-[#A3A3A3] p-20">
        <form class="p-[20px] border rounded-[10px] bg-white w-full">
            <label class="text-black">Informations générales</label>
            <div>
                <label class="text-black m-[20px] mr-[160px]">Date</label>
                <label class="text-black m-[20px] mr-[160px]">Heure</label>
                <label class="text-black m-[20px]">Coordonnées gps</label>
            </div>
            
            <div class="flex items-align ">
                <input type="date" class="bg-[lightgray] border-[3px]-black rounded-[5px] text-black text-center w-[200px] mr-[20px]">
                <input type="time" class="bg-[lightgray] border-[3px]-black rounded-[5px] text-black text-center w-[200px] mr-[20px]">
                <input type="" class="bg-[lightgray] border-[3px]-black rounded-[5px] text-black text-center w-[200px]">
            </div>
           <label class="text-black ">Information de la CTVR</label>
            <section >
                <div>
                    <label  class="text-black  m-[20px] mr-[90px]" >Nom chauffeur</label>
                    <label  class="text-black  m-[20px] mr-[90px]" >Numéro de permis</label>
                    <label  class="text-black  m-[20px] " >Immatriculation bus</label>
                </div>
                <div >
                    <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px] mr-[20px]">
                    <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px] mr-[20px]">
                    <input type="" class="bg-[lightgray] border-[10px]-black-500 rounded-[10px] text-black text-center  w-[200px]">
                </div>
                    
                
                <div>
                    <label  class="text-black mr-[170px] ml-[20px]">Ligne</label>
                    <label  class="text-black">Code intempérie</label> 
                </div>
                <div>
                    <input type="date" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px] mr-[20px] mb-[10px]">
                    <input type="" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px] mr-[10px]">
                </div>
                    <input type="" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center  w-[200px]">
               
            </section> 
            <label class="text-black mb-[10px]">Information des tier impliqués et blessés</label>
            <section>]
                <div class="border rounded-[20px] w-[700px]">
                    <div>
                        <label class="text-black mr-[150px] ml-[30px]">Nom tier</label>
                        <label class="text-black mr-[150px]">Prénom tier</label>
                        <label class="text-black">Âge tier</label>
                    </div><div>
                        <input type="number" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px] ml-[20px] mr-[20px]">
                        <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px] mr-[20px]">
                        <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px]">
                    </div>
                    <div>
                        <label class="text-black ml-[30px]">Adresse tier</label>
                        
                    </div> 
                    <div>
                        <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[400px] mr-[10px] ml-[20px]">
                    </div><div>
                        <label class="text-black ml-[30px]">Nature des blessures</label>
                    </div><div>
                        <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[400px] mr-[10px] ml-[20px] mb-[10px]">
                    </div>
                </div>
                <button class="ml-[30px] mt-[10px] bg-[darkgray] border rounded-[30px] w-[200px] h-[30px] text-black">+ Ajouter un tier</button>
            </section>
            <label class="text-black">Information des tier dont du matériel a été endommagé</label>
            <section>]
                <div class="border rounded-[20px] w-[700px]">
                    <div>
                        <label class="text-black mr-[150px] ml-[30px]">Nom tier</label>
                        <label class="text-black mr-[150px]">Prénom tier</label>
                        <label class="text-black">Âge tier</label>
                    </div><div>
                        <input type="number" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px] ml-[20px] mr-[20px]">
                        <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px] mr-[20px]">
                        <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[200px]">
                    </div>
                    <div>
                        <label class="text-black ml-[30px]">Adresse tier</label>
                        
                    </div> 
                    <div>
                        <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[400px] mr-[10px] ml-[20px]">
                    </div><div>
                        <label class="text-black ml-[30px]">Nature des blessures</label>
                    </div><div>
                        <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[8px] text-black text-center w-[400px] mr-[10px] ml-[20px] mb-[10px]">
                    </div>
                </div>
                <button class="ml-[30px] mt-[10px] bg-[darkgray] border rounded-[30px] w-[200px] h-[30px] text-black">+ Ajouter un tier</button>
            </section>
            <label class="text-black">Dommages CTVR</label>
            <div>
                 <label class="text-black ml-[30px]">Description des dégâts</label>
            </div><div>
                <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[5px] text-black text-center w-[500px] h-[100px] mr-[10px] ml-[20px]"> 
            </div>
            <label class="text-black">Circonstance de l'accident</label>
            <div class="ml-[30px]">
                <label class="text-black">Description des circonstances</label>
            </div><div>
                <input type="text" class="bg-[lightgray] border-[3px]-black rounded-[5px] text-black text-center w-[500px] h-[100px] mr-[10px] ml-[20px]">
            </div>
            <div class="flex flex-row justify-center mt-[20px]">
                <div class="w-[420px] h-[100px] mr-[50px]">
                    <input type="checkbox">
                    <label class="text-black text-sm">Le conducteur certifie sur l'honneur avoir remplis avec honnêteté et des information véridique la déclaration précédente en présence d'un contrôleur.</label>
                </div>
                <div class="w-[450px] h-[100px]">
                    <input type="checkbox">
                <label class="text-black text-sm">La personne en charge du control présente certifie sur l'honneur avoir vérifier et assure de la véridiction des informations indiquées dans la déclaration précédente par le conducteur</label>
                </div>   
            </div>
            <div class="flex justify-center">
                <button type="submit" class="bg-[darkgray] border rounded-[30px] w-[200px] h-[30px] text-black">Envoyer la déclaration</button>
            </div>
            
        </form>
    </body>
    
</template>


<script lang="ts">
export default{
    
}
</script>