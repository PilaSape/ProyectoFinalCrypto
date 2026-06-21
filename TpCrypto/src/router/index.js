import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/InicioView.vue'
import PortafolioView from '../views/PortafolioView.vue'
import TransaccionesView from '../views/TransaccionesView.vue'
import VentaCompraView from '../views/VentaCompraView.vue'
import MercadoView from '../views/MercadoView.vue'
import LoginView from '../views/LoginView.vue'
import SaldosView from '../views/SaldosView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/portafolio',
      name: 'portafolio',
      component: PortafolioView
    },
    {
      path: '/transacciones',
      name: 'transacciones',
      component: TransaccionesView
    },
    {
      path: '/ventacompra',
      name: 'ventacompra',
      component: VentaCompraView
    },
    {
      path: '/mercado',
      name: 'mercado',
      component: MercadoView
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/saldos',
      name: 'saldos',
      component: SaldosView
    }
  ],
})

// se ejecuta antes de entrar a cualquier ruta de la app
router.beforeEach(function(to, from, next) 
{
  // chequeo si hay un usuario guardado en localStorage
  const logueado = localStorage.getItem("login")
  
  // si la ruta a la que quiero ir no es el login Y no hay nadie logueado
  if (to.path !== '/login' && !logueado) 
  {
    // lo mando al login
    next('/login')
  } 
  else 
  {
    // si esta logueado, o ya va para el login, lo dejo pasar
    next()
  }
})

export default router
