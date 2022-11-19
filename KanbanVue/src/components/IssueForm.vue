<template>

    <div class="post">
        <span>Name: </span>
        <input type="text" placeholder="Name issue" v-model="issueRequest.name" />
        <br />
        <span>CreatorID: </span>
        <input type="text" placeholder="Creator" v-model="issueRequest.creatorID" />
        <br />
        <span>CreationDate: </span>
        <input type="text" placeholder="Creation Date" v-model="issueRequest.creationDate" />
        <br />
        <span>WorkerID: </span>
        <input type="text" placeholder="Worker" v-model="issueRequest.workerID" />
        <br />
        <span>StartDate: </span>
        <input type="text" placeholder="Start Date" v-model="issueRequest.startDate" />
        <br />
        <span>FinishDate: </span>
        <input type="text" placeholder="Finish Date" v-model="issueRequest.finishDate" />
        <br />
        <span>StatusID: </span>
        <input type="text" placeholder="Status" v-model="issueRequest.statusID" />
        <br />
        <span>Description: </span>
        <input type="text" placeholder="Description" v-model="issueRequest.description" />
        <br />
        <input type="button" value="Add Issue" v-on:click="onIssueClick"/>
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
            issueRequest: {
                id: 0,
                name: null,
                creatorID: 1,
                creationDate: new Date(),
                workerID: null,
                startDate: new Date(),
                finishDate: new Date(),
                statusID: 1,
                description: null,
            },
        };
    },
    created() {

    },
    watch: {

    },
    methods: {
        onIssueClick() {
            let vm = this;
            store.addIssue(vm.issueRequest).then(()=>{
                this.$router.push("/SignIn");
            });
        }
    },
    mounted: function (){
        if(store.issue != null)
        this.issueRequest = store.issue;
        }
});
</script>