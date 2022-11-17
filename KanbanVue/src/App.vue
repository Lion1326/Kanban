

<template>
    <br />
    <router-view></router-view>
</template>
<script>

export default {
    name: 'app',
    data () {
        return {
            currentUser: null
        };
    },
    computed: {
        
    },

    created () {
       
    },
    methods: {

    }
};
</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }
</style>
