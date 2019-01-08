<template>
  <div>
    <Parent>
      <template slot="mymyslot" slot-scope="obj">
        你好 {{obj.xxname}}
      </template>
    </Parent>    
  </div>
</template>

<script>
import Parent from "@/learn/slot用法/parent.vue";

export default {
  components: {
    Parent
  },
  data() {
    return {
      message:"hello world!"
    };
  }
};
</script>

<style>

</style>
