<template>
  <div>
    <p class="card">
      <span style="color:red;">{{userInfo.nickName}}</span>
      <span style="font-size:10px;">{{userInfo.gender?'先生':'女士'}}，</span>
      田园天府欢迎您！
    </p>
  </div>
</template>

<script>
export default {
  props: ['userInfo']
}
</script>

<style>
.card {
  padding: 10px;
}
</style>
