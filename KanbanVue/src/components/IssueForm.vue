<template>

    <div style="display: inline-block;">
        <table class="format-table-fields">
            <tr>
                <td class="label">Name</td>
                <td class="table-field"><input type="text" class="field" placeholder="Name issue"
                        v-model="issueRequest.name" /></td>
            </tr>
            <tr>
                <td class="label" style="padding-top: 0px;">Creator</td>
                <td class="table-field" style="padding-top: 0px;text-align: left;"><b>{{ currentUser.lastName }} {{
                        currentUser.firstName
                }}</b></td>
            </tr>
            <tr>
                <td class="label">Creation</td>
                <td class="table-field" style="padding-top: 10px;text-align: left;"><b>{{
                        formatdate(issueRequest.creationDate)
                }}</b></td>
            </tr>
            <tr>
                <td class="label">Worker</td>
                <td class="table-field" style="text-align: left;">
                    <select class="field" v-model="issueRequest.workerID">
                        <option :value="null">Select Worker</option>
                        <option v-for="(item, key) in users" :key="key" :value="item.id">{{ item.lastName }} {{
                                item.firstName
                        }}
                        </option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="label">StartDate</td>
                <td class="table-field">
                    <Datepicker class="field" v-model="issueRequest.startDate" :clearable="true"
                        placeholder="Start Date"></Datepicker>
                </td>
            </tr>
            <tr>
                <td class="label">FinishDate</td>
                <td class="table-field">
                    <Datepicker class="field" v-model="issueRequest.finishDate" :clearable="true"
                        placeholder="Finish Date"></Datepicker>
                </td>
            </tr>
            <tr>
                <td class="label">Status</td>
                <td class="table-field">
                    <select class="field" v-model="issueRequest.statusID">
                        <option v-for="(item, key) in statuses" :key="key" :value="item.id">{{ item.name }}
                        </option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="label">Description</td>
                <td class="table-field">
                    <textarea class="field" type="text" placeholder="Description"
                        v-model="issueRequest.description"></textarea>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="button" value="Save" class="button" v-on:click="onIssueClick"
                        style="margin-right: 5px;" />
                    <input type="button" value="Delete" class="button" v-if="issueRequest.id > 0"
                        v-on:click="onDeleteClick" style="margin-right: 5px;" />
                    <input type="button" value="Close" class="button" v-on:click="onCloseIssuePanel" />
                </td>
            </tr>
        </table>
    </div>
    <div style="display: inline-block;vertical-align: top;" v-if="issueRequest.taskTimes">
        <table class="data-table">
            <tr class="header-table">
                <td>Person</td>
                <td>Date</td>
                <td>Time</td>
                <td @click="onAddTime"> <a class="link">Spent</a></td>
            </tr>
            <tr v-for="(item, key) in issueRequest.taskTimes" v-bind:key="key">
                <td>{{ item.user.lastName }} {{ item.user.firstName }}</td>
                <td>{{ formatdate(item.date) }}</td>
                <td>{{ item.timeSpent }}</td>
                <td><a class="link" @click="onDeleteTime(item)">Delete</a></td>
            </tr>
        </table>
    </div>
</template>

<script lang="js">
import { defineComponent } from 'vue';
import { store } from '../store';
import moment from 'moment';
import Datepicker from 'vue3-datepicker'

export default defineComponent({
    data() {
        return {
            loading: false,
            post: null,
            issueRequest: {
                id: 0,
                name: null,
                creatorID: store.currentUser().id,
                creationDate: new Date(),
                workerID: null,
                startDate: null,
                finishDate: null,
                statusID: 1,
                description: null,
            },
        };
    },
    components: {
        Datepicker
    },
    computed: {
        statuses() {
            return store.statuses;
        },
        users() {
            return store.users;
        },
        currentUser() {
            return store.currentUser();
        }
    },
    created() {

    },
    watch: {

    },
    methods: {
        //Получение списка Issue
        onIssueClick() {
            let vm = this;
            store.addIssue(vm.issueRequest)
                .then(function () {
                    vm.onCloseIssuePanel();
                    store.getListIssue();
                });
        },
        //Отображение панели
        onAddTime() {
            store.showTimeSpentPanel = true;
        },
        //Метод для формирования Date
        formatdate(value) {
            if (value) {
                return moment(value).format('YYYY-MM-DD')
            }
        },
        //Обращение к APi для удаления списанного времени
        onDeleteTime(item) {
            store.deleteTaskTime(item)
            .then(function () {
                store.getListIssue();
                for (let index = 0; index < store.issue.taskTimes.length; index++) {
                    const element = store.issue.taskTimes[index];
                    if(element.id ==item.id)
                    {
                        store.issue.taskTimes.splice(index,1);
                    }
                }
            });
        },
        //Обращение к APi для удаления выбранной Issue
        onDeleteClick() {
            let vm = this;
            store.deleteIssue(vm.issueRequest)
                .then(function () {
                    vm.onCloseIssuePanel();
                    store.getListIssue();
                });
        },
        //Скрытие панели
        onCloseIssuePanel() {
            store.hideIssue();
        }
    },
    //Метод выполняющейся при загрузке страницы
    mounted: function () {
        store.getListUsers();
        if (store.issue != null)
            this.issueRequest = store.issue;
    }
});
</script>

<style>
.label {
    float: right;
    padding-top: 10px;
}

.table-field {
    text-align: left;
}
</style>