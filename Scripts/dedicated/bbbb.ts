declare const axios: any;
declare const toaster: any;
declare const bootstrap: any;

document.addEventListener('DOMContentLoaded', () => {
    loadComments();
    loadTags();
    loadAuthors();

    const closeBtns = document.querySelectorAll('.mdl-close');
    closeBtns.forEach(button => {
        button.addEventListener('click', () => {
            mdlCloseAll();
        });
    });
});

const tokenele = document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement;
const token = tokenele.value;
const config = {
    headers: {
        "Content-Type": "application/json",
        "RequestVerificationToken": token
    }
};
function loadAuthors() {
    axios.get('/api/blog/@blogDeet.Slug/authors')
        .then(response => {
            const data = response.data;
            var authorsdat = "";
            var authorsdat2 = "";
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {

                    authorsdat2 = authorsdat2 + '<a href="/author/' + data[i].userName + '" class="no-decor">' + data[i].firstName + ' ' + data[i].lastName + '</a>, ';
                }
                document.getElementById('authorsPlaceholder2')!.innerHTML = authorsdat2.slice(0, authorsdat2.lastIndexOf(',')) + authorsdat2.slice(authorsdat2.lastIndexOf(',') + 1);
            }
        })
        .catch(error => {
            console.error('Error:', error);
            document.getElementById('authorsPlaceholder2')!.innerHTML = "unable to fetch authors' data";
        });
}

function addcomment() {
    const blgcmnt = document.getElementById('blog_comment') as HTMLInputElement;
    if (blgcmnt.value == "") {
        toaster("error", "comment too short");
    }
    else {
        const cmntBtn = document.getElementById('commentbutton');
        cmntBtn!.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>posting.. ';
        cmntBtn!.classList.add('pe-none');

        axios({
            method: 'POST',
            url: '/api/blog/comment/add',
            headers: {
                'Content-Type': 'application/json',
            },
            data: {
                Slug: "@blogDeet.Slug",
                Comment: blgcmnt.value,
            },
        })
            .then(function (response) {
                cmntBtn!.innerHTML = 'Post Comment';
                loadComments();
                cmntBtn!.classList.remove('pe-none');
                toaster('success', 'Comment added');
                blgcmnt!.value = "";
            })
            .catch(function (error) {
                console.error('Error:', error);
                cmntBtn!.innerHTML = 'Post Comment';
                cmntBtn!.classList.remove('pe-none');
                toaster('error', 'Something went wrong');
            });
    }


}

function cmntEdit(id) {
    localStorage.removeItem("editid");
    localStorage.setItem('editid', id);
    const v = localStorage.getItem('editid');
    //$('#mdlEdit').modal('show');
    const bedit = document.getElementById("blog_commentEdit") as HTMLInputElement;
    mdlOpen('mdlEdit');
    var cmntid = "cm_" + id;
    var p = document.getElementById(cmntid)!.innerHTML;
    bedit!.value = p;
    if (p == "" || p == null) {
        bedit!.value = p;
        alert("entity");
    }
}
function replyEdit(id) {
    localStorage.removeItem("replyid");
    localStorage.setItem('replyid', id);
    const v = localStorage.getItem('replyid');
    //$('#mdlReply').modal('show');
    mdlOpen('mdlReply');
    const repedit = document.getElementById("blog_replyEdit") as HTMLInputElement;
    var replid = "reply_" + id;
    var p = document.getElementById(replid)!.innerHTML;
    repedit!.value = p;
    if (p == "" || p == null) {
        repedit!.value = p;
        alert("entity");
    }
}
function cmntEditSave() {
    var editId = localStorage.getItem('editid');
    var comment = document.querySelector("#blog_commentEdit") as HTMLInputElement;
    axios.post('/api/blog/comment/edit', {
        id: editId,
        comment: comment.value
    })
        .then(function (response) {
            console.log(response.data);
            loadComments();
            comment.value = "";
            //$('#mdlEdit').modal('hide');
            mdlClose('mdlEdit');
            localStorage.removeItem("editid");
            toaster("success", "changes saved");
        })
        .catch(function (error) {
            console.error(error);
            toaster("error", "something went wrong");
        });
}


