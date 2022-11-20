<template>
    <div>
        <table>
            <tr>
                <td>Date</td>
                <td>
                    <Datepicker v-model="taskTime.date" placeholder="Date" class="field"></Datepicker>
                </td>
            </tr>
            <tr>
                <td>Time (Minutes)</td>
                <td>
                    <input type="text" class="field" placeholder="Time minutes" v-model="taskTime.timeSpent" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="button" value="Save" class="button" v-on:click="onPushTime"
                        style="margin-right: 5px;" />
                    <input type="button" value="Close" class="button" v-on:click="onCloseTimePanel" />
                </td>
            </tr>
        </table>
    </div>
</template>

<script lang="js" >
import { store } from '../store'
import Datepicker from 'vue3-datepicker'

export default {
    compilerOptions: {
        isCustomElement: false
    },
    data() {
        return {
            loading: false,
            post: null,
            taskTime: {
                id: 0,
                taskID: store.issue.id,
                userID: store.currentUser().id,
                timeSpent: 0,
                date: new Date()
            }
        };
    },
    components: {
        Datepicker
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
        //Обращение к APi для добавление списанного времени
        onPushTime() {
            let vm = this;
            store.pushTaskTime(vm.taskTime).then(function (response) {
                response = JSON.parse(response);
                response.date = new Date(response.date);
                vm.onCloseTimePanel();
                store.issue.taskTimes.push(response)
                store.getListIssue();
            });
        },
        //Скрытие панели
        onCloseTimePanel() {
            store.showTimeSpentPanel = false;
        }
    },
};
</script>