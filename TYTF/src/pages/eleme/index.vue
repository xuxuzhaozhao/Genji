<template>
  <div class="eleme">
<!--head -->
 <view class="header" style="height:100px;">
    <view class="content-wrapper">
      <view class="avatar">
        <image :src="userInfo.avatarUrl" 
               style="width:60px;height:60px;"
               @click="refreshData">
        </image>
      </view>
      <view class="content">
        <view class="title"> 
          <image src="/static/img/brand@2x.png" style="width:30px;height:14px;"></image>    
          <span class="name" style="font-size:15px;">田园天府生态农业有限公司</span>
        </view>
        <view class="description">
            {{userInfo.nickName}}，请到总台处确认订单
        </view>
      </view>
    </view> 
    <view class="background">
      <image src="/static/img/tytf.jpg" 
             style="width:100%;height:100%;">
      </image>
    </view>
  </view> 
<!--head-->

<!-- {top:menuTop+'px'} -->
<view class="goods" v-if="status==0" :style="'top:100px'">
  <view class="menu-wrapper">
    <scroll-view scroll-y="true" :style="{height:menuHeight+'px'}">
      <view v-for="(item,index) in goods" 
            :key="index" 
            class="menu-item border-1px" 
            @click="selectMenu(index)">
        <view class="text">
          {{item.name}}
        </view>
      </view>
    </scroll-view>
  </view>
  <scroll-view scroll-y="true" 
               :style="{height:menuHeight+'px'}" 
               :scroll-into-view="toView" 
               class="foods-wrapper"
               :scroll-with-animation="true">
    <view v-for="(item,parentIndex) in goods" 
          :key="parentIndex"
          class="food-list food-list-hook">
      <view :id="'order'+parentIndex" class="title">{{item.name}}</view>
      <view v-for="(food,index) in item.foods" 
            :key="index"
            class="food-item">
        <view class="icon">
          <image :src="food.icon" style="width: 57px;height: 57px" @click="showImage(food.icon)"></image>
        </view>
        <view class="content">
          <view class="name">{{food.name}}</view>
          <view class="desc">{{food.description==null?"":food.description}}</view>
          <!-- <view class="extra">
            <text class="count">月售{{food.sellCount}}</text>
          </view> -->
          <view class="price">
            <view class="now">￥{{food.price}}&nbsp;{{food.pricetype}}</view>
            <view class="old" v-if="food.oldPrice">￥{{food.oldPrice}}</view>
          </view>

          <!--cartControl-->
          <view class="cartControl-wrapper">
            <view class="cartControl">
              <view name="fade">
                <view class="cart-decrease" 
                      v-if="food.count > 0" 
                      @click="decreaseCart(food,parentIndex,index)">
                  <view class="inner iconfont icon-jianhao" 
                        style="font-size:24px;color:#00A0DC;">
                  </view>
                </view>
                <view class="cart-count" v-if="food.count > 0 ">{{food.count}}</view>
                <view class=" iconfont icon-jiahao cart-add " 
                      style="color:#00A0DC; " 
                      @click="addCart(food,parentIndex,index)">
                </view>
              </view>
            </view>
          </view>
        </view>
      </view>
    </view>
  </scroll-view>

  <!--shopCart-->
  <view>
    <view>
      <view class="shopCart">
        <view class="content" @click="toggleList">
          <view class="content-left">
            <view class="logo-wrapper">
              <view class="logo highlight">
                <view class="iconfont icon-gouwuche highlight"></view>
              </view>
              <view class="num" v-if="totalCount> 0">{{totalCount}}</view>
            </view>
            <view class="price highlight">￥{{totalPrice}}</view>
            <view class="desc">已选{{totalCount}}件食物</view>
          </view>
          <view class="content-right">
            <view class="pay enough" 
                  @tap="gotoOld">
              选择桌台
            </view>
          </view>
        </view>
        <view class="shopcart-list" :style="{display:cartShow}">
          <view class="list-header">
            <view class="title">购物车</view>
            <text class="empty" @click="empty">清空</text>
          </view>
          <view class="list-content">
            <view class="shopcart-food" 
                  v-for="(item,index) in carArray"
                  :key="index">
              <text class="name" v-if="item.num>0">{{item.name}}</text>
              <view class="price" v-if="item.num>0">
                <span>￥{{item.price*item.num}}</span>   
              </view>

              <!--cartControl-->
              <view class="cartControl-wrapper" style="bottom:10px;" v-if="item.num > 0">
                <view class="cartControl" style="width:80px;">
                  <view name="fade">
                    <view class="cart-decrease" 
                          style="display:inline-block;" 
                          @click="decreaseShopCart(item,index)">
                      <view class="inner iconfont icon-jianhao" style="font-size:24px;color:#00A0DC;"></view>
                    </view>
                    <view class="cart-count" 
                          style="display:inline-block;width:32px;text-align:center;font-size:14px;">
                          {{item.num}}
                    </view>
                    <view class=" iconfont icon-jiahao cart-add " 
                          style="font-size:24px;color:#00A0DC; padding:0;" 
                          @click="addShopCart(item,index)"></view>
                  </view>
                </view>
              </view>
              <!--cartControl-->
            </view>
          </view>
        </view>
      </view>
    </view>
  </view>
