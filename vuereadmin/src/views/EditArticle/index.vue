<template>
  
  <div>
      <v-dialog
        v-model="dialog"
        fullscreen
        transition="dialog-bottom-transition"
        :overlay="false"
        scrollable
      >
      <v-progress-linear :indeterminate="true"></v-progress-linear>
        <v-card tile>
          <v-toolbar card dark color="primary">
            <v-btn icon @click.native="shutdownParentEditDialog" dark>
              <v-icon>close</v-icon>
            </v-btn>
            <v-toolbar-title>NEW ARTICLE</v-toolbar-title>

            <v-container grid-list-md>
              <v-layout row wrap>
                <v-flex xs3 sm3>
                  <v-text-field solo-inverted label="TITLE" v-model="article.title"></v-text-field>
                </v-flex>
                <v-flex xs2 sm2>
                  <x-select @refresh="refreshBlogList"></x-select>
                </v-flex>
                <v-flex xs2 sm3>
                  <v-text-field solo-inverted label="TAG （Split ‘ , ’）" v-model="article.tag"></v-text-field>
                </v-flex>
                <v-flex xs2 sm1>
                  <v-btn color="success" @click="uploadImage=true">上传图片</v-btn>
                </v-flex>
                <v-flex xs3 sm3>
                  <v-text-field solo-inverted label="TOPIC - IMAGEURL" v-model="article.image"></v-text-field>
                </v-flex>
              </v-layout>
            </v-container>

            
            <v-toolbar-items>
              <v-btn dark flat @click.native="save">Save</v-btn>
            </v-toolbar-items>
          </v-toolbar>
          
          <x-markdown :mdValuesP="msg.mdValue" 
                      :fullPageStatusP="false" 
                      :editStatusP="false" 
                      :previewStatusP="false"
                      :navStatusP="true"  
                      :icoStatusP="true"  
                      @childevent="childEventHandler">
           </x-markdown>
        </v-card>
      </v-dialog>
    <x-message :snackbarP="snackbar"
               :message="message"
               :messageType="messageType">
    </x-message>
    <x-upload :showUpload="uploadImage"
              @hideUpload="hideUpload">
    </x-upload>
  </div>
</template>

<script>
import XSelect from "@/components/XSelect/index";
import XMarkdown from "@/components/XMarkdown/index";
import XMessage from "@/components/XMessage/index";
import XUpload from "@/components/QiNiu/index";
export default {
  components: {
    XMarkdown,
    XSelect,
    XMessage,
    XUpload
  },
  props: ["editDialog", "articleId"],
  data() {
    return {
      msg: {
        mdValue: "## Vue-markdownEditor"
      },
      loading: true,
      articleIdd: this.articleId,
      article: {},
      dialog: this.editDialog,
      snackbar: false,
      message: "",
      messageType: "cyan darken-2",
      uploadImage: false
    };
  },
  watch: {
    editDialog: {
      handler() {
        this.dialog = this.editDialog;
      }
    },
    articleId: {
      handler() {
        this.articleIdd = this.articleId;
        if (this.articleIdd > 0) {
          this.getArticleById();
        } else {
          this.article = {};
          this.msg.mdValue = "# xuxuzhaozhao";
        }
      }
    }
  },
  methods: {
    hideUpload() {
      this.uploadImage = false;
    },
    shutdownParentEditDialog() {
      this.dialog = false;
      this.$emit("shutdownEditDialog");
    },
    // 监听事件，接收子组件传过来的数据
    childEventHandler: function(res) {
      // res会传回一个data,包含属性mdValue和htmlValue，具体含义请自行翻译
      this.msg = res;
      this.article.content = res.mdValue;
    },
    refreshBlogList(item) {
      this.article.category = item.value;
    },
    getArticleById() {
      this.$http
        .get(`${this.$domain}/api/Blog/getone?id=` + this.articleIdd)
        .then(res => {
          if (res.data.code === 20000) {
            this.article = res.data.data;
            this.msg.mdValue = res.data.data.content;
          }
        });
    },
    save() {
      if (this.articleIdd > 0) {
        this.$http
          .put(`${this.$domain}/api/Blog/` + this.articleIdd, this.article)
          .then(res => {
            this.snackbar = true;
            this.message = res.data.message;
            if (res.data.code === 20000) {
              this.messageType = "success";
              this.message = res.data.message;
              this.snackbar = true;
              this.shutdownParentEditDialog();
              this.$emit("updateParentTable");
            } else {
              this.messageType = "error";
              this.message = res.data.message;
              this.snackbar = true;
            }
          });
      } else {
        this.$http.post(`${this.$domain}/api/Blog`, this.article).then(res => {
          this.snackbar = true;
          this.message = res.data.message;
          if (res.data.code === 20000) {
            this.messageType = "success";
            this.message = res.data.message;
            this.snackbar = true;
            this.shutdownParentEditDialog();
            this.$emit("updateParentTable");
          } else {
            this.messageType = "error";
            this.message = res.data.message;
            this.snackbar = true;
          }
        });
      }
    }
  }
};
</script>

<style scoped>
.mdContainer {
  height: 93.4% !important;
}

</style>
