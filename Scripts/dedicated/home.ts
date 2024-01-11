import { Email } from '../Interfaces/email.interface.js';
import { acInit, acGetData, acPostData, acToast, validateEmail, prettifyDate } from '../global.js';

const mailForm = document.querySelector('.subscription-form') as HTMLFormElement;

acInit([
    loadTopBlogs,
    formSubmitEvent
])

async function formSubmitEvent() {
    mailForm.addEventListener('submit', async function (event) {
        event.preventDefault();

        const emailInput = document.getElementById('subscr-email') as HTMLInputElement;
        const emailData = {
            email: emailInput.value,
            origin: "HomePage",
        };

        await postEmailToAPI(emailData);

    });
}

async function postEmailToAPI(emailData: Email) {

    if (!validateEmail(emailData.email)) {
        acToast("error", "Invalid email")
        return;
    }
    const submitBtn = document.getElementById('submitMail') as HTMLButtonElement;
    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>&nbsp;loading...';
    const apiUrl = '/api/mailinglist/subscribe';
    const resp = await acPostData(apiUrl, emailData);
    try {
        acToast(resp.type, resp.data);
        mailForm.reset();
    } catch (error: any) {
        acToast(resp.type, error.data);
    }
    finally {
        submitBtn.textContent = "Subscribe";
    }
}

async function loadTopBlogs() {
    const resp = await acGetData("/api/topblogs/get");
    console.log(resp.data);
    try {
        await constructArticles(resp.data);
    } catch (error: any) {
        console.log(error.data);
    }
}


async function constructArticles(responsedata: any) {
    var articleTags = responsedata.map(post => `
        <article class="swiper-slide swiper-slide-active" role="group" aria-label="1/2" style="width: 416px; margin-right: 24px;">
            <a href="/blog/${post.datePosted.substring(0, 4)}/${post.urlHandle}">
                <img class="rounded-5" src="content/blogs/${post.datePosted.substring(0, 4)}/${[post.urlHandle]}/assets/cover.jpg" alt="Image">
            </a>
            <h3 class="h4 pt-4">
                <a href="/blog/${post.datePosted.substring(0, 4)}/${post.urlHandle}">${post.title}
                </a>
            </h3>
            <p>${post.description}
            </p>
            <div class="d-flex flex-wrap align-items-center pt-1 mt-n2">
                <a class="nav-link text-muted fs-sm fw-normal p-0 mt-2 me-3" href="/blog/${post.datePosted.substring(0, 4)}/${post.urlHandle}">
                ${post.likes}
                    <i class="ai-heart fs-lg ms-1"></i>
                </a>
                <a class="nav-link text-muted fs-sm fw-normal d-flex align-items-end p-0 mt-2" href="#">
                    ${post.comments}
                    <i class="ai-message fs-lg ms-1"></i>
                </a>
                <span class="fs-xs opacity-20 mt-2 mx-3">|</span><span class="fs-sm text-muted mt-2">${prettifyDate(post.datePosted)} </span><span class="fs-xs opacity-20 mt-2 mx-3">|</span > <a class="badge bg-faded-primary text-primary fs-xs mt-2" href = "/blogs/category/${post.locator}">${post.category}</a>
            </div>
        </article>
    `).join('');

    const artcl = document.getElementById('articlePlaceholder') as HTMLElement;
    if (artcl) {
        artcl.innerHTML = articleTags;
    }
}