<template>
    <v-select
      :items="items"
      v-model="select"
      single-line
      item-text="label"
      item-value="value"
      return-object
      :hint="`ArticleType --> ${select.label}`"
      persistent-hint
      @change="selectedChange"
    ></v-select>
</template>

<script>
export default {
  data() {
    return {
      select: { label: "All", value: 0 },
      items: [{ label: "All", value: 0 }]
    };
  },
  created() {
    this.getSelectList();
  },
  methods: {
    getSelectList() {
      this.$http.get(`${this.$domain}/api/Category/getSelectList`).then(res=>{
        res.data.data.forEach(item => {
          this.items.push(item)
        }); 
      })
    },
    selectedChange(val) {
      this.$emit("refresh", val);
    }
  }
};
</script>
