<template>
    <div>
        <table class="data-table">
            <tr class="field">
                <td>Name task</td>
                <td>Worker</td>
                <td>Start date</td>
                <td>Finish date</td>
                <td>Status</td>
                <td>Description</td>
            </tr>
            <tr v-for="(item, key) in issueList" v-bind:key="key" class="header-table">
                <td>{{item.name}}</td>
                <td>{{item.workerID}}</td>
                <td>{{formatdate(item.startDate)}}</td>
                <td>{{formatdate(item.finishDate)}}</td>
                <td>{{item.statusID}}</td>
                <td>{{item.description}}</td>
                <td>
                    <input type="button" class="button" value="Edit" v-on:click="onIssueClick(item)"/>
                    <input type="button" class="button" value="Delete" style="margin-left: 10px" v-on:click="onDeleteClick(item)"/>
                </td>
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
        };
    },
    computed: {
        issueList() {
            return store.issues;
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
        onIssueClick(item) {
            store.editIssue(item);
            this.$router.push("/IssueInfo");
        },
        onDeleteClick(item) {
            let vm = this;
            store.deleteIssue(item)
                .then(function () {
                    vm.loadData();
                });
                
            },
            formatdate(value){
                if (value) {
                    return moment(String(value)).format('DD.MM.YYYY')
                }
            },
            loadData: function () {
                store.getListIssue();
            }
        },
        mounted: function (){
            let vm = this;
            vm.loadData();
        }
    });
</script>