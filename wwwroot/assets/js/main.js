function e(e,t,n,r,i,a,l){try{var s=e[a](l),o=s.value}catch(e){n(e);return}s.done?t(o):Promise.resolve(o).then(r,i)}function t(t){return function(){var n=this,r=arguments;return new Promise(function(i,a){var l=t.apply(n,r);function s(t){e(l,i,a,s,o,"next",t)}function o(t){e(l,i,a,s,o,"throw",t)}s(void 0)})}}import{acGetData as n,acInit as r,fetchJsonFile as i,Url as a}from"./global.js";let l=document.getElementById("share-btn"),s=document.getElementById("global_search");function o(){return(o=t(function*(){try{let e=yield i("/store/updates.json"),t=document.getElementById("updates-placeholder");if(!t)return;if(e&&e.updates)for(let n of e.updates)0==n.link.length||""==n.link?t.innerHTML=t.innerHTML+`
                    <dt>${n.type}:</dt>
                    <dd class="nav-link"> <span>${n.title}</span></dd>
                `:t.innerHTML=t.innerHTML+`
                    <dt>${n.type}:</dt>
                    <dd class="nav-link"><a href="${n.link}"><span> ${n.title}</span></a></dd>
                `;else console.error("Invalid or missing changelog data")}catch(e){console.error("Error fetching or processing changelog:",e)}})).apply(this,arguments)}function d(){return c.apply(this,arguments)}function c(){return(c=t(function*(){let e=new a,t=document.getElementById("link-placeholder");t.innerHTML=e.fullUrl;let n=document.getElementById("copy-btn");n.addEventListener("click",()=>{navigator.clipboard.writeText(t.innerHTML),n.innerHTML='<i class="ai-check me-2 ms-n1"></i>Copied!',setTimeout(()=>{n.innerHTML='<i class="ai-copy me-2 ms-n1"></i>Copy'},2e3)})})).apply(this,arguments)}function u(){return p.apply(this,arguments)}function p(){return(p=t(function*(){let e=document.getElementById("search_stat"),t=document.getElementById("search_results");if(s.value.length>=2){let r=yield n("/api/liversearch/all/"+s.value);if("error"==r.type)return;let i="";console.log(r);for(let e=0;e<r.data.length;e++)i+=`
            <div class="d-flex align-items-center border-bottom pb-4 mb-4 fade-in">
                <a class="position-relative d-inline-block flex-shrink-0 bg-secondary rounded-1" href="${r.data[e].url}">
                <img src="/assets/images/search_thumbs/${r.data[e].image}.svg" width="70" alt="Product" /></a>
                <div class="ps-3">
                    <h4 class="h6 mb-2"><a href="${r.data[e].url}">${r.data[e].title}</a></h4>
                    <span class="fs-sm text-muted ms-auto">${r.data[e].description}</span>'
                </div>
            </div>
            `;t.innerHTML=i.toString(),e.innerHTML="Search"}else t.innerHTML="",e.innerHTML="Search"})).apply(this,arguments)}r([()=>{l&&l.addEventListener("click",d)},()=>s.addEventListener("keyup",u),function(){return o.apply(this,arguments)}]);
//# sourceMappingURL=main.js.map