<template>
  <div class="clickFood">
    <view class="section">

  <!-- banner -->
  <view class="banner" >
    <swiper :indicator-dots="indicatorDots"
            :autoplay="autoplay" 
            :interval="interval" 
            :duration="duration">
      <block v-for="(item,i) in imgUrls" :key="i">
        <swiper-item>
          <image :src="item" 
                 mode="scaleToFill" 
                 class="slide-image" />
        </swiper-item>
      </block>
    </swiper>
  </view>

  <!-- picker -->
  <view class="memu">
    <picker style="flex: 1;" 
            bindchange="foodTypeChange" 
            value="0" 
            :range="foodTypes">
        <view class="memu-bt">
          {{foodTypes[foodTypesIndex]}}
          <span style="font-size:22px;">﹀</span>
        </view>
    </picker>

    <picker style="flex: 1;" 
            bindchange="rankTypeChange" 
            value="0" 
            :range="rankTypes">
        <view class="memu-bt">
          {{rankTypes[rankTypesIndex]}}
          <span style="font-size:22px;">﹀</span>
        </view>
    </picker>
  </view>

  <!-- 食品列表 -->
  <view>
    <scroll-view style="height: 100%;" scroll-y="true">
      <view class="fooditem" v-for="(item,i) in halfFoodList" :key="i">
        <!-- 左边选择 -->
        <view v-if="foodList[i*2]!==undefined" class="fooditem-l" >
          <image mode="scaleToFill" 
                 :src="foodList[i*2].icon" 
                 @click="imageClick(foodList[i*2].id)" />
          <view class="foodtitle">{{foodList[i*2].title}}</view>
          <view class="fooditem-handle">
            <span class="fooditem-jia" 
                  @click="addClick(foodList[i*2])" >
                  +
            </span>
            <span class="fooditem-num">
              {{orderList[foodList[i*2].id]!=undefined ? orderList[foodList[i*2].id].num : 0}}
            </span>
            <span class="fooditem-jia" 
                  @click="reduceClick(foodList[i*2])">
                  -
            </span>
            <span class="fooditem-cost">￥{{foodList[i*2].cost}}</span>
          </view>
        </view>
        <!-- 右边选择 -->
        <view v-if="foodList[i*2+1]!==undefined"  class="fooditem-r" >
          <image mode="scaleToFill" 
                 :src="foodList[i*2+1].icon" 
                 @click="imageClick(foodList[i*2+1].id)" />
          <view class="foodtitle">{{foodList[i*2+1].title}}</view>
          <view class="fooditem-handle">
            <span class="fooditem-jia" 
                  @click="addClick(foodList[i*2+1])">
                  +
            </span>
            <span class="fooditem-num">
              {{orderList[foodList[i*2+1].id]!=undefined ? orderList[foodList[i*2+1].id].num : 0}}
            </span>
            <span class="fooditem-jia" 
                  @click="reduceClick(foodList[i*2+1])">
                  -
            </span>
            <span class="fooditem-cost">
              ￥{{foodList[i*2+1].cost}}
            </span>
          </view>
        </view>  

        <!-- 如果是奇数需要占位置 -->
        <view v-if="foodList[i*2+1]===undefined && foodList[i*2]!==undefined"  class="fooditem-r" >
        </view>     
      </view>
    </scroll-view>
  </view>

  <!-- toast -->
  <toast :hidden="toastHidden">已加入购物车</toast>

  <!-- 弹出面板 -->
 <view class="detail"
       :style="{display:cartModal}">
    <view class="detail-top" @click="closeModal"></view>
    <view class="detail-bottom">
      <image :src="detailFood.icon" />
      <view class="detail-title">{{detailFood.title}}</view>
      <view class="detail-cost">￥{{detailFood.cost}}</view>
      <view class="detail-desc">{{detailFood.desc}}</view>
    </view>
    <view class="detail-buybt" 
          @click="addCartClick">
          加入购物车
    </view>
  </view>

  <view style="height:45px;" ></view> 

  <!-- 提交订单-->
  <view class="order-view" :style="{display:orderModal}">
      <view class="order-number">
        <image mode="scaleToFill" 
               src="/static/img/cart.png" />
        <span class="order-num-txt">{{orderNum}}</span>
        <span>￥{{orderCost}}</span>
      </view>
      <view class="order-bt" @click="sublimitClick">提交订单</view>
  </view> 

</view>
  </div>
</template>

