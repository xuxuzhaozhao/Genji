<template>
 <div>
    <v-layout row wrap>
      <v-flex xs2>
        <v-btn @click="addItem"
          color="success">
          <v-icon>add</v-icon>新增文章
      </v-btn>
      </v-flex>
      <v-flex xs2>
        <x-select @refresh="refreshBlogList"></x-select>
      </v-flex>
    </v-layout>
    <v-data-table :loading="loading"
                  :headers="headers"
                  :items="items"
                  :pagination.sync="pagination"
                  :total-items="totalItems"
                  hide-actions
                  class="elevation-1">
    <template slot="items" slot-scope="props">
      <td>{{ props.item.title }}</td>
      <td class="text-xs-center">{{ props.item.categoryName }}</td>
      <td class="text-xs-center">{{ props.item.tag }}</td>
      <td class="text-xs-center">{{ props.item.readCount }}</td>
      <td class="text-xs-center">{{ props.item.likeCount }}</td>
      <td class="text-xs-center">{{ props.item.isPublished ? "已发布" : "未发布" }}</td>
      <td class="text-xs-center">{{ props.item.createTime }}</td>
      <td class="text-xs-center">{{ props.item.modifiedTime }}</td>
        <td class="justify-center layout px-0">
          <v-btn icon class="mx-0" @click="readItem(props.item)">
            <v-icon color="success">chat</v-icon>
          </v-btn>
          <v-btn icon class="mx-0" @click="publishItem(props.item)">
            <v-icon color="red">backup</v-icon>
          </v-btn>
          <v-btn icon class="mx-0" @click="editItem(props.item)">
            <v-icon color="teal">edit</v-icon>
          </v-btn>
          <v-btn icon class="mx-0" @click="deleteItem(props.item)">
            <v-icon color="pink">delete</v-icon> 
          </v-btn>
        </td>
    </template>
  </v-data-table>
  <div class="text-xs-right pt-2">
    <v-pagination v-model="pagination.page" :length="pages"></v-pagination>
  </div>

  <!-- 确认框 -->
  <x-confirm @recallTable="refreashTable"
             @recall="showDialog=!showDialog"
             :isShow="showDialog" 
             :actionType="actionType" 
             :primaryKey="primaryKey">
  </x-confirm>

  <!-- 新增 编辑对话框 -->
  <edit-article :articleId="articleIdd"
                :editDialog="editDialog"
                @shutdownEditDialog="shutdownEditDialog"
                @updateParentTable="refreashTable">
  </edit-article>
  
 </div>
</template>

<script>
import XSelect from "@/components/XSelect/index";
import XConfirm from "@/components/Confirm/index";
import EditArticle from "@/views/EditArticle/index";

export default {
  components: {
    XSelect,
    XConfirm,
    EditArticle
  },
  data() {
    return {
      articleIdd: 0,
      articleId: "",
      readDialog: false,
      editDialog: false,
      actionType: "",
      primaryKey: "",
      showDialog: false,
      loading: false,
      totalItems: 0,
      pagination: {
        rowsPerPage: 10,
        IsBackAdmin: true
      },
      headers: [
        {
          text: "标题",
          align: "left",
          sortable: false,
          value: "title"
        },
        { text: "分类", sortable: false, value: "categoryName" },
        { text: "标签", sortable: false, value: "tag" },
        { text: "阅读量", value: "readCount" },
        { text: "点赞次数", value: "likeCount" },
        { text: "是否发布", value: "isPublished" },
        { text: "创建日期", value: "createTime" },
        { text: "修改日期", value: "modifiedTime" },
        { text: "操作", value: "name", sortable: false }
      ],
      items: []
    };
  },
  computed: {
    pages() {
      if (
        this.pagination.rowsPerPage == null ||
        this.pagination.totalItems == null
      )
        return 0;

      return Math.ceil(
        this.pagination.totalItems / this.pagination.rowsPerPage
      );
    }
  },
  watch: {
    pagination: {
      handler() {
        this.refreashTable();
      },
      deep: true
    }
  },
  created() {},
  methods: {
    getDataFromApi() {
      this.loading = true;
      return new Promise((resolve, reject) => {
        let items = [];
        this.getList().then(res => {
          items = res.items;
          const total = res.total;
          setTimeout(() => {
            this.loading = false;
            resolve({
              items,
              total
            });
          }, 1000);
        });
      });
    },
    getList() {
      return new Promise((resolve, reject) => {
        let items;
        let total;
        this.$http
          .post(`${this.$domain}/api/Blog/getlist`, this.pagination)
          .then(res => {
            items = res.data.data.content;
            total = res.data.data.count;
            resolve({
              items,
              total
            });
          });
      });
    },
    refreshBlogList(item) {
      this.pagination.Category = item.value;
      this.refreashTable();
    },
    refreashTable() {
      this.getDataFromApi().then(data => {
        this.items = data.items;
        this.totalItems = data.total;
      });
    },
    shutdownEditDialog() {
      this.articleIdd = 0;
      this.editDialog = false;
    },
    shutdownDialog() {
      this.readDialog = false;
    },
    readItem(item) {
      //this.$router.push({ path: "/xxzz/article?id=" + item.id });
      window.open("/#/xxzz/article?id=" + item.id);
    },
    publishItem(item) {
      this.actionType = "PUBLISH";
      this.primaryKey = item.id;
      this.showDialog = true;
    },
    addItem() {
      this.articleIdd = 0;
      this.editDialog = true;
    },
    editItem(item) {
      this.articleIdd = item.id;
      this.editDialog = true;
    },
    deleteItem(item) {
      this.actionType = "DELETE";
      this.primaryKey = item.id;
      this.showDialog = true;
    }
  }
};
</script>

<style>
.input-group {
  padding: 0;
}
</style>
