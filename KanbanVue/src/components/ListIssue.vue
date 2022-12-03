<template>
    <div>
        <table class="data-table">
            <tr class="header-table">
                <td>ID</td>
                <td>Name task</td>
                <td>Worker</td>
                <td>Start date</td>
                <td>Finish date</td>
                <td>Status</td>
                <td>Description</td>
            </tr>
            <tr v-for="(item, key) in issueList" v-bind:key="key">
                <td>{{item.id}}</td>
                <td><a class="link" v-on:click="onIssueClick(item)">{{ item.name }}</a></td>
                <td><span v-if="item.worker">{{ item.worker.lastName }} {{ item.worker.firstName }}</span></td>
                <td>{{ formatdate(item.startDate) }}</td>
                <td v-bind:style="formatdate(item.finishDate) <= formatdate(dateNow) ? 'background-color:#ff6b6b' : '' " >{{ formatdate(item.finishDate) }}</td>
                <td>{{ statuses[item.statusID-1].name }}</td>
                <td>{{ item.description }}</td>
            </tr>
        </table>
    </div>
</template>

<script lang="js">
import { defineComponent } from 'vue';
import { store } from '../store';
import moment from 'moment';

export default defineComponent({
    name: 'ListIssue',
    data() {
        return {
            loading: false,
            post: null,
            dateNow: new Date,
        };
    },
    computed: {
        issueList() {
            return store.issues;
        },
        statuses() {
            return store.statuses;
        },
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
        //Отображение выбранной Issue
        onIssueClick(item) {
            store.editIssue(item);
        },
        //Метод для формировоания Date
        formatdate(value) {
            if (value) {
                return moment(String(value)).format('DD.MM.YYYY')
            }
        },
        //Обращение к APi для получения списка Issue
        loadData: function () {
            store.getListIssue();
        }
    },
    //Метод выполняющейся при загрузке страницы
    mounted: function () {
        let vm = this;
        vm.loadData();
    }
});
</script>