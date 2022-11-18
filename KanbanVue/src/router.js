import { createRouter, createWebHistory } from 'vue-router'
import SignIn from './components/SignIn.vue'
import SignUp from './components/SignUp.vue'
import Issue from './components/IssueForm.vue'
import { store } from './store'

const routes = [
    { path: '/SignIn', name: 'SignIn', component: SignIn, meta: { authorize: false } },
    { path: '/SignUp', name: 'SignUp', component: SignUp, meta: { authorize: false } },
    { path: '/Issue', name: 'Issue', component: Issue, meta: { authorize: false} },
    { path: '/Main', name: 'Main', component: SignUp, meta: { authorize: true } },
    {
        
        path: '/',
        redirect: () => {
          return { path: '/Main' }
        },
      },
]

const router = createRouter({
    // 4. Provide the history implementation to use. We are using the hash history for simplicity here.
    history: createWebHistory(process.env.BASE_URL),
    routes, // short for `routes: routes`
})
router.beforeEach((to,from,next) => {
    console.log(to);
    if (to.meta.authorize  && !store.isAuthenticated()) next({ path: 'SignIn' })
    else next()

    // const { authorize } = to.meta;
    // const currentUser = authenticationService.currentUserValue;
})

export default router