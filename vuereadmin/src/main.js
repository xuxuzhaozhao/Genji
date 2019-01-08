// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.

import 'vuetify/dist/vuetify.min.css' 
import '../static/css/style.css'
import 'material-design-icons-iconfont/dist/material-design-icons.css'
import Vue from 'vue'
import App from './App'
import router from './router'
import store from './utils/store'

import Vuetify from 'vuetify'
import { Upload } from 'element-ui';

import axios from 'axios'

Vue.config.productionTip = false

Vue.use(Vuetify)
Vue.use(Upload);

Vue.prototype.$http = axios
// Vue.prototype.$domain= 'http://localhost:9817';
Vue.prototype.$domain= 'http://localhost:9817';

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  components: { App },
  template: '<App/>'
})
