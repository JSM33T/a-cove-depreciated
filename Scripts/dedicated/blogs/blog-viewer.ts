import { acToast, acInit, classesToTags, acPostData, acFormHandler, acGetData } from '../../global.js'
declare const bootstrap: any;

const gl_slug = document.getElementById("ip_slug") as HTMLInputElement;
const gl_tag = document.getElementById("ip_tags") as HTMLInputElement;

const likeStat = document.getElementById("likesStat") as HTMLElement;
const commentStat = document.getElementById("commentStat") as HTMLElement;
const likeIcon = document.getElementById("likeIcon") as HTMLElement;
const likeBtn = document.getElementById("likeBtn") as HTMLButtonElement;

const blogRole = document.getElementById("ip_blogrole") as HTMLInputElement;
const blogCount = document.getElementById("blogCount") as HTMLElement;
const commentsHolder = document.getElementById("commentsHolder") as HTMLElement;

let delModal = new bootstrap.Modal(document.getElementById('mdlDelete'));
const delbtn = document.getElementById("delConfirm") as HTMLButtonElement;

let editModal = new bootstrap.Modal(document.getElementById('mdlEdit'));
const editSaveBtn = document.getElementById('saveChanges') as HTMLButtonElement;

let replyModal = new bootstrap.Modal(document.getElementById('mdlReply'));
const replySaveBtn = document.getElementById('saveReply') as HTMLButtonElement;


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
    () => classesToTags('p', 'fs-lg'),
    () => acFormHandler('comment-form', addComment),
    () => delbtn.addEventListener('click', delConfirm),
    () => replySaveBtn.addEventListener('click', postReply),
    () => editSaveBtn.addEventListener('click', saveEdits)
]);

async function applyEvents() {
    let delbuttons = document.querySelectorAll('.deletecontent') as NodeListOf<HTMLButtonElement>;
    let editbuttons = document.querySelectorAll('.editcontent') as NodeListOf<HTMLButtonElement>;
    let replybuttons = document.querySelectorAll('.replyto') as NodeListOf<HTMLButtonElement>;


    //assign events to all the listed comments and replies
    delbuttons.forEach(function (button) {
        button.addEventListener('click', function () {
            let dataId = button.dataset.contentid;
            let dataType = button.dataset.type;
            delMdl(dataType, dataId);
        });
    });


    editbuttons.forEach(function (button) {
        button.addEventListener('click', function () {
            let dataId = button.dataset.contentid;
            let dataType = button.dataset.type;
            editMdl(dataType, dataId);
        })
    });

    replybuttons.forEach(function (button) {
        button.addEventListener('click', function () {
            let dataId = button.dataset.contentid;
            if (dataId) {
                replyMdl(dataId);
            }

        });
    });
    console.log("delete events attached");
}


//check if the current blog is likees by the logged in user
async function isLiked() {

    const data = {
        slug: global_slug
    };

    const isliked = await acPostData('/api/blog/likestat', data)
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

}

//load no of likes
async function loadLikes() {
    //get no of likes

    const likes = await acGetData('/api/blog/' + global_slug + '/likes');
    console.log(likes);

    if (likes.type == "ok") {
        likeStat.innerHTML = likes.data;
    }
    else {
        likeStat.innerHTML = "0";
    }



}

//add/remove like
async function addLike() {
    const likedata = {
        Slug: global_slug,
    }
    const resp = await acPostData('/api/blog/addlike', likedata);
    console.log(resp);

    loadLikes();
    isLiked();
}

//split and arrange authors from blog-model's author property
async function loadAuthors() {
    const apiUrl = '/api/blog/' + global_slug + '/authors';

    try {
        const response = await acGetData(apiUrl);

        if (response.type === 'ok') {
            const data = response.data;
            let authorsdat = "";

            if (data.length !== 0) {
                for (let i = 0; i < data.length; i++) {
                    authorsdat += '<a href="/author/' + data[i].userName + '" class="no-decor">' + data[i].firstName + ' ' + data[i].lastName + '</a>, ';
                }
                document.getElementById('authorsPlaceholder')!.innerHTML = authorsdat.slice(0, authorsdat.lastIndexOf(',')) + authorsdat.slice(authorsdat.lastIndexOf(',') + 1);
            }
        } else {
            console.error('Error:', response.data);
            document.getElementById('authorsPlaceholder')!.innerHTML = "--";
        }
    } catch (error) {
        console.error('Error:', error);
        document.getElementById('authorsPlaceholder')!.innerHTML = "--";
    }
}

