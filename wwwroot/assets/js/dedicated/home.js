var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { acInit, acGetData, acPostData, acToast, validateEmail } from '../global.js';
const mailForm = document.querySelector('.subscription-form');
acInit([
    loadTopBlogs,
    formSubmitEvent
]);
function formSubmitEvent() {
    return __awaiter(this, void 0, void 0, function* () {
        mailForm.addEventListener('submit', function (event) {
            return __awaiter(this, void 0, void 0, function* () {
                event.preventDefault();
                const emailInput = document.getElementById('subscr-email');
                const emailData = {
                    email: emailInput.value,
                    origin: "HomePage",
                };
                yield postEmailToAPI(emailData);
            });
        });
    });
}
function postEmailToAPI(emailData) {
    return __awaiter(this, void 0, void 0, function* () {
        if (!validateEmail(emailData.email)) {
            acToast("error", "Invalid email");
            return;
        }
        const submitBtn = document.getElementById('submitMail');
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>&nbsp;loading...';
        const apiUrl = '/api/mailinglist/subscribe';
        const resp = yield acPostData(apiUrl, emailData);
        try {
            acToast(resp.type, resp.data);
            mailForm.reset();
        }
        catch (error) {
            acToast(resp.type, error.data);
        }
        finally {
            submitBtn.textContent = "Subscribe";
        }
    });
}
function loadTopBlogs() {
    return __awaiter(this, void 0, void 0, function* () {
        const resp = yield acGetData("/api/topblogs/get");
        try {
            yield constructArticles(resp.data);
        }
        catch (error) {
            console.log(error.data);
        }
    });
}
function constructArticles(responsedata) {
    return __awaiter(this, void 0, void 0, function* () {
        var articleTags = responsedata.map(post => `
        <article class="swiper-slide swiper-slide-active" role="group" aria-label="1/2" style="width: 416px; margin-right: 24px;">
            <a href="/blog/${post.datePosted.substring(0, 4)}/${post.urlHandle}">
                <img class="rounded-5" src="content/blogs/2023/${[post.urlHandle]}/assets/cover.jpg" alt="Image">
            </a>
            <h3 class="h4 pt-4">
                <a href="/blog/${post.datePosted.substring(0, 4)}/${post.urlHandle}">${post.title}
                </a>
            </h3>
            <p>${post.description}
            </p>
            <div class="d-flex flex-wrap align-items-center pt-1 mt-n2">
                <a class="nav-link text-muted fs-sm fw-normal p-0 mt-2 me-3" href="/blog/${post.datePosted.substring(0, 4)}/${post.urlHandle}">
                    3!
                    <i class="ai-heart fs-lg ms-1"></i>
                </a>
                <a class="nav-link text-muted fs-sm fw-normal d-flex align-items-end p-0 mt-2" href="#">
                    3!
                    <i class="ai-message fs-lg ms-1"></i>
                </a>
                <span class="fs-xs opacity-20 mt-2 mx-3">|</span><span class="fs-sm text-muted mt-2">${post.datePosted}</span><span class="fs-xs opacity-20 mt-2 mx-3">|</span><a class="badge bg-faded-primary text-primary fs-xs mt-2" href="/blogs/category/binge">Binge</a>
            </div>
        </article>
    `).join('');
        const artcl = document.getElementById('articlePlaceholder');
        if (artcl) {
            artcl.innerHTML = articleTags;
        }
    });
}
//# sourceMappingURL=home.js.map