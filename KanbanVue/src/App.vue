

<template>
    <div v-if="isAuthenticated" class="header">
        <input type="button" @click="onBoardClick" value="Board" class="button-signout"
            style="float: left;margin-left: 1%;" />
        <input type="button" @click="onTasksClick" value="Tasks" class="button-signout"
            style="float: left;margin-left: 1%;" />
        <input type="button" @click="onTaskClick" value="+" class="button-signout"
            style="float: left;margin-left: 1%;padding: 5px 10px;" />
        <input type="button" @click="onLogOutClick" value="Sign Out" class="button-signout"
            style="float: right;margin-right: 1%;" />
    </div>
    <router-view></router-view>
    <div class="modal-mask" style="background-color: rgba(165, 230, 255, .5);z-index: 999999;"
        v-if="showTimeSpentPanel">
        <div class="modal-wrapper">
            <div class="modal-container">
                <TimeSpent></TimeSpent>
            </div>
        </div>
    </div>
    <div class="modal-mask" style="background-color: rgba(165, 230, 255, .5);z-index: 999998;" v-if="showIssuePanel">
        <div class="modal-wrapper">
            <div class="modal-container">
                <IssueForm></IssueForm>
            </div>
        </div>
    </div>
</template>
<script>

import TimeSpent from './components/TimeSpent.vue'
import IssueForm from './components/IssueForm.vue'
import { store } from './store'
export default {
    name: 'app',
    data() {
        return {
            currentUser: null
        };
    },
    components: {
        TimeSpent,
        IssueForm
    },
    computed: {
        isAuthenticated() {
            return store.isAuthenticated;
        },
        showTimeSpentPanel() {
            return store.showTimeSpentPanel;
        },
        showIssuePanel() {
            return store.showIssuePanel;
        }
    },
    created() {

    },
    methods: {
        onBoardClick() {
            this.$router.push("/Main");
        },
        onTasksClick() {
            this.$router.push("/Issues");
        },
        onTaskClick() {
            store.onShowIssuePanel();
        },
        onLogOutClick() {
            store.signOut();
        }
    }
};
</script>

<style>
#app {
    font-family: Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
}

.header {
    width: 100%;
    display: block;
    height: 50px;
    padding-top: 20px;
    background-color: rgb(165, 230, 255);
}

.button-signout {
    background-color: white;
    color: #83dcff;
    border: none;
    padding: 5px 20px;
    cursor: pointer;
    font-size: 16px;
}

.button-signout:hover {
    color: #00b7ff;
}

.button {
    background-color: #83dcff;
    color: white;
    border: none;
    padding: 5px 20px;
    cursor: pointer;
}

.button:hover {
    background-color: #00b7ff;
}

.modal-mask {
    position: fixed;
    z-index: 999999;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, .5);
    display: table;
    transition: opacity .3s ease;
}

.modal-wrapper {
    display: table-cell;
    vertical-align: middle;
}

.modal-container {
    width: -moz-fit-content;
    width: fit-content;
    margin: 0px auto;
    padding: 20px 30px;
    background-color: #fff;
    border-radius: 2px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, .33);
    transition: all .3s ease;
}

.field {
    border: none;
    box-shadow: 1px 1px 4px 1px gray;
    padding: 6px;
    margin: 5px;
}

.field:focus {
    border: none;
    outline: none;
    box-shadow: 1px 1px 4px 1px #83dcff;
}

.field:active {
    border: none;
    outline: none;
    box-shadow: 1px 1px 4px 1px #83dcff;
}

.link {
    text-decoration: none;
    color: #83dcff;
    cursor: pointer;
}

.link:hover {
    outline: none;
    color: #00b7ff;
}

.--vdp-hover-bg-color {
    background-color: #83dcff;
}

.--vdp-selected-bg-color {

    background-color: #83dcff;
}

.data-table {
    border-spacing: 1px;
    empty-cells: show;
    border-collapse: collapse;
}
.data-table td{
    padding: 4px 10px;
    border: 1px solid #ccc;
}
.header-table {}

.header-table td {
    font-weight: bold;
    background-color: #eee;
}
</style>
