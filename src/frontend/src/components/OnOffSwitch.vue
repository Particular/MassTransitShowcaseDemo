<script setup lang="ts">
defineProps<{
  id: string;
  label: string;
  value: boolean | null;
}>();

const emit = defineEmits<{ toggle: [] }>();
</script>

<template>
  <div class="withLabel">
    <label>{{ label }}</label>
    <div class="onoffswitch">
      <input
        type="checkbox"
        :id="`onoffswitch${id}`"
        :name="`onoffswitch${id}`"
        class="onoffswitch-checkbox"
        @click="emit('toggle')"
        :checked="value ?? false"
      />
      <label class="onoffswitch-label" :for="`onoffswitch${id}`">
        <span class="onoffswitch-inner"></span>
        <span class="onoffswitch-switch"></span>
      </label>
    </div>
  </div>
</template>

<style scoped>
.withLabel {
  display: flex;
  align-items: center;
  gap: 0.5em;
}

.onoffswitch {
  -moz-user-select: none;
  -ms-user-select: none;
  -webkit-user-select: none;
  user-select: none;
  position: relative;
  width: 76px;
}

.onoffswitch-checkbox {
  display: none;
}

.onoffswitch-label {
  border: 2px solid #929e9e;
  border-radius: 20px;
  cursor: pointer;
  display: block;
  overflow: hidden;
}

.onoffswitch-inner {
  display: block;
  margin-left: -100%;
  transition: margin 0.3s ease-in 0s;
  width: 200%;
}

.onoffswitch-inner:before,
.onoffswitch-inner:after {
  box-sizing: border-box;
  color: white;
  display: block;
  float: left;
  font-family: Trebuchet, Arial, sans-serif;
  font-size: 14px;
  font-weight: bold;
  height: 30px;
  line-height: 30px;
  padding: 0;
  width: 50%;
}

.onoffswitch-inner:before {
  background-color: #00a3c4;
  color: #ffffff;
  content: "ON";
  padding-left: 10px;
}

.onoffswitch-inner:after {
  background-color: #f2f6f7;
  color: #929e9e;
  content: "OFF";
  padding-right: 10px;
  text-align: right;
}

.onoffswitch-switch {
  background: #ffffff;
  border: 2px solid #929e9e;
  border-radius: 20px;
  bottom: 0;
  display: block;
  margin: 6px;
  position: absolute;
  right: 42px;
  top: 0;
  transition: all 0.3s ease-in 0s;
  width: 22px;
}

.onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-inner {
  margin-left: 0;
}

.onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-switch {
  right: 0;
}
</style>
