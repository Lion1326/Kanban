<template>
    <div>
        <table style="margin: auto; border: 1px solid; border-top: white; border-spacing: 45px 10px; border-bottom-left-radius: 10px; border-bottom-right-radius: 10px;">
            <td>Open: {{openPrs}}%</td>
            <td>In progress: {{inProgressPrs}}%</td>
            <td>Done: {{donePrs}}%</td>
        </table>
    </div>
    <div class="kanban">
        <div class="kanban-column drop-zone" @drop="onDrop($event, 1)" @dragover.prevent @dragenter.prevent>
            <div class="kanban-column-title">Open</div>
            <div class="kanban-column-context">
                <div class="kanban-column-context-item drag-el" v-bind:style="format_date(item.finishDate) <= format_date(dateNow) ? 'background-color:#ff6b6b' : '' " v-for="(item, key) in issuesOpen" v-bind:key="key"
                    draggable="true" @dragstart="startDrag($event, item)" @click.stop="onIssueClick(item)">
                    <div style="overflow: hidden;">
                        <span style="  float: left;width: 60%;text-align: left;"> #{{item.id}}  {{ item.name }}</span>
                        <span style="  float: right;width: 40%;text-align: right;"> {{ calcSpentTime(item) }}</span>
                    </div>
                    <br>
                    <br>
                    <div style="overflow: hidden;">
                        <span style="  float: left;"> {{ format_date(item.creationDate) }}</span>
                        <span style="  float: right;" v-if="item.worker"> {{ item.worker.lastName }} {{
                                item.worker.firstName
                        }}</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="kanban-column drop-zone" @drop="onDrop($event, 2)" @dragover.prevent @dragenter.prevent>
            <div class="kanban-column-title">In Progress</div>
            <div class="kanban-column-context">
                <div class="kanban-column-context-item drag-el" v-bind:style="format_date(item.finishDate) <= format_date(dateNow) ? 'background-color:#ff6b6b' : '' " v-for="(item, key) in issuesInProgress" v-bind:key="key"
                    draggable="true" @dragstart="startDrag($event, item)" @click.stop="onIssueClick(item)">
                    <div style="overflow: hidden;">
                        <span style="  float: left;width: 60%;text-align: left;"> #{{item.id}}  {{ item.name }}</span>
                        <span style="  float: right;width: 40%;text-align: right;"> {{ calcSpentTime(item) }}</span>
                    </div>
                    <br>
                    <br>
                    <div style="overflow: hidden;">
                        <span style="  float: left;"> {{ format_date(item.creationDate) }}</span>
                        <span style="  float: right;" v-if="item.worker"> {{ item.worker.lastName }} {{
                                item.worker.firstName
                        }}</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="kanban-column drop-zone" @drop="onDrop($event, 3)" @dragover.prevent @dragenter.prevent>
            <div class="kanban-column-title">Done</div>
            <div class="kanban-column-context">
                <div class="kanban-column-context-item drag-el" v-bind:style="format_date(item.finishDate) <= format_date(dateNow) ? 'background-color:#ff6b6b' : '' " v-for="(item, key) in issuesDone" v-bind:key="key"
                    draggable="true" @dragstart="startDrag($event, item)" @click.stop="onIssueClick(item)">
                    <div style="overflow: hidden;">
                        <span style="  float: left;width: 60%;text-align: left;"> #{{item.id}}  {{ item.name }}</span>
                        <span style="  float: right;width: 40%;text-align: right;"> {{ calcSpentTime(item) }}</span>
                    </div>
                    <br>
                    <br>
                    <div style="overflow: hidden;">
                        <span style="  float: left;"> {{ format_date(item.creationDate) }}</span>
                        <span style="  float: right;" v-if="item.worker"> {{ item.worker.lastName }} {{
                                item.worker.firstName
                        }}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="js">
import { defineComponent } from 'vue';
import { store } from '../store'
import moment from 'moment'

export default defineComponent({
    name: 'SignIn',
    data() {
        return {
            loading: false,
            post: null,
            dateNow: new Date,
            openPrs: null,
            inProgressPrs: null,
            donePrs: null,
            signInRequest: {
                userName: '',
                password: ''
            }
        };
    },
    computed: {
        issuesOpen() {
            let vm = this;
            vm.openPrs = store.issues.filter(x => x.statusID == 1).length / store.issues.length * 100;
            return store.issues.filter(x => x.statusID == 1);
        },
        issuesInProgress() {
            let vm = this;
            vm.inProgressPrs = store.issues.filter(x => x.statusID == 2).length / store.issues.length * 100;
            return store.issues.filter(x => x.statusID == 2);
        },
        issuesDone() {
            let vm = this;
            vm.donePrs = store.issues.filter(x => x.statusID == 3).length / store.issues.length * 100;
            return store.issues.filter(x => x.statusID == 3);
        }
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
        //Подсчёт списанного времени
        calcSpentTime(item) {
            let result = 0;
            if (item.taskTimes && item.taskTimes.length>0) {
                for (let index = 0; index < item.taskTimes.length; index++) {
                    const element = item.taskTimes[index];
                    result+=Number(element.timeSpent);
                }
            }
            return  result;
        },
        //Отображение выбранной Issue
        onIssueClick(item) {
            store.editIssue(item);
        },
        //Метод для формировании Date
        format_date(value) {
            if (value) {
                return moment(String(value)).format('DD.MM.YYYY')
            }
        },
        startDrag(evt, item) {
            //evt.dataTransfer.dropEffect = 'move';
            //evt.dataTransfer.effectAllowed = 'move';
            evt.dataTransfer.setData('itemID', item.id);
        },
        onDrop(evt, list) {
            const itemID = evt.dataTransfer.getData('itemID');
            store.changeIssueStatus({ id: itemID, statusID: list })
                .then(function () {
                    store.getListIssue();
                });
        },
    },
    mounted() {
        store.getListIssue();
    },
});
</script>

<style>
.kanban {}

.kanban-column {
    display: inline-block;
    width: 29%;
    margin: 1%;
    border-radius: 3px;
    background-color: #eee;
    padding: 5px 1%;
    vertical-align: top;
    min-height: 500px;
}

.kanban-column-title {
    width: 100%;
    text-align: left;
    color: gray;
}

.kanban-column-context {}

.kanban-column-context-item {
    width: 99%;
    background-color: white;
    padding: 1%;
    margin-top: 13px;
    border-radius: 3px;
    box-shadow: 3px 3px 3px #b9b9b9;
    overflow: hidden;
    display: block;
}

.drop-zone {}

.drag-el {
    cursor: move;
}
</style>