</view>
  </div>
</template>

<script>
export default {
  data() {
    return {
      goods: [],
      toView: "order",
      scrollTop: 100,
      foodCounts: 0,
      totalPrice: 0, // 总价格
      totalCount: 0, // 总商品数
      carArray: {},
      minPrice: 20, //起送價格
      payDesc: "",
      deliveryPrice: 4, //配送費
      fold: true,
      selectFoods: [{ price: 20, count: 2 }],
      cartShow: "none",
      status: 0,
      menuTop: 122,
      detailFood: {},
      userInfo: {},
      submitOrder: false,
      menuHeight: 520,
      menuTypeHeight: 0,
      showMenuImage: ""
    };
  },
  created() {
    this.getSystemInfo().then(res => {
     // // console.log(res);
      const height = res;
      if (height <= 568) {
        this.menuHeight = 365;
      }
      if (height > 568 && height <= 640) {
        this.menuHeight = 365;
      }
      if (height > 640 && height <= 667) {
        this.menuHeight = 455;
      }
      //Nexus 5X Nexus 6
      if (height > 667 && height <= 732) {
        this.menuHeight = 520;
      }
      //iphone7plus
      if (height > 732 && height <= 736) {
        this.menuHeight = 520;
      }
      if (height > 736 && height <= 1024) {
        this.menuHeight = 760;
      }
      //ipad Air
      if (height >= 1112) {
        this.menuHeight = 860;
      }
    });
    this.getUserInfo();
    this.initializeData();
  },
  methods: {
    initializeData() {
      let self = this;
      wx.request({
        url: "https://tytf.xuxuzhaozhao.top/api/WxMini/InitializeData", //仅为示例，并非真实的接口地址
        header: {
          "content-type": "application/json" // 默认值
        },
        success: function(res) {
          self.goods = res.data.data;
        }
      });
    },
    refreshData() {
      var self = this;
      wx.showModal({
        title: "提示",
        content: "是否重新加载数据？",
        success: function(sm) {
          if (sm.confirm) {
            // 用户点击了确定 可以调用删除方法了
            self.initializeData();
          } else if (sm.cancel) {
         //   // console.log("用户点击取消重新加载");
          }
        }
      });
    },
    showImage(icon) {
      this.showMenuImage = icon;
      wx.previewImage({
        current: "", // 当前显示图片的http链接
        urls: [icon] // 需要预览的图片http链接列表
      });
    },
    gotoOld() {
      if (this.totalCount <= 0) {
        wx.showToast({
          title: "别闹，都没选东西！",
          icon: "none",
          duration: 2500
        });
      } else {
        this.cartShow = "none";
        this.submitOrder = false;
        //订单列表 传参
        var args = JSON.stringify(this.carArray);
        const url = `/pages/order/main?order=${args}`;
        // // console.log(url)
        wx.navigateTo({ url });
      }
    },
    selectMenu: function(index) {
      this.toView = "order" + index;
    },
    getSystemInfo() {
      return new Promise((reslove, reject) => {
        wx.getSystemInfo({
          success: function(res) {
            reslove(res.screenHeight);
          }
        });
      });
    },
    //移除商品
    decreaseCart: function(food, parentIndex, index) {
      this.goods[parentIndex].foods[index].count--;
      this.detailFood = food;
      this.changeNum(index, parentIndex, false);
      this.toView = "order";
    },
    decreaseShopCart: function(food, index) {
     // // console.log(index);
      let indeex = index.split("_")[1];
      this.decreaseCart(food, food.parentIndex, indeex);
    },
    //添加到购物车
    addCart(food, parentIndex, index) {
      this.goods[parentIndex].foods[index].count++;
      this.detailFood = food;
      this.changeNum(index, parentIndex, true);
      this.toView = "order";
    },
    addShopCart: function(food, index) {
      let indeex = index.split("_")[1];
      this.addCart(food, food.parentIndex, indeex);
    },
    changeNum(index, parentIndex, bool) {
      // console.log(this.carArray, index, parentIndex);
      var carArray = this.carArray;
      var obj = carArray[parentIndex + "_" + index];

      //如果存在，则数量变化
      if (obj) {
        if (bool) {
          obj.num = obj.num + 1;
        } else {
          if (obj.num > 0) {
            obj.num = obj.num - 1;
          } else {
            return; //已经减少为0
          }
        }
      } else {
        if (bool) {
          //不存在，点击增加，则写入一条订单数据，数量默认1
          obj = {
            id: this.detailFood.id,
            index: index,
            parentIndex: parentIndex,
            num: 1,
            price: this.detailFood.price,
            name: this.detailFood.name
          };
          carArray[parentIndex + "_" + index] = obj;
        } else {
          return; //不存在，并且点击的是减少
        }
      }

      var order_num = 0;
      var order_cost = 0;
      for (var k in carArray) {
        order_num = carArray[k].num + order_num; //计算总数量
        order_cost = order_cost + carArray[k].price * carArray[k].num; //计算总价格
      }

      this.totalPrice = order_cost;
      this.totalCount = order_num;
      this.carArray = carArray;

      if (this.totalCount <= 0) {
        this.cartShow = "none";
      }
    },
    pay() {
      if (this.data.totalPrice < this.data.minPrice) {
        return;
      }
      // window.alert('支付' + this.totalPrice + '元');
      //确认支付逻辑
      var resultType = "success";
      wx.redirectTo({
        url: "../goods/pay/pay?resultType=" + resultType
      });
    },
    empty() {
      for (const key in this.carArray) {
        const element = this.carArray[key];
        this.goods[element.parentIndex].foods[element.index].count = 0;
      }
      this.carArray = {};
      this.totalCount = 0;
      this.totalPrice = 0;
      this.cartShow = "none";
    },
    //彈起購物車
    toggleList: function() {
      if (!this.totalCount) {
        return;
      }
      this.fold = !this.fold;
      var fold = this.fold;
      this.cartShowF(fold);
    },
    cartShowF: function(fold) {
      if (fold == false) {
        this.cartShow = "block";
      } else {
        this.cartShow = "none";
      }
    },
    tabChange: function(e) {
      var showtype = e.target.dataset.type;
      this.setData({
        status: showtype
      });
    },
    onLoad: function(options) {
      // 页面初始化 options为页面跳转所带来的参数
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
    onReady: function() {
      // 页面渲染完成
    },

    onHide: function() {
      // 页面隐藏
    },
    onUnload: function() {
      // 页面关闭
    }
  },
  onShow: function() {
    // console.log("ONSHOW");
    this.submitOrder = this.$root.$mp.query.submitOrder;
    // console.log("this.submitOrder", this.submitOrder);
    if (this.submitOrder) {
      this.empty();
    }
  },
  onPullDownRefresh: function() {
    wx.stopPullDownRefresh();
    // console.log("onPullDownRefresh");
    this.initializeData();
  }
};
</script>

<style scoped>
/* pages/goodss/goodss.wxss */
.goods {
  display: flex;
  position: absolute;
  width: 100%;
  top: 174px;
  bottom: 46px;
  overflow: hidden;
  font-family: "微软雅黑";
}

.goods .menu-wrapper {
  flex: 0 0 80px;
  width: 80px;
  background: #f3f5f7;
}

.goods .menu-wrapper .menu-item {
  display: table;
  width: 56px;
  height: 54px;
  line-height: 14px;
  padding: 0 12px;
}

.goods .menu-wrapper .menu-item:current {
  position: relative;
  z-index: 10;
  margin-top: -1px;
  background: #fff;
  font-weight: 700;
}

.goods .menu-wrapper .menu-item:current .text {
  border: none;
}

.goods .menu-wrapper .menu-item .icon {
  display: inline-block;
  width: 12px;
  height: 12px;
  vertical-align: top;
  margin-right: 4px;
  background-size: 12px 12px;
  background-repeat: no-repeat;
}

.goods .menu-wrapper .menu-item .text {
  display: table-cell;
  width: 56px;
  vertical-align: middle;
  position: relative;
  font-size: 14px;
  border-bottom: 1px solid rgba(7, 17, 27, 0.1);
}

.goods .foods-wrapper {
  flex: 1;
}

.goods .foods-wrapper .food-list .title {
  padding-left: 14px;
  height: 26px;
  line-height: 26px;
  border-left: 2px solid #d9dde1;
  font-size: 12px;
  color: rgb(147, 153, 159);
  background: #f3f5f7;
}

.goods .foods-wrapper .food-list .food-item {
  display: flex;
  margin: 18px;
  padding-bottom: 18px;
  position: relative;
  border-bottom: 1px solid rgba(7, 17, 27, 0.1);
}

.goods .foods-wrapper .food-list .food-item:last-child {
  border: none;
  margin-bottom: 0px;
}

.goods .foods-wrapper .food-list .food-item .icon {
  flex: 0 0 57px;
  margin-right: 10px;
}

.goods .foods-wrapper .food-list .food-item .content {
  flex: 1;
}

.goods .foods-wrapper .food-list .food-item .content .name {
  margin: 2px 0 8px 0;
  font-size: 15px;
  line-height: 14px;
  height: 14px;
  color: rgb(7, 17, 27);
}

.goods .foods-wrapper .food-list .food-item .content .desc,
.extra {
  font-size: 11px;
  line-height: 10px;
  color: rgb(147, 153, 159);
}

.goods .foods-wrapper .food-list .food-item .content .desc {
  margin-bottom: 8px;
  line-height: 12px;
}

.goods .foods-wrapper .food-list .food-item .content .extra .count {
  margin-right: 12px;
}

.goods .foods-wrapper .food-list .food-item .content .price {
  font-weight: 700px;
  line-height: 24px;
}

.goods .foods-wrapper .food-list .food-item .content .price .now {
  margin-right: 8px;
  font-size: 14px;
  color: rgb(240, 20, 20);
}

.goods .foods-wrapper .food-list .food-item .content .price .old {
  font-size: 10px;
  color: rgb(147, 153, 159);
  text-decoration: line-through;
}

.goods .foods-wrapper .food-list .food-item .content .cartControl-wrapper {
  position: absolute;
  right: 0;
  bottom: 12px;
}

/* cartControl 样式 */

.goods
  .foods-wrapper
  .food-list
  .food-item
  .content
  .cartControl-wrapper
  .cartControl {
  font-size: 0;
}

.goods
  .foods-wrapper
  .food-list
  .food-item
  .content
  .cartControl-wrapper
  .cartControl
  .cart-decrease,
.cart-add {
  display: inline-block;
  padding: 4px 6px 6px 6px;
}

.goods
  .foods-wrapper
  .food-list
  .food-item
  .content
  .cartControl-wrapper
  .cartControl
  .cart-decrease,
.cart-add :fade-enter-active,
:fade-leave-active {
  transition: all 0.4s linear;
}

.goods
  .foods-wrapper
  .food-list
  .food-item
  .content
  .cartControl-wrapper
  .cartControl
  .cart-decrease,
.cart-add :fade-enter,
:fade-leave-active {
  opacity: 0 transform translate3d(24px, 0, 0);
}

.goods
  .foods-wrapper
  .food-list
  .food-item
  .content
  .cartControl-wrapper
  .cartControl
  .cart-decrease,
.cart-add .inner {
  display: inline-block;
  line-height: 24px;
  font-size: 24px;
  vertical-align: top;
  color: rgb(0, 160, 220, 0.2);
}

.goods
  .foods-wrapper
  .food-list
  .food-item
  .content
  .cartControl-wrapper
  .cartControl
  .cart-decrease,
.cart-add :inner-enter-active,
:inner-leave-active {
  transition: all 0.4s linear;
  transform: rotate(0);
}

.goods
  .foods-wrapper
  .food-list
  .food-item
  .content
  .cartControl-wrapper
  .cartControl
  .cart-decrease,
.cart-add :inner-enter,
:inner-leave-active {
  opacity: 0 transform rotate(180deg);
}

.goods
  .foods-wrapper
  .food-list
  .food-item
  .content
  .cartControl-wrapper
  .cartControl
  .cart-count {
  display: inline-block;
  font-size: 10px;
  line-height: 24px;
  width: 12px;
  vertical-align: top;
  padding-top: 6px;
  text-align: center;
  color: rgb(147, 153, 159);
}

.goods
  .foods-wrapper
  .food-list
  .food-item
  .content
  .cartControl-wrapper
  .cartControl
  .cart-add {
  display: inline-block;
  padding: 6px;
  line-height: 24px;
  font-size: 24px;
  vertical-align: top;
  color: rgb(0, 160, 220, 0.2);
}

/* shopcart 样式 */

.shopCart {
  position: fixed;
  left: 0px;
  bottom: 0px;
  z-index: 50;
  width: 100%;
  height: 48px;
}

.shopCart .content {
  display: flex;
  background: #141d27;
}

.shopCart .content .content-left {
  flex: 1;
}

.shopCart .content .content-left .logo-wrapper {
  display: inline-block;
  position: relative;
  top: -10px;
  margin: 0 12px;
  padding: 6px;
  width: 56px;
  height: 56px;
  box-sizing: border-box;
  vertical-align: top;
  border-radius: 50%;
  background: #141d27;
}

.shopCart .content .content-left .logo-wrapper .num {
  position: absolute;
  top: 0;
  right: 0;
  width: 24px;
  height: 24px;
  line-height: 24px;
  text-align: center;
  border-radius: 16px;
  font-size: 9px;
  font-weight: 700;
  color: #fff;
  background: rgb(240, 20, 20);
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.4);
}