function replyEditSave() {
    let replyId = localStorage.getItem('replyid');
    let editedReply = document.getElementById('blog_replyEdit') as HTMLInputElement;

    axios({
        method: 'POST',
        url: '/api/blog/reply/edit',
        headers: {
            'Content-Type': 'application/json',
        },
        data: {
            replyId: replyId,
            reply: editedReply.value,
        },
    })
        .then(function (response) {
            loadComments();
            mdlClose('mdlReply');
            editedReply.value = '';
            toaster('Success', 'Reply edited successfully');
            localStorage.removeItem('replyid');
        })
        .catch(function (error) {
            console.log(error);
            toaster('error', 'Something went wrong');
        });

}

export function cmntDelete(id) {
    localStorage.setItem('delid', id);
    const v = localStorage.getItem('delid');
    //$('#mdlDelete').modal('show');
    mdlOpen('mdlDelete');
}

function cmntDeleteConfirm() {
    axios.post('/api/blog/comment/delete', {
        id: localStorage.getItem('delid')
    })
        .then(function (response) {
            console.log(response.data);
            loadComments();
            //$('#mdlEdit').modal('hide');
            //$('#mdlDelete').modal('hide');
            mdlClose('mdlEdit');
            mdlClose('mdlDelete');

            localStorage.removeItem('delid');
            toaster("success", "comment deleted");
        })
        .catch(function (error) {
            console.error(error);
            //$('#mdlDelete').modal('hide');
            mdlClose('mdlDelete');
            localStorage.removeItem('delid');
            toaster("error", "something went wrong");
        })
}


function cmntReply(id) {
    localStorage.setItem('replyto', id);
    const v = localStorage.getItem('replyto')
    const replbx = document.getElementById("addReplybox") as HTMLInputElement;
    replbx.value = "";

    mdlOpen('mdladdReply');
}

function mdlOpen(mdlId) {
    document.getElementById("backdrop")!.style.display = "block"
    document.getElementById(mdlId)!.style.display = "block"
    document.getElementById(mdlId)!.classList.add("show")
    var modal = document.getElementById(mdlId);
    window.onclick = function (event) {
        if (event.target == modal) {
            mdlClose(mdlId)
        }
    }
}

function mdlClose(mdlId) {
    document.getElementById("backdrop")!.style.display = "none"
    document.getElementById(mdlId)!.style.display = "none"
    document.getElementById(mdlId)!.classList.remove("show")
}
function mdlCloseAll() {
    const openModal = document.querySelector('.modal.show');
    if (openModal) {
        const modalId = openModal.getAttribute('id');
        const modal = new bootstrap.Modal(openModal);
        modal.hide();
        mdlClose(modalId);
    }
}



function replAdd() {
    const repltext = document.getElementById("addReplybox") as HTMLInputElement;
    axios({
        method: "POST",
        url: "/api/blog/reply/add",
        headers: {
            "Content-Type": "application/json",
        },
        data: {
            Slug: "@blogDeet.Slug",
            ReplyText: repltext.value,
            CommentId: localStorage.getItem("replyto"),
        },
    })
        .then(function (response) {
            loadComments();
            mdlClose("mdladdReply");
            toaster("Success", "Reply added");
        })
        .catch(function (error) {
            console.error("Error:", error);
            toaster("Error", error.response.data);
        });
}

function replyDelete(id) {
    localStorage.setItem('delid', id);
    const v = localStorage.getItem('delid');
    mdlOpen("mdlreplyDelete");
}


function loadTags() {
    var input = '@blogDeet.Tags';
    var parts = input.split(',');
    var tags = "";
    parts.forEach(function (part) {
        var words = part.split(' ');
        for (var i = 0; i < words.length; i++) {
            tags = tags + `<a class="badge bg-secondary rounded-pill pt-2 py-2 mt-2 ms-2" href="/blogs/tag/${words[i]}">${words[i]}</a>`;
        }
    });
    document.getElementById('tagsPlaceholder')!.innerHTML = tags;
}

