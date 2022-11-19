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
import { reactive } from 'vue'


function getAccessToken(resolve, reject) {
    let session = fromLocalStorage();
    if (session) {
        alert('not');
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
    fetch('token', {
        method: "PUT",
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        body: JSON.stringify({ access_token: session.access_token, refresh_token: session.refresh_token })
    })
        .then(function (res) {
            if (res.status === 200) {
                res.json().then(function (json) {
                    session = json;
                    localStorage.setItem("kanban_session", JSON.stringify(json));
                    resolve(session.access_token);
                });
            } else {
                if (res.status < 400 && res.status >= 500) {
                    res.text().then(function (error) {
                        reject(error);
                    });
                } else {
                    alert(res.statusText);
                }
                reject(res);
            }
        });

}
function fromLocalStorage() {
    var sessionStr = localStorage.getItem("kanban_session");
    if (sessionStr == null) return null;
    else
        return JSON.parse(sessionStr);
}


export const store = reactive({
    issues: [{ name: "open 1", statusID: 1 }, { name: "open 2", statusID: 1 }, { name: "open 3", statusID: 1 }, { name: "InProgress 1", statusID: 2 }, { name: "InProgress 2", statusID: 2 }, { name: "InProgress 3", statusID: 2 }, { name: "Done 1", statusID: 3 }, { name: "Done 2", statusID: 3 }, { name: "Done 3", statusID: 3 }],
    checkOnAuthorization() {
        let session = fromLocalStorage();
        if (!session) {
            this.isAuthenticated = false;
            return false;
        }
        let refresh_expires = new Date(session.refresh_expires);
        if (refresh_expires <= new Date()) {
            this.isAuthenticated = false;
            return false;
        }

        let access_expires = new Date(session.access_expires);
        if (access_expires.getTime() <= new Date()) {
            this.isAuthenticated = false;
            return false;
        }
        this.isAuthenticated = true;
        return true;
    },
    isAuthenticated: false,
    onChangeIssueStatus(item){
        for(let i = 0;i<this.issues.length;i++)
        {
            if(this.issues[i].name==item.name){
                this.issues[i].statusID = item.statusID;
            }
        }
    },
    defaultRequest(method, path, body) {
        let str = this;
        return new Promise(function (resolve, reject) {
            getAccessToken(
                function (access_token) {
                    let xhr = new XMLHttpRequest();
                    xhr.open(method, path, true);
                    xhr.setRequestHeader("Authorization", "Bearer " + access_token);
                    xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
                    xhr.onload = function () {
                        if (xhr.status < 400) {
                            if (xhr.status >= 301) {
                                window.open(xhr.responseText);
                                //HideProgressMaster();
                                return;
                            }
                            str.isAuthenticated = true;
                            if (this.responseType == "json") {
                                resolve(xhr.response);
                            } else {
                                resolve(xhr.responseText);
                            }
                        } else {
                            str.isAuthenticated = false;
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
        let str = this;
        return fetch('token', {
            method: "POST",
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
        })
            .then((response) => response.json())
            .then(function (response) {
                str.isAuthenticated = true;
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
        return fetch('issue/create',{
            method: "POST",
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
        })
        .then(function (response) {
            console.log(response);
            console.log(new Date());
        });
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