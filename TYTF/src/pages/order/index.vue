<template>
  <div class="order">
    <view>

      <view class="table">
        <view class="table-num">桌号</view>
        
        <picker @change="bindPickerChange" :value="tableIndex" :range="tables" range-key="Name">
          <view class="table-input">{{tables[tableIndex].Name}}</view>
          <span class="table-arrow">﹀</span>
        </picker>
      </view>

      <view class="beizhu">
        <view class="beizhu-txt">备注</view>
        <input class="beizhu-input" v-model="Note" />
      </view>

      <view class="order">
        <view class="cost">总价：<span>￥{{total}}</span></view>
        <view class="shuoming">说明：如果菜品中包含点杀系列或菜品按重量计价，则总价不具有参考意义！</view>

        <view class="orderlist" v-for="(item,i) in orderList" :key="i">
          <span>{{item.name}}</span>
          <span style="float:right;">
            ￥{{item.price}} * {{item.num}}
          </span>
        </view>
      </view>

      <view class="bt-area">
        <view class="bt-return" @click="returnClick">返回修改</view>
        <view class="bt-ok" @click="okClick">确定提交</view>
      </view>
    </view>
  </div>
</template>

<script>
import card from "@/components/card";
export default {
  components: {
    card
  },
  props: ["orderListr", ""],
  data() {
    return {
      waiterId: 0,
      orderList: [],
      total: 0, //总价格
      toastHidden: true,
      toastTxt: "",
      tables: [], //桌号
      tableIndex: 0,
      userInfo: {},
      initializeTables: [],
      Note: ""
    };
  },
  onRead() {},
  onLoad() {
    //获取用户的微信信息
    this.getUserInfo();

    //object 转 array
    var order = JSON.parse(this.$root.$mp.query.order);
    this.waiterId = this.$root.$mp.query.waiterId;
    var t_order = [];
    var t_total = 0;
    for (var k in order) {
      if (order[k].num > 0) {
        t_order.push(order[k]);
        t_total = t_total + order[k].price * order[k].num; //计算总价格
      }
    }
    this.orderList = t_order;
    this.total = t_total;
    this.initializeTablePosition();
  },
  created() {},
  methods: {
    //返回修改
    returnClick() {
      const url = `/pages/eleme/main`;
      wx.navigateTo({ url });
    },

    initializeTablePosition() {
      let self = this;
      wx.request({
        url: "https://tytf.xuxuzhaozhao.top/api/WxMini/GetTablePosition", //仅为示例，并非真实的接口地址
        header: {
          "content-type": "application/json" // 默认值
        },
        success: function(res) {
          self.tables = res.data.data;
        }
      });
    },

    getUserInfo() {
      // 调用登录接口
      wx.login({
        success: () => {
          wx.getUserInfo({
            success: res => {
              this.userInfo = res.userInfo;
            }
          });
        }
      });
    },
    okClick() {
      var self = this;
      wx.showModal({
        title: "提示",
        content: "提交后将只能在后台修改？",
        success: function(sm) {
          if (sm.confirm) {
            // 用户点击了确定 可以调用删除方法了
            self.submitOrder();
          } else if (sm.cancel) {
            // console.log("用户点击取消");
          }
        }
      });
    },
    //确认提交
    submitOrder() {
      // TODO 发送请求
      let detailTempsT = [];
      for (let i = 0; i < this.orderList.length; i++) {
        const item = this.orderList[i];
        detailTempsT.push({
          MenuId: item.id,
          SinglePrice: item.price,
          Weight: item.num
        });
      }

      let dataObject = {
        PositionId: this.tables[this.tableIndex].Id,
        ClientName: "",
        WaiterName: this.userInfo.nickName,
        Note: this.Note,
        detailTemps: detailTempsT
      };
      // console.log(this.orderList, dataObject);

      wx.request({
        url: "https://tytf.xuxuzhaozhao.top/api/WxMini/CreateOrder", //仅为示例，并非真实的接口地址
        header: {
          "content-type": "application/json" // 默认值
        },
        method: "POST",
        data: dataObject,
        success: function(res) {
          // console.log(res.data);
          if (res.data.code == 20000) {
            //1s后关闭
            wx.showToast({
              title: "已提交订单！",
              icon: "success",
              duration: 2500
            });
            setTimeout(() => {
              const url = `/pages/eleme/main?submitOrder=false`;
              wx.navigateTo({ url });
            }, 2500);
          } else {
            //1s后关闭
            let mess = res.data.message;
            if (mess === "非法账号！") {
              mess = "抱歉，只支持田园天府服务人员提交订单，您只可查看菜单！";
            }
            wx.showToast({
              title: mess,
              icon: "none",
              duration: 10000
            });
          }
        },
        fail: function(res) {
          wx.showToast({
            title: "cuowu！",
            icon: "success",
            duration: 2500
          });
        }
      });
    },

    bindPickerChange(e) {
      this.tableIndex = e.mp.detail.value;
      // console.log("哪一桌?", this.tables[this.tableIndex]);
    },
    bindChange(e) {
      // console.log(e.mp.detail);
    }
  }
};
</script>

