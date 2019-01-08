<template>
    <div>
        <img src="/static/img/tytf.jpg" style="width:414px;;" />
        <div class="welcome">{{userInfo.nickName}}，田园天府欢迎您！</div>
        <button 
            class="authorizeButton"
            type="warn"
            @click="makeMeCall"
            >给我们打电话</button>
        <button 
            class="authorizeButton"
            type="primary"
            @click="gotoTytf"
            >导航到田园天府</button>
    </div>
</template>

<script>
export default {
  data() {
    return { userInfo: {} };
  },

  created() {
    this.getUserInfo();
  },

  methods: {
    getUserInfo() {
      // 调用登录接口
      wx.login({
        success: () => {
          wx.getUserInfo({
            success: res => {
              this.userInfo = res.userInfo;
              console.log(this.userInfo);
            }
          });
        }
      });
    },
    makeMeCall() {
      wx.makePhoneCall({
        phoneNumber: "13990339009" //仅为示例，并非真实的电话号码
      });
    },
    gotoTytf() {
        //104.033204,30.132608
      wx.openLocation({
        latitude: 30.132608,
        longitude: 104.033204,
        scale: 18,
        name: "四川省田园天府生态农业有限公司",
        address: "成黑环湖西路观音滩桥旁"
      });
    }
  },

  onLoad() {}
};
</script>

<style>
.authorizeButton{
    width: 95%;
    margin-top:5%;
}
.welcome{
    text-align: center;
    margin-top:5%;
}
</style>
