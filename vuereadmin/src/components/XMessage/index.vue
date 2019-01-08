<template>
  <v-snackbar
      :timeout="timeout"
      :top="y === 'top'"
      :bottom="y === 'bottom'"
      :right="x === 'right'"
      :left="x === 'left'"
      :multi-line="mode === 'multi-line'"
      :vertical="mode === 'vertical'"
      :color="messageType"
      v-model="snackbar"
    >
    {{ message }}
    <v-btn flat dark @click.native="snackbar = false">
      <v-icon>close</v-icon>   
    </v-btn>
  </v-snackbar>
</template>

<script>
export default {
  props: ["snackbarP", "message", "messageType"],
  data() {
    return {
      timeout: 6000,
      snackbar: this.snackbarP,
      y: "top",
      x: null,
      mode: ""
    };
  },
  watch: {
    snackbarP: {
      handler() {
        this.snackbar = this.snackbarP;
      }
    }
  }
};
</script>

<style>

</style>
