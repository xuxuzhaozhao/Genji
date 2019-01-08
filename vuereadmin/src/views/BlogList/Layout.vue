<template>
  <v-app id="inspire">
    <v-navigation-drawer fixed v-model="drawerRight" right clipped app>
      <v-container class="rightContainer">
        <v-layout column>
          <v-flex xs12 sm12 xl12>
            <v-text-field solo-inverted label="ArticleTitle" v-model="articleTitle"></v-text-field>
          </v-flex>
          <v-flex style="margin-left: 60%;">
            <v-btn dark @click="searchTitle">Search</v-btn>
          </v-flex>
        </v-layout>
      </v-container>
      <div style="text-align:center;">----------------------------------------------</div>
      <div class="text-xs-center">
        <a v-for="label in labels">
          <v-chip
            :color="label.color"
            text-color="white"
            label
            @click="transferLabel(label.label)"
          >{{label.label}}</v-chip>
        </a>
      </div>
      <div style="text-align:center;">----------------------------------------------</div>
      <v-card style="margin:9%;margin-top:19%">
        <v-card-media :src="tytf" height="200px"></v-card-media>
        <v-card-title primary-title>
          <h3>黑龙滩旅游</h3>
        </v-card-title>
        <v-spacer></v-spacer>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="learnHLT" flat color="purple">了解哈</v-btn>
        </v-card-actions>
      </v-card>
    </v-navigation-drawer>
    <v-toolbar color="red darken-4" dark fixed app clipped-right>
      <v-toolbar-side-icon @click.stop="drawer = !drawer"></v-toolbar-side-icon>
      <v-layout column>
        <v-container class="selectContainer">
          <v-layout>
            <v-flex xl2>
              <x-select @refresh="sendXContentEvent"></x-select>
            </v-flex>
          </v-layout>
        </v-container>
      </v-layout>
      <a @click="drawerRight = !drawerRight">
        <v-avatar size="40px" class="mr-3">
          <img src="/static/images/header1.png" alt>
        </v-avatar>
      </a>
    </v-toolbar>
    <v-navigation-drawer fixed v-model="drawer" left app>
      <div>
        <img width="300px" src="/static/images/header1.png" alt="xuxuzhaozhao">
        <br>
        <div style="text-align:center">
          <h1 style="font-size:30px;">徐 &nbsp;徐 &nbsp;赵 &nbsp;赵</h1>
          <br>
          <h3 style="font-family:Monaco">CSharp Progarmmer</h3>
          <br>
          <br>

          <div class="funcLinka" style="font-size:18px;">
            <a href="/" class="funcLink">
              <i class="fa fa-home"></i>
              <span>&nbsp;&nbsp;首&nbsp;页</span>
            </a>
            <br>
            <a href="/#/xxzz" class="funcLink">
              <i class="fa fa-file-code-o"></i>
              <span>&nbsp;&nbsp;文&nbsp;章</span>
            </a>
            <br>
            <a href="/Tools/JsonValidate" target="_blank" class="funcLink">
              <i class="fa fa-th-large"></i>
              <span>&nbsp;&nbsp;工&nbsp;具</span>
            </a>
            <br>
            <a href="/#/sendMessageToxzz" target="_blank" class="funcLink">
              <i class="fa fa-inbox"></i>
              <span>&nbsp;&nbsp;留&nbsp;言</span>
            </a>
            <br>
            <br>
            <a href="/#/aboutxxzz" target="_blank" class="funcLink">
              <i class="fa fa-user-circle-o"></i>
              <span>&nbsp;&nbsp;关&nbsp;于&nbsp;我</span>
            </a>
          </div>
          <br>
          <br>
          <div>
            <a class="socialLink google" href="https://www.google.com" target="_blank">
              <i class="fa fa-google fa-2x"></i>
            </a>
            <a class="socialLink github" href="https://github.com/xuxuzhaozhao" target="_blank">
              <i class="fa fa-github fa-2x"></i>
            </a>
            <a class="socialLink weibo" href="https://www.google.com" target="_blank">
              <i class="fa fa-weibo fa-2x"></i>
            </a>
            <a class="socialLink qq" href="https://www.google.com" target="_blank">
              <i class="fa fa-qq fa-2x"></i>
            </a>
          </div>
        </div>
      </div>
    </v-navigation-drawer>
    <v-content>
      <v-progress-linear v-show="showLoading" indeterminate color="black"></v-progress-linear>
      <v-container fluid fill-height style="height: 100%;">
        <v-layout>
          <x-contents
            :show.sync="showLoading"
            :articleTypeId="articleTypeId"
            :articleTitle="sendArticleTitle"
          ></x-contents>
        </v-layout>
      </v-container>
    </v-content>
    <v-footer color="red darken-4" class="white--text" app>
      <span>&nbsp;&nbsp;xuxuzhaozhao</span>
      <v-spacer></v-spacer>
      <span>&copy; 2015 - 2018&nbsp;&nbsp;</span>
    </v-footer>
  </v-app>
</template>

<script>
import EasyScrollbar from "@/components/EasyScrollbar/index";
import XContents from "./Contents";
import XSelect from "@/components/XSelect/index";
export default {
  components: {
    XContents,
    EasyScrollbar,
    XSelect
  },
  data: () => ({
    tytf: "/static/assets/hlt.jpg",
    drawer: false,
    drawerRight: false,
    showLoading: true,
    right: null,
    left: null,
    articleTypeId: 0,
    articleTitle: "",
    sendArticleTitle: "",
    labels: [
      {
        label: "Linux",
        color: "red"
      },
      {
        label: "基础",
        color: "pink"
      },
      {
        label: "汇编",
        color: "cyan darken-3"
      },
      {
        label: "小技巧",
        color: "pink"
      },
      {
        label: "Windows",
        color: "cyan darken-3"
      },
      {
        label: "GO",
        color: "pink"
      }
    ]
  }),
  created() {
    this.$nextTick(() => {
      var x = window.innerHeight;
      var y = window.innerWidth;
      if (navigator.appVersion.indexOf("iPhone") >= 0) {
        document.getElementsByTagName("html")[0].style.overflowY = "scroll";
      }
      if (x > y) {
        //手机
        this.drawer = false;
        this.drawerRight = false;
      } else {
        this.drawer = true;
        this.drawerRight = true;
      }
    });
  },
  props: {
    source: String
  },
  methods: {
    sendXContentEvent(val) {
      this.articleTypeId = val.value;
    },
    searchTitle() {
      this.sendArticleTitle = this.articleTitle;
    },
    transferLabel(label) {
      this.articleTitle = label;
    },
    learnHLT() {
      window.open("/#/TYTF");
    }
  }
};
</script>

<style lang="scss" scoped>
.progress-circular {
  margin: 1rem;
}

.funcLinka {
  a:hover {
    color: red;
  }
}

.funcLink {
  color: black;
  text-decoration: none;
}

.socialLink {
  margin: 9px;
  text-decoration: none;
}

.google {
  color: #2196f3;
}

.github {
  color: #009688;
}

.weibo {
  color: #b71c1c;
}

.qq {
  color: #ffc107;
}
</style>