.shopCart .content .content-left .logo-wrapper .logo {
  width: 100%;
  height: 100%;
  text-align: center;
  border-radius: 50%;
  background: #2b343c;
}

.shopCart .content .content-left .logo-wrapper .logo.highlight {
  background: rgb(0, 160, 220);
}

.shopCart .content .content-left .logo-wrapper .logo .icon-gouwuche {
  line-height: 44px;
  font-size: 24px;
  color: #80858a;
}

.shopCart
  .content
  .content-left
  .logo-wrapper
  .logo
  .icon-qicheqianlian-
  .highlight {
  color: #fff;
}

.shopCart .content .content-left .price {
  display: inline-block;
  vertical-align: top;
  margin-top: 12px;
  line-height: 24px;
  padding-right: 12px;
  box-sizing: border-box;
  border-right: 1px solid rgba(255, 255, 255, 0.1);
  font-size: 16px;
  font-weight: 700;
  color: rgba(255, 255, 255, 0.4);
}

.shopCart .content .content-left .price.highlight {
  color: #fff;
}

.shopCart .content .content-left .desc {
  display: inline-block;
  vertical-align: top;
  line-height: 24px;
  margin-left: 12px;
  margin-top: 12px;
  color: #fff;
  font-size: 12px;
}

.shopCart .content .content-right {
  flex: 0 0 105px;
  width: 105px;
}

