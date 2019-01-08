<template>
    <v-dialog v-model="dialog"
              fullscreen
              transition="dialog-bottom-transition"
              :overlay="false"
              scrollable>
        <v-card tile>
            <v-toolbar card dark color="primary">
                <v-btn icon @click.native="menuHide" dark>
                    <v-icon>close</v-icon>
                </v-btn>
            </v-toolbar>
            <v-card-text>
                <vue-preview style="display:flex" :slides="slide1"></vue-preview>
            </v-card-text>
            <v-card-text>
                <vue-preview :slides="slide2"></vue-preview>
            </v-card-text>
            <div style="flex: 1 1 auto;" />
        </v-card>
    </v-dialog>
</template>

<script>
import Vue from "vue";
import VuePreview from "vue-preview";

// defalut install
Vue.use(VuePreview);

export default {
  props: ["menuShow", "isMenu"],
  created() {
    this.getExploreMenu(true);
  },
  data() {
    return {
      dialog: false,
      slide1: [],
      slide2: []
    };
  },
  watch: {
    menuShow: {
      handler() {
        this.dialog = this.menuShow;
      }
    },
    isMenu: {
      handler() {
        if (this.isMenu) {
          this.getExploreMenu(true);
        } else {
          this.getExploreMenu(false);
        }
      }
    }
  },
  methods: {
    menuHide() {
      this.dialog = false;
      this.$emit("menuHide");
    },
    getExploreMenu(val) {
      this.$http
        .get(`${this.$domain}/api/TYTF/explores?isMenu=${val}`)
        .then(res => {
          const data = res.data.data;
          this.slide1 = data.slice(0, 5);
          this.slide2 = data.slice(5, 10);
        });
    }
  }
};
</script>

<style>
figure {
  margin: 1em 3px;
}
.my-gallery {
  display: flex;
  margin-top: 5%;
  margin-left: 2%;
}
.my-gallery a {
  margin: 15px;
}
</style>
