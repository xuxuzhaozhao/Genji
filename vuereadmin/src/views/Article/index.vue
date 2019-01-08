<template>
  <div class="xarticle">
    <v-progress-linear v-show="showLoading" indeterminate color="black"></v-progress-linear>
    <v-container fluid>
      <v-layout>
        <v-flex xs0 xl3></v-flex>
        <v-flex xs12 xl6>
          <v-fab-transition>
            <x-markdown
              :mdValuesP="mdValue"
              :fullPageStatusP="false"
              :editStatusP="false"
              :previewStatusP="true"
              :navStatusP="false"
              :icoStatusP="false"
            ></x-markdown>
          </v-fab-transition>
        </v-flex>
        <v-flex xs0 xl3>
          <v-card>
            <v-fab-transition>
              <v-speed-dial fab bottom right fixed :style="addMargin">
                <v-btn slot="activator" color="blue darken-2" light fab hover>
                  <v-icon style="line-height: 2.2">add</v-icon>
                  <v-icon style="line-height: 2.2">close</v-icon>
                </v-btn>
                <v-btn fab small color="green" @click="advice">
                  <v-icon style="line-height: 2;">edit</v-icon>
                </v-btn>
                <v-btn fab small color="red" @click="likeArticle">
                  <v-icon style="line-height: 2;">thumb_up</v-icon>
                </v-btn>
              </v-speed-dial>
            </v-fab-transition>
            <v-fab-transition>
              <v-btn
                floating
                fab
                bottom
                right
                fixed
                color="pink"
                v-show="show"
                class="upBtn"
                @click="returnTop"
              >
                <v-icon style="line-height: 2.2">keyboard_arrow_up</v-icon>
              </v-btn>
            </v-fab-transition>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
    <!-- 通知框 -->
    <x-message
      :snackbarP="xmessage.snackbar"
      :message="xmessage.message"
      :messageType="xmessage.messageType"
    ></x-message>
    <!-- 留言框 -->
    <v-dialog
      :fullscreen="fullscreen"
      class="adviceModal"
      v-model="showDialog"
      max-width="500px"
      transition="dialog-bottom-transition"
    >
      <v-card>
        <v-card-title>
          <span class="headline">留言 ლ(╹◡╹ლ)</span>
        </v-card-title>
        <v-card-text>
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12>
                <v-text-field
                  v-model="adviceMessage.Email"
                  label="您的邮箱"
                  required
                  :rules="[rules.required, rules.email]"
                ></v-text-field>
              </v-flex>
              <v-flex xs12>
                <v-text-field v-model="adviceMessage.NickName" label="您的昵称" required></v-text-field>
              </v-flex>
              <v-flex xs12>
                <v-text-field v-model="adviceMessage.Advice" multi-line label="留言" required></v-text-field>
              </v-flex>
            </v-layout>
          </v-container>
          <small style="color:red">*文章有不明白的地方，可以留言给我，我尽量帮你解答。(゜-゜)つロ 干杯~</small>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" flat @click.native="showDialog = false">Close</v-btn>
          <v-btn color="blue darken-1" flat @click.native="adviceSend">Send</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import XMarkdown from "@/components/XMarkdown/index";
