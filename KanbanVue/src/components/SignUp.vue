<template>
    <div class="modal-mask" style="background-color: rgb(165, 230, 255);">
        <div class="modal-wrapper">
            <div class="modal-container">
                <input type="text" class="field" placeholder="User name" v-model="signUpRequest.userName" />
                <br />
                <input type="text" class="field" placeholder="Password" v-model="signUpRequest.password" />
                <br />
                <input type="text" class="field" placeholder="Firstname" v-model="signUpRequest.firstName" />
                <br />
                <input type="text" class="field" placeholder="Lastname" v-model="signUpRequest.lastName" />
                <br />
                <input type="text" class="field" placeholder="Email" v-model="signUpRequest.email" />
                <br />
                <router-link class="link" to="/SignIn" style="margin-right: 5px;">Back</router-link>
                <input type="button" class="button" value="Registration" v-on:click="onSignUpClick" />
            </div>
        </div>
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
        //Обращение к API для регистрации юзера
        onSignUpClick() {
            let vm = this;
            store.signUp(vm.signUpRequest).then(() => {
                this.$router.push("/SignIn")
            });
        },
        //Вернуться на главную страницу
        onBackClick() {
            this.$router.push("/SignIn");
        }
    },
});
</script>