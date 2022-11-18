<template>
    <div class="kanban">
        <div class="kanban-column drop-zone" @drop="onDrop($event, 1)" @dragover.prevent @dragenter.prevent>
            <div class="kanban-column-title">Open</div>
            <div class="kanban-column-context">
                <div class="kanban-column-context-item drag-el" v-for="(item, key) in issuesOpen" v-bind:key="key"
                    draggable="true" @dragstart="startDrag($event, item)">
                    {{ item.name }}
                </div>
            </div>
        </div>
        <div class="kanban-column drop-zone" @drop="onDrop($event, 2)" @dragover.prevent @dragenter.prevent>
            <div class="kanban-column-title">In Progress</div>
            <div class="kanban-column-context">
                <div class="kanban-column-context-item drag-el" v-for="(item, key) in issuesInProgress" v-bind:key="key"
                    draggable="true" @dragstart="startDrag($event, item)">
                    {{ item.name }}
                </div>
            </div>
        </div>
        <div class="kanban-column drop-zone" @drop="onDrop($event, 3)" @dragover.prevent @dragenter.prevent>
            <div class="kanban-column-title">Done</div>
            <div class="kanban-column-context">
                <div class="kanban-column-context-item drag-el" v-for="(item, key) in issuesDone" v-bind:key="key"
                    draggable="true" @dragstart="startDrag($event, item)">
                    {{ item.name }}
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="js">
import { defineComponent } from 'vue';
import { store } from '../store'

export default defineComponent({
    name: 'SignIn',
    data() {
        return {
            loading: false,
            post: null,
            signInRequest: {
                userName: '',
                password: ''
            }
        };
    },
    computed: {
        issuesOpen() {
            return store.issues.filter(x => x.statusID == 1);
        },
        issuesInProgress() {
            return store.issues.filter(x => x.statusID == 2);
        },
        issuesDone() {
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
        startDrag(evt, item) {
            //evt.dataTransfer.dropEffect = 'move';
            //evt.dataTransfer.effectAllowed = 'move';
            evt.dataTransfer.setData('itemID', item.name);
        },
        onDrop(evt, list) {
            const itemID = evt.dataTransfer.getData('itemID');
            store.onChangeIssueStatus({ name: itemID, statusID: list });
        },
    },
    mounted() {
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
}

.drop-zone {}

.drag-el {
    cursor: move;
    text-decoration-skip-ink: auto;
}
</style>