.shopCart .content .content-right .pay {
  height: 48px;
  line-height: 48px;
  text-align: center;
  font-size: 16px;
  color: rgba(255, 255, 255, 0.4);
  font-weight: 700;
  background: #2b333b;
}

.shopCart .content .content-right .pay.not-enough {
  background: #2b333b;
}

.shopCart .content .content-right .pay.enough {
  background: red;
  color: #fff;
}

.shopCart .ball-container .ball {
  position: fixed;
  left: 32px;
  bottom: 22px;
  z-index: 200;
  transition: all 0.6s cubic-bezier(0.49, -0.29, 0.75, 0.41);
}

.shopCart .ball-container .ball .inner {
  width: 16px;
  height: 16px;
  border-radius: 50%;
  background: red;
  transition: all 0.4s linear;
}

.shopCart .shopcart-list {
  position: absolute;
  top: 0;
  left: 0;
  z-index: -1;
  width: 100%;
  transform: translate3d(0, -100%, 0);
}

.shopCart .shopcart-list :fade-enter-active,
:fade-leave-active {
  transition: all 0.5s transform translate3d(0, -100%, 0);
}

.shopCart .shopcart-list :fade-enter,
:fade-leave-active {
  transform: translate3d(0, 0, 0);
}

.shopCart .shopcart-list .list-header {
  height: 40px;
  line-height: 40px;
  padding: 0 18px;
  background: #f3f5f7;
  border-bottom: 1px solid rgba(7, 17, 27, 0.1);
}

