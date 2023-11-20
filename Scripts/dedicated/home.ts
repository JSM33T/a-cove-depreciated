// main.ts
import { getData, submitMail } from '../global.js';

submitMail();
loadTopBlogs();

async function loadTopBlogs() {


    getData("/api/topblogs/get")
        .then(async (data) => {
            console.log('Data received:', data);
            await constructArticles(data);
        })
        .catch((error) => {
            console.error('Error:', error.message);
        });


}

async function constructArticles(responsedata:any)
{
    var articleTags = responsedata.map(post => `
        <article class="col">
            <div class="pb-4 pt-2 pt-xl-3 ms-md-3 border-bottom">
                <h3 class="h4">
                    <a href="/blog/${post.datePosted.substring(0,4)}/${post.urlHandle}">${post.title}</a>
                </h3>
                
                <p class="mb-4">${post.description}</p>
                <div class="d-flex align-items-center">
                    <span class="fs-sm text-body-secondary">${post.dateFormatted}</span>
                    <span class="fs-xs opacity-20 mx-3">|</span>
                    <a class="badge text-nav fs-xs border" href="#">${post.category}</a>
                </div>
            </div>
        </article>
    `).join('');


    const artcl = document.getElementById('articlePlaceholder');
    if (artcl) {
        artcl.innerHTML = articleTags;
    }
}