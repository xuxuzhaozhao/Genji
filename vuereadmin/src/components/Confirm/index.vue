<template>
  <div>
    <v-layout row justify-center>
      <v-dialog v-model="show" persistent max-width="290">
        <v-card>
          <v-card-title class="headline">WARNING</v-card-title>
          <v-card-text>
            <h3>Are sure <b style="color:red;">{{actionType}}</b> this article?</h3>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="green darken-1" flat @click.native="notExecuteAction">NO</v-btn>
            <v-btn color="green darken-1" flat @click.native="executeAction">YES</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-layout>
    <x-message :snackbarP="snackbar"
               :message="message"
               :messageType="messageType">
    </x-message>
  </div>
</template>

<script>
import XMessage from "@/components/XMessage/index";
export default {
  components: {
    XMessage
  },
  props: {
    isShow: {
      default: false
    },
    actionType: "", //DELETE,PUBLISH
    primaryKey: ""
  },
  data() {
    return {
      show: this.isShow,
      timeout: 6000,
      snackbar: false,
      message: "",
      messageType: "cyan darken-2",
      y: "top",
      x: null,
      mode: ""
    };
  },
  watch: {
    isShow: {
      handler() {
        this.show = this.isShow;
      }
    }
  },
  methods: {
    executeAction() {
      this.show = false;
      this.$emit("recall", false);
      if (this.actionType === "PUBLISH") {
        this.$http
          .get(
            `${this.$domain}/api/Blog/publishArticle?id=` +
              this.primaryKey
          )
          .then(res => {
            if (res.data.code === 20000) {
              this.messageType = "success";
            } else {
              this.messageType = "error";
            }
            this.message = res.data.message;
            this.snackbar = true;
            this.$emit("recallTable");
          });
      }
      if (this.actionType === "DELETE") {
        this.$http
          .delete(`${this.$domain}/api/Blog/` + this.primaryKey)
          .then(res => {
            if (res.data.code === 20000) {
              this.messageType = "cyan darken-2";
            } else {
              this.messageType = "error";
            }
            this.message = res.data.message;
            this.snackbar = true;
            this.$emit("recallTable");
          });
      }
    },
    notExecuteAction() {
      this.show = false;
      this.$emit("recall", false);
    }
  }
};
</script>