.shopCart .shopcart-list .list-header .title {
  float: left;
  font-size: 14px;
  color: rgb(7, 17, 27);
}

.shopCart .shopcart-list .list-header .empty {
  float: right;
  font-size: 12px;
  color: rgb(0, 160, 220);
}

.shopCart .shopcart-list .list-content {
  padding: 0 18px;
  /* max-height: 217px; */
  overflow: hidden;
  background: #fff;
}

.shopCart .shopcart-list .list-content .shopcart-food {
  position: relative;
  padding: 12px 0;
  box-sizing: border-box;
  /*border-top: 1px solid rgba(7,17,27,0.1);*/
}

.shopCart .shopcart-list .list-content .shopcart-food .name {
  line-height: 24px;
  font-size: 14px;
  color: rgb(7, 17, 27);
}

.shopCart .shopcart-list .list-content .shopcart-food .price {
  position: absolute;
  right: 110px;
  bottom: 14px;
  line-height: 24px;
  font-size: 14px;
  font-weight: 700;
  color: rgb(240, 20, 20);
}

.shopCart .shopcart-list .list-content .shopcart-food .cartControl-wrapper {
  position: absolute;
  right: 0;
  bottom: 6px;
}

/* head */

.header {
  position: relative;
  color: #fff;
  overflow: hidden;
  background: rgba(7, 17, 27, 0.5);
}

