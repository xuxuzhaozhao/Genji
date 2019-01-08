<template>
  <div class="xcontent">
    <v-container class="contentContainer" fluid grid-list-md>
      <v-layout row wrap>
        <v-flex xs12 xl3 lg4 md4 v-for="card in cards" :key="card.id" style="max-width:100%">
          <v-card class="contentCards">
            <v-card-media :src="card.image" height="150px"></v-card-media>
            <v-card-title primary-title>
              <div>
                <div>{{card.title}}</div>
                <span class="grey--text">{{card.createTime}}</span>
              </div>
            </v-card-title>
            <v-spacer></v-spacer>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn flat @click="share(card.id)">Share</v-btn>
              <v-btn flat @click="read(card.id)" color="purple">Explore</v-btn>
            </v-card-actions>
          </v-card>
        </v-flex>
      </v-layout>
      <v-layout row class="paginationLayout">
        <v-spacer></v-spacer>
        <v-pagination
          v-show="pages>1"
          color="cyan darken-3"
          :length="pages"
          v-model="pagination.page"
          :total-visible="totalVisible"
        ></v-pagination>
      </v-layout>
    </v-container>
  </div>
</template>

<script>
import XSelect from "@/components/XSelect/index";
export default {
  components: {
    XSelect
  },
  created() {
    var x = window.innerHeight;
    var y = window.innerWidth;
    if (x < 600) {
      this.totalVisible = 4;
    }
    if (x > 600 && x < 800) {
      this.totalVisible = 6;
    }
    if (y < 1920) {
      document.getElementsByTagName("html")[0].style.overflowY = "scroll";
    }
    this.getlist();
  },
  props: ["articleTypeId", "articleTitle"],
  data: () => ({
    page: 1,
    total: 0,
    totalVisible: 7,
    showLoading: true,
    pagination: {
      rowsPerPage: 8,
      IsBackAdmin: false,
      page: 1
    },
    cards: []
  }),
  computed: {
    pages() {
      return Math.ceil(this.total / this.pagination.rowsPerPage);
    }
  },
  watch: {
    "pagination.page": {
      handler() {
        this.getlist();
      }
    },
    articleTypeId: {
      handler() {
        this.pagination.Category = this.articleTypeId;
        this.getlist();
      }
    },
    articleTitle: {
      handler() {
        this.pagination.Title = this.articleTitle;
        this.getlist();
      }
    },
    deep: true
  },
  methods: {
    getlist() {
      this.$http
        .post(`${this.$domain}/api/Blog/getlist`, this.pagination)
        .then(res => {
          this.$emit("update:show", false);
          this.cards = res.data.data.content;
          this.total = res.data.data.count;
        });
    },
    read(id) {
      window.open("/#/xxzz/article?id=" + id);
    },
    share(id) {
      alert("http://www.xuxuzhaozhao.top/#/xxzz/article?id=" + id);
    }
  }
};
</script>

<style lang="scss">
.xcontent {
  @media screen and (max-width: 1920px) {
    .contentCards {
      margin-top: 40px;
      margin-left: 20px;
    }
    .contentContainer {
      margin-top: 1%;
      .selectContainer {
        margin: 0;
        padding: 0px;
        padding-left: 24px;
        .input-group {
          padding: 0;
        }
      }
    }

    .paginationLayout {
      margin-top: 4% !important;
    }
  }

  @media screen and (max-width: 768px) {
    .contentCards {
      margin-top: 0px;
      margin-left: 0px;
      margin-bottom: 10px;
    }

    .paginationLayout {
      margin-top: 0% !important;
    }

    .contentContainer {
      margin-top: 0%;
      margin-left: 0;
    }
  }
}
</style>