<script>
export default {
  data() {
    return {
      foodList: [
        {
          id: 1,
          title: "黑胡椒意酱面",
          cost: 45,
          desc: "进口意大利通心粉制作，搭配有机番茄秘制酱汁。",
          icon:
            "https://fuss10.elemecdn.com/8/05/0b0f3719bf1c9c1673ed69e262888jpeg.jpeg",
          num: 0
        },
        {
          id: 2,
          title: "吉士意大利虾仁面",
          cost: 40,
          desc: "进口意大利通心粉制作，搭配进口地中海大虾仁。",
          icon:
            "https://fuss10.elemecdn.com/4/df/ff64bc5b06893a12aafb1e94f8b31jpeg.jpeg",
          num: 0
        },
        {
          id: 3,
          title: "牛排意大利面",
          cost: 38,
          desc: "进口意大利通心粉制作，搭配新鲜酱汁牛排和甜糯玉米。",
          icon:
            "https://fuss10.elemecdn.com/3/42/70aae8406958d22657c5772e2412ejpeg.jpeg",
          num: 0
        },
        {
          id: 4,
          title: "香炸鸡翅",
          cost: 12,
          desc: "有机食品，绿色农场放心肉源",
          icon:
            "https://fuss10.elemecdn.com/e/0e/df4f9d07b191d34ceddc3fedd88dcjpeg.jpeg",
          num: 0
        },
        {
          id: 5,
          title: "一品寿司",
          cost: 22,
          desc: "百分百手工，百分百口味，来自东海岸的问候。",
          icon:
            "https://fuss10.elemecdn.com/8/53/353cf146fc9cab79479efcfb6e88ajpeg.jpeg",
          num: 0
        },
        {
          id: 6,
          title: "水果拼盘",
          cost: 16,
          desc: "新鲜时蔬，源自生活的百味奇珍。",
          icon:
            "https://fuss10.elemecdn.com/0/b9/42b68495a09ec2e501ec3eaa36c6ejpeg.jpeg",
          num: 0
        },
        {
          id: 7,
          title: "巧手三明治",
          cost: 22,
          desc: "好吃不贵，明治选择。",
          icon:
            "https://fuss10.elemecdn.com/a/fe/0d647946855f76e9dcdfbedfcad61jpeg.jpeg",
          num: 0
        },
        {
          id: 8,
          title: "培根焗饭",
          cost: 26,
          desc: "好吃不解释，真的不解释。",
          icon:
            "https://fuss10.elemecdn.com/9/de/30676686cf98d88961eb69f1f3083jpeg.jpeg",
          num: 0
        },
        {
          id: 9,
          title: "培根焗饭2",
          cost: 28,
          desc: "好吃不解释，真的不解释。",
          icon:
            "https://fuss10.elemecdn.com/9/de/30676686cf98d88961eb69f1f3083jpeg.jpeg",
          num: 0
        }
      ],
      detailFood: {},
      modalHidden: true,

      //订单数据
      orderList: {},
      orderNum: 0,
      orderCost: 0,
      toastHidden: true,

      //轮播图
      imgUrls: [
        "https://fuss10.elemecdn.com/d/c0/56cfcdabba9fec97a3307b571ca8cjpeg.jpeg",
        "https://fuss10.elemecdn.com/6/f2/5cd85b966281a8d545c34019d0fd1jpeg.jpeg",
        "https://fuss10.elemecdn.com/e/55/b00aef689cb424aaaeae9d50d3e76jpeg.jpeg",
        "https://fuss10.elemecdn.com/a/88/98d10d5580ce28db07673e28726ccjpeg.jpeg"
      ],
      indicatorDots: true,
      autoplay: true,
      interval: 4000,
      duration: 1000,

      //picker
      foodTypes: ["全部菜品", "披萨", "面条", "水果", "寿司", "三明治"],
      foodTypesIndex: 0,
      rankTypes: ["综合排序", "热度", "价格", "好评", "时间"],
      rankTypesIndex: 0
    };
  },
  created() {
    this.getFoodList();
  },
  computed: {
    halfFoodList() {
      return this.foodList.slice(0, this.foodList.length / 2 + 1);
    },
    cartModal() {
      return this.modalHidden ? "none" : "block";
    },
    orderModal() {
      return this.modalHidden ? "flex" : "none";
    }
  },
  methods: {
    getFoodList() {
      //// console.log(this.foodList[10]);
    },
    //关闭食品详情弹窗
    closeModal() {
      this.modalHidden = true;
    },

    //展示食品详情弹窗
    imageClick: function(e) {
      var dataset = e.currentTarget.dataset;

      var appInstance = getApp();
      var t_foodList = appInstance.globalData.foodList;

      //设置数据，自动刷新界面
      this.setData({
        modalHidden: false,
        detailFood: t_foodList[dataset.index],
        detailIndex: dataset.index
      });
    },

    //提交订单
    sublimitClick() {
      //订单列表 传参
      var args = JSON.stringify(this.orderList);
      const url = "/pages/order/main?order=" + args;
     // // console.log(url);
      wx.navigateTo({ url });
    },

    //加入到购物车
    addCartClick() {
      this.changeNum(this.detailFood.id, true);
     // // console.log(this.orderList)
      this.toastHidden = false;

      //1s后关闭
      var _this = this;
      setTimeout(function() {
        _this.toastHidden = true;
      }, 1000);
      //关闭商品详情面板
      this.closeModal();
    },

    //增加数量
    addClick(food) {
      this.modalHidden = false;
      this.detailFood = food;
    },

    //减少数量
    reduceClick(food) {
      this.changeNum(this.detailFood.id, false);
     // // console.log(this.orderList);
    },

    changeNum(index, bool) {
      var orderList = this.orderList;

      var obj = orderList[index];

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
            id: index,
            num: 1,
            cost: this.detailFood.cost,
            title: this.detailFood.title
          };
          this.orderList[index] = obj;
        } else {
          return; //不存在，并且点击的是减少
        }
      }

      var order_num = 0;
      var order_cost = 0;
      for (var k in orderList) {
        order_num = orderList[k].num + order_num; //计算总数量
        order_cost = order_cost + orderList[k].cost * orderList[k].num; //计算总价格
      }

      this.orderList = orderList;
      this.orderCost = order_cost;
      this.orderNum = order_num;
    },

    //食品类型
    foodTypeChange: function(e) {
     // // console.log("picker发送选择改变，携带值为", e.detail.value);
      this.setData({
        foodTypesIndex: e.detail.value
      });
    },

    //排序类型
    rankTypeChange: function(e) {
     // // console.log("picker发送选择改变，携带值为", e.detail.value);
      this.setData({
        rankTypesIndex: e.detail.value
      });
    }
  }
};
</script>

