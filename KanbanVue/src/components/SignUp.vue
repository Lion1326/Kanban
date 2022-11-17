<template>
    <div class="post">
        <span>User name: </span>
        <input type="text" placeholder="User name" v-model="signUpRequest.userName" />
        <br />
        <span>Password: </span>
        <input type="text" placeholder="Password" v-model="signUpRequest.password" />
        <br />
        <span>Firtname: </span>
        <input type="text" placeholder="Password" v-model="signUpRequest.firstName" />
        <br />
        <span>Lastname: </span>
        <input type="text" placeholder="Password" v-model="signUpRequest.lastName" />
        <br />
        <span>Email: </span>
        <input type="text" placeholder="Password" v-model="signUpRequest.email" />
        <br />
        <input type="button" value="Registration" v-on:click="onSignUpClick"/>
    </div>
</template>

<script lang="js">
import { defineComponent } from 'vue';
import { store } from '../store'

export default defineComponent({
    data() {
        return {
            loading: false,
            post: null,
            signUpRequest: {
                userName: '',
                password: '',
                firstName: '',
                lastName: '',
                email: ''
            }
        };
    },
    created() {

    },
    watch: {

    },
    methods: {
        onSignUpClick() {
            let vm = this;
            store.signUp(vm.signUpRequest).then(()=>{
                this.$router.push("/SignIn")
            });
        }
    },
});
</script>