<style scoped>
.fooditem-jia {
  font-size: 20px;
  width: 18px;
  height: 18px;
  display: inline-block;
  border: 1px solid #ff2d2d;
  color: #ff2d2d;
  border-radius: 10px;
  text-align: center;
  line-height: 16px;
}

.userinfo {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.userinfo-avatar {
  width: 128rpx;
  height: 128rpx;
  margin: 20rpx;
  border-radius: 50%;
}

.userinfo-nickname {
  color: #aaa;
}

.table-num {
  margin-right: 20px;
  font-size: 16px;
  margin-top: 4px;
  float: left;
}
.table-input {
  width: 60%;
  height: 30px;
  display: inline-block;
  border: 1px solid #ccc;
  border-radius: 3px;
  vertical-align: middle;
  text-align: center;
  line-height: 30px;
  font-size: 14px;

  /* border: 1px solid #ccc;
  padding: 7px 0px;
  border-radius: 3px;
  padding-left: 5px;
  -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
  box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
  -webkit-transition: border-color ease-in-out 0.15s,
    -webkit-box-shadow ease-in-out 0.15s;
  -o-transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
  transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;.add(key, value) */
}
.table-arrow {
  width: 30px;
  height: 30px;
  margin-left: -1px;
  display: inline-block;
  border: 1px solid #ccc;
  border-radius: 3px;
  text-align: center;
  line-height: 35px;
  vertical-align: middle;
}

.beizhu {
  padding: 20px 20px 20px 10px;
  border-bottom: 1px solid #aaa;
}
.beizhu-txt {
  margin-top: 4px;
  margin-left: 4px;
  margin-right: 20px;
  font-size: 16px;
  float: left;
}
.beizhu-input {
  width: 70%;
  height: 30px;
  border: 1px solid #ccc;
  border-radius: 3px;
  line-height: 25px;
  font-size: 14px;
}

.order {
  padding: 20px 20px 48px 30px;
}
.shuoming{
    font-size: 12px;
    font-weight: 700;
}
.cost {
  font-size: 16px;
  margin-bottom: 20px;
}
.cost span {
  color: #ff2d2d;
  margin-left: 12px;
}
.orderlist {
  margin-top: 20px;
}
.orderlist span {
  font-size: 15px;
}

.bt-area {
  position: fixed;
  bottom: 0px;
  left: 0px;
  right: 0px;
}
.bt-return {
  width: 50%;
  height: 48px;
  display: inline-block;
  background-color: #eee;
  text-align: center;
  line-height: 48px;
  font-size: 20px;
}
.bt-ok {
  width: 50%;
  height: 48px;
  display: inline-block;
  background-color: #09bb07;
  text-align: center;
  line-height: 48px;
  font-size: 20px;
  color: #fff;
}
</style>
