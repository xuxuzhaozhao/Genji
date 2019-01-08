
  <template>
  <v-layout row>
    <v-flex xs12 sm6 offset-sm3>
      <v-card>
        <v-card-media
          src="/static/images/header1.png"
          height="400px"
        >
        </v-card-media>
        <v-card-title primary-title>
          <div>
            <div class="headline">你谁啊你？进我后台，想干什么！</div>
          </div>
        </v-card-title>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn style="color:white;" icon @click.native="show = !show">
            <v-icon>{{ show ? 'keyboard_arrow_down' : 'keyboard_arrow_up' }}</v-icon>
          </v-btn>
        </v-card-actions>
        <v-slide-y-transition>
          <v-container v-show="show">
            <v-layout column>
              <v-flex xl6 xs3 sm3>
                <v-text-field solo label="Name" v-model="user.username"></v-text-field>
              </v-flex>
              <v-flex xl6 xs3 sm3>
                <v-text-field type="password" solo-inverted label="Password" v-model="user.password"></v-text-field>
              </v-flex>
              <v-flex xl6 xs3 sm3>
                <v-spacer></v-spacer>
                <v-btn dark color="pink" @click="login">Login</v-btn>
              </v-flex>
            </v-layout>
          </v-container>
        </v-slide-y-transition>
      </v-card>
    </v-flex>
  </v-layout>
</template>

<script>
export default {
  data() {
    return {
      show: false,
      user: {
        username: "",
        password: ""
      }
    };
  },
  methods: {
    login() {
      this.$http.post(`${this.$domain}/api/User/login`, this.user).then(res => {
        if (res.data.code === 20000) {
          this.$store.commit('login', res.data.data);
          let redirect = decodeURIComponent(this.$route.query.redirect || "/");
          this.$router.push({
            path: redirect
          });
        }
      });
    }
  }
};
</script>

<style>

</style>
