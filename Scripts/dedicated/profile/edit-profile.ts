
import { acFormHandler, acGetData, acInit, acPostData, acToast } from '../../global.js'
import { IProfile } from '../../Interfaces/profile.interface.js';

const firstName = document.getElementById("firstName") as HTMLInputElement;
const lastName = document.getElementById("lastName") as HTMLInputElement;
const emailId = document.getElementById("emailId") as HTMLInputElement;
const userName = document.getElementById("userName") as HTMLInputElement;
const bio = document.getElementById("bio") as HTMLTextAreaElement;

// const detailsForm = document.getElementById("basicInfoForm") as HTMLFormElement;
// const passForm = document.getElementById("passForm") as HTMLFormElement;

const Avatar = document.getElementById("avatarPlaceHolder") as HTMLElement;
const avatarDdl = document.getElementById("avatarDdl") as HTMLSelectElement;


acInit([
    async () => {
        await Promise.all([
            onAvatarChangeEvent(),
            loadavatarDdl().then(fetchDetails),
            acFormHandler("basicInfoForm", submitDetails),
            acFormHandler("passForm", submitPass),
        ]);
    }
]);



//onAvatarChangeEvent,
//async () => {
//    await loadavatarDdl();
//    await fetchDetails();
//},
//() => acFormHandler('basicInfoForm', submitDetails),
//() => acFormHandler('passForm', submitPass),

async function onAvatarChangeEvent() {

    avatarDdl!.addEventListener("change", () => {
        var selectedOption = avatarDdl!.options[avatarDdl!.selectedIndex];
        var imgValue = selectedOption.dataset.img;
        Avatar!.style.backgroundImage = 'url(/assets/images/avatars/default/' + imgValue + '.png)';
    });
}


async function getCheckedRadioButton() {
    return null;
}




async function fetchDetails() {
    const g_m = document.getElementById('male') as HTMLInputElement;
    const g_f = document.getElementById('female') as HTMLInputElement;
    const g_o = document.getElementById('other') as HTMLInputElement;

    const response = acGetData('/api/profile/getdetails');
    console.log((await response).data);
    const resp: IProfile = (await response).data;
    firstName.value = resp.firstName;
    lastName.value = resp.lastName;
    userName.value = resp.userName;
    emailId.value = resp.eMail;
    bio.value = resp.bio;
    avatarDdl.value = resp.avatarId!.toString();

    if (resp.gender == 'm') { g_m.checked}
    else if (resp.gender == 'f') { g_f.checked }
    else { g_o.checked }

    Avatar.style.backgroundImage = 'url(/assets/images/avatars/default/' + resp.avatarImg + '.png)';
}
    
async function loadavatarDdl() {
    const optns = await acGetData("/api/getavatars");
    let options = ` <option value="" selected disabled>Select avatar</option>`;
    var i;
    for (i = 1; i < optns.data.length; i++) {
        options += `<option value="${optns.data[i].id}" data-img="${optns.data[i].image}">${optns.data[i].title}</option>`;
    }
    avatarDdl.innerHTML = options;
}

async function submitDetails() {
    console.log(getCheckedRadioButton);
    const data = {
        firstName: firstName.value,
        lastName: lastName.value,
        bio: bio.value,
        avatarId: parseInt(avatarDdl.value) || 1,
        eMail: emailId.value,
        gender: getCheckedRadioButton,
        userName: userName.value
    }
    const resp = await acPostData('/api/profile/update', data);
        if(resp.type == "ok")
        {
            acToast("Success", resp.data);
            await updatePfp();
            const something = document.getElementsByClassName('name-placeholder');
            something[0].innerHTML = firstName.value;
            something[1].innerHTML = firstName.value;

        }
        else
        {
            acToast("error",resp.data);
        }    
}

async function updatePfp() {

    const selectedOption = avatarDdl.options[avatarDdl.selectedIndex];
    let dataImgValue = selectedOption.getAttribute("data-img");
    
    let imgElements = document.getElementsByClassName("avatar-placeholder") as HTMLCollectionOf<HTMLImageElement>;

    for (let i = 0; i < imgElements.length; i++) {

        imgElements[i].src = "/assets/images/avatars/default/" + dataImgValue + ".png";
        console.log(imgElements[i].src);
    }


}

async function submitPass() {

    const newpass = document.getElementById('new-pass') as HTMLInputElement;
    const confirmpass = document.getElementById('confirm-pass') as HTMLInputElement;
    if (newpass.value == "" || newpass.value.length < 6) {
        acToast('validation issue', 'password too short');
        return
    }
    if (newpass.value != confirmpass.value) {
        acToast('error', 'Passwords don\'t match');
        return;
    }

    const data = {
        password: newpass.value
    };
    const res = await acPostData('/api/profile/password/update', data);
    console.log(res);
    if (res.type == "ok") {
        acToast('success', res.data);
    }
    else {
        acToast('error', res.data);
    }
}
async function logOutFromAll() {
    const resp = acGetData('/api/account/disposekey');
    console.log(resp);
}
async function clearPreferences() {
}