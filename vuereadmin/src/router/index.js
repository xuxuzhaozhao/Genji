import Vue from 'vue'
import Router from 'vue-router'
import store from '@/utils/store/index'
import * as types from '@/utils/store/types'

import HelloWorld from '@/learn/slot用法/children'
import Layout from '@/components/Layout/index'
import Article from '@/views/Article/index'
import BlogAdmin from '@/views/BlogAdmin/index'
import BlogList from '@/views/BlogList/Layout'
import Login from '@/views/BlogAdmin/Login/index'

const TYTF = () => import('@/views/TYTF/index')

Vue.use(Router)

const router = new Router({
  // mode: 'history',//.netcore 后台用不起。。。。。
  routes: [{
      path: '/',
      name: 'HelloWorld',
      component: HelloWorld
    }, 
    {
      path: '/login',
      name: 'Login',
      component: Login
    },
    {
      path: '/vueadmin',
      name: '后台',
      component: Layout,
      children: [{
        path: '/vueadmin/blog',
        name: 'BlogAdmin',
        component: BlogAdmin
      }],
      meta: {
        requireAuth: true
      }
    },
    {
      path: '/xxzz',
      name: 'bloglist',
      component: BlogList
    },
    {
      path: '/TYTF',
      name: 'TYTF',
      component: TYTF
    },
    {
      path: '/xxzz/article',
      name: 'article',
      component: Article
    }
  ]
})

// 页面刷新时，重新赋值token
if (window.localStorage.getItem('token')) {
  store.commit(types.LOGIN, window.localStorage.getItem('token'))
}

router.beforeEach((to, from, next) => {
  if (to.matched.some(r => r.meta.requireAuth)) {
      if (store.state.token) {
          next();
      }
      else {
          next({
              path: '/login',
              query: {redirect: to.fullPath}
          })
      }
  }
  else {
      next();
  }
})

export default router
