function t(t,e,a,s,n,r,i){try{var l=t[r](i),o=l.value}catch(t){a(t);return}l.done?e(o):Promise.resolve(o).then(s,n)}function e(e){return function(){var a=this,s=arguments;return new Promise(function(n,r){var i=e.apply(a,s);function l(e){t(i,n,r,l,o,"next",e)}function o(e){t(i,n,r,l,o,"throw",e)}l(void 0)})}}import{acInit as a,acGetData as s,acPostData as n,acToast as r,validateEmail as i,acFormHandler as l}from"../global.js";let o=document.querySelector(".subscription-form");function c(){return d.apply(this,arguments)}function d(){return(d=e(function*(){let t={email:document.getElementById("subscr-email").value,origin:"HomePage"};yield function(t){return u.apply(this,arguments)}(t)})).apply(this,arguments)}function u(){return(u=e(function*(t){if(!i(t.email)){r("error","Invalid email");return}let e=document.getElementById("submitMail");e.innerHTML='<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>&nbsp;loading...';let a=yield n("/api/mailinglist/subscribe",t);try{r("mail submitted",a.data),o.reset()}catch(t){r(a.type,t.data)}finally{e.textContent="Subscribe"}})).apply(this,arguments)}function m(){return(m=e(function*(){let t=yield s("/api/topblogs/get");console.log(t.data);try{yield function(t){return p.apply(this,arguments)}(t.data)}catch(t){console.log(t.data)}})).apply(this,arguments)}function p(){return(p=e(function*(t){var e=t.map(t=>`
        <article class="swiper-slide swiper-slide-active" role="group" aria-label="1/2" style="width: 416px; margin-right: 24px;">
            <a href="/blog/${t.datePosted.substring(0,4)}/${t.urlHandle}">
                <img class="rounded-5" src="content/blogs/${t.datePosted.substring(0,4)}/${[t.urlHandle]}/assets/cover.webp" alt="Image">
            </a>
            <h3 class="h4 pt-4">
                <a href="/blog/${t.datePosted.substring(0,4)}/${t.urlHandle}">${t.title}
                </a>
            </h3>
            <p>${t.description}
            </p>
            <div class="d-flex flex-wrap align-items-center pt-1 mt-n2">
                <a class="nav-link text-muted fs-sm fw-normal p-0 mt-2 me-3" href="/blog/${t.datePosted.substring(0,4)}/${t.urlHandle}">
                ${t.likes}
                    <i class="ai-heart fs-lg ms-1"></i>
                </a>
                <a class="nav-link text-muted fs-sm fw-normal d-flex align-items-end p-0 mt-2" href="#">
                    ${t.comments}
                    <i class="ai-message fs-lg ms-1"></i>
                </a>
                <span class="fs-xs opacity-20 mt-2 mx-3">|</span><span class="fs-sm text-muted mt-2">${t.dateFormatted} </span><span class="fs-xs opacity-20 mt-2 mx-3">|</span > <a class="badge bg-faded-primary border border-primary text-primary fs-xs mt-2" href = "/blogs/category/${t.locator}">${t.category}</a>
            </div>
        </article>
    `).join("");let a=document.getElementById("articlePlaceholder");a&&(a.innerHTML=e)})).apply(this,arguments)}a([function(){return m.apply(this,arguments)},()=>l("sub-form",c)]);
//# sourceMappingURL=home.js.map