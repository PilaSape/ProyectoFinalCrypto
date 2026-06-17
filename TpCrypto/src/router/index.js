import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/InicioView.vue'
import PortafolioView from '../views/PortafolioView.vue'
import TransaccionesView from '../views/TransaccionesView.vue'
import VentaCompraView from '../views/VentaCompraView.vue'
import MercadoView from '../views/MercadoView.vue'
import LoginView from '../views/LoginView.vue'

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
    }
  ],
})

export default router
