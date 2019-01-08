<template>
  <v-dialog class="imageDialog" 
            title="图片上传" 
            v-model="isUploadShow" 
            max-width="500px"
            style=" background: white;margin-top: -30%;">
    <el-upload class="upload-demo" 
                action="http://up-z2.qiniu.com" 
                :on-success="handleSuccess" 
                :on-remove="handleRemove" 
                :before-upload="beforeUpload" 
                :data="form" 
                :file-list="file_logo_img_url" 
                list-type="picture">
      <v-btn dark color="pink">点击上传</v-btn>
      <div slot="tip" class="el-upload__tip">只能上传jpg/png文件，反正不是小于2M就是小于4M,而且名称里面不能有空格及中文</div>
    </el-upload>
    <v-text-field solo v-model="imgUrl"></v-text-field>
  </v-dialog>
</template>

<script>
import * as moment from "moment";

export default {
  props: ["showUpload"],
  data() {
    return {
      isUploadShow: false, // 是否显示upload组件
      form: {},
      imgUrl: "",
      file_logo_img_url: []
    };
  },
  watch: {
    showUpload: {
      handler() {
        this.isUploadShow = this.showUpload;
      }
    },
    isUploadShow: {
      handler() {
        if (this.isUploadShow == false) {
          this.$emit("hideUpload");
        }
      }
    }
  },
  created() {
    this.gettoken();
  },
  methods: {
    beforeUpload(file) {
      let curr = moment()
        .format("YYYYMMDD")
        .toString();
      let prefix = moment(file.lastModified)
        .format("HHmmss")
        .toString();
      let suffix = file.name;
      let key = encodeURI(`${curr}/${prefix}_${suffix}`);
      this.form.key = key;
    },
    gettoken() {
      this.$http.get(`${this.$domain}/api/Qiniu`).then(res => {
        if (res.data.code === 20000) {
          this.form.token = res.data.data;
        } else {
          this.$message({
            type: "error",
            message: res.data.message
          });
        }
      });
    },
    handleSuccess(response, file, fileList) {
      console.log("成功处理" + JSON.stringify(response));
      this.file_logo_img_url = fileList;
      this.imgUrl = "http://img.xuxuzhaozhao.top/" + response.key;
    },
    handleRemove(file, fileList) {
      this.file_logo_img_url = fileList;
    },
    // handleClose() {
    //   this.$emit("hideUpload", false);
    //   console.log(this.isUploadShow);
    //   this.isUploadShow = false;
    // }
  }
};
</script>

<style scoped>

.imageDialog {
  width: 20%;
  background: white;
  margin-top: -30%;
}
</style>