//add comment and refresh comment span
async function addComment() {
    const blgcmnt = document.getElementById('blog_comment') as HTMLInputElement;
    if (blgcmnt.value.length < 2) {
        acToast("error", "comment too short");
    }
    else {
        const cmntBtn = document.getElementById('commentbutton') as HTMLButtonElement;
        cmntBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>posting.. ';
        cmntBtn.classList.add('pe-none');

        const commentData = {
            Slug: global_slug,
            Comment: blgcmnt.value,
        };
        const resp = await acPostData('/api/blog/comment/add', commentData);
        try {
            acToast(resp.type, resp.data);
            cmntBtn.innerHTML = 'Post Comment';
            loadComments();
            cmntBtn.classList.remove('pe-none');
            blgcmnt.value = "";

        } catch (error: any) {
            console.log("catch block" + error);
        }
        finally {
            cmntBtn.innerHTML = 'Post Comment';
            cmntBtn.classList.remove('pe-none');
        }
    }
}

//load comment span
async function loadComments() {
    const roleData = blogRole.value;
    const data = {
        Slug: global_slug
    };

    const response = await acPostData('/api/blog/comments/load', data)
    console.log(response);
    // response.data.sort((a, b) => a.id - b.id);
    let commentsHTML = "";
    let c = 0;
    if (response.data.length == 0) {
        commentsHTML = "";
    }
    else {
        for (let i = 0; i < response.data.length; i++) {
            const comment = response.data[i];
            c = i + 1;
            commentsHTML += `<div class="border-bottom py-4 mt-2 mb-4">
                                <div class="d-flex align-items-center pb-1 mb-3">
                                    <img class="rounded-circle" src="/assets/images/avatars/default/${comment.avatar}.png" width="48" alt="Comment author">
                                    <div class="ps-3">
                                        <h6 class="mb-0">${comment.fullname}</h6><span class="fs-sm text-muted">${comment.date}</span>
                                    </div>
                                </div>
                                <div class="btn-group-sm me-2 ml-2 mx-2" style="float:right;" role="group" aria-label="Settings group">
                                    ${roleData !== 'guest' ? `<button type="button" data-contentid="${comment.id}" class="replyto btn btn-secondary btn-icon px-2"><i class="ai-redo"></i></button>&nbsp;` : ''}
                                    ${comment.edit ? `<button type="button" data-type="comment" data-contentid="${comment.id}" class="editcontent btn btn-secondary btn-icon px-2"><i class="ai-edit"></i></button>&nbsp;` : ''}
                                    ${comment.edit ? `<button type="button" data-type="comment" data-contentid="${comment.id}" class="deletecontent btn btn-secondary btn-icon px-2"><i class="ai-trash"></i></button>&nbsp;` : ''}
                                </div>
                                <span class="pb-2 mb-0" id="comment_${comment.id}">${comment.comment}</span>
                            </div>
                            `;


            if (comment.replies && comment.replies.length > 0) {
                comment.replies.sort((a, b) => a.replyId - b.replyId);
                for (let j = 0; j < comment.replies.length; j++) {
                    const reply = comment.replies[j];
                    commentsHTML +=
                        `   <div class="card card-body border-0 bg-secondary mt-4">
                                <div class="d-flex align-items-center pb-1 mb-3">
                                    <img class="rounded-circle" src="/assets/images/avatars/default/${reply.replyAvatar}.png" width="48" alt="Comment author">
                                    <div class="ps-3">
                                        <h6 class="mb-0">${reply.replyFullName}</h6><span class="fs-sm text-muted">${reply.replyDate}</span>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center justify-content-between mb-3" role="group" aria-label="Settings group">
                                    <p class="mb-0"><a class="fw-bold text-decoration-none" href="#">@${comment.username}</a>&nbsp;&nbsp;<span id="reply_${reply.replyId}">${reply.replyComment}</span></p>
                                    <div>
                                        ${reply.replyEdit ? `<button type="button" data-type="reply" data-contentid="${reply.replyId}" data-action="edit" class="editcontent btn btn-sm btn-secondary btn-icon px-2"><i class="ai-edit"></i></button>&nbsp;` : ''}
                                        ${reply.replyEdit ? `<button type="button" data-type="reply" data-contentid="${reply.replyId}" class="deletecontent btn btn-sm btn-secondary btn-icon px-2"><i class="ai-trash"></i></button>&nbsp;` : ''}
                                    </div>
                                </div>
                            </div>
                            `;
                }
            }
            commentsHTML += `</div>`;
        }
    }
    commentsHolder.innerHTML = commentsHTML;
    blogCount.innerHTML = c.toString();
    applyEvents();

}