<style scoped>
.slide-image {
  width: 96%;
  height: 250px;
  margin: 0 auto;
  margin-left: 2%;
}

/* 下拉菜单 */
.memu {
  display: flex;
  flex-direction: row;
  text-align: center;
  border-bottom: 1px solid #ccc;
}
.memu-bt {
  display: block;
  flex: 1;
  padding: 10px 0px;
}
.memu-bt span {
  position: relative;
  top: 7px;
  left: 5px;
}

/* 食品列表 */
.fooditem {
  flex-direction: row;
  display: flex;
  margin: 15px 0px;
}
.fooditem-l,
.fooditem-r {
  flex: 1;
}
.fooditem-l {
  margin-left: 5%;
}
.fooditem-r {
  margin-left: 0%;
}
.fooditem image {
  width: 90%;
  height: 100px;
}
.foodtitle {
  font-size: 14px;
  margin-top: 10px;
}
.fooditem-handle {
  margin-top: 12px;
}
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
.fooditem-num {
  font-size: 14px;
  display: inline-block;
  color: #ff2d2d;
  width: 28px;
  text-align: center;
  margin-top: -3px;
}
.fooditem-cost {
  font-size: 14px;
  color: #ff2d2d;
  width: 40px;
  float: right;
  margin-top: 5px;
}

/* 提交订单 */
.order-view {
  position: fixed;
  display: flex;
  bottom: 0px;
  left: 0px;
  right: 0px;
  height: 45px;
}
.order-number {
  width: 50%;
  background-color: #eee;
  color: #ff2d2d;
  text-align: center;
  line-height: 45px;
}
.order-number image {
  position: absolute;
  width: 20px;
  height: 20px;
  left: 19px;
  top: 13px;
}
.order-num-txt {
  position: absolute;
  width: 20px;
  height: 20px;
  display: block;
  font-size: 14px;
  border-radius: 10px;
  background-color: #ff2d2d;
  color: #fff;
  line-height: 22px;
  left: 35px;
  top: 5px;
}
.order-bt {
  width: 50%;
  background-color: #09bb07;
  color: #fff;
  text-align: center;
  line-height: 45px;
}

/* 食品详情-弹出面板 */
.detail {
  width: 100%;
  position: fixed;
  bottom: 0px;
  top: 0px;
  left: 0px;
  right: 0px;
}
.detail-top {
  height: 16%;
  background-color: #000;
  opacity: 0.75;
}
.detail-bottom {
  height: 84%;
  background-color: #fff;
}
.detail-bottom image {
  width: 100%;
}
.detail-title {
  font-size: 14px;
  margin: 20px 0px 0px 20px;
}
.detail-cost {
  font-size: 14px;
  color: #ff2d2d;
  margin: 10px 0px 20px 20px;
}
.detail-desc {
  font-size: 14px;
  color: #aaa;
  padding: 20px 0px 0px 20px;
  border-top: 1px solid #999;
}
.detail-buybt {
  position: absolute;
  bottom: 0px;
  left: 0px;
  right: 0px;
  height: 50px;
  background-color: #09bb07;
  line-height: 50px;
  text-align: center;
  color: #fff;
  font-size: 20px;
}
</style>
