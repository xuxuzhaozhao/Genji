<template>
  <div>
    <button 
        class="authorizeButton"
        v-if="canIUse" 
        open-type="getUserInfo" 
        type="primary"
        @getuserinfo="bindGetUserInfo"
    >授权登录</button>
    <view v-else>请升级微信版本</view>
  </div>
</template>

<script>
export default {
  data() {
    return {
      canIUse: wx.canIUse("button.open-type.getUserInfo"),
      isOK: "OK"
    };
  },
  created() {},
  onLoad: function() {
    this.getSettingInfo();
  },
  methods: {
    bindGetUserInfo: function(e) {
      // console.log(e);
      if (e.target.userInfo) {
        //用户按了允许授权按钮
        // console.log("bindGetUserInfo", e.target.userInfo);
        this.getSettingInfo();
      } else {
        //用户按了拒绝按钮
        wx.showToast({
          title: "未授权，不可登录田园天府小程序！",
          icon: "none",
          duration: 5500
        });
      }
    },
    getSettingInfo() {
      let self = this;
      wx.request({
        url: "https://tytf.xuxuzhaozhao.top/api/BaseTable/TytfSetting/getlist", //仅为示例，并非真实的接口地址
        header: {
          "content-type": "application/json" // 默认值
        },
        success: function(res) {
          console.log(res.data.data);
          self.tables = res.data.data;
          res.data.data.forEach(item => {
            if (item.Name == "IsOK") {
              if (item.Value == "OK") {
                wx.getSetting({
                  success: function(res) {
                    if (res.authSetting["scope.userInfo"]) {
                      wx.getUserInfo({
                        success: function(res) {
                          //// console.log(res.userInfo);
                          //用户已经授权过
                          const url = `/pages/eleme/main`;
                          wx.navigateTo({ url });
                        }
                      });
                    }
                  }
                });
              } else {
                const url = `/pages/showpage/main`;
                wx.navigateTo({ url });
              }
            }
          });
        }
      });
    }
  }
};
</script>

<style>
.authorizeButton {
  width: 95%;
  margin-top: 55%;
}
</style>
