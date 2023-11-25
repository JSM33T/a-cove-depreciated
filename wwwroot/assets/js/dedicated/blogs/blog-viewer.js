var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { acToast, acInit, classesToTags, acPostData, acFormHandler, acGetData } from '../../global.js';
const tokenele = document.querySelector('input[name="__RequestVerificationToken"]');
const gl_slug = document.getElementById("ip_slug");
const gl_tag = document.getElementById("ip_tags");
const likeStat = document.getElementById("likesStat");
const commentStat = document.getElementById("commentStat");
const likeIcon = document.getElementById("likeIcon");
const likeBtn = document.getElementById("likeBtn");
const token = tokenele.value;
const global_slug = gl_slug.value;
const global_tags = gl_tag.value;
acInit([
    loadTags,
    loadAuthors,
    loadLikes,
    isLiked,
    () => likeBtn.addEventListener('click', addLike),
    () => classesToTags('img', 'rounded-3'),
    () => acFormHandler('comment-form', addComment)
]);
function isLiked() {
    return __awaiter(this, void 0, void 0, function* () {
        const data = {
            slug: global_slug
        };
        const isliked = yield acPostData('/api/blog/likestat', data);
        const isBlogLiked = isliked.data;
        if (isliked.type == "ok") {
            if (isBlogLiked == true) {
                likeIcon.classList.remove("ai-heart");
                likeIcon.classList.add("ai-heart-filled");
            }
            else {
                likeIcon.classList.remove("ai-heart-filled");
                likeIcon.classList.add("ai-heart");
            }
        }
        else {
            console.log("notliked + error");
        }
    });
}
function loadLikes() {
    return __awaiter(this, void 0, void 0, function* () {
        const likes = yield acGetData('/api/blog/' + global_slug + '/likes');
        console.log(likes);
        if (likes.type == "ok") {
            likeStat.innerHTML = likes.data;
        }
        else {
            likeStat.innerHTML = "0";
        }
    });
}
function addLike() {
    return __awaiter(this, void 0, void 0, function* () {
        const likedata = {
            Slug: global_slug,
        };
        const resp = yield acPostData('/api/blog/addlike', likedata);
        console.log(resp);
        loadLikes();
        isLiked();
    });
}
function loadAuthors() {
    return __awaiter(this, void 0, void 0, function* () {
        const apiUrl = '/api/blog/' + global_slug + '/authors';
        try {
            const response = yield acGetData(apiUrl);
            if (response.type === 'ok') {
                const data = response.data;
                let authorsdat = "";
                if (data.length !== 0) {
                    for (let i = 0; i < data.length; i++) {
                        authorsdat += '<a href="/author/' + data[i].userName + '" class="no-decor">' + data[i].firstName + ' ' + data[i].lastName + '</a>, ';
                    }
                    document.getElementById('authorsPlaceholder').innerHTML = authorsdat.slice(0, authorsdat.lastIndexOf(',')) + authorsdat.slice(authorsdat.lastIndexOf(',') + 1);
                }
            }
            else {
                console.error('Error:', response.data);
                document.getElementById('authorsPlaceholder').innerHTML = "Unable to fetch authors' data";
            }
        }
        catch (error) {
            console.error('Error:', error);
            document.getElementById('authorsPlaceholder').innerHTML = "Something went wrong while fetching authors' data";
        }
    });
}
function addComment() {
    return __awaiter(this, void 0, void 0, function* () {
        const blgcmnt = document.getElementById('blog_comment');
        if (blgcmnt.value.length < 2) {
            acToast("error", "comment too short");
        }
        else {
            const cmntBtn = document.getElementById('commentbutton');
            cmntBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>posting.. ';
            cmntBtn.classList.add('pe-none');
            const commentData = {
                Slug: global_slug,
                Comment: blgcmnt.value,
            };
            const resp = yield acPostData('/api/blog/comment/add', commentData);
            try {
                acToast(resp.type, resp.data);
                cmntBtn.innerHTML = 'Post Comment';
                loadComments();
                cmntBtn.classList.remove('pe-none');
                blgcmnt.value = "";
            }
            catch (error) {
                console.log("catch block" + error);
            }
            finally {
                cmntBtn.innerHTML = 'Post Comment';
                cmntBtn.classList.remove('pe-none');
            }
        }
    });
}
function loadComments() {
    return __awaiter(this, void 0, void 0, function* () {
        console.log("comment load function called");
        acToast('check', 'comment added ig check db');
    });
}
function loadTags() {
    var input = global_tags;
    var parts = input.split(',');
    var tags = "";
    parts.forEach(function (part) {
        var words = part.split(' ');
        for (var i = 0; i < words.length; i++) {
            tags = tags + `<a class="btn btn-outline-secondary btn-sm rounded-pill mt-2 ms-2" href="/blogs/browse/tag/${words[i]}">${words[i]}</a>`;
        }
    });
    document.getElementById('tagsPlaceholder').innerHTML = tags;
}
function injectClasses() {
    classesToTags('img', 'rounded-3');
}