//load tags related to slug
async function loadTags() {
    var input = global_tags;
    var parts = input.split(',');
    var tags = "";
    parts.forEach(function (part) {
        var words = part.split(' ');
        for (var i = 0; i < words.length; i++) {
            tags = tags + `<a class="btn btn-outline-secondary btn-sm rounded-pill mt-2 ms-2" href="/blogs/browse/tag/${words[i]}">${words[i]}</a>`;

        }
    });
    document.getElementById('tagsPlaceholder')!.innerHTML = tags;
}


async function editMdl(contenttype: any, contentid: any) {
    // delModal!.show();
    const ip = document.getElementById('editIp') as HTMLTextAreaElement;
    let something: string = "";
    if (contenttype == "comment") {
        something = "comment_" + contentid;
    }
    else if (contenttype == "reply") {
        something = "reply_" + contentid;
    }

    let currComment = document.getElementById(something) as HTMLSpanElement;
    if (currComment) {
        ip.value = currComment.innerHTML;
        console.log(currComment.innerHTML);
    }
    localStorage.setItem('type', contenttype);
    localStorage.setItem('contentid', contentid);
    editModal!.show();
}

async function saveEdits() {
    let ipedits = document.getElementById('editIp') as HTMLTextAreaElement;
    let contentid = localStorage.getItem('contentid');
    let type = localStorage.getItem('type');
    let url = "/api/blog/" + type + "/edit";
    const data = {
        Id: contentid,
        comment: ipedits.value
    }
    let resp = await acPostData(url, data);
    console.log(data);
    console.log(resp);
    await loadComments();
    editModal!.hide();
}

async function replyMdl(contentid: string) {
    localStorage.setItem('action', 'reply');
    localStorage.setItem('contentid', contentid);
    replyModal!.show();
}

async function postReply() {
    let contentid = localStorage.getItem('contentid');
    let something = document.getElementById('txtarReply') as HTMLTextAreaElement;
    console.log(contentid + "\n" + something.value);
    try {
        const resp = await acPostData('/api/blog/reply/add', {
            CommentId: contentid?.trim(),
            ReplyText: something.value.trim(),
            Reply: something.value.trim()
        });
        acToast(resp.type, resp.data);
    }
    catch {
        acToast('error', 'something went wrong');
    }
    finally {
        replyModal!.hide();
        something.value = "";
        loadComments();
    }
}

async function delMdl(contenttype: any, contentid: any) {
    let c_type = document.getElementById("mdlContentType") as HTMLElement;
    if (contenttype == "comment") {
        c_type.innerHTML = "comment";
        delModal!.show();
    }
    else if (contenttype == "reply") {
        c_type.innerHTML = "reply";
        delModal!.show();
    }

    localStorage.setItem('content-type', contenttype);
    localStorage.setItem('content-id', contentid);
    localStorage.setItem('action', "delete");
}

async function delConfirm() {
    if (localStorage.getItem('action') == "delete") {
        let did = localStorage.getItem('content-type');
        const cid = localStorage.getItem('content-id');
        if (did == "comment") {
            const resp = await acPostData('/api/blog/comment/delete', {
                id: cid
            })
            delModal.hide();
            acToast(resp.type, resp.data)
        }
        else if (did == "reply") {
            const resp = await acPostData('/api/blog/reply/delete', {
                replyid: cid
            })
            delModal.hide();
            acToast(resp.type, resp.data)
        }
    }
    loadComments();
    localStorage.removeItem('action');
}

