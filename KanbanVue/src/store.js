import { reactive } from 'vue'

// function onServerError(error) {
//     alert(error);
// }
// class Event {
//     id = 0;
//     name = "";
//     creationDate = null;
//     creatorID = null;
//     creator = null;
//     fields = [];
// }
// class EventField {
//     id = 0;
//     eventID = 0;
//     name = "new field";
//     description = "";
//     type = 0;
//     value = "";
// }
function getAccessToken(resolve, reject) {
    let session = fromLocalStorage();
    if (session) {
        alert('TCS API session has been expired. Please sign in.');
        store.signOut();
        return;
    }
    let refresh_expires = new Date(session.refresh_expires);
    if (refresh_expires <= new Date()) {
        alert('TCS API session has been expired. Please sign in.');
        store.signOut();
        return;
    }

    let access_expires = new Date(session.access_expires);
    if (access_expires.getTime() >= ((new Date()).getTime() - 1000 * 30)) {
        resolve(session.access_token);
        return;
    }
    resolve("123");
    reject(321);

}
function fromLocalStorage() {
    var sessionStr = localStorage.getItem("kanban_session");
    if (sessionStr == null) return null;
    else
        return JSON.parse(sessionStr);
}


export const store = reactive({
    events: [],
    selectedEvent: null,
    showPanel: false,
    issue: null,
    isAuthenticated() {
        let a = fromLocalStorage();
        if (a)
            return true;
        else
            return false;
    },
    defaultRequest(method, path, body) {
        //this.isAuthenticated = true;
        return new Promise(function (resolve, reject) {
            getAccessToken(

                function (access_token) {
                    console.log(access_token)
                    let xhr = new XMLHttpRequest();
                    xhr.open(method, path, true);
                    //xhr.setRequestHeader("Authorization", "Bearer " + access_token);
                    xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
                    xhr.onload = function () {
                        if (xhr.status < 400) {
                            if (xhr.status >= 301) {
                                window.open(xhr.responseText);
                                //HideProgressMaster();
                                return;
                            }
                            if (this.responseType == "json") {
                                resolve(xhr.response);
                            } else {
                                resolve(xhr.responseText);
                            }
                        } else {
                            if (xhr.responseText) reject(xhr.responseText);
                            else reject(xhr.statusText);
                        }
                    };

                    xhr.onerror = function () {
                        reject('An error occurred during the transaction');
                    };
                    xhr.ontimeout = function () {
                        reject('Connection timeout');
                    };

                    if (body != null) xhr.send(typeof (body) == "object" ? JSON.stringify(body) : body);
                    else xhr.send();
                },
                function (error) {
                    reject(error);
                }
            );
        });
    },
    signUp(data) {
        return fetch('token/signup', {
            method: "POST",
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
        })
    },
    signIn(data) {
        return fetch('token', {
            method: "POST",
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
        })
            .then((response) => response.json())
            .then(function (response) {
                console.log(response);
                localStorage.setItem("kanban_session", JSON.stringify(response));
            });

    },
    revoke(access_token, refresh_token) {
        return fetch('token', {
            method: "DELETE",
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify({ access_token: access_token, refresh_token: refresh_token })
        })
            .then(function (response) {
                console.log(response);
            });
    },
    signOut() {
        let postRevokeAction = function () {
            localStorage.removeItem("kanban_session");
            window.location.href = process.env.BASE_URL;
        };

        let session = fromLocalStorage();
        if (session != null)
            this.revoke(session.access_token, session.refresh_token).
                then(postRevokeAction).catch(postRevokeAction);
        else
            postRevokeAction();
    },
    addIssue(data){
        this.issue = null;
        return fetch('issue/create',{
            method: "POST",
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
            
        })

    },
    editIssue(data){
        this.issue = data;
    },
    getListIssue(data){
        return fetch('list',{
            method: "POST",
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
        })
    },
    deleteIssue(data){
        return fetch('issue/delete',{
            method: "DELETE",
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
        })
    }
})