function loadComments() {
//    const roleData = "@blogDeet.Role";
//    let c = 0;
//    const data = {
//        Slug: '@blogDeet.Slug'
//    };

//    axios.post("/api/blog/comments/load", data)
//        .then((response) => {
//            response.data.sort((a, b) => a.id - b.id);
//            let commentsHTML = "";
//            if (response.data.length == 0) {
//                commentsHTML = "";
//            }
//            else {
//                for (let i = 0; i < response.data.length; i++) {
//                    const comment = response.data[i];
//                     c = i + 1;
//                    commentsHTML += '<div class="border-bottom py-4 mt-2 mb-4">' +
//                        '<div class="d-flex align-items-center pb-1 mb-3"> <img class="rounded-circle" src="/assets/images/avatars/default/' + comment.avatar + '.png" width="48" alt="Comment author">' +
//                        '<div class="ps-3">' +
//                        '<h6 class="mb-0">' + comment.fullname + ' </h6><span class="fs-sm text-muted">' + comment.date + '</span>' +
//                        '</div>' +
//                        '</div>' +
//                        '<div class="btn-group-sm me-2 ml-2 mx-2" style="float:right;" role="group" aria-label="Settings group">' +
//                        (roleData != 'guest' ? '<button type="button" id="repl_' + comment.id + '" onclick="cmntReply(' + comment.id + ')" class="btn btn-secondary btn-icon px-2"><i class="ai-redo"></i></button>&nbsp;' : '') +
//                        (comment.edit ? '<button type="button" id="edt_' + comment.id + '" onclick="cmntEdit(' + comment.id + ')" class="btn btn-secondary btn-icon px-2"><i class="ai-edit"></i></button>&nbsp;' : '') +
//                        (comment.edit ? '<button type="button" id="dlt_' + comment.id + '" onclick="cmntDelete(' + comment.id + ')" class="btn btn-secondary btn-icon px-2"><i class="ai-trash"></i></button>&nbsp;' : '') +
//                        '</div>' +
//                        '<span class="pb-2 mb-0" id="cm_' + comment.id + '">' + comment.comment + '</span>';

//                    if (comment.replies && comment.replies.length > 0) {
//                        comment.replies.sort((a, b) => a.replyId - b.replyId);
//                        for (let j = 0; j < comment.replies.length; j++) {
//                            const reply = comment.replies[j];
//                            commentsHTML +=
//                                '<div class="card card-body border-0 bg-secondary mt-4">' +
//                                '    <div class="d-flex align-items-center pb-1 mb-3">' +
//                                '        <img class="rounded-circle" src="/assets/images/avatars/default/' + reply.replyAvatar + '.png" width="48" alt="Comment author">' +
//                                '        <div class="ps-3">' +
//                                '            <h6 class="mb-0">' + reply.replyFullName + '</h6><span class="fs-sm text-muted">' + reply.replyDate + '</span>' +
//                                '        </div>' +
//                                '    </div>' +
//                                '    <div class="d-flex align-items-center justify-content-between mb-3" role="group" aria-label="Settings group">' +
//                                '        <p class="mb-0"><a class="fw-bold text-decoration-none" href="#">@@' + comment.username + '</a>&nbsp;&nbsp;<span id="reply_' + reply.replyId + '">' + reply.replyComment + '</span></p>' +
//                                '        <div>' +
//                                (reply.replyEdit ? '<button type="button" id="edt' + reply.replyId + '" onclick="replyEdit(' + reply.replyId + ')" class="btn btn-sm btn-secondary btn-icon px-2"><i class="ai-edit"></i></button>&nbsp;' : '') +
//                                (reply.replyEdit ? '<button type="button" id="repldlt_' + reply.replyId + '" onclick="replyDelete(' + reply.replyId + ')" class="btn btn-sm btn-secondary btn-icon px-2"><i class="ai-trash"></i></button>&nbsp;' : '') +
//                                '        </div>' +
//                                '    </div>' +
//                                '</div>';

//                        }
//                    }
//                    commentsHTML += `</div>`;
//                }
//            }

//            document.getElementById("comments")!.innerHTML = commentsHTML;
//            document.getElementById("blogcount")!.innerHTML = c.toString();
//        })
//        .catch((error) => {
//            if (error.response) {
//                const errorMessage = error.response.data.error;
//                console.error(errorMessage);
//            } else if (error.request) {
//                console.error('Network error:', error.request);
//            } else {
//                console.error('Error:', error.message);

//            }
//            document.getElementById("comments")!.innerHTML = "";
//            document.getElementById("blogcount")!.innerHTML = "0";

//        });
}

function replyDeleteConfirm() {
    axios.post('/api/blog/reply/delete', {
        replyId: localStorage.getItem('delid')
    })
        .then(function (response) {
            mdlClose("mdlreplyDelete");
            loadComments();
            localStorage.removeItem('delid');
            toaster("success", "reply deleted");
        })
        .catch(function (error) {
            mdlClose("mdlreplyDelete");
            localStorage.removeItem('delid');
            toaster("error", "something went wrong");
        })
}