import { acToast,acInit  ,classesToTags} from '../../global.js'
declare const axios;



const tokenele = document.querySelector('input[name="__RequestVerificationToken"]') as HTMLInputElement;
const gl_slug = document.getElementById("ip_slug") as HTMLInputElement;
const gl_tag = document.getElementById("ip_tags") as HTMLInputElement;

const token = tokenele.value;
const global_slug = gl_slug.value;
const global_tags = gl_tag.value;


acInit([
    loadTags,
    loadAuthors,
    injectClasses
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

function loadAuthors() {
    axios.get('/api/blog/' + global_slug + '/authors')
        .then(response => {
            const data = response.data;
            var authorsdat = "";
            var authorsdat = "";
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    authorsdat = authorsdat + '<a href="/author/' + data[i].userName + '" class="no-decor">' + data[i].firstName + ' ' + data[i].lastName + '</a>, ';
                }
                document.getElementById('authorsPlaceholder')!.innerHTML = authorsdat.slice(0, authorsdat.lastIndexOf(',')) + authorsdat.slice(authorsdat.lastIndexOf(',') + 1);
            }
        })
        .catch(error => {
            console.error('Error:', error);
            document.getElementById('authorsPlaceholder')!.innerHTML = "unable to fetch authors' data";
        });
}
//function addcomment() {
//    const blgcmnt = document.getElementById('blog_comment');
//    if (blgcmnt.value == "") {
//        toaster("error", "comment too short");
//    }
//    else {
//        const cmntBtn = document.getElementById('commentbutton');
//        cmntBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>posting.. ';
//        cmntBtn.classList.add('pe-none');
//        axios({
//            method: 'POST',
//            url: '/api/blog/comment/add',
//            headers: {
//                'Content-Type': 'application/json',
//            },
//            data: {
//                Slug: global_slug,
//                Comment: blgcmnt.value,
//            },
//        })
//            .then(function (response) {
//                cmntBtn.innerHTML = 'Post Comment';
//                loadComments();
//                cmntBtn.classList.remove('pe-none');
//                toaster('success', 'Comment added');
//                blgcmnt.value = "";
//            })
//            .catch(function (error) {
//                console.error('Error:', error);
//                cmntBtn.innerHTML = 'Post Comment';
//                cmntBtn.classList.remove('pe-none');
//                toaster('error', 'Something went wrong');
//            });
//    }
//}

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
