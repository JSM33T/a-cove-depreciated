function n(n,e,t,o,r,i,l){try{var a=n[i](l),c=a.value}catch(n){t(n);return}a.done?e(c):Promise.resolve(c).then(o,r)}function e(e){return function(){var t=this,o=arguments;return new Promise(function(r,i){var l=e.apply(t,o);function a(e){n(l,r,i,a,c,"next",e)}function c(e){n(l,r,i,a,c,"throw",e)}a(void 0)})}}import{acToast as t,acPostData as o,acFormHandler as r,acInit as i}from"../../global.js";let l=document.getElementById("userName"),a=document.getElementById("password"),c=document.getElementById("submitBtn");function u(){return s.apply(this,arguments)}function s(){return(s=e(function*(){let n=l.value,e=a.value;n.length<=2?t("error","Username should be at least 3 characters long."):e.length<6?t("error","Password should be at least 6 characters long."):yield function(n,e){return d.apply(this,arguments)}(n,e)})).apply(this,arguments)}function d(){return(d=e(function*(n,e){c.innerHTML="Loading...";try{let r=yield o("/api/account/login",{username:n,password:e});if("ok"===r.type){c.innerHTML="Logging in...",c.classList.add("pe-none");let n=localStorage.getItem("curr_link");n?window.location.href=n:window.location.href="/"}else t("error",r.data);console.log(r.data)}catch(n){console.error("Error during login:",n),t("error","Something went wrong")}finally{c.innerHTML="Log In",c.classList.remove("pe-none")}})).apply(this,arguments)}i([()=>r("login-form",u)]);
//# sourceMappingURL=login.js.map