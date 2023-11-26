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
const gl_slug = document.getElementById("ip_slug");
const gl_tag = document.getElementById("ip_tags");
const likeStat = document.getElementById("likesStat");
const commentStat = document.getElementById("commentStat");
const likeIcon = document.getElementById("likeIcon");
const likeBtn = document.getElementById("likeBtn");
const blogRole = document.getElementById("ip_blogrole");
const blogCount = document.getElementById("blogCount");
const commentsHolder = document.getElementById("commentsHolder");
let delModal = new bootstrap.Modal(document.getElementById('mdlDelete'));
const delbtn = document.getElementById("delConfirm");
const global_slug = gl_slug.value;
const global_tags = gl_tag.value;
acInit([
    loadTags,
    loadAuthors,
    loadLikes,
    isLiked,
    loadComments,
    () => likeBtn.addEventListener('click', addLike),
    () => classesToTags('img', 'rounded-3'),
    () => acFormHandler('comment-form', addComment),
    () => delbtn.addEventListener('click', delConfirm)
]);
function applyEvents() {
    return __awaiter(this, void 0, void 0, function* () {
        let buttons = document.querySelectorAll('.deletecontent');
        buttons.forEach(function (button) {
            button.addEventListener('click', function () {
                let dataId = button.dataset.contentid;
                let dataType = button.dataset.type;
                delMdl(dataType, dataId);
            });
        });
    });
}
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
                document.getElementById('authorsPlaceholder').innerHTML = "--";
            }
        }
        catch (error) {
            console.error('Error:', error);
            document.getElementById('authorsPlaceholder').innerHTML = "--";
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
        const roleData = blogRole.value;
        const data = {
            Slug: global_slug
        };
        const response = yield acPostData('/api/blog/comments/load', data);
        console.log(response);
        let commentsHTML = "";
        let c = 0;
        if (response.data.length == 0) {
            commentsHTML = "";
        }
        else {
            for (let i = 0; i < response.data.length; i++) {
                const comment = response.data[i];
                c = i + 1;
                commentsHTML += '<div class="border-bottom py-4 mt-2 mb-4">' +
                    '<div class="d-flex align-items-center pb-1 mb-3"> <img class="rounded-circle" src="/assets/images/avatars/default/' + comment.avatar + '.png" width="48" alt="Comment author">' +
                    '<div class="ps-3">' +
                    '<h6 class="mb-0">' + comment.fullname + ' </h6><span class="fs-sm text-muted">' + comment.date + '</span>' +
                    '</div>' +
                    '</div>' +
                    '<div class="btn-group-sm me-2 ml-2 mx-2" style="float:right;" role="group" aria-label="Settings group">' +
                    (roleData !== 'guest' ? '<button type="button" id="repl_' + comment.id + '" onclick="cmntReply(' + comment.id + ')" class="btn btn-secondary btn-icon px-2"><i class="ai-redo"></i></button>&nbsp;' : '') +
                    (comment.edit ? '<button type="button" id="edt_' + comment.id + '" onclick="cmntEdit(' + comment.id + ')" class="btn btn-secondary btn-icon px-2"><i class="ai-edit"></i></button>&nbsp;' : '') +
                    (comment.edit ? '<button type="button" data-type="comment" data-contentid="' + comment.id + '" class="deletecontent btn btn-secondary btn-icon px-2"><i class="ai-trash"></i></button>&nbsp;' : '') +
                    '</div>' +
                    '<span class="pb-2 mb-0" id="cm_' + comment.id + '">' + comment.comment + '</span>';
                if (comment.replies && comment.replies.length > 0) {
                    comment.replies.sort((a, b) => a.replyId - b.replyId);
                    for (let j = 0; j < comment.replies.length; j++) {
                        const reply = comment.replies[j];
                        commentsHTML +=
                            '<div class="card card-body border-0 bg-secondary mt-4">' +
                                '    <div class="d-flex align-items-center pb-1 mb-3">' +
                                '        <img class="rounded-circle" src="/assets/images/avatars/default/' + reply.replyAvatar + '.png" width="48" alt="Comment author">' +
                                '        <div class="ps-3">' +
                                '            <h6 class="mb-0">' + reply.replyFullName + '</h6><span class="fs-sm text-muted">' + reply.replyDate + '</span>' +
                                '        </div>' +
                                '    </div>' +
                                '    <div class="d-flex align-items-center justify-content-between mb-3" role="group" aria-label="Settings group">' +
                                '        <p class="mb-0"><a class="fw-bold text-decoration-none" href="#">@@' + comment.username + '</a>&nbsp;&nbsp;<span id="reply_' + reply.replyId + '">' + reply.replyComment + '</span></p>' +
                                '        <div>' +
                                (reply.replyEdit ? '<button type="button" id="edt' + reply.replyId + '" onclick="replyEdit(' + reply.replyId + ')" class="btn btn-sm btn-secondary btn-icon px-2"><i class="ai-edit"></i></button>&nbsp;' : '') +
                                (reply.replyEdit ? '<button type="button" data-type="reply" data-contentid="' + reply.replyId + '" class="deletecontent btn btn-sm btn-secondary btn-icon px-2"><i class="ai-trash"></i></button>&nbsp;' : '') +
                                '        </div>' +
                                '    </div>' +
                                '</div>';
                    }
                }
                commentsHTML += `</div>`;
            }
        }
        commentsHolder.innerHTML = commentsHTML;
        blogCount.innerHTML = c.toString();
        applyEvents();
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
function delMdl(contenttype, contentid) {
    let c_type = document.getElementById("mdlContentType");
    if (contenttype == "comment") {
        c_type.innerHTML = "comment";
        delModal.show();
    }
    else if (contenttype == "reply") {
        c_type.innerHTML = "reply";
        delModal.show();
    }
    localStorage.setItem('content-type', contenttype);
    localStorage.setItem('content-id', contentid);
    localStorage.setItem('action', "delete");
}
function delConfirm() {
    return __awaiter(this, void 0, void 0, function* () {
        console.log("reached");
        if (localStorage.getItem('action') == "delete") {
            let did = localStorage.getItem('content-type');
            const cid = localStorage.getItem('content-id');
            if (did == "comment") {
                const resp = yield acPostData('/api/blog/comment/delete', {
                    id: cid
                });
                delModal.hide();
                acToast(resp.type, resp.data);
            }
            else if (did == "reply") {
                const resp = yield acPostData('/api/blog/reply/delete', {
                    replyid: cid
                });
                delModal.hide();
                acToast(resp.type, resp.data);
            }
        }
        loadComments();
        localStorage.removeItem('action');
    });
}
