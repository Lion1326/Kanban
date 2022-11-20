<template>
    <div>
        <div style="margin-top: 30px">
            <span>Issue name</span>
            <input type="text" style="margin-left: 30px;" v-model="issueInfo.name" />
            <br />
            <span>Creator</span>
            <input type="text" style="margin-left: 59px; margin-top: 10px;" v-model="issueInfo.creatorID" />
            <br />
            <span>Creation date</span>

            <br />
            <span>Worker</span>
            <input type="text" style="margin-left: 61px; margin-top: 10px;" v-model="issueInfo.workerID" />
            <br />
            <span>Start date</span>
            <input type="text" style="margin-left: 43px; margin-top: 10px;" v-model="issueInfo.startDate" />
            <br />
            <span>Finish date</span>
            <input type="text" style="margin-left: 34px; margin-top: 10px;" v-model="issueInfo.finishDate" />
            <br />
            <span>Status</span>
            <input type="text" style="margin-left: 67px; margin-top: 10px;" v-model="issueInfo.statusID" />
            <br />
            <span>Description</span>
            <input type="text" style="margin-left: 31px; margin-top: 10px;" v-model="issueInfo.description" />
            <br />

        </div>
        <div style="margin-top: 20px">
            <input type="button" class="button" value="Save" v-on:click="onIssueClick(issueInfo)" />
            <input type="button" class="button" value="Delete" style="margin-left: 10px;"
                v-on:click="onDeleteClick(issueInfo)" />
            <input type="button" class="button" value="Cansel" style="margin-left: 10px;" />
        </div>
        
    </div>
</template>

<script lang="js">
import { store } from '../store';
import moment from 'moment';
//import Datepicker from 'vue3-datepicker'

export default {

    name: 'ListIssue',
    data() {
        return {
            loading: false,
            post: null,
            issueInfo: {
                id: 0,
                name: null,
                creatorID: null,
                creationDate: null,
                workerID: null,
                startDate: null,
                finishDate: null,
                statusID: null,
                description: null,
            },
            showTimeSpentPanel: false
        };
    },
    components: {
        //Datepicker,
        //'bdl-TimeSpent': TimeSpent
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
        //Обращение к APi для добавления Issue и перехода на главную страницу
        onIssueClick(issue) {
            store.editIssue(issue);
            this.$router.push("/Issue");
        },
        //Обращение к APi для удаление Issue
        onDeleteClick(issue) {
            let vm = this;
            store.deleteIssue(issue)
                .then(function () {
                    vm.loadData();
                });

        },
        //Метод для формирования Date
        formatdate(value) {
            if (value) {
                return moment(String(value)).format('DD.MM.YYYY')
            }
        },
        //Отображение панели
        onAddTime() {
            store.showTimeSpentPanel = true;
        }
    },
    //Метод выполняющейся при загрузке страницы
    mounted: function () {
        this.issueInfo = store.issue;
    }
};
</script>