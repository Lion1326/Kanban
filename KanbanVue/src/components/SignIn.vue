<template>
    <div class="post">
        <div>
            <input type="text" placeholder="User name" v-model="signInRequest.userName" />
            <br />
            <input type="text" placeholder="Password" v-model="signInRequest.password" />
        </div>
        <div>
            <router-link to="/SignUp">SignUp</router-link>
            <input type="button" value="Sign in" v-on:click="onSignInClick"/>
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import { store } from '../store'

    export default defineComponent({
        name: 'SignIn',
        data() {
            return {
                loading: false,
                post: null,
                signInRequest: {
                    userName: '',
                    password: ''
                }
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            //this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            //'$route': 'fetchData'
        },
        methods: {
            onSignInClick() {
                let vm = this;
                store.signIn(vm.signInRequest);
            }
        },
    });
</script>