import { acToast,acInit  ,classesToTags, acPostData, acFormHandler, acGetData} from '../../global.js'
declare const axios: { get: (arg0: string) => Promise<any>; };


const tokenele = document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement;
const gl_slug = document.getElementById("ip_slug") as HTMLInputElement;
const gl_tag = document.getElementById("ip_tags") as HTMLInputElement;

const token = tokenele.value;
const global_slug = gl_slug.value;
const global_tags = gl_tag.value;


acInit([
    loadTags,
    loadAuthors,
    //add desired classes to markdown
    () => classesToTags('img', 'rounded-3'),
    //assign addcomment method as default submit action of comment form
    () => acFormHandler('comment-form', addComment),
]);
 

//function isLiked() {

//    //check if its likes by the logged in user
//    const data = {
//        slug: global_slug
//    };
//    axios.post('/api/blog/likestat', data, token)
//        .then(response => {
//            const isBlogLiked = response.data;
//            if (isBlogLiked == true) {
//                cls.value = "-filled";
//            }
//            else {
//                cls.value = "";
//            }
//        })
//        .catch(error => {
//            console.log(error.response);
//        });
//}

function loadLikes() {
    const likes = document.getElementById("likes") as HTMLElement;
    //get no of likes
    axios.get('/api/blog/' + global_slug + '/likes')
        .then(response => {
            likes.innerHTML = response.data;
        })
        .catch(error => {
            likes.innerHTML = "0";
        });
}

//function addLike() {

//    //add a like
//    if (cls.value == "") {
//        cls.value = "-filled";
//    }
//    else {
//        cls.value = "";
//    }
//    const likedata = {
//        Slug: global_slug,
//    }
//    axios.post('/api/blog/addlike', likedata)
//        .then(response => {
//            console.log(response.data);
//            loadLikes();
//            isLiked();

//        })
//        .catch(error => {
//            cls.value = "";
//        });
//}

// function loadAuthors() {
//     axios.get('/api/blog/' + global_slug + '/authors')
//         .then(response => {
//             const data = response.data;
//             var authorsdat = "";
//             var authorsdat = "";
//             if (data.length != 0) {
//                 for (var i = 0; i < data.length; i++) {
//                     authorsdat = authorsdat + '<a href="/author/' + data[i].userName + '" class="no-decor">' + data[i].firstName + ' ' + data[i].lastName + '</a>, ';
//                 }
//                 document.getElementById('authorsPlaceholder')!.innerHTML = authorsdat.slice(0, authorsdat.lastIndexOf(',')) + authorsdat.slice(authorsdat.lastIndexOf(',') + 1);
//             }
//         })
//         .catch(error => {
//             console.error('Error:', error);
//             document.getElementById('authorsPlaceholder')!.innerHTML = "unable to fetch authors' data";
//         });
// }

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
            document.getElementById('authorsPlaceholder')!.innerHTML = "Unable to fetch authors' data";
        }
    } catch (error) {
        console.error('Error:', error);
        document.getElementById('authorsPlaceholder')!.innerHTML = "Something went wrong while fetching authors' data";
    }
}


async function addComment() {
   const blgcmnt = document.getElementById('blog_comment') as HTMLInputElement;
   if (blgcmnt.value.length < 2) {
       acToast("error", "comment too short");
   }
   else {
       const cmntBtn = document.getElementById('commentbutton') as HTMLButtonElement;
       cmntBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>posting.. ';
       cmntBtn.classList.add('pe-none');

    const  commentData = {
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
           console.log("catch block"+ error);
       }
       finally {
        cmntBtn.innerHTML = 'Post Comment';
        cmntBtn.classList.remove('pe-none');
       }





    //    axios({
    //        method: 'POST',
    //        url: '/api/blog/comment/add',
    //        headers: {
    //            'Content-Type': 'application/json',
    //        },
    //        data: {
    //            Slug: global_slug,
    //            Comment: blgcmnt.value,
    //        },
    //    })
    //        .then(function (response) {
    //            cmntBtn.innerHTML = 'Post Comment';
    //            loadComments();
    //            cmntBtn.classList.remove('pe-none');
    //            acToast('success', 'Comment added');
    //            blgcmnt.value = "";
    //        })
    //        .catch(function (error) {
    //            console.error('Error:', error);
    //            cmntBtn.innerHTML = 'Post Comment';
    //            cmntBtn.classList.remove('pe-none');
    //            acToast('error', 'Something went wrong');
    //        });
   }
}

async function loadComments(){
    console.log("comment load function called")
    acToast('check','comment added ig check db');
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
    document.getElementById('tagsPlaceholder')!.innerHTML = tags;
}

function injectClasses()
{
    classesToTags('img', 'rounded-3');
}
