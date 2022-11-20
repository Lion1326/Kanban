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
    if (session == null) {
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
            if (res.status == 200) {
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

function ApiUser(id, username, lastName, firstName) {
    return {
        username: username,
        id: id,
        lastName: lastName,
        firstName: firstName
    }
}
export const store = reactive({
    issue: null,
    issues: [],
    users: [],
    statuses: [{ id: 1, name: 'Open' }, { id: 2, name: 'In Progress' }, { id: 3, name: 'Done' }],
    showTimeSpentPanel: false,
    showIssuePanel: false,
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
            .then(function (res) {
                if (res.status == 200) {
                    return res.json().then(function (json) {
                        str.isAuthenticated = true;
                        localStorage.setItem("kanban_session", JSON.stringify(json));
                    });
                } else {
                    str.isAuthenticated = false;
                    return res.text().then(function (error) {
                        alert(error);
                    });
                }
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
    addIssue(data) {
        return this.defaultRequest("POST", "issues", data);
    },
    onShowIssuePanel(){
        this.showIssuePanel = true;
    },
    editIssue(data) {
        this.issue = data;
        this.showIssuePanel = true;
    },
    hideIssue() {
        this.issue = null;
        this.showIssuePanel = false;
    },
    getListIssue(data) {
        let str = this;
        return this.defaultRequest("POST", "issues/list", data)
            .then(function (response) {
                response = JSON.parse(response);
                response.forEach(element => {
                    element.creationDate = new Date(element.creationDate);
                    if (element.startDate)
                        element.startDate = new Date(element.startDate);
                    if (element.finishDate)
                        element.finishDate = new Date(element.finishDate);
                });
                str.issues = response;
                console.log(response);
            });
    },
    deleteIssue(data) {
        return this.defaultRequest("DELETE", "issues", data);
    },
    changeIssueStatus(data) {
        return this.defaultRequest("POST", "issues/statuschange", data);
    },
    getListUsers() {
        let str = this;
        return this.defaultRequest("GET", "users")
            .then(function (response) {
                str.users = JSON.parse(response);
            });
    },
    pushTaskTime(data){
        return this.defaultRequest("POST", "TaskTime", data);
    },
    deleteTaskTime(data){
        return this.defaultRequest("DELETE", "TaskTime",data);
    },
    currentUser() {
        let session = fromLocalStorage();
        if (session != null) {
            let base64Url = session.access_token.split('.')[1];
            let base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            let strPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));

            let jsonPayload = JSON.parse(strPayload);
            return ApiUser(
                jsonPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
                jsonPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
                jsonPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"],
                jsonPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"]
            );
        } else return {};
    }
})