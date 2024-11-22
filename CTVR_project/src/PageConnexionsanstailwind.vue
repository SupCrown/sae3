<template>
  <div class="body">
  <div class="rectangledeCO">
  <div class="coteleft">
  <img src="@/assets/logo.jpg" />
  <div class="texte">
  <h2>Connexion</h2>
  </div>
  </div>
  <div class="coteright">
  <form @submit.prevent="onSubmit">
    <div class="input-field">
    <input type="text" name="Login" size="20" placeholder=" " required>
    <label class="label">Login</label>
    </div><div class="input-field">
    <input type="password" name="Motdepasse" size="12" placeholder=" " required>
    <label class="label">Motdepasse</label>
    </div><div class="theButton">
    <button type="submit">Connexion</button>
    </div>
  </form>
  </div>
  </div>
  </div>
</template>

<script>
</script>

<style>
.body {
  display:flex;
  align-items:center;
  justify-content:center;
  min-height: 100vh;
  width: 103%;
  padding: 0 10px;
  background-color:#A3A3A3;
}
.rectangledeCO{
  width:900px;
  height:500px;
  border-radius:10px;
  border-color:white;
  padding:40;
  background-color:#D9F3E4;
  border : 12px solid white;
  display:flex;
  justify-content:center;
}
.coteleft{
  display: flex;
  flex-direction:column;
  justify-content:left;
  width: 462px;
  padding-left:50px;
  padding-top: 80px;
}
img{
  width:100px;
  height:100px;
  padding-left:40px;
}
h2 {
  font-size:250%;
  margin:0;
}
.coteright{
  padding-top:80px;
  display: flex;
  justify-content:center;
  width: 462px;
}
.body a:hover{
  text-decoration:underline;
}
.theButton{
  position:relative;
  margin:20px 0;
  display:flex;
  justify-content:center; 
  padding-top:50px;
}
button{
  background-color:#003E1C;
  font-weight:600;
  width:90%;
  height:50px;
  text-align:center;
  color:white;
  cursor:pointer;
  border-radius:25px;
  font-size:14px;
  border:2px solid #003E1C;
  box-shadow:0 4px 10px gray;
}
.input-field{
  position:relative;
  margin:20px 0;
  display:flex;
  justify-content:center;
}
input:focus + .label,
input:not(:placeholder-shown) + .label{
  position:absolute;
  font-size:14px;
  top:-10px;
  left:20px;
  background-color:#A3A3A3;
  color:white;
  border-radius:30px;
  padding: 0 10px;
}
.input-field .label{
  transition: opacity 0.3s ease, transform 0.3s ease;
  position:absolute;
  top:15px;
  font-size:20px;
  color:#A3A3A3;
  pointer-events: none;
}
.input-field input{
  width:110%;
  height:50px;
  font-size:16px;
  border-radius:25px;
  text-align:center;
  box-shadow:0 4px 10px gray;
}
</style>
