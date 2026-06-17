<template>
  <div class="min-vh-100 d-flex justify-content-center align-items-center">
    <table class="table text-center" style="width:auto; background-color: lightsteelblue;">
      <tr>
        <td>Ingresar al sistema</td>
      </tr>
      <tr>
        <td><input type="text" v-model="usuario" placeholder="Usuario" class="form-control"></td>
      </tr>
      <tr>
        <td><input type="password" v-model="password" placeholder="Contraseña" class="form-control"></td>
      </tr>
      <tr>
        <td>
          <div v-if="error" class="text-danger">{{ error }}</div>
          <button class="btn btn-primary mt-2" @click="login">Ingresar</button>
        </td>
      </tr>
    </table>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const usuario = ref("")
const password = ref("")
const error = ref("")
const router = useRouter()

const USUARIOS = [
  { usuario: "admin", password: "admin123", rol: "admin" },
  { usuario: "user", password: "pwd", rol: "usuario" }
]

function login() {
  const encontrado = USUARIOS.find(function(u) { return u.usuario === usuario.value && u.password === password.value })
  if (encontrado) {
    localStorage.setItem("login", JSON.stringify(encontrado))
    router.push("/")
  } else {
    error.value = "Usuario o contraseña incorrectos."
  }
}
</script>