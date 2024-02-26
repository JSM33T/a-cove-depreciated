function e(e,s,a,t,l,o,r){try{var i=e[o](r),n=i.value}catch(e){a(e);return}i.done?s(n):Promise.resolve(n).then(t,l)}function s(s){return function(){var a=this,t=arguments;return new Promise(function(l,o){var r=s.apply(a,t);function i(s){e(r,l,o,i,n,"next",s)}function n(s){e(r,l,o,i,n,"throw",s)}i(void 0)})}}let a=document.querySelector("router-view"),t={props:["param1","param2"],watch:{$route(e,s){(e.params.param1!==s.params.param1||e.params.param2!==s.params.param2)&&(a&&(a.innerHTML=this.blogs),this.$nextTick(()=>{this.isLoading=!0,this.titleItem="Blogs '"+this.param2+"'",this.loadDefault(),window.scrollTo({top:0,behavior:"smooth"})}))}},mounted(){this.loadDefault("",""),window.scrollTo({top:0,behavior:"smooth"})},methods:{loadDefault(){return s(function*(){try{let e=(yield axios.get("/api/blogs/0/"+this.param1+"/"+this.param2)).data;this.blogs=e}catch(e){console.error("Error fetching data from API:",e)}finally{this.isLoading=!1}}).apply(this)}},data(){return{blogs:[],isLoading:!0,titleItem:"Blogs '"+this.param2+"'"}},template:`
        <h1 class="pb-3 pb-lg-4" id="titleBlog">{{titleItem}}</h1>
        <div v-if="isLoading">
            <article class="row g-0 border-0 mb-4">
                <a class="col-sm-5 rounded-5 placeholder placeholder-wave" style="min-height:14rem"></a>
                <div class="col-sm-7">
                    <div class="pt-4 pb-sm-4 ps-sm-4 pe-lg-4">
                        <p class="card-text placeholder-glow">
                            <span class="placeholder placeholder-sm col-7 me-2"></span>
                            <span class="placeholder placeholder-sm col-4"></span>
                            <span class="placeholder placeholder-sm col-4 me-2"></span>
                            <span class="placeholder placeholder-sm col-6"></span>
                            <span class="placeholder placeholder-sm col-8"></span>
                        </p>
                    </div>
                </div>
            </article>
        </div>
        <div v-else>
            <div v-for="blog in blogs" :key="blog.title">
                <article class="row g-0 border-0 mb-4 fade-in">
                    <a class="col-sm-5 bg-repeat-0 bg-size-cover bg-position-center rounded-5" v-bind:href="'/blog/' + blog.datePosted.substring(0,4) + '/' + blog.urlHandle " v-bind:style="{ 'background-image': 'url(https://res.cloudinary.com/dkpmezpui/image/upload/v1707656187/almondcoveassets/blogs/' + blog.datePosted.substring(0, 4) + '/' + blog.urlHandle + '/cover.webp)', 'min-height': '14rem' }"></a>
                    <div class="col-sm-7">
                        <div class="pt-4 pb-sm-4 ps-sm-4 pe-lg-4">
                            <h3>
                                <a v-bind:href="'/blog/' + blog.datePosted.substring(0, 4)+'/' + blog.urlHandle">
                                    {{blog.title}}
                                </a>
                            </h3>
                            <p class="d-sm-none d-md-block">{{blog.description}}</p><div class="d-flex flex-wrap align-items-center mt-n2"><a class="nav-link text-muted fs-sm fw-normal d-flex align-items-end p-0 mt-2" href="#">{{blog.comments}}<i class="ai-message fs-lg ms-1"></i></a><span class="fs-xs opacity-20 mt-2 mx-3">|</span><span class="fs-sm text-muted mt-2">{{blog.datePosted.substring(0, 7)}}</span><span class="fs-xs opacity-20 mt-2 mx-3">|</span><router-link class="badge text-nav fs-xs border mt-2" :to="'/blogs/category/'+  blog.locator ">{{blog.category}}</router-link></div>
                        </div>
                    </div>
                </article>
            </div>
        </div>
                            `},l={template:`
            <h1 class="pb-3 pb-lg-4" id="titleBlog">{{titleItem}}</h1>
            <div v-if="isLoading">
                <article class="row g-0 border-0 mb-4">
                    <a class="col-sm-5 rounded-5 placeholder placeholder-wave" style="min-height:14rem"></a>
                    <div class="col-sm-7">
                        <div class="pt-4 pb-sm-4 ps-sm-4 pe-lg-4">
                            <p class="card-text placeholder-glow"><span class="placeholder placeholder-sm col-7 me-2"></span><span class="placeholder placeholder-sm col-4"></span><span class="placeholder placeholder-sm col-4 me-2"></span><span class="placeholder placeholder-sm col-6"></span><span class="placeholder placeholder-sm col-8"></span></p>
                        </div>
                    </div>
                </article>
            </div>
            <div v-else>
                <div v-if="blogs.length > 0">
                    <div v-for="blog in blogs" :key="blog.title" class="fade-in">
                        <article class="row g-0 border-0 mb-4 ">
                            <a class="col-sm-5 bg-repeat-0 bg-size-cover bg-position-center rounded-5" v-bind:href="'/blog/' + blog.datePosted.substring(0,4) + '/' + blog.urlHandle " v-bind:style="{ 'background-image': 'url(https://res.cloudinary.com/dkpmezpui/image/upload/v1707656187/almondcoveassets/blogs/' + blog.datePosted.substring(0, 4) + '/' + blog.urlHandle + '/cover.webp)', 'min-height': '14rem' }"></a>
                            <div class="col-sm-7">
                                <div class="pt-4 pb-sm-4 ps-sm-4 pe-lg-4 ">
                                    <h3>
                                        <a v-bind:href="'/blog/' + blog.datePosted.substring(0, 4)+'/' + blog.urlHandle">
                                            {{blog.title}}
                                        </a>
                                    </h3>
                                    <p class="d-sm-none d-md-block">{{blog.description}}</p><div class="d-flex flex-wrap align-items-center mt-n2"><a class="nav-link text-muted fs-sm fw-normal d-flex align-items-end p-0 mt-2" href="#">{{blog.comments}}<i class="ai-message fs-lg ms-1"></i></a><span class="fs-xs opacity-20 mt-2 mx-3">|</span><span class="fs-sm text-muted mt-2">{{blog.datePosted.substring(0, 7)}}</span><span class="fs-xs opacity-20 mt-2 mx-3">|</span><router-link class="badge text-nav fs-xs border mt-2" :to="'/blogs/category/'+  blog.locator ">{{blog.category}}</router-link></div>
                                </div>
                            </div>
                        </article>
                    </div>
                </div>
                <div v-else>
                    <h1 class="">no blogs found!!</h1>
                </div>
            </div>
                            `,data:()=>({blogs:[],isLoading:!0,titleItem:"Blogs"}),mounted(){return s(function*(){window.scrollTo({top:0,behavior:"smooth"});try{let e=(yield axios.get("/api/blogs/0/na/na")).data;this.blogs=e,console.log(e)}catch(e){console.error("Error fetching data from API:",e)}finally{this.isLoading=!1}}).apply(this)},watch:{"_$route.query.search":{handler(e,a){var t=this;this.$nextTick(s(function*(){"string"==typeof e&&e.length>=1?(t.loadSearches(e),t.titleItem="Blogs '"+e+"'"):(t.loadDefaults(),t.titleItem="Blogs")}))},immediate:!0},get"$route.query.search"(){return this["_$route.query.search"]},set"$route.query.search"(value){this["_$route.query.search"]=value}},methods:{loadDefaults(){return s(function*(){try{let e=(yield axios.get("/api/blogs/0/na/na")).data;this.blogs=e,console.log("default triggered")}catch(e){console.error("Error fetching data from API:",e)}finally{this.isLoading=!1}}).apply(this)},loadSearches(e){return s(function*(){try{this.titleItem="Searching";let s=(yield axios.get("/api/blogs/0/search/"+e)).data;this.blogs=s}catch(e){console.error("Error fetching data from API:",e)}finally{this.isLoading=!1}}).apply(this)}}},o=VueRouter.createRouter({history:VueRouter.createWebHistory(),routes:[{path:"/blogs",component:l},{path:"/blogs/:param1/:param2",component:t,props:!0}]}),r=Vue.createApp({data:()=>({isLoading:!0,titleItem:"Blogs",categories:[],inputValue:""}),mounted(){return s(function*(){this.loadCategories()}).apply(this)},methods:{navigateToBlog(){return s(function*(){this.$nextTick(()=>{this.inputValue.length>=1?(this.$router.push({path:"/blogs",query:{search:this.inputValue}}),this.titleItem="searching"):this.$router.push({path:"/blogs"})})}).apply(this)},loadCategories(){return s(function*(){try{let e=(yield axios.get("/api/blogs/categories/load")).data;this.categories=e}catch(e){console.error("Error fetching data from API:",e)}finally{this.isLoading=!1}}).apply(this)}}});r.use(o),r.mount("#app");export{};
//# sourceMappingURL=index.js.map