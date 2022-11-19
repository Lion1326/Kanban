<template>
    <div>
        <table>
            <tr class="field">
                <td>Name task</td>
                <td>Creator</td>
                <td>Create date</td>
                <td>Worker</td>
                <td>Start date</td>
                <td>Finish date</td>
                <td>Status</td>
                <td>Description</td>
            </tr>
            <tr v-for="(item, key) in issueList" v-bind:key="key">
                <td>{{item.name}}</td>
                <td>{{item.creatorID}}</td>
                <td>{{item.creationDate}}</td>
                <td>{{item.workerID}}</td>
                <td>{{item.startDate}}</td>
                <td>{{item.finishDate}}</td>
                <td>{{item.statusID}}</td>
                <td>{{item.description}}</td>
                <td>
                    <input type="button" value="Edit" v-on:click="onIssueClick(item)"/>
                    <input type="button" value="Delete" v-on:click="onDeleteClick(item)"/>
                </td>
            </tr>
        </table>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import { store } from '../store'

    export default defineComponent({
        name: 'ListIssue',
        data() {
            return {
                loading: false,
                post: null,
                issueList: null,
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
            onIssueClick(item) {
                store.editIssue(item);
                this.$router.push("/Issue");
            },
            onDeleteClick(item) {
                let vm = this;
                store.deleteIssue(item)
                .then (function (){
                    vm.loadData();
                });
                
            },
            loadData: function (){
                let vm = this;
                store.getListIssue()
                .then((response) => response.json())
                .then(function(response){
                    vm.issueList = response;
                    console.log(vm.issueList);
                });
            }
        },
        mounted: function (){
            let vm = this;
            vm.loadData();
        }
    });
</script>