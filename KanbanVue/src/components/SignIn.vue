<template>
    <div class="modal-mask" style="background-color: rgb(165, 230, 255);">
        <div class="modal-wrapper">
            <div class="modal-container">
                <div>
                    <input type="text" class="field" style="width:200px" placeholder="User name"
                        v-model="signInRequest.userName" />
                    <br />
                    <input type="text" class="field" style="width:200px" placeholder="Password"
                        v-model="signInRequest.password" />
                </div>
                <div>
                    <router-link class="link" to="/SignUp" style="margin-right: 5px;">SignUp</router-link>
                    <input type="button" class="button" value="Sign in" v-on:click="onSignInClick" />
                </div>
            </div>
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
            store.signIn(vm.signInRequest)
                .then(() => { this.$router.push("/Main"); });

        }
    },
});
</script>