import EasyScrollbar from "@/components/EasyScrollbar/index";
import XMessage from "@/components/XMessage/index";
export default {
  components: {
    XMarkdown,
    EasyScrollbar,
    XMessage
  },
  data() {
    return {
      xmessage: {
        snackbar: false,
        message: "",
        messageType: "success"
      },
      showLoading: true,
      fullscreen: false,
      showDialog: false,
      addMargin: 0,
      show: false,
      mdValue: "",
      articleId: this.$route.query.id,
      adviceMessage: {
        ArticleId: this.$route.query.id,
        Email: "",
        NickName: "",
        Advice: ""
      },
      rules: {
        required: value => !!value || "Required.",
        email: value => {
          const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
          return pattern.test(value) || "你的 Email 格式不合法哦!!.";
        }
      }
    };
  },
  created() {
    document.getElementsByTagName("html")[0].style.overflowY = "scroll";
    this.read();
  },
  mounted() {
    window.addEventListener("scroll", this.showReturnTopBtn, { passive: true });
  },
  methods: {
    read() {
      if (
        this.articleId === null ||
        this.articleId === "" ||
        this.articleId <= 0
      ) {
        this.mdValue = "# 无此文章";
      }
      this.$http
        .get(`${this.$domain}/api/Blog/getone?id=` + this.articleId)
        .then(res => {
          if (res.data.code === 20000) {
            this.showLoading = false;
            this.mdValue = res.data.data.content;
          }
        });
    },
    likeArticle() {
      this.$http
        .get(`${this.$domain}/api/Blog/addLikeCount?id=` + this.articleId)
        .then(res => {
          if (res.data.code === 20000) {
            this.xmessage.messageType = "success";
            this.xmessage.message = "感谢您的赞！(゜-゜)つロ 干杯~";
            this.xmessage.snackbar = true;
          }
        });
    },
    advice() {
      this.showDialog = true;
      this.xmessage.snackbar = false;
    },
    adviceSend() {
      const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      if (pattern.test(this.adviceMessage.Email)) {
        this.showDialog = false;
        this.$http
          .post(`${this.$domain}/api/Blog/advice`, this.adviceMessage)
          .then(res => {
            if (res.data.code === 20000) {
              this.xmessage.messageType = "success";
              this.xmessage.message = "感谢您的留言！(゜-゜)つロ 干杯~";
              this.xmessage.snackbar = true;
            }
          });
      } else {
        this.xmessage.messageType = "error";
        this.xmessage.message = "邮箱格式不合法！";
        this.xmessage.snackbar = true;
      }
    },
    returnTop() {
      //document.documentElement.scrollTo(0, 0);
      //window.scrollTo({ behavior: "smooth", top: target.offsetTop });
      this.scrollToTop(300); //300ms滑动到顶
      this.show = false;
      this.addMargin = { marginRight: "0px" };
    },
    showReturnTopBtn(e) {
      if (document.documentElement.scrollTop > 0) {
        this.show = true;
        this.addMargin = { marginRight: "70px" };
      } else {
        this.show = false;
        this.addMargin = { marginRight: "0px" };
      }
    },
    scrollToTop(scrollDuration) {
      var scrollStep = -window.scrollY / (scrollDuration / 15),
        scrollInterval = setInterval(function() {
          if (window.scrollY != 0) {
            window.scrollBy(0, scrollStep);
          } else clearInterval(scrollInterval);
        }, 15);
    }
  }
};
</script>

<style lang="scss">
.card {
  background: white;
}
/* 设置滚动条的样式 */
::-webkit-scrollbar {
  width: 8px;
}
/* 滚动槽 */
::-webkit-scrollbar-track {
  -webkit-box-shadow: inset006pxrgba(0, 0, 0, 0.3);
  border-radius: 5px;
}
/* 滚动条滑块 */
::-webkit-scrollbar-thumb {
  border-radius: 5px;
  background: rgba(0, 0, 0, 0.1);
  -webkit-box-shadow: inset006pxrgba(0, 0, 0, 0.5);
}
::-webkit-scrollbar-thumb:window-inactive {
  background: rgba(255, 0, 0, 0.4);
}
// .xarticle {
//   @media screen and (max-device-width: 1380px) {
//     .adviceModal {
//       .dialog {
//         max-width: 482px !important;
//         margin-right: -72.5% !important;
//       }
//     }
//   }
//   @media screen and (max-device-width: 768px) {
//     .adviceModal {
//       .dialog {
//         max-width: 100% !important;
//         margin-right: 5% !important;
//         background: white;
//       }
//     }
//   }
// }
</style>