.header .content-wrapper {
  position: relative;
  padding: 24px 12px 18px 24px;
  font-size: 0;
}

.header .content-wrapper .avatar {
  display: inline-block;
  vertical-align: top;
}

.header .content-wrapper .avatar img {
  border-radius: 2px;
}

.header .content-wrapper .content {
  display: inline-block;
  font-size: 14px;
  margin-left: 16px;
}

.header .content-wrapper .content .title {
  margin: 2px 0 8px 0;
}

.header .content-wrapper .content .title .brand {
  display: inline-block;
  vertical-align: top;
  width: 30px;
  height: 18px;
  /*bg-image('brand')*/
  background-size: 30px 18px;
  background-repeat: no-repeat;
}

.header .content-wrapper .content .title .name {
  margin-left: 6px;
  font-size: 16px;
  line-height: 14px;
  font-weight: bold;
}

.header .content-wrapper .content .description {
  margin-bottom: 10px;
  line-height: 12px;
  font-size: 12px;
}

.header .content-wrapper .content .support .icon {
  display: inline-block;
  width: 12px;
  height: 12px;
  vertical-align: top;
  margin-right: 4px;
  background-size: 12px 12px;
  background-repeat: no-repeat;
  /*&.decrease
            bg-image('decrease_1')
          &.discount
            bg-image('discount_1')
          &.guarantee
            bg-image('guarantee_1')
          &.invoice
            bg-image('invoice_1')
          &.special
            bg-image('special_1')*/
}

.header .content-wrapper .content .support .text {
  line-height: 12px;
  font-size: 12px;
}

.header .content-wrapper .content .supports-count {
  position: absolute;
  right: 12px;
  bottom: 14px;
  padding: 0 8px;
  height: 24px;
  line-height: 24px;
  border-radius: 12px;
  background: rgba(0, 0, 0, 0.2);
  text-align: content;
}

.header .content-wrapper .content .supports-count .count {
  font-size: 10px;
  vertical-align: top;
}

.header .content-wrapper .content .supports-count .icon {
  margin-left: 2px;
  line-height: 24px;
  font-size: 10px;
}

.header .bulletin-wrapper {
  position: relative;
  background-color: rgba(7, 17, 27, 0.2);
  height: 28px;
  line-height: 28px;
  padding: 0 22px 0 12px;
  white-space: normal;
  overflow: hidden;
  text-overflow: ellipsis;
}

.header .bulletin-wrapper .bulletin-title {
  display: inline-block;
  vertical-align: top;
  margin-top: 8px;
  width: 22px;
  height: 12px;
  /*bg-image('bulletin')*/
  background-size: 22px 12px;
  background-repeat: no-repeat;
}

.header .bulletin-wrapper .bulletin-text {
  vertical-align: top;
  margin: 0 4px;
  font-size: 12px;
}

.header .bulletin-wrapper .icon {
  position: absolute;
  font-size: 20px;
  right: 12px;
  top: 0px;
}

.header .background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: -1;
  filter: blur(10px);
}

.header .background .detail {
  position: fixed;
  z-index: 100;
  top: 0;
  right: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background: rgba(7, 17, 27, 0.8);
  -webkit-backdrop-filter: blur(10px);
}

/* tab */
.tab {
  display: -ms-flexbox;
  display: flex;
  width: 100%;
  height: 40px;
  line-height: 40px;
  /* border: 1px solid rgba(7,17,27,0.1); */
  position: relative;
}

.tab .tab-item {
  -ms-flex: 1;
  flex: 1;
  text-align: center;
}

.tab .tab-item a {
  display: block;
  font-size: 14px;
  color: #4d555d;
}

.tab .tab-item .active {
  color: #f01414;
}

.list-mask {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: 40;
  /*backdrop-filter: blur(10px);*/
  -webkit-backdrop-filter: 1;
  background: rgba(7, 17, 27, 0.6);
}

.list-mask :fade-enter-active,
:fade-leave-active {
  opacity: 1;
  transition: all 0.5s;
  background: rgba(7, 17, 27, 0.6);
}

.food-list :fade-enter,
:fade-leave-active {
  opacity: 0;
  background: rgba(7, 17, 27, 0);
}

/* 字体图标 */
@font-face {
  font-family: "iconfont";
  src: url("data:application/x-font-woff;charset=utf-8;base64,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")
      format("woff"),
    url("/static/font/iconfont.ttf?t=1524644900004") format("truetype");

  font-weight: 500;
  font-style: normal;
}

.iconfont {
  font-family: "iconfont" !important;
  font-size: 16px;
  font-style: normal;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

.icon-jianhao:before {
  content: "\e63f";
}

.icon-jiahao:before {
  content: "\e642";
}

.icon-gouwuche:before {
  content: "\e61d";
  color: white;
}

/* src: url(data:application/font-woff;charset=utf-8;base64,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)
      format("woff"),
      url('iconfont.ttf')
      ; */
/* @font-face {
  font-family: 'iconfont';
  src: url('/static/font/iconfont.eot');
  src: url('/static/font/iconfont.eot?#iefix') format('embedded-opentype'),
  url('/static/font/iconfont.woff') format('woff'),
  url('/static/font/iconfont.ttf') format('truetype'),
  url('/static/font/iconfont.svg#iconfont') format('svg');
  
  font-weight: 500;
  font-style: normal;
}

.iconfont {
  font-family: "iconfont" !important;
  font-size: 16px;
  font-style: normal;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

.icon-gouwuche:before {
  content: "\e601";
}
.icon-jian:before {
  content: "\e629";
}

.icon-jia:before {
  content: "\e622";
} */
</style>
